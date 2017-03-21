Shader "Custom/Circle" {
      Properties {
          _Color ("Color", Color) = (1,0,0,0)
          _Thickness("Thickness", Range(0.0,2.0)) = 0.2
          _Radius("Radius", Range(0.0, 0.5)) = 0.45
          _Dropoff("Dropoff", Range(0.0, 0.5)) = 0.03
          _OuterFadeOffset("OuterFadeOffset", Range(0.0, 0.5)) = 0.05
          _InnerFadeOffset("InnerFadeOffset", Range(0.0, 0.5)) = 0.005
      }
      SubShader {
          Pass {
              Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
              Blend SrcAlpha OneMinusSrcAlpha
              ZWrite Off
              ZTest Always
              CGPROGRAM
             
              #pragma vertex vert
              #pragma fragment frag
             
             fixed4 _Color;
             float _Thickness;
             float _Radius;
             float _Dropoff;
             uniform float _OuterFadeOffset;
             uniform float _InnerFadeOffset;
             
              struct appdata {
                  float4 vertex : POSITION;
                  float4 uv : TEXCOORD0;
              };

              struct v2f {
                  float4 pos : SV_POSITION;
                  float2 uv : TEXTCOORD0;
              };
  
              v2f vert (appdata v)
              {
                  v2f o;
                  o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
                  o.uv = v.uv.xy - fixed2(0.5,0.5);
  
                  return o;
              }
  
              float drawCircle(float radius, float distSqr, float thickness, float dropoff) {
                 dropoff *=  radius;
                 radius *= radius;
                 thickness *= radius;
                 float r1 = radius + thickness/2.0 - dropoff/2.0;
                 float r2 = radius - thickness/2.0 - dropoff/2.0;
                 float bigCircle = 1 - smoothstep(r1, r1 + dropoff, distSqr);
                 float smallCircle = smoothstep(r2, r2 + dropoff, distSqr);
                 float intersection = smallCircle * bigCircle;

                 float mask = 1.0;
                 /*
                 if (radius + thickness >= 0.25 - _FadeOffset*_FadeOffset) {
                    mask = 0.1;
                 }
                 */
                 //mask = 1-smoothstep(radius + thickness, radius + thickness + 0.25 - _FadeOffset*_FadeOffset, distSqr);
                 mask = smoothstep(0.25 - _OuterFadeOffset*_OuterFadeOffset, 0, radius + thickness);
                 mask *= smoothstep(0, _InnerFadeOffset, radius + thickness);
                 return intersection * mask;
              }
              
              fixed4 frag(v2f i) : SV_Target {
                 float x = i.uv.x;
                 float y = i.uv.y;
                 float distSqr = x*x + y*y;
                     
                 return fixed4(_Color.rgb, _Color.a * drawCircle(_Radius, distSqr, _Thickness, _Dropoff));
              }
              
              
              ENDCG
          }
      }
  }