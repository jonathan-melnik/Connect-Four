Shader "Unlit/Quad2"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Magnitude ("Magnitude", Range(-1, 1)) = 0.1
        _Radius ("Radius", Range(-1, 1)) = 0.01
        _RadiusThickness("RadiusThickness", Range(-1, 1)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Radius;
            float _Magnitude;
            float _RadiusThickness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 vec = i.uv - float2(0.5, 0.5);
                float dist = length(vec);
                //float outCircle = smoothstep(_Radius + _RadiusThickness * 0.5, _Radius - _RadiusThickness, dist);
                //float inCircle = smoothstep(_Radius - _RadiusThickness * 0.5, _Radius + _RadiusThickness, dist);                
                float outCircle = smoothstep(_Radius + _RadiusThickness, _Radius, dist);
                float inCircle = smoothstep(_Radius - _RadiusThickness, _Radius, dist);
                float circle2 = smoothstep(_Radius - _RadiusThickness * 0.3, _Radius, dist);                
                float circle3 = smoothstep(_Radius + _RadiusThickness * 0.3, _Radius, dist);                
                float inner = circle3 * circle2;
                float circle = outCircle * inCircle - inner * 0.4;                
                //return circle;
                float2 distort = circle * normalize(vec);
                //return float4(distort, 0, 1);
                fixed4 col = tex2D(_MainTex, i.uv + distort * _Magnitude);                
                return col;
            }
            ENDCG
        }
    }
}
