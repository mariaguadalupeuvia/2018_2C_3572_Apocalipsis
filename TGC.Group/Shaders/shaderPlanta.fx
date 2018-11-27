
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

texture texCubeMap;
samplerCUBE cubeMap = sampler_state
{
	Texture = (texCubeMap);
	ADDRESSU = WRAP;
	ADDRESSV = WRAP;
	MINFILTER = LINEAR;
	MAGFILTER = LINEAR;
	MIPFILTER = LINEAR;
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

texture NormalMap;
sampler2D bumpSampler = sampler_state 
{
	Texture = (NormalMap);
	MinFilter = Linear;
	MagFilter = Linear;
	AddressU = Wrap;
	AddressV = Wrap;
};

//________________________________________________________________________________________________________
//________________VARIABLES_______________________________________________________________________________

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
float alturaEnY = 0;

//variables para la iluminacion
float3 fvLightPosition = float3(100.00, 10.00, 1000.00);
float3 fvEyePosition = float3(0.00, -1000.00, 0.00);
float k_la = 0.3;							// luz ambiente global
float k_ld = 1;						// luz difusa
float k_ls = 1;//65;							// luz specular
float fSpecularPower = 6.84;				// exponente de la luz specular
float4 fogColor = float4(0.11f, 0.245f, 0.29f, 0.6f);
float fogStart = 3000;
float blendStart = 2000;

float reflection = 0.4;
float delta = 150.0;

float scaleFactor = 0.2;
float colorVida = 0;

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

    float4 PosLight : TEXCOORD8;
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

    Output.PosLight = mul(Output.Pos, g_mViewLightProj);
	return(Output);
}

VS_OUTPUT vs_explosivo(VS_INPUT Input)
{
	VS_OUTPUT Output;

	//para mover el vertice como latidos
	//float displacement = _nivelExpansion;
    float displacement = scaleFactor * sin( _Time * 10);
	float4 displacementDirection = float4(Input.Normal.x, Input.Normal.y, Input.Normal.z, 0);
	float4 newPosition = Input.Position + displacement * displacementDirection; //para engordar un mesh
	
	Input.Position = newPosition;

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

    Output.PosLight = mul(Output.Pos, g_mViewLightProj);
	return(Output);
}

