// SPECIAL THANKS TO https://github.com/izmhr/KinectV2DepthPoints for the   
// CODE [now modified] AND INSPIRATION                                      

Shader "Custom/KinectDepthBasic"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Displacement("Displacement", Range(0, 0.1)) = 0.003
		_BaseColor("Base Color", Color) = (0,0,0,0)
		_TempColor("Temp Color", Color) = (0,0,0,0)
	}
	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 5
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 col : COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Displacement;
			fixed4 _BaseColor;
			fixed4 _TempColor;

			v2f vert(appdata v)
			{
				// depth information based on color info of texture
				float4 col = tex2Dlod(_MainTex, float4(v.uv, 1, 0));
	
				float d = col.x * 1500 * _Displacement;

				float3 test = v.vertex.xyz;
				// expand the points to space with depth coordinate data
				v.vertex.x = -v.vertex.x * d / 3.656;
				v.vertex.y = v.vertex.y * d / 3.656;
				v.vertex.z = d;	
				
				// transform the mesh to pass to vertex shader
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv = v.uv;

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{

				fixed4 col;

				// texture color
				//fixed4 col2 = tex2D(_MainTex, i.uv);
				// predefined color
				//col = _Color;
				
				// check positon on point
				if (i.vertex.z > 0.0006) 
                {
					float pct = abs(sin(_Time * 7));
					col = _BaseColor * (1 - pct) + _TempColor * pct;
				}
				// discard if not close enough
				else 
                {
					discard;	
				}
				
				return col;
			}
			ENDCG
		}
	}
}