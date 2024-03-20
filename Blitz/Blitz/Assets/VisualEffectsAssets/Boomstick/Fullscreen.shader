Shader "Unlit/Fullscreen"
{
    Properties
    {
        [NoScaleOffset]_MainTex("MainTex", 2D) = "white" {}
        _FullscreenIntensity("FullscreenIntensity", Range(0, 1)) = 0.7
        [HDR]_Color("Color", Color) = (1, 0.3956207, 0.0235849, 1)
        _VoronoiSpeed("VoronoiSpeed", Float) = 1
        _VoronoiScale("VoronoiScale", Float) = 5
        _VignetteIntensity("VignetteIntensity", Float) = 7.5
        _VignetteRadiusPower("VignetteRadiusPower", Range(0, 1)) = 0.7
        _VoronoiPower("VoronoiPower", Float) = 2
        [HideInInspector]_CastShadows("_CastShadows", Float) = 1
        [HideInInspector]_Surface("_Surface", Float) = 1
        [HideInInspector]_Blend("_Blend", Float) = 0
        [HideInInspector]_AlphaClip("_AlphaClip", Float) = 0
        [HideInInspector]_SrcBlend("_SrcBlend", Float) = 1
        [HideInInspector]_DstBlend("_DstBlend", Float) = 0
        [HideInInspector][ToggleUI]_ZWrite("_ZWrite", Float) = 0
        [HideInInspector]_ZWriteControl("_ZWriteControl", Float) = 0
        [HideInInspector]_ZTest("_ZTest", Float) = 4
        [HideInInspector]_Cull("_Cull", Float) = 0
        [HideInInspector]_AlphaToMask("_AlphaToMask", Float) = 0
        [HideInInspector]_QueueOffset("_QueueOffset", Float) = 0
        [HideInInspector]_QueueControl("_QueueControl", Float) = -1
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Unlit"
            "Queue"="Transparent"
            "DisableBatching"="False"
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalUnlitSubTarget"
        }
        Pass
        {
            Name "Universal Forward"
        
        // Render State
        Cull [_Cull]
        Blend [_SrcBlend] [_DstBlend]
        ZTest [_ZTest]
        ZWrite [_ZWrite]
        AlphaToMask [_AlphaToMask]
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma instancing_options renderinglayer
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma shader_feature _ _SAMPLE_GI
        #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
        #pragma shader_feature_fragment _ _SURFACE_TYPE_TRANSPARENT
        #pragma shader_feature_local_fragment _ _ALPHAPREMULTIPLY_ON
        #pragma shader_feature_local_fragment _ _ALPHAMODULATE_ON
        #pragma shader_feature_local_fragment _ _ALPHATEST_ON
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_UNLIT
        #define _FOG_FRAGMENT 1
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float3 positionWS : INTERP1;
             float3 normalWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float _FullscreenIntensity;
        float4 _Color;
        float _VoronoiSpeed;
        float _VoronoiScale;
        float _VignetteIntensity;
        float _VignetteRadiusPower;
        float _VoronoiPower;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // Graph Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_PolarCoordinates_float(float2 UV, float2 Center, float RadialScale, float LengthScale, out float2 Out)
        {
            float2 delta = UV - Center;
            float radius = length(delta) * 2 * RadialScale;
            float angle = atan2(delta.x, delta.y) * 1.0/6.28 * LengthScale;
            Out = float2(radius, angle);
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Power_float(float A, float B, out float Out)
        {
            Out = pow(A, B);
        }
        
        float2 Unity_Voronoi_RandomVector_Deterministic_float (float2 UV, float offset)
        {
            Hash_Tchou_2_2_float(UV, UV);
            return float2(sin(UV.y * offset), cos(UV.x * offset)) * 0.5 + 0.5;
        }
        
        void Unity_Voronoi_Deterministic_float(float2 UV, float AngleOffset, float CellDensity, out float Out, out float Cells)
        {
            float2 g = floor(UV * CellDensity);
            float2 f = frac(UV * CellDensity);
            float t = 8.0;
            float3 res = float3(8.0, 0.0, 0.0);
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    float2 lattice = float2(x, y);
                    float2 offset = Unity_Voronoi_RandomVector_Deterministic_float(lattice + g, AngleOffset);
                    float d = distance(lattice + offset, f);
                    if (d < res.x)
                    {
                        res = float3(d, offset.x, offset.y);
                        Out = res.x;
                        Cells = res.y;
                    }
                }
            }
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
        {
            Out = lerp(A, B, T);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_32c1dd3dbc53479c92676d8f1ff824ce_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_5f580663d14d4c61a0008f2c4b4a0b91_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_32c1dd3dbc53479c92676d8f1ff824ce_Out_0_Texture2D.tex, _Property_32c1dd3dbc53479c92676d8f1ff824ce_Out_0_Texture2D.samplerstate, _Property_32c1dd3dbc53479c92676d8f1ff824ce_Out_0_Texture2D.GetTransformedUV(IN.uv0.xy) );
            float _SampleTexture2D_5f580663d14d4c61a0008f2c4b4a0b91_R_4_Float = _SampleTexture2D_5f580663d14d4c61a0008f2c4b4a0b91_RGBA_0_Vector4.r;
            float _SampleTexture2D_5f580663d14d4c61a0008f2c4b4a0b91_G_5_Float = _SampleTexture2D_5f580663d14d4c61a0008f2c4b4a0b91_RGBA_0_Vector4.g;
            float _SampleTexture2D_5f580663d14d4c61a0008f2c4b4a0b91_B_6_Float = _SampleTexture2D_5f580663d14d4c61a0008f2c4b4a0b91_RGBA_0_Vector4.b;
            float _SampleTexture2D_5f580663d14d4c61a0008f2c4b4a0b91_A_7_Float = _SampleTexture2D_5f580663d14d4c61a0008f2c4b4a0b91_RGBA_0_Vector4.a;
            float4 _Property_5ffb5747bbeb4ba1ab7172eb854ea6b5_Out_0_Vector4 = IsGammaSpace() ? LinearToSRGB(_Color) : _Color;
            float2 _PolarCoordinates_a6465c16780b43bd8ebbbaeba3a759db_Out_4_Vector2;
            Unity_PolarCoordinates_float(IN.uv0.xy, float2 (0.5, 0.5), 1, 1, _PolarCoordinates_a6465c16780b43bd8ebbbaeba3a759db_Out_4_Vector2);
            float _Split_4e384efad9444017a6f5f30d34dc25c8_R_1_Float = _PolarCoordinates_a6465c16780b43bd8ebbbaeba3a759db_Out_4_Vector2[0];
            float _Split_4e384efad9444017a6f5f30d34dc25c8_G_2_Float = _PolarCoordinates_a6465c16780b43bd8ebbbaeba3a759db_Out_4_Vector2[1];
            float _Split_4e384efad9444017a6f5f30d34dc25c8_B_3_Float = 0;
            float _Split_4e384efad9444017a6f5f30d34dc25c8_A_4_Float = 0;
            float _Property_ddf09f92686c4130af2dd74c5571566b_Out_0_Float = _VignetteRadiusPower;
            float _Multiply_77b378143eaf4886b256a1e80752541e_Out_2_Float;
            Unity_Multiply_float_float(_Split_4e384efad9444017a6f5f30d34dc25c8_R_1_Float, _Property_ddf09f92686c4130af2dd74c5571566b_Out_0_Float, _Multiply_77b378143eaf4886b256a1e80752541e_Out_2_Float);
            float _Property_493a9f7b1e19461fa4f7cec75d2cf583_Out_0_Float = _VignetteIntensity;
            float _Power_26feda8235884c72bb1c169e8aceea59_Out_2_Float;
            Unity_Power_float(_Multiply_77b378143eaf4886b256a1e80752541e_Out_2_Float, _Property_493a9f7b1e19461fa4f7cec75d2cf583_Out_0_Float, _Power_26feda8235884c72bb1c169e8aceea59_Out_2_Float);
            float _Property_f8933f1326ae4b6880995b46ceb99229_Out_0_Float = _VoronoiSpeed;
            float _Multiply_a5b98bd983e2459d813dc99dd26a612a_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, _Property_f8933f1326ae4b6880995b46ceb99229_Out_0_Float, _Multiply_a5b98bd983e2459d813dc99dd26a612a_Out_2_Float);
            float _Property_7c0fbdda94fd42d4813f78afbf30b140_Out_0_Float = _VoronoiScale;
            float _Voronoi_2b2b64d817fa4e82bf661cff46c99cbb_Out_3_Float;
            float _Voronoi_2b2b64d817fa4e82bf661cff46c99cbb_Cells_4_Float;
            Unity_Voronoi_Deterministic_float(IN.uv0.xy, _Multiply_a5b98bd983e2459d813dc99dd26a612a_Out_2_Float, _Property_7c0fbdda94fd42d4813f78afbf30b140_Out_0_Float, _Voronoi_2b2b64d817fa4e82bf661cff46c99cbb_Out_3_Float, _Voronoi_2b2b64d817fa4e82bf661cff46c99cbb_Cells_4_Float);
            float _Property_62d0f7f085b94fccac6d832029ec6411_Out_0_Float = _VoronoiPower;
            float _Power_33287d553f194d6197a551f652593b34_Out_2_Float;
            Unity_Power_float(_Voronoi_2b2b64d817fa4e82bf661cff46c99cbb_Out_3_Float, _Property_62d0f7f085b94fccac6d832029ec6411_Out_0_Float, _Power_33287d553f194d6197a551f652593b34_Out_2_Float);
            float _Multiply_5f64c669f5c248b99e33b64084ae048a_Out_2_Float;
            Unity_Multiply_float_float(_Power_26feda8235884c72bb1c169e8aceea59_Out_2_Float, _Power_33287d553f194d6197a551f652593b34_Out_2_Float, _Multiply_5f64c669f5c248b99e33b64084ae048a_Out_2_Float);
            float4 _Multiply_3ed161cbd51f496bbecd6c930f26293b_Out_2_Vector4;
            Unity_Multiply_float4_float4(_Property_5ffb5747bbeb4ba1ab7172eb854ea6b5_Out_0_Vector4, (_Multiply_5f64c669f5c248b99e33b64084ae048a_Out_2_Float.xxxx), _Multiply_3ed161cbd51f496bbecd6c930f26293b_Out_2_Vector4);
            float _Property_9b581ae33fbb412bbf17956f24a6bbbf_Out_0_Float = _FullscreenIntensity;
            float4 _Lerp_3cacb711150a41ec8bc23ea12cdfe41c_Out_3_Vector4;
            Unity_Lerp_float4(_SampleTexture2D_5f580663d14d4c61a0008f2c4b4a0b91_RGBA_0_Vector4, _Multiply_3ed161cbd51f496bbecd6c930f26293b_Out_2_Vector4, (_Property_9b581ae33fbb412bbf17956f24a6bbbf_Out_0_Float.xxxx), _Lerp_3cacb711150a41ec8bc23ea12cdfe41c_Out_3_Vector4);
            surface.BaseColor = (_Lerp_3cacb711150a41ec8bc23ea12cdfe41c_Out_3_Vector4.xyz);
            surface.Alpha = _Multiply_5f64c669f5c248b99e33b64084ae048a_Out_2_Float;
            surface.AlphaClipThreshold = 0.26;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
            output.uv0 = input.texCoord0;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/UnlitPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
    }
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    CustomEditorForRenderPipeline "UnityEditor.ShaderGraphUnlitGUI" "UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset"
    FallBack "Hidden/Shader Graph/FallbackError"
}