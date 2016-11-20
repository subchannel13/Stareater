#version 150

uniform sampler2D textureSampler;
uniform vec4 color;

in vec2 textureCoord;

out vec4 outputF;

void main()
{
   outputF = vec4(textureCoord.x, textureCoord.y, 0, 1);
   //texture2D(textureSampler, textureCoord);
   // * color;
   //outputF = color;
}