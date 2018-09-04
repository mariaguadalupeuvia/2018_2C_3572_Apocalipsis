
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

float3 DiffuseLightDirection = float3(1, 0, 0);
float4 DiffuseColor = float4(1, 1, 1, 1);
float DiffuseIntensity = 1.0;

float Shininess = 200;
float4 SpecularColor = float4(1, 1, 1, 1);
float SpecularIntensity = 1;
float3 ViewVector = float3(1, 0, 0);


float _Time = 0;

//variables para la iluminacion
float3 fvLightPosition = float3(-100.00, 140.00, 3000.00);
float3 fvEyePosition = float3(0.00, 0.00, -100.00);
float k_la = 0.3;							// luz ambiente global
float k_ld = 0.6;						// luz difusa
float k_ls = 1.0;							// luz specular
float fSpecularPower = 7.5;//16.84;				// exponente de la luz specular

float4 fogColor = float4(0.2f, 0.9f, 1.0f, 1.0f);
float fogStart = 2000;
float blendStart = 2000;

float reflection = 0.4;
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

	Input.Texcoord.y = Input.Texcoord.y + cos(_Time);
	Output.Texcoord = Input.Texcoord;
	Output.Norm = normalize(mul(Input.Normal, matWorld));

	return(Output);
}


//Pixel Shader blend 
float4 ps_main(float3 Texcoord: TEXCOORD0, float3 N : TEXCOORD1, float3 Pos : TEXCOORD2, float3 Pos2 : TEXCOORD3, float3 WorldPosition : TEXCOORD4, float3 WorldNormal : TEXCOORD5, float3 Tangent : TEXCOORD6, float3 Binormal : TEXCOORD7) : COLOR0
{
	//Obtener el texel de textura
	    float4 fvBaseColor = tex2D(diffuseMap, Texcoord);
		
		////cambio las coordenadas de textura
		//float3 newTexcoord = Texcoord;
		////repito la textura varias veces (depende de _zoom)
		//float _zoom = 25.0f;
		//newTexcoord *= _zoom;
		//newTexcoord.x += step(1., step(newTexcoord.x, 1.0)) * 0.5;
		//newTexcoord = frac(newTexcoord);

	// Calculate the normal, including the information in the bump map
	   float3 bump = BumpConstant * (tex2D(bumpSampler, Texcoord) - (0.5, 0.5, 0.5));
	   float3 bumpNormal = N + (bump.x * Tangent + bump.y * Binormal);
	   bumpNormal = normalize(bumpNormal);
	   N = bumpNormal;

	   float ld = 0;		// luz difusa
	   float le = 0;		// luz specular

	   // calcula la luz diffusa
	   float3 LD = normalize(fvLightPosition - float3(Pos.x,Pos.y,Pos.z));
	   ld += saturate(dot(N, LD))*k_ld;

	   // calcula la reflexion specular
	   float3 D = normalize(float3(Pos.x,Pos.y,Pos.z) - fvEyePosition);
	   float ks = saturate(dot(reflect(LD,N), D));
	   ks = pow(ks,fSpecularPower);
	   le += ks * k_ls;

	   float4 RGBColor = 0;
	   RGBColor.rgb = saturate(fvBaseColor*(saturate(k_la + ld)) + le);

	   RGBColor.a = 0.7;
	   return RGBColor;
}

technique RenderScene
{
	pass Pass_0

	{
		AlphaBlendEnable = TRUE;
		DestBlend = INVSRCALPHA;
		SrcBlend = SRCALPHA;
		//CullMode = Ccw;//Cw;//None;
	  //  ZEnable = false;
		VertexShader = compile vs_3_0 vs_main();
		PixelShader = compile ps_2_0 ps_main();

	}

}


