Shader "Unlit/StencilWall"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        ColorMask 0
    

        Pass { }
    }
}
