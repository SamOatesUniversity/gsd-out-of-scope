float4x4 world_view_projection_xform;
texture Texture;

sampler MeshTextureSampler = 
sampler_state
{
    Texture = <Texture>;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float2 TextureUV  : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 TextureUV  : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    output.Position = mul(input.Position, world_view_projection_xform);
	output.TextureUV = input.TextureUV;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 tex = tex2D( MeshTextureSampler, input.TextureUV );
	clip(tex.z < 0.1f ? -1 : 1 );
    return tex;
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
