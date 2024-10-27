Shader "Custom/SunShader"
{
    Properties
    {
        _Color ("Color", Color) = (1, 0.5, 0, 1)
        _MainTex ("Texture", 2D) = "white" {}
        _EmissionColor ("Emission Color", Color) = (1, 0.5, 0, 1)
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Opaque" }
        LOD 100

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
                float4 pos : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _Color;
            float4 _EmissionColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                return col + _EmissionColor; // ajout de l'émission pour la brillance
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}