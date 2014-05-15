#version 300 es

//in lowp vec4 colorVarying;
in highp vec3 textureCoordinate;
in highp vec3 vertexE;
in highp vec3 normalE;

out lowp vec4 FragColor;

uniform lowp sampler2DArray text;
uniform highp vec3 light;

void main()
{
	highp vec3 L = normalize (light - vertexE);
    highp vec3 E = normalize (-vertexE);
    highp vec3 R = normalize (-reflect (L, normalE));

    lowp vec4 amb = vec4 (.4, .4, .4, 1.0);
    lowp vec4 diff = vec4 (.6, .6, .6, 1.0) * max(dot(normalE,L), 0.0);
    diff = clamp (diff, 0.0, 1.0);

    FragColor = texture (text, textureCoordinate) * (amb + diff);
}
