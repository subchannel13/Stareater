#version 140

uniform float minR;
uniform float maxR;
uniform vec4 color;

in  vec2 orbitPositionFrag;

out vec4 outputF;

void main()
{
   float r = length(orbitPositionFrag);
   
   if (r >= minR && r <= maxR)
      outputF = color;
   else
      outputF = vec4(0, 0, 0, 0);
}