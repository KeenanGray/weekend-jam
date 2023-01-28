// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/WorldPositionColor"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Color2 ("Color2", Color) = (1,1,1,1)
        _Scalar ("Scalar", Float) = 10.0
        _MainTex("Texture", 2D) = "white"
    }

    SubShader
    {
        Tags { 
            "Queue" = "Transparent"
            "RenderType"="Transparent" 
        }
        Pass{
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex: POSITION;
                float2 uv : TEXCOORD0;
            };

            fixed4 _Color;
            fixed4 _Color2;

            float _Scalar;
            sampler2D _MainTex;

            struct v2f
            {
                float4 vertex: SV_POSITION;
                float3 world: TEXCOORD1;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.world = mul (unity_ObjectToWorld, v.vertex).xyz/ _Scalar;
                o.uv = v.uv;
                
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                //float4 color = tex2D(_MainTex, i.uv) * float4(1, i.vertex.g, 0,1);
                //float4 color = tex2D(_MainTex,i.uv) * float4(1,i.world.g,1,1)/_Cancel * _Color;
                float avg = i.world.g/3;
                float4 color = ( ((1-avg) * _Color2) 
                + ((avg) * _Color) )
                * tex2D(_MainTex,i.uv);
                
                clip(1-avg+.0001);
                return color;
            };
            ENDCG
        }
    }
    FallBack "Diffuse"
}
