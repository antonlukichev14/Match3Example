#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoord;

out vec2 texCoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

uniform float time;

void main()
{
    texCoord = aTexCoord;

    float size = 1 + ((sin(time * 3) + 1) / 2) * .2;
    gl_Position = vec4(aPosition * size, 1.0) * model * view * projection;
}