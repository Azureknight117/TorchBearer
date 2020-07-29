Shader "Nautilus"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"
            #include "UnityLightingCommon.cginc "
            //Mesh data: vertex position, vertex normal, UVs, tangents, vertex colors
            struct VertexInput
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv0    : TEXCOORD0;
                
                //float4 colors : COLOR;
                //float4 tangent : TANGENT;
                //float2 uv1 : TEXCOORD1;
            };

            struct VertexOutput
            {
                float4 clipSpacepos : SV_POSITION;
                float2 uv0          :TEXCOORD0;
                float3 normal       :TEXCOORD1;
                float3 worldPos     :TEXCOORD2;
            };


           // sampler2D _MainTex;
           // float4 _MainTex_ST;

            float4 _Color;
            float _Gloss;
         // Vertex shader
            VertexOutput vert (VertexInput v)
            {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.normal = v.normal;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.clipSpacepos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (VertexOutput o) : SV_Target
            {
                float2 uv = o.uv0;
                float3 normal = normalize(o.normal); //interpolated
                //Lighting
                float3 lightDir = _WorldSpaceLightPos0.xyz;
                float3 lightColor = _LightColor0.rgb;

                //Direct diffuse Light
                float lightFallOff = max(0 ,dot (lightDir, normal));
                float3 directDiffuseLight = lightColor * lightFallOff;
                //Ambient Light
                float3 ambientLight = float3( 0.2, 0.2, 0.2);

                //Direct specular light
                float3 camPos = _WorldSpaceCameraPos;
                float3 fragToCam = camPos - o.worldPos;
                float3 viewDir = normalize(fragToCam);
                float3 viewReflect = reflect( -viewDir, normal);
                float3 specularFalloff = max (0, dot( viewReflect, lightDir));

                //Phong
                //Blinn-Phong
                

                //Composite
                float3 diffuseLight = ambientLight + directDiffuseLight;
                float3 finalSurfaceColor = diffuseLight * _Color.rgb + specularFalloff;

                return float4(_Color.rgb, 0);
            }
            ENDCG
        }
    }
}
