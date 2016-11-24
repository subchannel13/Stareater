#version 140

uniform mat4 localtransform;
uniform float z;

in vec2 localPosition;
varying  vec2 orbitPosition;

void main()
{
   gl_Position = localtransform * vec4 (localPosition, z, 1.0);
   orbitPosition = orbitPosition;
}