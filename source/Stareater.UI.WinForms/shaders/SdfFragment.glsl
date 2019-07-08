#version 140

uniform sampler2D textureSampler;
uniform vec4 color;

in vec2 textureCoord;

out vec4 outputF;

void main()
{
   vec4 color = texture2D(textureSampler, textureCoord) * color;

   if (color.a > 0.5)
      outputF = texture2D(textureSampler, textureCoord) * color;
   else
      outputF = vec4(0, 0, 0, 0);
}