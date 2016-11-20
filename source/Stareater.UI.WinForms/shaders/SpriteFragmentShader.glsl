#version 150

uniform sampler2D textureSampler;
uniform vec4 color;

in vec2 textureCoord;

out vec4 outputF;

void main()
{
   outputF = texture2D(textureSampler, textureCoord) * color;
}