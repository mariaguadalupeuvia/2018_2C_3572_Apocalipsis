/**************************************************************************************/
/* Variables comunes */
/**************************************************************************************/

//Matrices de transformacion
float4x4 matWorld; //Matriz de transformacion World
float4x4 matWorldView; //Matriz World * View
float4x4 matWorldViewProj; //Matriz World * View * Projection
float4x4 matInverseTransposeWorld; //Matriz Transpose(Invert(World))


//________________________________________________________________________________________________________
//________________TEXTURAS________________________________________________________________________________

texture g_txShadow;
sampler2D g_samShadow =
sampler_state
{
    Texture = <g_txShadow>;
    MinFilter = Point;
    MagFilter = Point;
    MipFilter = Point;
    AddressU = Clamp;
    AddressV = Clamp;
};

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

texture mainText;
sampler2D mainMap = sampler_state
{
	Texture = (mainText);
	ADDRESSU = WRAP;
	ADDRESSV = WRAP;
	MINFILTER = LINEAR;
	MAGFILTER = LINEAR;
	MIPFILTER = LINEAR;
};

texture alphaMap;
sampler2D alphaSampler = sampler_state 
{
	Texture = (alphaMap);
	MinFilter = Linear;
	MagFilter = Linear;
	AddressU = Wrap;
	AddressV = Wrap;
};

//________________________________________________________________________________________________________
//________________VARIABLES_______________________________________________________________________________

//variables para la iluminacion
float3 fvLightPosition = float3(500.00, 2000.00, 3000.00);
float3 fvEyePosition = float3(0.00, 100.00, -100.00);
float k_la = 0.3;						// luz ambiente global
float k_ld = 0.6;						// luz difusa
float k_ls = 1;							//luz specular
float fSpecularPower = 99.8;			// exponente de la luz specular

float4 fogColor = float4(0.11f, 0.245f, 0.29f, 0.6f);
float fogStart = 3000;
float blendStart = 2000;

#define EPSILON 0.05f
float4x4 g_mViewLightProj;
float4x4 g_mProjLight;
float3 g_vLightPos; // posicion de la luz (en World Space) = pto que representa patch emisor Bj
float3 g_vLightDir; // Direcion de la luz (en World Space) = normal al patch Bj

//_________________________________________________________________________________________________________________________
//______________STRUCTS____________________________________________________________________________________________________
//Input del Vertex Shader
struct VS_INPUT
{
	float4 Position : POSITION0;
	float3 Normal : NORMAL0;
	float4 Color :  COLOR0;
	float2 Texcoord : TEXCOORD0;
};

//Output del Vertex Shader
struct VS_OUTPUT
{
	float4 Position : POSITION0;
	float2 Texcoord : TEXCOORD0;
	float3 Norm :     TEXCOORD1;	    // Normales
	float3 Pos :      TEXCOORD2;		// Posicion real 3d
	float3 Pos2 :     TEXCOORD3;		// Posicion en 2d
	float fogfactor : FOG;
    float4 PosLight : TEXCOORD4;
};

//_________________________________________________________________________________________________________________________
//______________VERTEX SHADER______________________________________________________________________________________________

VS_OUTPUT vs_main(VS_INPUT Input)
{
	VS_OUTPUT Output;

	// Calculo la posicion real (en world space)
	float4 pos_real = mul(Input.Position, matWorld);

	// Y la propago usando las coordenadas de texturas 2
	Output.Pos = float3(pos_real.x, pos_real.y, pos_real.z);
	Input.Normal = normalize(Input.Position.xyz);

	Output.Position = mul(Input.Position, matWorldViewProj);
	Output.Pos2 = Output.Position;

	Output.fogfactor = saturate(Output.Position.z);
	Output.Texcoord = Input.Texcoord;
	Output.Norm = normalize(mul(Input.Normal, matWorld));

    Output.PosLight = mul(Output.Pos, g_mViewLightProj);
	return(Output);
}

