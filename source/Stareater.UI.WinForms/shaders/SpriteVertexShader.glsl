#version 150

uniform highp mat4 localtransform;
uniform mediump float z;

in highp vec2 localPosition;
in highp vec2 texturePosition;

out highp vec2 TextureCoord;

void main()
{
   gl_Position = localtransform * vec4 (localPosition, z, 1.0);
   TextureCoord = texturePosition;
}