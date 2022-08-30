Shader "Unlit/Brickdrop"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CurrentTime ("Current Time", Float) = 2
        _Speed ("Speed", Float) = 1
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _CurrentTime;
            float _Speed;
            float temp;
            float4 height = float4(0, 2, 0, 0);

            v2f vert(appdata v)
            {
                v2f o;
                temp = (_Time - _CurrentTime) * _Speed;
                temp = clamp(temp, 0, 1);
                temp = lerp(1, 0, temp);
                v.vertex += height * temp;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
