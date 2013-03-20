
using System;
using System.IO;
using System.Collections;

namespace Stareater.AppData.Expressions {

public class Token {
	public int kind;    // token kind
	public int pos;     // token position in bytes in the source text (starting at 0)
	public int charPos;  // token position in characters in the source text (starting at 0)
	public int col;     // token column (starting at 1)
	public int line;    // token line (starting at 1)
	public string val;  // token value
	public Token next;  // ML 2005-03-11 Tokens are kept in linked list
}

//-----------------------------------------------------------------------------------
// Scanner
//-----------------------------------------------------------------------------------
public class Scanner {
	static class Buffer
    {
        public const int EOF = char.MaxValue + 1;
    }

	const char EOL = '\n';
	const int eofSym = 0; /* pdt */
	const int maxT = 36;
	const int noSym = 36;
	char valCh;       // current input character (for token.val)

	public string input; // scanner input
	
	Token t;          // current token
	int ch;           // current input character
	int pos;          // byte position of current character
	int charPos;      // position by unicode characters starting with 0
	int col;          // column number of current character
	int line;         // line number of current character
	int oldEols;      // EOLs that appeared in a comment;
	static readonly Hashtable start; // maps first token character to start state

	Token tokens;     // list of tokens already peeked (first token is a dummy)
	Token pt;         // current peek token
	
	char[] tval = new char[128]; // text of current token
	int tlen;         // length of current token
	
	static Scanner() {
		start = new Hashtable(128);
		for (int i = 48; i <= 57; ++i) start[i] = 1;
		for (int i = 97; i <= 122; ++i) start[i] = 7;
		start[61] = 8; 
		start[60] = 37; 
		start[8800] = 10; 
		start[8804] = 12; 
		start[62] = 38; 
		start[8805] = 14; 
		start[126] = 15; 
		start[38] = 16; 
		start[8743] = 17; 
		start[8896] = 18; 
		start[124] = 19; 
		start[8744] = 20; 
		start[8897] = 21; 
		start[64] = 22; 
		start[8853] = 23; 
		start[43] = 24; 
		start[45] = 39; 
		start[42] = 25; 
		start[47] = 26; 
		start[92] = 27; 
		start[37] = 28; 
		start[94] = 29; 
		start[39] = 30; 
		start[91] = 32; 
		start[44] = 33; 
		start[93] = 34; 
		start[40] = 35; 
		start[41] = 36; 
		start[Buffer.EOF] = -1;

	}
	
	public Scanner (string input) {
        this.input = input;
        Init();
	}
	
	void Init() {
		pos = -1; line = 1; col = 0; charPos = -1;
		oldEols = 0;
		NextCh();
		pt = tokens = new Token();  // first token is a dummy
	}
	
	void NextCh() {
		if (oldEols > 0) { ch = EOL; oldEols--; }
         else
        {
			pos = charPos;
            charPos++;

            if (charPos >= input.Length)
                ch = Buffer.EOF;
            else
            {
                ch = input[charPos]; col++;
                // replace isolated '\r' by '\n' in order to make
                // eol handling uniform across Windows, Unix and Mac
                if (ch == '\r' && input.Length > charPos && input[charPos] != '\n') ch = EOL;
                if (ch == EOL) { line++; col = 0; }
            }
        }
		if (ch != Buffer.EOF) {
			valCh = (char) ch;
			ch = char.ToLower((char) ch);
		}

	}

	void AddCh() {
		if (tlen >= tval.Length) {
			char[] newBuf = new char[2 * tval.Length];
			Array.Copy(tval, 0, newBuf, 0, tval.Length);
			tval = newBuf;
		}
		if (ch != Buffer.EOF) {
			tval[tlen++] = valCh;
			NextCh();
		}
	}




	void CheckLiteral() {
		switch (t.val.ToLower()) {
			case "inf": t.kind = 3; break;
			default: break;
		}
	}

