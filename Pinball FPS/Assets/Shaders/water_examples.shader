Shader "Examples/Water"
{
    Properties
    {
        // shallow water gets light blue
        _DepthGradientShallow("Depth Gradient Shallow", Color) = (0.325, 0.807, 0.971, 0.725)
        // deep water gets darker blue
        _DepthGradientDeep("Depth Gradient Deep", Color) = (0.086, 0.407, 1, 0.749)
        // there is a max depth, so it won't be too large,
        // and anything deeper than this will no longer change color
        _DepthMaxDistance("Depth Maximum Distance", Float) = 1
        // for texture to get noise effect
        _SurfaceNoise("Surface Noise", 2D) = "white" {}
        _SurfaceNoiseCutoff("Surface Noise Cutoff", Range(0, 1)) = 0.777 // 0.8
        // for texture to distort motion so there appears to be some randomness in motion
        _SurfaceDistortion("Surface Distortion", 2D) = "white" {}
    }
    SubShader
    {
        Tags
        {
            // render objects with this shader after all objects
            // here, overlay transparent water on top of opaque objects and blend them together
            //"Queue" = "Transparent"
        }

        Pass
        {
            // regular alpha blending
            //Blend SrcAlpha OneMinusSrcAlpha
            // do not write object into depth buffer, which would occlude objects behind it,
            // instead of partially obscuring them
            //ZWrite Off

            // actual "code" for shader starts here
            // usual initial statements
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            // input of Vertex Shader
            struct appdata
            {
                float4 vertex : POSITION; // for passing vertex information
                float4 uv : TEXCOORD0; // for noise
            };

            // output of Vertex Shader, and input of Fragment Shader
            struct v2f
            {
                float4 vertex : SV_POSITION; // for passing vertex information
                float4 screenPosition : TEXCOORD2; // Vertex Shader will convert to screen position
                float2 noiseUV : TEXCOORD0; // for noise texture
                float2 distortUV : TEXCOORD1; // for motion distortion texture
            };

            // for noise
            sampler2D _SurfaceNoise;
            // this value automatically gets Tiling and Offset
            float4 _SurfaceNoise_ST;
            float _SurfaceNoiseCutoff;
            // for distortion of motion
            sampler2D _SurfaceDistortion;
            float4 _SurfaceDistortion_ST;

            // Vertex Shader: code that runs on each vertex of the 3D model.
            // Usually transform vertex position from 3D mesh space into so called "clip space",
            // or the screen space where mesh/object is drawn.
            v2f vert (appdata v)
            {
                v2f o;
                // Clip Space, think of it as transformed from object space to clip space,
                // or what will appear on screen.
                // float4 for (x,y,z,w) or Homogeneous coordinates
                // https:/_/www.songho.ca/math/homogeneous/homogeneous.html
                // this function is almost always here in vertex shader
                o.vertex = UnityObjectToClipPos(v.vertex);
                // Screen Space is clipPos.xyz / clipPos.w,
                // or transformed into 0.0 to 1.0 range.
                o.screenPosition = ComputeScreenPos(o.vertex);
                // TRANSFORM_TEX transforms by Tiling and Offset using _SurfaceNoise_ST
                o.noiseUV = TRANSFORM_TEX(v.uv, _SurfaceNoise);
                o.distortUV = TRANSFORM_TEX(v.uv, _SurfaceDistortion);
                return o;
            }

            float4 _DepthGradientShallow;
            float4 _DepthGradientDeep;
            float _DepthMaxDistance;
            // depth texture is a greyscale image
            // objects closer to the camera are more white
            // objects further away are darker
            sampler2D _CameraDepthTexture;

            // Fragment Shader: code that runs on each pixel that mesh/object occupies on screen.
            // Calculates and outputs the color of each pixel.
            float4 frag (v2f i) : SV_Target
            {
                //
                // just color pixels white
                //return float4(1, 1, 1, 1);
                //
                // just color pixels blue
                //return float4(0, 0, 1, 1);
                //
                // read depth map and compute depth from screen to object, this is black and white colors
                float existingDepth01 = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPosition)).r;
                float existingDepthLinear = LinearEyeDepth(existingDepth01);
                float depthDifference = existingDepthLinear - i.screenPosition.w;
                //return depthDifference;
                //
                // convert depth to blue colors
                // shallow water gets light blue, and deep water gets darker blue
                float waterDepthDifference01 = saturate(depthDifference / _DepthMaxDistance);
                float4 waterColor = lerp(_DepthGradientShallow, _DepthGradientDeep, waterDepthDifference01);
                //return waterColor;
                //
                // read noise texture and add to color, to give it natural noisy effect
                float surfaceNoiseSample = tex2D(_SurfaceNoise, i.noiseUV).r;
                //return waterColor + surfaceNoiseSample;
                //
                // for noisy texture, get binary look of either take texture or not
                // values darker than cutoff threshold are not used,
                // and values above are drawn completely white, so get white bubbles on water.
                float surfaceNoise = surfaceNoiseSample > _SurfaceNoiseCutoff ? 1 : 0;
                return waterColor + surfaceNoise;
                //
                // adjust noise based on depth, so shallow part gets white color too,
                // so the sides tend to get white color.
                //float sideDepthDifference01 = saturate(depthDifference / 0.08); // try change the constant
                //float surfaceNoiseCutoff = sideDepthDifference01 * _SurfaceNoiseCutoff;
                //float surfaceNoise = surfaceNoiseSample > surfaceNoiseCutoff ? 1 : 0;
                //return waterColor + surfaceNoise;
                //
                // add Motion with fixed (x,y) values, try change constants to make it move at different (x,y) speeds
                //float2 noiseUV = float2(i.noiseUV.x + _Time.y * 0.03, i.noiseUV.y + _Time.y * 0.03);
                //float surfaceNoiseSample = tex2D(_SurfaceNoise, noiseUV).r;
                //float surfaceNoise = surfaceNoiseSample > _SurfaceNoiseCutoff ? 1 : 0;
                //return waterColor + surfaceNoise;
                //
                // add Motion with more random movements, so get distortion texture and use it to adjust (x,y) movement
                // texture xy has range 0 to 1, convert to 2D vector with range -1 to 1. try adjust 0.27 constant
                //float2 distortSample = (tex2D(_SurfaceDistortion, i.distortUV).xy * 2 - 1) * 0.27;
                //float2 noiseUV = float2((i.noiseUV.x + _Time.y * 0.03) + distortSample.x, (i.noiseUV.y + _Time.y * 0.03) + distortSample.y);
                //float surfaceNoiseSample = tex2D(_SurfaceNoise, noiseUV).r;
                //float surfaceNoise = surfaceNoiseSample > _SurfaceNoiseCutoff ? 1 : 0;
                //return waterColor + surfaceNoise;
            }
            ENDCG
        }
    }
}
