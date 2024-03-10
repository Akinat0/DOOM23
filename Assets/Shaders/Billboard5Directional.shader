// ===> ADDITIONAL READING <===

// https://jsantell.com/model-view-projection/

// values in matrices
// http://www.opengl-tutorial.org/beginners-tutorials/tutorial-3-matrices/ 

//matrix operations 
//https://gist.github.com/mattatz/86fff4b32d198d0928d0fa4ff32cf6fa

//to demonstate matrices
//matrixmultiplication.xyz 

//documentation
//https://ru.wikipedia.org/wiki/HLSL

/*
Spaces:
"Model space" - Also sometimes called local or object space.
"World space" - Also sometimes called scene or game space, or omitted altogether. Gets extra confusing when there are multiple versions of "world space", like the HDRP which has "absolute world space" and "world space" and they're not the same thing. The later is more accurately the "camera relative world oriented space", which is a mouthful.
"View space" - Also sometimes called camera or eye space. This is also confusing because in some uses Unity defines view and camera space as different things, where view space is -Z forward and camera space is +Z forward, but other times they use the term "camera" when they really mean "view".
"Clip space" - Also more correctly called homogeneous clip space, or less accurately projection space, or incorrectly called screen space.
"Normalized Device Coordinate space" - Usually just called NDC space. This is probably the only one that's ever 100% consistent. It's also a space most people don't really think about because it's usually hidden by the GPU.
"Viewport Space" - Sometimes called screen or normalized window or just "window" space. Not to be confused with view space. Also sometimes referred to as "screen space UVs" as that's where they show up the most often. 0.0 to 1.0 range for x and y for what's visible on screen.
"Viewport Space" - Sometimes called screen, pixel or window space. Not to be confused with ... wait ... Yes, I did just type the same name twice. No, that was not a mistake. I did this to highlight how inconsistent the terminology is. This is the on screen pixel coordinate space, which is totally different than the normalized window space.* (* Debatable...)

Transforms:
"Model matrix" - Transforms from local to world space. Also sometimes called the object, or world, or object to world matrix.
"View matrix" - Transforms from world to view space. Also sometimes called the camera matrix, though again be wary of Unity's -Z "view" vs +Z "camera" forward stuff.
"Projection matrix" - Transforms from view to clip space. Also sometimes called the perspective matrix. Unity usually has multiple versions of this matrix depending on if it should be used with the "view" or "camera" matrix as it needs to correctly handle the Z sign. In Unity's shaders it tends to call the projection matrix that handles +Z the "camera projection" matrix.
*/



Shader "Billboard/5 Directional"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
        
        Tags 
        { 
            "RenderType"      = "Transparent"
            "DisableBatching" = "True"
        }
        
        LOD 100

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "Billboard.cginc"

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
                float cameraAngle : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            
            
            v2f vert (appdata v)
            {
                v2f o;

                
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = v.normal;

                //from https://docs.unity3d.com/Packages/com.unity.shadergraph@6.9/manual/Camera-Node.html
                //UNITY_MATRIX_I_M == unity_WorldToObject, UNITY_MATRIX_I_V == unity_MatrixInvV
				float3 cameraDir =  -1 * mul(UNITY_MATRIX_M, transpose(mul(unity_WorldToObject, UNITY_MATRIX_I_V))[2].xyz);
                cameraDir.y = 0; //remove pitch

                float2 cameraDir2D = normalize(cameraDir.xz);

                float2 vectorForward2D = mul(UNITY_MATRIX_M, float4(0, 0, 1, 0)).xz;
                
                float angle = dot(vectorForward2D, cameraDir2D);

                //sometimes Dot can return value outside of [-1, 1] range which fails Acos
                angle = clamp(angle, -1, 1);

                float angleRad = acos(angle);

                float3 crossProduct = cross(
                    float3(vectorForward2D.x, 0, vectorForward2D.y),
                    float3(cameraDir2D.x, 0, cameraDir2D.y));

                if(dot(crossProduct, float3(0, 1, 0)) < 0)
                    angleRad = -angleRad;
                
                float angleNormalized = angleRad / 3.1415;
                
                o.cameraAngle = (angleNormalized + 1) / 2;

                
                float3 newVertex;

                //WE CAN'T SCALE THIS SHADER BECAUSE MODEL TRANSFORMATION IS STILL THINKING THAT VERTEX IS NOT ROTATED
                Unity_RotateAboutAxis_Radians(v.vertex, float3(0, 1, 0), angleRad, newVertex);

                o.vertex = UnityObjectToClipPos(newVertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                i.normal.y = 0; //remove pitch from normal

                //during dir transformation we must set last coord to 0 to avoid translation scaling
                //for position transformation we should put last coord to 1
                float3 worldNormal = mul(UNITY_MATRIX_M, float4(i.normal, 0.0)).xyz;
                float3 localNormal = fixed4(i.normal, 1);

                float2 normal2D = normalize(i.normal.xz);

                // 0.0625 = half of 1/8 direction
                // + 1 is required because fmod seems to not work with negative values
                float tileAngle = fmod(i.cameraAngle - 0.0625 + 1, 1); 
                
                float tile = floor(lerp(0, 8, 1 - tileAngle));

                float2 uv;
                
                if(tile <= 4)
                {
                    Unity_Flipbook(i.uv, 4, 2, tile, float2(0, 1), uv);
                }
                else // symmetric tile
                {
                    //flip uv   
                    Unity_Flipbook(float2(1 - i.uv.x, i.uv.y), 4, 2, 8 - tile, float2(0, 1), uv);
                }
                
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
