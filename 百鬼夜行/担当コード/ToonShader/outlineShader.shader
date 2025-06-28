Shader "Unlit/outlineShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _RampTex ("Ramp", 2D) = "white" {}
        _OutlineWidth("Outline Width", Float) = 0.04
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        // ---------- アウトライン描画 ----------
        Pass
        {
            Name "OUTLINE"
            Cull Front

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
                float4 vertex : SV_POSITION;
            };

            float _OutlineWidth;

            v2f vert (appdata v)
            {
                v2f o;
                // ローカル頂点 → ワールド空間に変換
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                // 法線もワールド空間に変換（スケーリング補正付き）
                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                // 法線方向に拡張
                worldPos += normalize(worldNormal) * _OutlineWidth;
                // 最終的にクリップ空間へ変換
                o.vertex = UnityWorldToClipPos(float4(worldPos, 1.0));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return fixed4(0, 0, 0, 1); // 黒いアウトライン
            }
            ENDCG
        }

        // ---------- 通常描画 ----------
        Pass
        {
            Name "BASE"
            Cull Back

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv_MainTex : TEXCOORD0;
                float2 uv_RampTex : TEXCOORD1;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float2 uv_MainTex : TEXCOORD0;
                float2 uv_RampTex : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _RampTex;
            float4 _RampTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = normalize(UnityObjectToWorldNormal(v.normal));
                o.uv_MainTex = TRANSFORM_TEX(v.uv_MainTex, _MainTex);
                o.uv_RampTex = TRANSFORM_TEX(v.uv_RampTex, _RampTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // ランプマップ照明計算
                half nl = dot(i.normal, _WorldSpaceLightPos0.xyz) * 0.5 + 0.5;
                fixed3 ramp = tex2D(_RampTex, float2(nl, 0.5)).rgb;
                fixed4 col = tex2D(_MainTex, i.uv_MainTex);
                col.rgb *= ramp;
                return col;
            }
            ENDCG
        }
    }
}