///**************************************************************************************/
///* Variables comunes */
///**************************************************************************************/
//
////Matrices de transformacion
//float4x4 matWorld; //Matriz de transformacion World
//float4x4 matWorldView; //Matriz World * View
//float4x4 matWorldViewProj; //Matriz World * View * Projection
//float4x4 matInverseTransposeWorld; //Matriz Transpose(Invert(World))
//
////Textura para DiffuseMap
//texture texDiffuseMap;
//sampler2D diffuseMap = sampler_state
//{
//	Texture = (texDiffuseMap);
//	ADDRESSU = WRAP;
//	ADDRESSV = WRAP;
//	MINFILTER = LINEAR;
//	MAGFILTER = LINEAR;
//	MIPFILTER = LINEAR;
//};
//
////Textura para AlphaMap
//texture texAlphaMap;
//sampler2D alphaMap = sampler_state
//{
//	Texture = (texAlphaMap);
//	ADDRESSU = WRAP;
//	ADDRESSV = WRAP;
//	MINFILTER = LINEAR;
//	MAGFILTER = LINEAR;
//	MIPFILTER = LINEAR;
//};
//
////Textura utilizada para BumpMapping
//texture texNormalMap;
//sampler2D normalMap = sampler_state
//{
//	Texture = (texNormalMap);
//	ADDRESSU = WRAP;
//	ADDRESSV = WRAP;
//	MINFILTER = LINEAR;
//	MAGFILTER = LINEAR;
//	MIPFILTER = LINEAR;
//};
//
////variables para la iluminacion
//float3 fvLightPosition = float3( 100.00, 140.00, 3000.00 );
//float3 fvEyePosition = float3( 0.00, 0.00, -100.00 );
//float k_la = 0.9;							// luz ambiente global
//float k_ld = 0.6;						// luz difusa
//float k_ls = 0.7;							// luz specular
//float fSpecularPower = 7.5;//16.84;				// exponente de la luz specular
//
////Intensidad de efecto Bump
//float bumpiness = 1.0f;
//const float3 BUMP_SMOOTH = { 0.5f, 0.5f, 1.0f };
//
//float _Time = 0.0f;
//
//struct VS_INPUT
//{
//	float4 Position : POSITION0;
//	float3 Normal :   NORMAL0;
//	float4 Color : COLOR;
//	float2 Texcoord : TEXCOORD0;
//	//para bump
//	float3 Tangent : TANGENT0;
//	float3 Binormal : BINORMAL0;
//};
//
////Output del Vertex Shader
//struct VS_OUTPUT 
//{
//   float4 Position : POSITION0;
//   float2 Texcoord : TEXCOORD0;
//   float3 Norm :     TEXCOORD1;			// Normales
//   float3 Pos :      TEXCOORD2;		// Posicion real 3d
//   float3 Pos2 :     TEXCOORD3;		// Posicion en 2d
//   float3 WorldPosition : TEXCOORD4;
//   float3 WorldNormal	: TEXCOORD5;
//
//   //para bump
//	float3 WorldTangent	: TEXCOORD6;
//	float3 WorldBinormal : TEXCOORD7;
//};
//
//
//VS_OUTPUT vs_main( VS_INPUT Input )
//{
//	VS_OUTPUT Output;
//
//   // Calculo la posicion real (en world space)
//   float4 pos_real = mul(Input.Position,matWorld);
//
//   Output.Pos = float3(pos_real.x,pos_real.y,pos_real.z);
//
//    Output.Position = mul( Input.Position, matWorldViewProj);
//
//	//Posicion pasada a World-Space
//	Output.WorldPosition = mul(Input.Position, matWorld).xyz;
//
//	//Pasar normal, tangent y binormal a World-Space
//	Output.WorldNormal = mul(Input.Normal, matInverseTransposeWorld).xyz;
//	Output.WorldTangent = mul(Input.Tangent, matInverseTransposeWorld).xyz;
//	Output.WorldBinormal = mul(Input.Binormal, matInverseTransposeWorld).xyz;
//
//
//    Output.Pos2 = Output.Position;
//
//    Output.Texcoord = Input.Texcoord;
//   
//    // Transformo la normal y la normalizo (si la escala no es uniforme usar la inversa Traspta)
//    Output.Norm = normalize(mul(Input.Normal, matWorld));
//     
//	return( Output );  
//}
//
//// ------------------------------------------------------------------
//
////Pixel Shader blend 
//float4 ps_main( float3 Texcoord: TEXCOORD0, float3 N:TEXCOORD1, float3 Pos: TEXCOORD2, float3 Pos2 : TEXCOORD3,
//	            float3 WorldPosition : TEXCOORD4, float3 WorldNormal: TEXCOORD5, 
//	            float3 WorldTangent : TEXCOORD6, float3 WorldBinormal : TEXCOORD7) : COLOR0
//{      
//	float ld = 0;		// luz difusa
//	float le = 0;		// luz specular
//
//	//para bump
//	//Normalizar vectores
//	float3 Nn = normalize(WorldNormal);
//	float3 Tn = normalize(WorldTangent);
//	float3 Bn = normalize(WorldBinormal);
//
//	//Obtener normal de normalMap y ajustar rango de [0, 1] a [-1, 1]
//	float3 bumpNormal = tex2D(normalMap, Texcoord).rgb;
//	bumpNormal = (bumpNormal * 2.0f) - 1.0f;
//
//	//Suavizar con bumpiness
//	bumpNormal = lerp(BUMP_SMOOTH, bumpNormal, bumpiness);
//
//	//Pasar de Tangent-Space a World-Space
//	bumpNormal = Nn + bumpNormal.x * Tn + bumpNormal.y * Bn;
//	N = normalize(bumpNormal);
//	//hasta aca
//
//	//N = normalize(N);
//
//	// calcula la luz diffusa
//	float3 LD = normalize(fvLightPosition-float3(Pos.x,Pos.y,Pos.z));
//	ld += saturate(dot(N, LD))*k_ld;
//	
//	// calcula la reflexion specular
//	float3 D = normalize(float3(Pos.x,Pos.y,Pos.z)-fvEyePosition);
//	float ks = saturate(dot(reflect(LD,N), D));
//	ks = pow(ks,fSpecularPower);
//	le += ks*k_ls;
//
//	//Obtener el texel de textura
//    float4 fvBaseColor = tex2D( diffuseMap, Texcoord);
//
//	float4 RGBColor = 0;
//
//    RGBColor.rgb = saturate(fvBaseColor*(saturate(k_la+ld)) + le);
//   
//	RGBColor.a = 1.0;
//    return RGBColor;
//}  
//	
//technique normal
//{
//   pass Pass_0
//   {
//          AlphaBlendEnable =TRUE;
//          DestBlend= INVSRCALPHA;
//          SrcBlend= SRCALPHA;
//		  VertexShader = compile vs_3_0 vs_main();
//		  PixelShader = compile ps_3_0 ps_main(); 
//   }
//}
//
//technique bump
//{
//	pass Pass_0
//	{
//		AlphaBlendEnable = TRUE;
//		DestBlend = INVSRCALPHA;
//		SrcBlend = SRCALPHA;
//		VertexShader = compile vs_3_0 vs_main();
//		PixelShader = compile ps_3_0 ps_main();
//	}
//}