	Token NextToken() {
		while (ch == ' ' ||
			ch >= 9 && ch <= 10 || ch == 13
		) NextCh();

		int recKind = noSym;
		int recEnd = pos;
		t = new Token();
		t.pos = pos; t.col = col; t.line = line; t.charPos = charPos;
		int state;
		if (start.ContainsKey(ch)) { state = (int) start[ch]; }
		else { state = 0; }
		tlen = 0; AddCh();
		
		switch (state) {
			case -1: { t.kind = eofSym; break; } // NextCh already done
			case 0: {
				if (recKind != noSym) {
					tlen = recEnd - t.pos;
					SetScannerBehindT();
				}
				t.kind = recKind; break;
			} // NextCh already done
			case 1:
				recEnd = pos; recKind = 1;
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 1;}
				else if (ch == 'e') {AddCh(); goto case 2;}
				else if (ch == '.') {AddCh(); goto case 5;}
				else {t.kind = 1; break;}
			case 2:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 4;}
				else if (ch == '-') {AddCh(); goto case 3;}
				else {goto case 0;}
			case 3:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 4;}
				else {goto case 0;}
			case 4:
				recEnd = pos; recKind = 1;
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 4;}
				else {t.kind = 1; break;}
			case 5:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 6;}
				else {goto case 0;}
			case 6:
				recEnd = pos; recKind = 1;
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 6;}
				else if (ch == 'e') {AddCh(); goto case 2;}
				else {t.kind = 1; break;}
			case 7:
				recEnd = pos; recKind = 2;
				if (ch >= 'a' && ch <= 'z') {AddCh(); goto case 7;}
				else {t.kind = 2; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 8:
				{t.kind = 4; break;}
			case 9:
				{t.kind = 5; break;}
			case 10:
				{t.kind = 6; break;}
			case 11:
				{t.kind = 8; break;}
			case 12:
				{t.kind = 9; break;}
			case 13:
				{t.kind = 11; break;}
			case 14:
				{t.kind = 12; break;}
			case 15:
				{t.kind = 13; break;}
			case 16:
				{t.kind = 14; break;}
			case 17:
				{t.kind = 15; break;}
			case 18:
				{t.kind = 16; break;}
			case 19:
				{t.kind = 17; break;}
			case 20:
				{t.kind = 18; break;}
			case 21:
				{t.kind = 19; break;}
			case 22:
				{t.kind = 20; break;}
			case 23:
				{t.kind = 21; break;}
			case 24:
				{t.kind = 22; break;}
			case 25:
				{t.kind = 24; break;}
			case 26:
				{t.kind = 25; break;}
			case 27:
				{t.kind = 26; break;}
			case 28:
				{t.kind = 27; break;}
			case 29:
				{t.kind = 28; break;}
			case 30:
				{t.kind = 29; break;}
			case 31:
				{t.kind = 30; break;}
			case 32:
				{t.kind = 31; break;}
			case 33:
				{t.kind = 32; break;}
			case 34:
				{t.kind = 33; break;}
			case 35:
				{t.kind = 34; break;}
			case 36:
				{t.kind = 35; break;}
			case 37:
				recEnd = pos; recKind = 7;
				if (ch == '>') {AddCh(); goto case 9;}
				else if (ch == '=') {AddCh(); goto case 11;}
				else {t.kind = 7; break;}
			case 38:
				recEnd = pos; recKind = 10;
				if (ch == '=') {AddCh(); goto case 13;}
				else {t.kind = 10; break;}
			case 39:
				recEnd = pos; recKind = 23;
				if (ch == 39) {AddCh(); goto case 31;}
				else {t.kind = 23; break;}

		}
		t.val = new String(tval, 0, tlen);
		return t;
	}
	
	private void SetScannerBehindT() {
		charPos = t.pos;
		NextCh();
		line = t.line; col = t.col; charPos = t.charPos;
		for (int i = 0; i < tlen; i++) NextCh();
	}
	
	// get the next token (possibly a token already seen during peeking)
	public Token Scan () {
		if (tokens.next == null) {
			return NextToken();
		} else {
			pt = tokens = tokens.next;
			return tokens;
		}
	}

	// peek for the next token, ignore pragmas
	public Token Peek () {
		do {
			if (pt.next == null) {
				pt.next = NextToken();
			}
			pt = pt.next;
		} while (pt.kind > maxT); // skip pragmas
	
		return pt;
	}

	// make sure that peeking starts at the current scan position
	public void ResetPeek () { pt = tokens; }

} // end Scanner
}