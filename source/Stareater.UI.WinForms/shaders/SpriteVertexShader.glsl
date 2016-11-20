#version 150

uniform mat4 localtransform;
uniform float z;

in vec2 localPosition;
in vec2 texturePosition;

out vec2 textureCoord;

void main()
{
   gl_Position = localtransform * vec4 (localPosition, z, 1.0);
   textureCoord = texturePosition;
}