#version 140

uniform sampler2D textureSampler;
uniform vec4 color;

in vec2 textureCoord;

out vec4 outputF;

void main()
{
   vec4 distColor = texture2D(textureSampler, textureCoord);

   if (distColor.a > 0.5)
      outputF = color;
   else
      outputF = vec4(0, 0, 0, 0);
}