float movimientoY = 1;
VS_OUTPUT vs_explosivo2(VS_INPUT Input)
{
    VS_OUTPUT Output;

    float displacement = 10 *_Time; 
    float4 displacementDirection = float4(Input.Normal.x, Input.Normal.y, Input.Normal.z, 0);
    
    Input.Position.y += movimientoY;
    float4 newPosition = Input.Position + displacement * displacementDirection; 
    Input.Position = newPosition;

	// Calculo la posicion real (en world space)
    float4 pos_real = mul(newPosition, matWorld);

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

    Output.PosLight = mul(Output.Pos, g_mViewLightProj);
    return (Output);
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

float4 ps_cube(float2 Texcoord : TEXCOORD0, float3 N : TEXCOORD1, float3 Pos : TEXCOORD2, float3 Pos2 : TEXCOORD3, float3 WorldPosition : TEXCOORD4, float3 WorldNormal : TEXCOORD5, float3 Tangent : TEXCOORD6, float3 Binormal : TEXCOORD7, float fogfactor : FOG, float4 PosLight : TEXCOORD8) : COLOR0
{
   //Obtener el texel de textura
	float4 fvBaseColor = tex2D(diffuseMap, Texcoord);

	float ld = 0;		// luz difusa
	float le = 0;		// luz specular

	//calcula la luz diffusa
	float3 LD = normalize(fvLightPosition - float3(Pos.x,Pos.y,Pos.z));
	ld += saturate(dot(N, LD)) * k_ld;

	//calcula la reflexion specular
	float3 D = normalize(float3(Pos.x,Pos.y,Pos.z) - fvEyePosition);
	float ks = saturate(dot(reflect(LD,N), D));
	ks = pow(ks,fSpecularPower);
	le += ks * k_ls;

	//calcular los factores de fog y alpha blending que actuan en profundidad
	fogfactor = saturate((5000.0f - Pos2.z) / (fogStart)); 
	float blendfactor = saturate((5000.0f - Pos2.z) / (blendStart));
    fvBaseColor = (fvBaseColor * fogfactor) + (fogColor * (1.0 - fogfactor));


    //Normalizar vectores
    float3 Nn = normalize(WorldNormal);
	//Obtener texel de CubeMap
    float3 Vn = normalize(fvEyePosition - WorldPosition);
    float3 R = reflect(Vn, Nn);

    float3 reflectionColor = texCUBE(cubeMap, R).rgb;
    float4 MezclaTex = float4((fvBaseColor.xyz * (1 - reflection)) + (reflectionColor * reflection), 1.0f);
    
    float4 RGBColor = 0;
    RGBColor.rgb = saturate(MezclaTex * (saturate(k_la + ld)) + le);
    RGBColor.a = 1;
       
    float K = factorSombra(Pos, PosLight);
    RGBColor.rgb *= 0.5 + 0.5 * K;
    return RGBColor;
}

float4 ps_main(float3 Texcoord : TEXCOORD0, float3 N : TEXCOORD1, float3 Pos : TEXCOORD2, float3 Pos2 : TEXCOORD3, float3 WorldPosition : TEXCOORD4, float3 WorldNormal : TEXCOORD5, float3 Tangent : TEXCOORD6, float3 Binormal : TEXCOORD7, float fogfactor : FOG, float4 PosLight : TEXCOORD8) : COLOR0
{
   //Obtener el texel de textura
	float4 fvBaseColor = tex2D(diffuseMap, Texcoord);

	float ld = 0;		// luz difusa
	float le = 0;		// luz specular

	//calcula la luz diffusa
	float3 LD = normalize(fvLightPosition - float3(Pos.x,Pos.y,Pos.z));
	ld += saturate(dot(N, LD))*k_ld;

	//calcula la reflexion specular
	float3 D = normalize(float3(Pos.x,Pos.y,Pos.z) - fvEyePosition);
	float ks = saturate(dot(reflect(LD,N), D));
	ks = pow(ks,fSpecularPower);
	le += ks * k_ls;

	//calcular los factores de fog y alpha blending que actuan en profundidad
	fogfactor = saturate((5000.0f - Pos2.z) / (fogStart)); // (fogEnd - z) /(fogEnd - fogStart)
	float blendfactor = saturate((5000.0f - Pos2.z) / (blendStart));

	float4 RGBColor = 0;
	fvBaseColor = (fvBaseColor *  fogfactor) + (fogColor * (1.0 - fogfactor));
	RGBColor.rgb = saturate(fvBaseColor*(saturate(k_la + ld)) + le);
	RGBColor.a = blendfactor;

    float K = factorSombra(Pos, PosLight);
    RGBColor.rgb *= 0.5 + 0.5 * K;
    return RGBColor;
}

float4 ps_calado(float3 Texcoord : TEXCOORD0, float3 N : TEXCOORD1, float3 Pos : TEXCOORD2, float3 Pos2 : TEXCOORD3, float3 WorldPosition : TEXCOORD4, float3 WorldNormal : TEXCOORD5, float3 Tangent : TEXCOORD6, float3 Binormal : TEXCOORD7, float fogfactor : FOG, float4 PosLight : TEXCOORD8) : COLOR0
{
    //Texcoord.y = Texcoord.y + _Time / 4;
   //Obtener el texel de textura
    float4 fvBaseColor;

    if ((_Time % 2) == 0)
    {
        fvBaseColor = tex2D(bumpSampler, Texcoord);
       
    }
    else
    {
        fvBaseColor = tex2D(diffuseMap, Texcoord);
    }

    float ld = 0; // luz difusa
    float le = 0; // luz specular

	//calcula la luz diffusa
    float3 LD = normalize(fvLightPosition - float3(Pos.x, Pos.y, Pos.z));
    ld += saturate(dot(N, LD)) * k_ld;

	//calcula la reflexion specular
    float3 D = normalize(float3(Pos.x, Pos.y, Pos.z) - fvEyePosition);
    float ks = saturate(dot(reflect(LD, N), D));
    ks = pow(ks, fSpecularPower);
    le += ks * k_ls;

	//calcular los factores de fog y alpha blending que actuan en profundidad
    fogfactor = saturate((5000.0f - Pos2.z) / (fogStart)); // (fogEnd - z) /(fogEnd - fogStart)
    float blendfactor = saturate((5000.0f - Pos2.z) / (blendStart));

    float4 RGBColor = 0;
    fvBaseColor = (fvBaseColor * fogfactor) + (fogColor * (1.0 - fogfactor));
    RGBColor.rgb = saturate(fvBaseColor * (saturate(k_la + ld)));//+le);
    RGBColor.rgb *= 2;
    RGBColor.a = (RGBColor.r + RGBColor.g + RGBColor.b) / 3; //blendfactor;

    float K = factorSombra(Pos, PosLight);
    RGBColor.rgb *= 0.5 + 0.5 * K;
    return RGBColor;
}
float4 ps_calado2(float2 Texcoord : TEXCOORD0, float3 N : TEXCOORD1, float3 Pos : TEXCOORD2, float3 Pos2 : TEXCOORD3, float3 WorldPosition : TEXCOORD4, float3 WorldNormal : TEXCOORD5, float3 Tangent : TEXCOORD6, float3 Binormal : TEXCOORD7, float fogfactor : FOG , float4 PosLight : TEXCOORD8) : COLOR0
{
    float4 fvBaseColor = tex2D(diffuseMap, Texcoord);

    float ld = 0; // luz difusa
    float le = 0; // luz specular

	//calcula la luz diffusa
    float3 LD = normalize(fvLightPosition - float3(Pos.x, Pos.y, Pos.z));
    ld += saturate(dot(N, LD)) * k_ld;

	//calcula la reflexion specular
    float3 D = normalize(float3(Pos.x, Pos.y, Pos.z) - fvEyePosition);
    float ks = saturate(dot(reflect(LD, N), D));
    ks = pow(ks, fSpecularPower);
    le += ks * k_ls;

	//calcular los factores de fog y alpha blending que actuan en profundidad
    fogfactor = saturate((5000.0f - Pos2.z) / (fogStart)); // (fogEnd - z) /(fogEnd - fogStart)

    float4 RGBColor = 0;
    fvBaseColor = (fvBaseColor * fogfactor) + (fogColor * (1.0 - fogfactor));
    RGBColor.rgb = saturate(fvBaseColor * (saturate(k_la + ld))); 
    RGBColor.rgb *= 2;
    float alfa = (RGBColor.r + RGBColor.g + RGBColor.b) / 3;
    if (alfa < 0.2)
    {
        discard;
    }
    else
    {
        RGBColor.a =  1.0; 
    }

    float K = factorSombra(Pos, PosLight);
    RGBColor.rgb *= 0.5 + 0.5 * K;
    return RGBColor;
}

float4 ps_progresivo(float3 Texcoord : TEXCOORD0, float3 N : TEXCOORD1, float3 Pos : TEXCOORD2, float3 Pos2 : TEXCOORD3, float3 WorldPosition : TEXCOORD4, float3 WorldNormal : TEXCOORD5, float3 Tangent : TEXCOORD6, float3 Binormal : TEXCOORD7, float fogfactor : FOG, float4 PosLight : TEXCOORD8) : COLOR0
{
	float ld = 0;		// luz difusa
	float le = 0;		// luz specular

	//calcula la luz diffusa
	float3 LD = normalize(fvLightPosition - float3(Pos.x,Pos.y,Pos.z));
	ld += saturate(dot(N, LD))*k_ld;

	//calcula la reflexion specular
	float3 D = normalize(float3(Pos.x,Pos.y,Pos.z) - fvEyePosition);
	float ks = saturate(dot(reflect(LD,N), D));
	ks = pow(ks,fSpecularPower);
	le += ks * k_ls;

	//calcular los factores de fog y alpha blending que actuan en profundidad
	fogfactor = saturate((5000.0f - Pos2.z) / (fogStart)); // (fogEnd - z) /(fogEnd - fogStart)
	float blendfactor = saturate((5000.0f - Pos2.z) / (blendStart));

	float4 RGBColor = 0;

	if ((Pos.y > 290) && (Pos.y < 590))
	{
		Texcoord.x += Texcoord.x + _Time * 5;
		float4 fvBaseColor = tex2D(diffuseMap, Texcoord);
		fvBaseColor = (fvBaseColor *  fogfactor) + (fogColor * (1.0 - fogfactor));
		RGBColor.rgb = saturate(fvBaseColor*(saturate(k_la + ld)) + le);

		if (Pos.y > (290 + alturaEnY))
		{
			RGBColor.r = RGBColor.g;
			RGBColor.g = RGBColor.b;
		}

		RGBColor.a = 0.5;
	}
	else
	{
		float4 fvBaseColor = tex2D(diffuseMap, Texcoord);
		fvBaseColor = (fvBaseColor *  fogfactor) + (fogColor * (1.0 - fogfactor));
		RGBColor.rgb = saturate(fvBaseColor*(saturate(k_la + ld)) + le);
		RGBColor.a = blendfactor;
	}
 
    float K = factorSombra(Pos, PosLight);
    RGBColor.rgb *= 0.5 + 0.5 * K;
    return RGBColor;
}

float4 ps_hielo(float3 Texcoord : TEXCOORD0, float3 N : TEXCOORD1, float3 Pos : TEXCOORD2, float3 Pos2 : TEXCOORD3, float3 WorldPosition : TEXCOORD4, float3 WorldNormal : TEXCOORD5, float3 Tangent : TEXCOORD6, float3 Binormal : TEXCOORD7, float fogfactor : FOG, float4 PosLight : TEXCOORD8) : COLOR0
{
	//Obtener el texel de textura
	float4 fvBaseColor = tex2D(diffuseMap, Texcoord);

	float ld = 0;		// luz difusa
	float le = 0;		// luz specular

	//calcula la luz diffusa
	float3 LD = normalize(fvLightPosition - float3(Pos.x,Pos.y,Pos.z));
	ld += saturate(dot(N, LD))*k_ld;

	//calcula la reflexion specular
	float3 D = normalize(float3(Pos.x,Pos.y,Pos.z) - fvEyePosition);
	float ks = saturate(dot(reflect(LD,N), D));
	ks = pow(ks,fSpecularPower);
	le += ks * k_ls;

	//calcular los factores de fog y alpha blending que actuan en profundidad
	fogfactor = saturate((5000.0f - Pos2.z) / (fogStart)); // (fogEnd - z) /(fogEnd - fogStart)
	float blendfactor = saturate((5000.0f - Pos2.z) / (blendStart));

	float4 RGBColor = 0;
	fvBaseColor = (fvBaseColor *  fogfactor) + (fogColor * (1.0 - fogfactor));
	RGBColor.rgb = saturate(fvBaseColor*(saturate(k_la + ld)) + le);
	RGBColor.a = blendfactor;
	RGBColor.b = RGBColor.g * 1.8;
	RGBColor.g = RGBColor.r;

    float K = factorSombra(Pos, PosLight);
    RGBColor.rgb *= 0.5 + 0.5 * K;
    return RGBColor;
}

float4 ps_transparente(float3 Texcoord : TEXCOORD0, float3 N : TEXCOORD1, float3 Pos : TEXCOORD2, float3 Pos2 : TEXCOORD3, float3 WorldPosition : TEXCOORD4, float3 WorldNormal : TEXCOORD5, float3 Tangent : TEXCOORD6, float3 Binormal : TEXCOORD7, float fogfactor : FOG, float4 PosLight : TEXCOORD8) : COLOR0
{
	//Obtener el texel de textura
	float4 fvBaseColor = tex2D(diffuseMap, Texcoord);

	float ld = 0;		// luz difusa
	float le = 0;		// luz specular

	//calcula la luz diffusa
	float3 LD = normalize(fvLightPosition - float3(Pos.x,Pos.y,Pos.z));
	ld += saturate(dot(N, LD))*k_ld;

	//calcula la reflexion specular
	float3 D = normalize(float3(Pos.x,Pos.y,Pos.z) - fvEyePosition);
	float ks = saturate(dot(reflect(LD,N), D));
	ks = pow(ks,fSpecularPower);
	le += ks * k_ls;

	//calcular los factores de fog y alpha blending que actuan en profundidad
	fogfactor = saturate((5000.0f - Pos2.z) / (fogStart)); // (fogEnd - z) /(fogEnd - fogStart)
	float blendfactor = saturate((5000.0f - Pos2.z) / (blendStart));

	float4 RGBColor = 0;
	fvBaseColor = (fvBaseColor *  fogfactor) + (fogColor * (1.0 - fogfactor));
	RGBColor.rgb = saturate(fvBaseColor*(saturate(k_la + ld)) + le);
	RGBColor.a = 0.2;

    float K = factorSombra(Pos, PosLight);
    RGBColor.rgb *= 0.5 + 0.5 * K;
    return RGBColor;
}

float4 ps_zombie(float3 Texcoord : TEXCOORD0, float3 N : TEXCOORD1, float3 Pos : TEXCOORD2, float3 Pos2 : TEXCOORD3, float3 WorldPosition : TEXCOORD4, float3 WorldNormal : TEXCOORD5, float3 Tangent : TEXCOORD6, float3 Binormal : TEXCOORD7, float fogfactor : FOG, float4 PosLight : TEXCOORD8) : COLOR0
{
	//Obtener el texel de textura
	 float4 fvBaseColor = tex2D(diffuseMap, Texcoord);

	 float ld = 0;		// luz difusa
	 float le = 0;		// luz specular

	 //calcula la luz diffusa
	 float3 LD = normalize(fvLightPosition - float3(Pos.x,Pos.y,Pos.z));
	 ld += saturate(dot(N, LD))*k_ld;

	 //calcula la reflexion specular
	 float3 D = normalize(float3(Pos.x,Pos.y,Pos.z) - fvEyePosition);
	 float ks = saturate(dot(reflect(LD,N), D));
	 ks = pow(ks,fSpecularPower);
	 le += ks * k_ls;

	 //calcular los factores de fog y alpha blending que actuan en profundidad
	 fogfactor = saturate((5000.0f - Pos2.z) / (fogStart)); // (fogEnd - z) /(fogEnd - fogStart)
	 float blendfactor = saturate((5000.0f - Pos2.z) / (blendStart));

	 float4 RGBColor = 0;
	 fvBaseColor = (fvBaseColor *  fogfactor) + (fogColor * (1.0 - fogfactor));
	 RGBColor.rgb = saturate(fvBaseColor*(saturate(k_la + ld)) + le);
	 RGBColor.r += colorVida;
	 //RGBColor.b -= colorVida;
	 RGBColor.a = blendfactor;

    float K = factorSombra(Pos, PosLight);
    RGBColor.rgb *= 0.5 + 0.5 * K;
    return RGBColor;
}

float4 ps_dark(float3 Texcoord : TEXCOORD0) : COLOR0
{
    float4 fvBaseColor = tex2D(diffuseMap, Texcoord);
    return float4(0, 0, 0, 1);//fvBaseColor.a);
}
float4 ps_glow(float3 Texcoord : TEXCOORD0) : COLOR0
{
    float4 fvBaseColor = tex2D(diffuseMap, Texcoord);
    return float4(0, 0, 0, 1);//fvBaseColor.a);
}

//____________________________________________________________________________________________________________________________________________________________________
//______________TECNICAS______________________________________________________________________________________________________________________________________________

technique RenderScene
{
	pass Pass_0
	{
		AlphaBlendEnable = TRUE;
		DestBlend = INVSRCALPHA;
		SrcBlend = SRCALPHA;
		VertexShader = compile vs_3_0 vs_main();
		PixelShader = compile ps_3_0 ps_main();
	}
}

technique RenderSceneProgresivo
{
	pass Pass_0
	{
		AlphaBlendEnable = TRUE;
		DestBlend = INVSRCALPHA;
		SrcBlend = SRCALPHA;
		VertexShader = compile vs_3_0 vs_main();
		PixelShader = compile ps_3_0 ps_progresivo();
	}
}

technique RenderSceneCongelada
{
	pass Pass_0
	{
		AlphaBlendEnable = TRUE;
		DestBlend = INVSRCALPHA;
		SrcBlend = SRCALPHA;
		VertexShader = compile vs_3_0 vs_main();
		PixelShader = compile ps_3_0 ps_hielo();
	}
}

technique Transparente
{
	pass Pass_0
	{
		AlphaBlendEnable = TRUE;
		DestBlend = INVSRCALPHA;
		SrcBlend = SRCALPHA;
		VertexShader = compile vs_3_0 vs_main();
		PixelShader = compile ps_3_0 ps_transparente();
	}
}

technique Explosivo
{
	pass Pass_0
	{
		AlphaBlendEnable = TRUE;
		DestBlend = INVSRCALPHA;
		SrcBlend = SRCALPHA;
		VertexShader = compile vs_3_0 vs_explosivo();
		PixelShader = compile ps_3_0 ps_main();
	}
}
technique Explosivo2
{
    pass Pass_0
    {
        AlphaBlendEnable = TRUE;
        DestBlend = INVSRCALPHA;
        SrcBlend = SRCALPHA;
        VertexShader = compile vs_3_0 vs_explosivo2();
        PixelShader = compile ps_3_0 ps_calado2();
    }
}
technique Explosivo3
{
    pass Pass_0
    {
        AlphaBlendEnable = TRUE;
        DestBlend = INVSRCALPHA;
        SrcBlend = SRCALPHA;
        VertexShader = compile vs_3_0 vs_explosivo2();
        PixelShader = compile ps_3_0 ps_main();
    }
}
technique calado
{
    pass Pass_0
    {
        AlphaBlendEnable = TRUE;
        DestBlend = INVSRCALPHA;
        SrcBlend = SRCALPHA;
        VertexShader = compile vs_3_0 vs_explosivo();
        PixelShader = compile ps_3_0 ps_calado();
    }
}

technique RenderZombie
{
	pass Pass_0
	{
		AlphaBlendEnable = TRUE;
		DestBlend = INVSRCALPHA;
		SrcBlend = SRCALPHA;
		VertexShader = compile vs_3_0 vs_main();
		PixelShader = compile ps_3_0 ps_zombie();
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

technique cube
{
   pass Pass_0
   {
          AlphaBlendEnable =TRUE;
          DestBlend= INVSRCALPHA;
          SrcBlend= SRCALPHA;
		  VertexShader = compile vs_3_0 vs_main();
		  PixelShader = compile ps_3_0 ps_cube(); 
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