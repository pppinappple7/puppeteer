
Shader "Custom/SpriteChunkJitter"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)

        _Amplitude ("Shift Strength", Range(0, 0.05)) = 0.008
        _Frequency ("Update Frequency", Range(1, 30)) = 12
        _ChunkSize ("Chunk Size", Range(2, 100)) = 20
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "RenderPipeline"="UniversalPipeline"
            "CanUseSpriteAtlas"="True"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _Color;
                float _Amplitude;
                float _Frequency;
                float _ChunkSize;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            float Hash(float2 p)
            {
                return frac(sin(dot(p, float2(127.1, 311.7)))
                    * 43758.5453);
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.color = IN.color * _Color;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float frame = floor(_Time.y * _Frequency);

                
                float2 chunk = floor(IN.uv * _ChunkSize);

                
                float2 randomOffset;
                randomOffset.x = Hash(chunk + frame) * 2.0 - 1.0;
                randomOffset.y = Hash(chunk + frame + 123.4) * 2.0 - 1.0;

                
                float2 uv = IN.uv + randomOffset * _Amplitude;

                half4 col = SAMPLE_TEXTURE2D(
                    _MainTex, sampler_MainTex, uv
                );

                return col * IN.color;
            }
            ENDHLSL
        }
    }
}
