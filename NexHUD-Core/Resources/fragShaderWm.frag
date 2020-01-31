#version 450 core

uniform sampler2D _MainTex;
uniform sampler2D _AlphaMap;
uniform float _blend;
out vec4 color;

void main(void)
{
	//Doesn't work :'(

	float a = texelFetch(_MainTex, ivec2(gl_FragCoord.xy), 0).w;
	if( _blend < 1 )
	{
		float _mask = texelFetch(_AlphaMap, ivec2(gl_FragCoord.xy), 0).x;
		if( _mask < _blend )
		{
			a = 0;
		}
	}
	color = vec4(texelFetch(_MainTex, ivec2(gl_FragCoord.xy), 0).xyz, a );
}