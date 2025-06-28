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

        // ---------- �A�E�g���C���`�� ----------
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
                // ���[�J�����_ �� ���[���h��Ԃɕϊ�
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                // �@�������[���h��Ԃɕϊ��i�X�P�[�����O�␳�t���j
                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                // �@�������Ɋg��
                worldPos += normalize(worldNormal) * _OutlineWidth;
                // �ŏI�I�ɃN���b�v��Ԃ֕ϊ�
                o.vertex = UnityWorldToClipPos(float4(worldPos, 1.0));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return fixed4(0, 0, 0, 1); // �����A�E�g���C��
            }
            ENDCG
        }

        // ---------- �ʏ�`�� ----------
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
                // �����v�}�b�v�Ɩ��v�Z
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
