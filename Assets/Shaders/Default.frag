#version 330 core
out vec4 FragColor;

in vec2 texCoord;

uniform sampler2D texture0;
uniform vec2 tiling;

void main()
{
    vec2 tiledCoords = texCoord * tiling;
    FragColor = texture(texture0, tiledCoords);
}