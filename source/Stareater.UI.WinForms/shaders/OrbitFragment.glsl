#version 140

uniform float minR;
uniform float maxR;
uniform vec4 color;

in vec2 orbitPositionFrag;

out vec4 outputF;

#define M_PI 3.1415926535897932384626433832795

vec2 cartToPolar(vec2 v)
{
	if (v.x == 0 && v.y == 0)
		return vec2(0, 0);

	float r = length(v);
	float angle = abs(v.x) > abs(v.y) ?
		v.x >= 0 ? asin(v.y / r) : M_PI - asin(v.y / r) :
		acos(v.x / r) * sign(v.y);

	return vec2(r, angle >= 0 ? angle : angle + 2 * M_PI);
}

void main()
{
   vec2 p = cartToPolar(orbitPositionFrag);
   float r = p.x;
   
   if (r >= minR && r <= maxR)
      outputF = color;
   else
      outputF = vec4(0, 0, 0, 0);
}