//_________________________________________________________________________________________________________________________
//______________PIXELS SHADER______________________________________________________________________________________________
float factorSombra(float3 Pos, float4 PosLight)
{
    float3 Light = normalize(float3(Pos - g_vLightPos));
    float cono = dot(Light, g_vLightDir);
    float4 K = 0.0;
    if (cono > 0.7)
    {
		// coordenada de textura CT
        float2 CT = 0.5 * PosLight.xy / PosLight.w + float2(0.5, 0.5);
        CT.y = 1.0f - CT.y;

		// sin ningun aa. conviene con smap size >= 512
        float I = (tex2D(g_samShadow, CT) + EPSILON < PosLight.z / PosLight.w) ? 0.0f : 1.0f;

        if (cono < 0.8)
            I *= 1 - (0.8 - cono) * 10;

        K = I;
    }
    return K;
}

float4 ps_main(float3 Texcoord : TEXCOORD0, float3 N : TEXCOORD1, float3 Pos : TEXCOORD2, float3 Pos2 : TEXCOORD3, float fogfactor : FOG,  float4 PosLight : TEXCOORD4) : COLOR0
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
	float4 fvBaseAlpha = tex2D(alphaSampler, Texcoord);

	//cambio las coordenadas de textura
	float3 newTexcoord = Texcoord;

	//if (Pos2.z < 2000)//si el pixel estaba lejos en world space va sin detalles
	//{ 
		//repito la textura varias veces (depende de _zoom)
		float _zoom = 45.0f;
		newTexcoord *= _zoom;
		newTexcoord.x += step(1., step(newTexcoord.x, 1.0)) * 0.5;
		newTexcoord = frac(newTexcoord);
	//}

	float4 mainColor = tex2D(mainMap, newTexcoord);

	// calcula la luz diffusa
	float3 LD = normalize(fvLightPosition - float3(Pos.x,Pos.y,Pos.z));
	ld += saturate(dot(N, LD))*k_ld;

	// calcula la reflexion specular
	float3 D = normalize(float3(Pos.x,Pos.y,Pos.z) - fvEyePosition);
	float ks = saturate(dot(reflect(LD,N), D));
	ks = pow(ks,fSpecularPower);
	le += ks * k_ls;	

	fvBaseAlpha = fvBaseAlpha - 0.4;
	fvBaseColor = (fvBaseColor * (1.0 - fvBaseAlpha)) + (mainColor * fvBaseAlpha);
	fvBaseColor = (fvBaseColor * fogfactor) + (fogColor * (1.0 - fogfactor));

	RGBColor.rgb = saturate(fvBaseColor * (saturate(k_la + ld)) + le);
	RGBColor.a = blendfactor;

    float K = factorSombra(Pos, PosLight);
    RGBColor.rgb *= 0.5 + 0.5 * K;
	return RGBColor;
}
float4 ps_noche(float2 Texcoord : TEXCOORD0, float3 N : TEXCOORD1, float3 Pos : TEXCOORD2, float3 Pos2 : TEXCOORD3, float fogfactor : FOG, float4 PosLight : TEXCOORD4) : COLOR0
{
    float4 RGBColor = fogColor;
	//para calcular la iluminacion del pixel
    float ld = 0; // luz difusa
    float le = 0; // luz specular

	//calcular los factores de fog y alpha blending que actuan en profundidad
    fogfactor = saturate((5000.0f - Pos2.z) / (fogStart)); // (fogEnd - z) /(fogEnd - fogStart)
    float blendfactor = saturate((5000.0f - Pos2.z) / (blendStart));

	//Obtener el texel de textura
    float4 fvBaseColor = tex2D(diffuseMap, Texcoord);
    float4 fvBaseAlpha = tex2D(alphaSampler, Texcoord);

	//cambio las coordenadas de textura
    float2 newTexcoord = Texcoord;
    float _zoom = 45.0f;
    newTexcoord *= _zoom;
    newTexcoord.x += step(1., step(newTexcoord.x, 1.0)) * 0.5;
    newTexcoord = frac(newTexcoord);

    float4 mainColor = tex2D(mainMap, newTexcoord);

	// calcula la luz diffusa
    float3 LD = normalize(fvLightPosition - float3(Pos.x, Pos.y, Pos.z));
    ld += saturate(dot(N, LD)) * k_ld;

	// calcula la reflexion specular
    float3 D = normalize(float3(Pos.x, Pos.y, Pos.z) - fvEyePosition);
    float ks = saturate(dot(reflect(LD, N), D));
    ks = pow(ks, fSpecularPower);
    le += ks * k_ls;

    fvBaseAlpha = fvBaseAlpha - 0.4;
    fvBaseColor = (fvBaseColor * (1.0 - fvBaseAlpha)) + (mainColor * fvBaseAlpha);
    fvBaseColor = (fvBaseColor * fogfactor) + (fogColor * (1.0 - fogfactor));
    RGBColor *= 0.1;
    RGBColor.rgb = saturate(fvBaseColor * (saturate(k_la + ld)) + le);
    RGBColor.a = blendfactor;

    float K = factorSombra(Pos, PosLight);
    RGBColor.rgb *= 0.5 + 0.5 * K;
    return RGBColor;
}

