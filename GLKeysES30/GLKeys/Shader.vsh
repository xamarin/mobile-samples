#version 300 es

in vec4 position;
in vec4 normal;
out vec3 textureCoordinate;
out vec3 vertexE;
out vec3 normalE;

uniform float translate;
uniform mat4 projection;
uniform mat4 view;
uniform mat4 normalMatrix;
uniform mat4 texProjection;
uniform int[24] texDepth;

void main()
{
	vec4 vpos = position;
	vertexE = (view * position).xyz;
	normalE = normalize((normalMatrix * normal).xyz);
	int line = gl_InstanceID / 6;
    vpos.x += float(gl_InstanceID % 6)*2.2 - 5.5 + float((line) % 2)*.6 - .3;
    vpos.y += float(line)*2.2 - 3.3;
    gl_Position = projection * vpos;

    textureCoordinate = (texProjection * position).xyz;
    textureCoordinate.z = float(texDepth[gl_InstanceID]);
}
