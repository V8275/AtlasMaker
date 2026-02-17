#version 330 core

struct Material {
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float shininess;
};

struct Light {
    vec3 position;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

uniform Material material;
uniform Light light;
uniform vec3 viewPos;
uniform sampler2D texture0;
uniform sampler2D shadowMap;

in vec2 texCoord;
in vec3 Normal;
in vec3 FragPos;
in vec4 FragPosLightSpace;

out vec4 outputColor;

float ShadowCalculation(vec4 fragPosLightSpace, vec3 normal, vec3 lightDir)
{
    vec3 projCoords = fragPosLightSpace.xyz / fragPosLightSpace.w;
    projCoords = projCoords * 0.5 + 0.5;
    
    if(projCoords.z > 1.0)
        return 0.0;
    
    float closestDepth = texture(shadowMap, projCoords.xy).r;
    float currentDepth = projCoords.z;
    float bias = max(0.05 * (1.0 - dot(normal, lightDir)), 0.005);
    
    float shadow = 0.0;
    vec2 texelSize = 1.0 / textureSize(shadowMap, 0);
    
    for(int x = -1; x <= 1; ++x)
    {
        for(int y = -1; y <= 1; ++y)
        {
            float pcfDepth = texture(shadowMap, projCoords.xy + vec2(x, y) * texelSize).r;
            shadow += currentDepth - bias > pcfDepth ? 1.0 : 0.0;        
        }    
    }
    shadow /= 9.0;
    
    return shadow;
}

void main()
{
    vec4 textureColor = texture(texture0, texCoord);
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(light.position - FragPos);
    vec3 viewDir = normalize(viewPos - FragPos);
    
    float shadow = ShadowCalculation(FragPosLightSpace, norm, lightDir);
    
    vec3 ambient = light.ambient * material.ambient;
    
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * material.diffuse;
    
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = light.specular * spec * material.specular;
    
    vec3 result = (ambient + (1.0 - shadow) * (diffuse + specular)) * textureColor.rgb;
    outputColor = vec4(result, textureColor.a);
}