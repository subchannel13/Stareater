#version 150

uniform sampler2D textureSampler;
uniform mediump vec4 color;

in highp vec2 textureCoord;

out vec4 outputF;

void main()
{
   outputF = texture2D(textureSampler, textureCoord) * color;
}