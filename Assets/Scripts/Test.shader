Shader "Custom/Test" {
	Properties {
          _Color ("Color", Color) = (1,1,1,1)
          _MainTex ("Albedo (RGB)", 2D) = "white" {}
          _Smoothness ("Smoothness", Range(0,1)) = 0.5
          _Strength ("Strength", Range(0,1.0)) = 0 
          _Distance ("Distance", Range(-0.5,0.5)) = 0 
 
      }
      SubShader {
          Tags { 
              "RenderType"="Opaque" 
          }
          LOD 200
          
          CGPROGRAM
          #pragma surface surf Standard vertex:vert addshadow 
          #pragma target 3.0
 
          struct Input {
              float2 uv_MainTex;
              float3 vertexColor; // Vertex color stored here by vert() method
          };
 
          float _Strength;
          float _Distance;
  
          void vert (inout appdata_full v, out Input o) {
              UNITY_INITIALIZE_OUTPUT(Input,o);
              // Save the Vertex Color in the Input for the surf() method
              o.vertexColor = v.color; 
 
              float3 wpos = v.vertex.xyz;
 
              //vertex displacing
              wpos.x += (sin(_Time.y * _Strength*2) + 1) * _Distance/2 * (wpos.z*wpos.z/2);
			  wpos.z += (sin(_Time.y * _Strength*2)) * _Distance/2 * (wpos.y*wpos.y/2);
              v.vertex.xyz = wpos.xyz;
          }
  
          sampler2D _MainTex;
          half _Smoothness;
          fixed4 _Color;
  
          void surf (Input IN, inout SurfaceOutputStandard o) {
              // Albedo comes from a texture tinted by color
              fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
               // Combine normal color with the vertex color
              o.Albedo = c.rgb * IN.vertexColor;
              o.Smoothness = _Smoothness;
              o.Alpha = c.a;
 
          }
          ENDCG
     }
 }