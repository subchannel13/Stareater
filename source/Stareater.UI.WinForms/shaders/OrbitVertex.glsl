#version 140

uniform mat4 localtransform;
uniform float z;

in vec2 localPosition;
in vec2 orbitPositionVert;

out vec2 orbitPositionFrag;

void main()
{
   gl_Position = localtransform * vec4 (localPosition, z, 1.0);
   orbitPositionFrag = orbitPositionVert;
}