float4 ps_apocalipsis(float3 Texcoord : TEXCOORD0, float3 N : TEXCOORD1, float3 Pos : TEXCOORD2, float3 Pos2 : TEXCOORD3, float fogfactor : FOG, float4 PosLight : TEXCOORD4) : COLOR0
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
	float4 fvBaseAlpha = tex2D(alphaSampler, Texcoord);

	//cambio las coordenadas de textura
	float3 newTexcoord = Texcoord;

	//if (Pos2.z < 2000)//si el pixel estaba lejos en world space va sin detalles
	//{ 
		//repito la textura varias veces (depende de _zoom)
		float _zoom = 45.0f;
		newTexcoord *= _zoom;
		newTexcoord.x += step(1., step(newTexcoord.x, 1.0)) * 0.5;
		newTexcoord = frac(newTexcoord);
		//}

		float4 mainColor = tex2D(mainMap, newTexcoord);

		// calcula la luz diffusa
		float3 LD = normalize(fvLightPosition - float3(Pos.x,Pos.y,Pos.z));
		ld += saturate(dot(N, LD))*k_ld;

		// calcula la reflexion specular
		float3 D = normalize(float3(Pos.x,Pos.y,Pos.z) - fvEyePosition);
		float ks = saturate(dot(reflect(LD,N), D));
		ks = pow(ks,fSpecularPower);
		le += ks * k_ls;

		fvBaseAlpha = fvBaseAlpha - 0.4;
		fvBaseColor = (fvBaseColor * (1.0 - fvBaseAlpha)) + (mainColor * fvBaseAlpha);
		fvBaseColor = (fvBaseColor * fogfactor) + (fogColor * (1.0 - fogfactor));

		RGBColor.r = saturate(fvBaseColor.r * 2.0);
		RGBColor.g = saturate(fvBaseColor.g * 0.1);
		RGBColor.b = saturate(fvBaseColor.b * 0.3);


		RGBColor.rgb = saturate(RGBColor * (saturate(k_la + ld)) + le);
		RGBColor.a = blendfactor;

        float K = factorSombra(Pos, PosLight);
        RGBColor.rgb *= 0.5 + 0.5 * K;

		return RGBColor;
}

