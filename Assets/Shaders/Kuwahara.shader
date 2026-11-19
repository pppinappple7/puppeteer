Shader "Custom/URP/Kuwahara"
{
    Properties
    {
        _Radius ("Filter Radius", Range(1, 8)) = 4
        _Intensity ("Effect Intensity", Range(0, 1)) = 1
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
        }

        ZWrite Off
        ZTest Always
        Cull Off

        Pass
        {
            Name "Kuwahara"
            
            Stencil
            {
                Ref 1
                Comp NotEqual
                Pass Keep
            }
            
            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag
            #pragma target 4.5

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float _Radius;
                float _Intensity;
            CBUFFER_END
            
            void EvaluateRegion(
                float2 uv,
                int radius,
                int left,
                int top,
                out float3 mean,
                out float variance
            )
            {
                float3 sum = 0;
                float3 sumSquares = 0;

                float count = 0;

                for (int y = -8; y <= 8; y++)
                {
                    if (abs(y) > radius)
                        continue;

                    if (top == 1 && y > 0)
                        continue;

                    if (top == 0 && y < 0)
                        continue;

                    for (int x = -8; x <= 8; x++)
                    {
                        if (abs(x) > radius)
                            continue;

                        if (left == 1 && x > 0)
                            continue;

                        if (left == 0 && x < 0)
                            continue;

                        float2 offset =
                            float2(x, y) * _BlitTexture_TexelSize.xy;

                        float3 color = SAMPLE_TEXTURE2D_X(
                            _BlitTexture,
                            sampler_LinearClamp,
                            uv + offset
                        ).rgb;

                        sum += color;
                        sumSquares += color * color;

                        count += 1.0;
                    }
                }

                mean = sum / count;

                float3 varianceRGB =
                    abs(sumSquares / count - mean * mean);

                variance =
                    varianceRGB.r +
                    varianceRGB.g +
                    varianceRGB.b;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                float2 uv = input.texcoord;

                int radius = (int)round(_Radius);

                float3 means[4];
                float variances[4];
                
                EvaluateRegion(
                    uv, radius, 1, 1,
                    means[0], variances[0]
                );
                
                EvaluateRegion(
                    uv, radius, 0, 1,
                    means[1], variances[1]
                );
                
                EvaluateRegion(
                    uv, radius, 1, 0,
                    means[2], variances[2]
                );

                EvaluateRegion(
                    uv, radius, 0, 0,
                    means[3], variances[3]
                );
                
                float minVariance = variances[0];
                float3 result = means[0];

                [unroll]
                for (int i = 1; i < 4; i++)
                {
                    if (variances[i] < minVariance)
                    {
                        minVariance = variances[i];
                        result = means[i];
                    }
                }

                float4 original = SAMPLE_TEXTURE2D_X(
                    _BlitTexture,
                    sampler_LinearClamp,
                    uv
                );
                
                float3 finalColor = lerp(
                    original.rgb,
                    result,
                    _Intensity
                );

                return float4(finalColor, original.a);
            }

            ENDHLSL
        }
    }
}