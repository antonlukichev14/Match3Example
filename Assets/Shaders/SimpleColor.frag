#version 330 core
out vec4 FragColor;

in vec2 texCoord;

uniform vec3 objectColor;

void main()
{
    FragColor = vec4(objectColor, 1f);
}