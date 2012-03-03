Shader "Crystani/DFTShaderCullOff" {
	Properties {
		_Color ("Main Color", Color) =(1.0,1.0,1.0,1.0)
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 100

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha 
		Cull Off
		
		Pass {
			SetTexture [_MainTex] {
				constantColor [_Color]
				Combine constant, texture * constant
			} 
		}	
	}
}
