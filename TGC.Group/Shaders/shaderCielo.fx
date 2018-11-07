
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

float time = 1.0;

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
};

// ------------------------------------------------------------------

// vertex shader  
VS_OUTPUT vs_main( VS_INPUT Input )
{
	VS_OUTPUT Output;
	Output.Texcoord = Input.Texcoord;
	Output.Position = mul( Input.Position, matWorldViewProj);
	return( Output );  
}

// ------------------------------------------------------------------

//Pixel Shader 
float4 ps_main( float2 Texcoord: TEXCOORD0) : COLOR0
{      
    float4 fvBaseColor = tex2D( diffuseMap, Texcoord);
	//fvBaseColor.g = fvBaseColor.g  +cos(time) / 8;
	//fvBaseColor.b = fvBaseColor.b + cos(time) / 16;

    fvBaseColor.rgb = saturate(fvBaseColor);
    return fvBaseColor;
}

//Pixel Shader 
float4 ps_tarde( float2 Texcoord: TEXCOORD0) : COLOR0
{      
    float4 fvBaseColor = tex2D( diffuseMap, Texcoord);
	//fvBaseColor.r = saturate(fvBaseColor.r *2.0);
	fvBaseColor.g = saturate(fvBaseColor.g *2.2);
    fvBaseColor.b = saturate(fvBaseColor.b * 3); // 2);
    fvBaseColor.rgb = saturate(fvBaseColor);

    return fvBaseColor;
}
float4 ps_Apocalipsis(float2 Texcoord: TEXCOORD0) : COLOR0
{
	float4 fvBaseColor = tex2D(diffuseMap, Texcoord);
	float4 rgbColor = fvBaseColor;

	fvBaseColor.r = rgbColor.b;
	fvBaseColor.g = rgbColor.g;
	fvBaseColor.b = rgbColor.r;

	//fvBaseColor.r = saturate(fvBaseColor.r * 4.0);
	//fvBaseColor.g = saturate(fvBaseColor.g * 0.3);
	//fvBaseColor.b = saturate(fvBaseColor.b * 0.1);
	fvBaseColor.rgb = saturate(fvBaseColor);

	return fvBaseColor;
}


float4 ps_dark(float2 Texcoord : TEXCOORD0) : COLOR0
{
    float4 fvBaseColor = tex2D(diffuseMap, Texcoord);
    return float4(0, 0, 0, 1);//fvBaseColor.a);
}

float4 ps_noche(float2 Texcoord : TEXCOORD0) : COLOR0
{
    float4 fvBaseColor = tex2D(diffuseMap, Texcoord);
    fvBaseColor *= 0.2;
    return float4(fvBaseColor.x, fvBaseColor.y, fvBaseColor.z, 1); //fvBaseColor.a);
}
// ------------------------------------------------------------------
technique RenderScene
{
   pass Pass_0
   {
		AlphaBlendEnable =TRUE;
		DestBlend= INVSRCALPHA;
		SrcBlend= SRCALPHA;
		VertexShader = compile vs_2_0 vs_main();
		PixelShader = compile ps_2_0 ps_main();
   }
}

technique helado
{
   pass Pass_0
   {
		AlphaBlendEnable =TRUE;
		DestBlend= INVSRCALPHA;
		SrcBlend= SRCALPHA;
		VertexShader = compile vs_2_0 vs_main();
		PixelShader = compile ps_2_0 ps_tarde();
   }
}

technique apocalipsis
{
	pass Pass_0
	{
		AlphaBlendEnable = TRUE;
		DestBlend = INVSRCALPHA;
		SrcBlend = SRCALPHA;
		VertexShader = compile vs_2_0 vs_main();
		PixelShader = compile ps_2_0 ps_Apocalipsis();
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