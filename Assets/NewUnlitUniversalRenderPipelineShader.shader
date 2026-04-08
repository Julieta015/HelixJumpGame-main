Shader "Custom/Triplanar"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Scale ("Scale", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
            };

            sampler2D _MainTex;
            float _Scale;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 n = normalize(abs(i.worldNormal));

                float2 xUV = i.worldPos.yz * _Scale;
                float2 yUV = i.worldPos.xz * _Scale;
                float2 zUV = i.worldPos.xy * _Scale;

                fixed4 xTex = tex2D(_MainTex, xUV);
                fixed4 yTex = tex2D(_MainTex, yUV);
                fixed4 zTex = tex2D(_MainTex, zUV);

                return xTex * n.x + yTex * n.y + zTex * n.z;
            }
            ENDCG
        }
    }
}