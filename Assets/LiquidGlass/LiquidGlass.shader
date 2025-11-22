Shader "Post/LiquidGlassRender"
{
    Properties
    {
        _BlitTexture ("Source Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Overlay" }

        Pass
        {
            Name "LiquidGlassEffectPass"
            ZTest Always Cull Off ZWrite Off
            Blend One Zero
            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

            TEXTURE2D(_BlitTexture);
            SAMPLER(sampler_BlitTexture);

            #define BLUR_KERNEL_RADIUS 4
            #define MAX_SUPPORTED_CONCURRENT_ELEMENTS 128

            TEXTURE2D(_RadiusTexture);
            SAMPLER(sampler_RadiusTexture);

            float4 _ScreenParams;

            struct LiquidGlassElement
            {
                float4 rect;
                float radius;
                float4 tint;
                float blur;
            };

            StructuredBuffer<LiquidGlassElement> _LiquidGlassElements;

            // Returns the index of the position relative to the rectangle defined by the LiquidGlassElement.
            // 0 - completely inside the base rectangle
            // 0-1 - between the base rectangle and the shape's edge (interpolated parabolically)
            // 1 - outside the shape entirely
            float get_position_index(LiquidGlassElement liquid_glass_element, float2 position)
            {
                float4 rect = liquid_glass_element.rect;
                float radius = liquid_glass_element.radius;
                if (all(bool4(position >= rect.xy, position <= rect.zw))) return 0.0;
                float hrz = saturate(max(rect.x - position.x, position.x - rect.z) / radius);
                float vrt = saturate(max(rect.y - position.y, position.y - rect.w) / radius);
                float square_offset = hrz * hrz + vrt * vrt;
                float position_index = 1 - sqrt(saturate(1 - square_offset));
                return position_index;
            }

            // Returns the offset applied to a position to simulate an appropriate distortion effect associated
            // with a provided LiquidGlassElement.
            float2 get_position_offset(LiquidGlassElement liquid_glass_element, float2 position)
            {
                float position_index = get_position_index(liquid_glass_element, position);
                if (position_index >= 1.0) return float2(0, 0);
                float2 rect_center_dir = normalize(position - 0.5 * (liquid_glass_element.rect.xy + liquid_glass_element.rect.zw));
                float2 offset = -2 * liquid_glass_element.radius * position_index * rect_center_dir;
                return offset;
            }

            // Returns the updated sample position associated with a given original sample position to simulate an
            // appropriate distortion effect associated with a provided LiquidGlassElement.
            float2 get_sample_position(LiquidGlassElement liquid_glass_element, float2 position)
            {
                return position + get_position_offset(liquid_glass_element, position);
            }

            half4 get_blurred_backgroun_color(LiquidGlassElement liquid_glass_element, float2 position)
            {
                float sigma = liquid_glass_element.blur;
                half4 color = 0;
                float weight_sum = 0;
                for (int y = -BLUR_KERNEL_RADIUS; y <= BLUR_KERNEL_RADIUS; y++)
                for (int x = -BLUR_KERNEL_RADIUS; x <= BLUR_KERNEL_RADIUS; x++)
                {
                    float2 offset = float2(x, y);
                    float weight = exp(-dot(offset, offset) / (2.0 * sigma * sigma));
                    weight_sum += weight;
                    float2 sample_position = get_sample_position(liquid_glass_element, position + offset);
                    float2 sample_uv = sample_position / _ScreenParams.xy;
                    color += SAMPLE_TEXTURE2D(_BlitTexture, sampler_BlitTexture, sample_uv) * weight;
                }
                color /= weight_sum;
                return color;
            }

            half4 apply_tint(LiquidGlassElement liquid_glass_element, half4 color)
            {
                half4 tint = liquid_glass_element.tint;
                float tint_opacity = tint.w;
                color.xyz *= 1 - tint_opacity;
                color.xyz += tint.xyz * tint_opacity;
                return color;
            }

            half4 get_liquid_glass_pixel(LiquidGlassElement liquid_glass_element, float2 position)
            {
                half4 color = 0;
                color = get_blurred_backgroun_color(liquid_glass_element, position);
                color = apply_tint(liquid_glass_element, color);
                return color;
            }

            int get_first_active_element_index(float2 position)
            {
                for (int i = 0; i < MAX_SUPPORTED_CONCURRENT_ELEMENTS; i++)
                {
                    LiquidGlassElement element = _LiquidGlassElements[i];
                    float position_index = get_position_index(element, position);
                    if (position_index < 1.0) return i;
                }
                return -1;
            }

            struct Attributes
            {
                uint vertexID : SV_VertexID;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;

                float2 positions[3] = {
                    float2(-1.0, -1.0),
                    float2( 3.0, -1.0),
                    float2(-1.0,  3.0)
                };

                float2 uvs[3] = {
                    float2(0.0, 1.0),
                    float2(2.0, 1.0),
                    float2(0.0, -1.0)
                };

                output.positionHCS = float4(positions[input.vertexID], 0.0, 1.0);
                output.uv = uvs[input.vertexID];
                return output;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                float2 resolution = _ScreenParams.xy;
                float2 uv = input.uv;
                float2 screen_pos = uv * resolution;
                int element_index = get_first_active_element_index(screen_pos);
                if (element_index < 0) return SAMPLE_TEXTURE2D(_BlitTexture, sampler_BlitTexture, uv);
                LiquidGlassElement element = _LiquidGlassElements[element_index];
                half4 color = get_liquid_glass_pixel(element, screen_pos);
                return color;
            }

            ENDHLSL
        }
    }
    FallBack Off
}
