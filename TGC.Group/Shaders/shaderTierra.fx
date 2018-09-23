/**************************************************************************************/
/* Variables comunes */
/**************************************************************************************/

//Matrices de transformacion
float4x4 matWorld; //Matriz de transformacion World
float4x4 matWorldView; //Matriz World * View
float4x4 matWorldViewProj; //Matriz World * View * Projection
float4x4 matInverseTransposeWorld; //Matriz Transpose(Invert(World))

texture texDiffuseMap;
sampler2D diffuseMap = sampler_state
{
	Texture = (texDiffuseMap);
	ADDRESSU = WRAP;
	ADDRESSV = WRAP;
	MINFILTER = LINEAR;
	MAGFILTER = LINEAR;
	MIPFILTER = LINEAR;
};

texture NormalMap;
sampler2D bumpSampler = sampler_state {
	Texture = (NormalMap);
	MinFilter = Linear;
	MagFilter = Linear;
	AddressU = Wrap;
	AddressV = Wrap;
};

float BumpConstant = 1;

float3 tangente = float3(1, 0, 0);
float3 bitangente = float3(1, 0, 0);

float time = 0;

//variables para la iluminacion
float3 fvLightPosition = float3(500.00, 2000.00, 3000.00);
float3 fvEyePosition = float3(0.00, 100.00, -100.00);
float k_la = 0.3;							// luz ambiente global
float k_ld = 0.6;						// luz difusa
float k_ls = 1;							// luz specular
float fSpecularPower = 99.8;//7.5;//16.84;				// exponente de la luz specular

float4 fogColor = float4(0.11f, 0.245f, 0.29f, 0.6f);
float fogStart = 3000;
float blendStart = 2000;

float delta = 150.0;

//Input del Vertex Shader
struct VS_INPUT
{
	float4 Position : POSITION0;
	float3 Normal : NORMAL0;
	float4 Color :  COLOR0;
	float2 Texcoord : TEXCOORD0;
	//para bump
	float3 Tangent : TANGENT0;
	float3 Binormal : BINORMAL0;
};

//Output del Vertex Shader
struct VS_OUTPUT
{
	float4 Position : POSITION0;
	float2 Texcoord : TEXCOORD0;
	float3 Norm :     TEXCOORD1;	    // Normales
	float3 Pos :      TEXCOORD2;		// Posicion real 3d
	float3 Pos2 :     TEXCOORD3;		// Posicion en 2d
	float3 WorldPosition : TEXCOORD4;
	float3 WorldNormal	: TEXCOORD5;
	float fogfactor : FOG;
	//para bump
	float3 Tangent : TEXCOORD6;
	float3 Binormal : TEXCOORD7;
};

VS_OUTPUT vs_main(VS_INPUT Input)
{
	VS_OUTPUT Output;

	// Calculo la posicion real (en world space)
	float4 pos_real = mul(Input.Position, matWorld);

	// Y la propago usando las coordenadas de texturas 2
	Output.Pos = float3(pos_real.x, pos_real.y, pos_real.z);
	Input.Normal = normalize(Input.Position.xyz);

	tangente = float3(Input.Position.x, 0, Input.Position.z + delta);
	bitangente = float3(Input.Position.x + delta, 0, Input.Position.z);
	Output.Tangent = normalize(mul(tangente, matWorld));
	Output.Binormal = normalize(mul(bitangente, matWorld));

	Output.Position = mul(Input.Position, matWorldViewProj);
	Output.WorldPosition = mul(Input.Position, matWorld).xyz;
	Output.WorldNormal = mul(Input.Normal, matInverseTransposeWorld).xyz;
	Output.Pos2 = Output.Position;

	Output.fogfactor = saturate(Output.Position.z);
	Output.Texcoord = Input.Texcoord;
	Output.Norm = normalize(mul(Input.Normal, matWorld));

	return(Output);
}

float4 ps_main(float3 Texcoord: TEXCOORD0, float3 N : TEXCOORD1, float3 Pos : TEXCOORD2, float3 Pos2 : TEXCOORD3, float3 WorldPosition : TEXCOORD4, float3 WorldNormal : TEXCOORD5, float3 Tangent : TEXCOORD6, float3 Binormal : TEXCOORD7, float fogfactor : FOG) : COLOR0
{
	float4 RGBColor = fogColor;
	//para calcular la iluminacion del pixel
	float ld = 0;		// luz difusa
	float le = 0;		// luz specular

	//calcular los factores de fog y alpha blending que actuan en profundidad
	fogfactor = saturate((5000.0f - Pos2.z) / (fogStart)); // (fogEnd - z) /(fogEnd - fogStart)
	float blendfactor = saturate((5000.0f - Pos2.z) / (blendStart));
	//Obtener el texel de textura
	float4 fvBaseColor = tex2D(diffuseMap, Texcoord);

	if (Pos2.z < 2000)//si el pixel estaba lejos en world space va sin detalles
	{ 
		//cambio las coordenadas de textura
		float3 newTexcoord = Texcoord;
		//repito la textura varias veces (depende de _zoom)
		float _zoom = 45.0f;
		newTexcoord *= _zoom;
		newTexcoord.x += step(1., step(newTexcoord.x, 1.0)) * 0.5;
		newTexcoord = frac(newTexcoord);

		// Calculate the normal, including the information in the bump map
		float3 bump = BumpConstant * (tex2D(bumpSampler, newTexcoord) - (0.5, 0.5, 0.5));
		float3 bumpNormal = N + (bump.x * Tangent + bump.y * Binormal);
		N = normalize(bumpNormal);
	}

	// calcula la luz diffusa
	float3 LD = normalize(fvLightPosition - float3(Pos.x,Pos.y,Pos.z));
	ld += saturate(dot(N, LD))*k_ld;

	// calcula la reflexion specular
	float3 D = normalize(float3(Pos.x,Pos.y,Pos.z) - fvEyePosition);
	float ks = saturate(dot(reflect(LD,N), D));
	ks = pow(ks,fSpecularPower);
	le += ks * k_ls;	

	fvBaseColor = (fvBaseColor *  fogfactor) + (fogColor * (1.0 - fogfactor));
	RGBColor.rgb = saturate(fvBaseColor * (saturate(k_la + ld)) + le);
	RGBColor.a = blendfactor;

	return RGBColor;
}

technique RenderScene
{
   pass Pass_0
   {
          AlphaBlendEnable =TRUE;
          DestBlend= INVSRCALPHA;
          SrcBlend= SRCALPHA;
		  VertexShader = compile vs_3_0 vs_main();
		  PixelShader = compile ps_3_0 ps_main(); 
   }
}

