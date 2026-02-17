#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aTexCoord;
layout (location = 2) in vec3 aNormal;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform mat4 lightSpaceMatrix;

out vec2 texCoord;
out vec3 Normal;
out vec3 FragPos;
out vec4 FragPosLightSpace;

void main(void)
{
    texCoord = aTexCoord;
    Normal = aNormal;
    FragPos = vec3(model * vec4(aPosition, 1.0));
    FragPosLightSpace = lightSpaceMatrix * vec4(FragPos, 1.0);
    gl_Position = projection * view * model * vec4(aPosition, 1.0);
}