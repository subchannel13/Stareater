#version 150

uniform sampler2D textureSampler;
uniform mediump vec4 color;

in highp vec2 TextureCoord;

out vec4 outputF;

void main()
{
   outputF = texture2D(textureSampler, TextureCoord) * color;
}