float4 ps_dark(float2 Texcoord : TEXCOORD0) : COLOR0
{
    float4 fvBaseColor = tex2D(diffuseMap, Texcoord);
    return float4(0, 0, 0, 1);//fvBaseColor.a);
}
float4 ps_helado(float2 Texcoord : TEXCOORD0, float3 N : TEXCOORD1, float3 Pos : TEXCOORD2, float3 Pos2 : TEXCOORD3, float fogfactor : FOG, float4 PosLight : TEXCOORD4) : COLOR0
{
    float4 RGBColor = fogColor;
	//para calcular la iluminacion del pixel
    float ld = 0; // luz difusa
    float le = 0; // luz specular

	//calcular los factores de fog y alpha blending que actuan en profundidad
    fogfactor = saturate((5000.0f - Pos2.z) / (fogStart)); // (fogEnd - z) /(fogEnd - fogStart)
    float blendfactor = saturate((5000.0f - Pos2.z) / (blendStart));

	//Obtener el texel de textura
    float4 fvBaseColor = tex2D(diffuseMap, Texcoord);
    float4 fvBaseAlpha = tex2D(alphaSampler, Texcoord);

	//cambio las coordenadas de textura
    float2 newTexcoord = Texcoord;

	//if (Pos2.z < 2000)//si el pixel estaba lejos en world space va sin detalles
	//{ 
		//repito la textura varias veces (depende de _zoom)
    float _zoom = 45.0f;
    newTexcoord *= _zoom;
    newTexcoord.x += step(1., step(newTexcoord.x, 1.0)) * 0.5;
    newTexcoord = frac(newTexcoord);
	//}

    float4 mainColor = tex2D(mainMap, newTexcoord);

	// calcula la luz diffusa
    float3 LD = normalize(fvLightPosition - float3(Pos.x, Pos.y, Pos.z));
    ld += saturate(dot(N, LD)) * k_ld;

	// calcula la reflexion specular
    float3 D = normalize(float3(Pos.x, Pos.y, Pos.z) - fvEyePosition);
    float ks = saturate(dot(reflect(LD, N), D));
    ks = pow(ks, fSpecularPower);
    le += ks * k_ls;

    fvBaseAlpha = fvBaseAlpha - 0.4;
    fvBaseColor = (fvBaseColor * (1.0 - fvBaseAlpha)) + (mainColor * fvBaseAlpha);
    fvBaseColor = (fvBaseColor * fogfactor) + (fogColor * (1.0 - fogfactor));

    RGBColor.rgb = saturate(fvBaseColor * (saturate(k_la + ld)) + le);
    RGBColor *= 6;
    RGBColor.b *= 16;
    RGBColor.a = blendfactor;

    float K = factorSombra(Pos, PosLight);
    RGBColor.rgb *= 0.5 + 0.5 * K;
    return RGBColor;
}

//____________________________________________________________________________________________________________________________________________________________________
//______________TECNICAS______________________________________________________________________________________________________________________________________________

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

technique helado
{
    pass Pass_0
    {
        AlphaBlendEnable = TRUE;
        DestBlend = INVSRCALPHA;
        SrcBlend = SRCALPHA;
        VertexShader = compile vs_3_0 vs_main();
        PixelShader = compile ps_3_0 ps_helado();
    }
}

technique apocalipsis
{
	pass Pass_0
	{
		AlphaBlendEnable = TRUE;
		DestBlend = INVSRCALPHA;
		SrcBlend = SRCALPHA;
		VertexShader = compile vs_3_0 vs_main();
		PixelShader = compile ps_3_0 ps_apocalipsis();
	}
}

technique noche
{
    pass Pass_0
    {
        AlphaBlendEnable = TRUE;
        DestBlend = INVSRCALPHA;
        SrcBlend = SRCALPHA;
        VertexShader = compile vs_3_0 vs_main();
        PixelShader = compile ps_3_0 ps_noche();
    }
}

technique dark
{
   pass Pass_0
   {
          AlphaBlendEnable =TRUE;
          DestBlend= INVSRCALPHA;
          SrcBlend= SRCALPHA;
		  VertexShader = compile vs_3_0 vs_main();
		  PixelShader = compile ps_3_0 ps_dark(); 
   }
}

//------------------------------------------------------------------------------------------------------------------------------------------
//para shadow map
//------------------------------------------------------------------------------------------------------------------------------------------
void VertShadow(float4 Pos : POSITION, float3 Normal : NORMAL, out float4 oPos : POSITION, out float2 Depth : TEXCOORD0)
{
	// transformacion estandard
    oPos = mul(Pos, matWorld); // uso el del mesh
    oPos = mul(oPos, g_mViewLightProj); // pero visto desde la pos. de la luz

	// devuelvo: profundidad = z/w
    Depth.xy = oPos.zw;
}

//-----------------------------------------------------------------------------
// Pixel Shader para el shadow map, dibuja la "profundidad"
//-----------------------------------------------------------------------------
void PixShadow(float2 Depth : TEXCOORD0, out float4 Color : COLOR)
{
    Color = Depth.x / Depth.y;
}

technique RenderShadow
{
    pass p0
    {
        VertexShader = compile vs_3_0 VertShadow();
        PixelShader = compile ps_3_0 PixShadow();
    }
}