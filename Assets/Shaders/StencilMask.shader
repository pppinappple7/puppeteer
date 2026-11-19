Shader "Custom/StencilMask"
{
    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
            "Queue" = "Geometry"
        }

        Pass
        {
            Name "StencilMask"
            Tags
            {
                "LightMode" = "UniversalForward"
            }

            ColorMask 0
            ZWrite Off
            ZTest LEqual
            Cull Back

            Stencil
            {
                Ref 1
                Comp Always
                Pass Replace
            }

            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;

                output.positionCS = TransformObjectToHClip(
                    input.positionOS.xyz
                );

                return output;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                return 0;
            }

            ENDHLSL
        }
    }
}
