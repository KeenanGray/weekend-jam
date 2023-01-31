// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/WorldPositionColor"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Color2 ("Color2", Color) = (1,1,1,1)
        _Scaling("Scaling",Float) = 1
        _Threshold ("Threshold", Range(0.0,0.0525)) = .01
        _Texture("Texture", 2D) = "" {}
        _FalloffTex("Falloff",2D) = "" {}
        _Noise("Noise",2D) = "" {}
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
            float _Scaling;
            float3 _WorldOffset;

            sampler2D _Texture;
            sampler2D _FalloffTex;
            sampler2D _Noise;

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
                o.uv = v.uv*_Scaling;
                
                return o;
            }

            float random (float2 uv)
            {
                return frac(sin(dot(uv,float2(12.9898,78.233)))*43758.5453123);
            }
            float4 frag(v2f i) : SV_Target
            {
                //float4 color = tex2D(_MainTex, i.uv) * float4(1, i.vertex.g, 0,1);
                //float4 color = tex2D(_MainTex,i.uv) * float4(1,i.world.g,1,1)/_Cancel * _Color;
                
                float avg = i.world.g * _Threshold;
                
                
                float4 color = ( ((1-avg) * _Color2) 
                + ((avg) * _Color)) 
                * tex2D(_Texture,i.uv+float2(.5,.25));
                

                //float4 color = tex2D(_Texture,i.uv);
                //color.a = clamp(random(i.uv) - tex2D(_FalloffTex,i.uv) ,0,1);
                //clip(tex2D(_FalloffTex,i.uv));
                //color.a = clamp(random(i.uv) + tex2D(_Noise,i.uv) * 1- tex2D(_FalloffTex,i.uv) ,0,1);
                
                color.a = clamp(1 - tex2D(_FalloffTex,i.uv), 0, 1);
                return color;
            };

            
            ENDCG
        }
    }
    FallBack "Diffuse"
}
