rem Copies {svnRoot}/podaci and {svnRoot}/slike
rem to source/Zvjezdojedac/bin/Debug/ and
rem source/Zvjezdojedac/bin/Release/

echo off
robocopy ../build/podaci/ ../source/Zvjezdojedac/bin/Debug/podaci/ /xd .svn /e
robocopy ../build/slike/ ../source/Zvjezdojedac/bin/Debug/slike/ /xd .svn /e
robocopy ../build/jezici/ ../source/Zvjezdojedac/bin/Debug/jezici/ /xd .svn /e
robocopy ../build/ ../source/Zvjezdojedac/bin/Debug/ slika.txt

robocopy ../build/podaci/ ../source/Zvjezdojedac/bin/Release/podaci/ /xd .svn /e
robocopy ../build/slike/ ../source/Zvjezdojedac/bin/Release/slike/ /xd .svn /e
robocopy ../build/jezici/ ../source/Zvjezdojedac/bin/Release/jezici/ /xd .svn /e
robocopy ../build/ ../source/Zvjezdojedac/bin/Release/ slika.txt
pause
