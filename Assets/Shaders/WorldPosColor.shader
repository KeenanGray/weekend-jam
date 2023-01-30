// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/WorldPositionColor"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Color2 ("Color2", Color) = (1,1,1,1)
        _Threshold ("Threshold", Range(0.0,0.0525)) = .01
        _MainTex("Texture", 2D) = "white"
        _FalloffTex("Falloff",2D) = "white"
        _WorldOffset("WorldOffset", Vector) = (0.0,0.0,0,0)
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

            float _Threshold;
            float3 _WorldOffset;

            sampler2D _MainTex;
            sampler2D _FalloffTex;

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
                o.world = mul (unity_ObjectToWorld, v.vertex + _WorldOffset ).xyz ;
                o.uv = v.uv;
                
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                //float4 color = tex2D(_MainTex, i.uv) * float4(1, i.vertex.g, 0,1);
                //float4 color = tex2D(_MainTex,i.uv) * float4(1,i.world.g,1,1)/_Cancel * _Color;
                
                float avg = i.world.g * _Threshold;
                
                float4 color = ( ((1-avg) * _Color2) 
                + ((avg) * _Color) )
                * tex2D(_MainTex,i.uv);

                color.a = 1 - tex2D(_FalloffTex,i.uv);
                //clip(tex2D(_FalloffTex,i.uv));

                return color;
            };
            ENDCG
        }
    }
    FallBack "Diffuse"
}
