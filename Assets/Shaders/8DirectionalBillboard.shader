Shader "Billboard/8Directional"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"      = "Transparent"
            "DisableBatching" = "True"
        }
        LOD 100
        Cull off

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "DoomBillboard.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float angle : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;


            v2f vert (appdata v)
            {
                v2f o;

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.normal = v.normal;

                //https://docs.unity3d.com/Packages/com.unity.shadergraph@6.9/manual/Camera-Node.html
                float3 cameraDir = -1 * mul(UNITY_MATRIX_M, transpose(mul(unity_WorldToObject, UNITY_MATRIX_I_V))[2].xyz);
                cameraDir.y = 0;

                float2 cameraDir2D = normalize(cameraDir.xz);
                

                float2 vectorForward2D = mul(UNITY_MATRIX_M, float4(0, 0, 1, 0)).xz;

                float angle = dot(vectorForward2D, cameraDir2D);

                float angleRad = acos(angle);

                float3 crossProduct = cross(
                    float3(vectorForward2D.x, 0, vectorForward2D.y),
                    float3(cameraDir2D.x, 0, cameraDir2D.y));

                if(dot(crossProduct, float3(0, 1, 0)) < 0)
                    angleRad = -angleRad;

                float angleNormalized = angleRad / 3.1415;


                o.angle = (angleNormalized + 1) / 2;


                float3 newVertex;
                Unity_RotateAboutAxis_Radians_float(v.vertex, float3(0, 1, 0), angleRad, newVertex);

                o.vertex = UnityObjectToClipPos(newVertex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {   
                

                float tileAngle = fmod(i.angle + 0.0625, 1);

                float tile = floor(lerp(0, 8, tileAngle));

                float2 uv;
                Unity_Flipbook_float(i.uv, 4, 2, tile, float2(0, 0), uv);

                // sample the texture
                fixed4 color = tex2D(_MainTex, uv);

                if(color.a < 0.001)
                    discard;

                return color;
            }
            ENDCG
        }
    }
}
