// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:33323,y:32452,varname:node_3138,prsc:2|emission-3238-OUT,alpha-3994-OUT;n:type:ShaderForge.SFN_Tex2d,id:1179,x:32687,y:32706,ptovrint:False,ptlb:Texture,ptin:_MainTex,varname:node_1179,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:8c2bbf0851b936c44b3ca98632bdda19,ntxv:0,isnm:False|UVIN-5507-UVOUT;n:type:ShaderForge.SFN_Append,id:7063,x:31858,y:32599,varname:node_7063,prsc:2|A-9665-OUT,B-3399-OUT;n:type:ShaderForge.SFN_Time,id:2963,x:31858,y:32747,varname:node_2963,prsc:2;n:type:ShaderForge.SFN_Multiply,id:3097,x:32020,y:32662,varname:node_3097,prsc:2|A-7063-OUT,B-2963-T;n:type:ShaderForge.SFN_ScreenPos,id:7588,x:32020,y:32809,varname:node_7588,prsc:2,sctp:1;n:type:ShaderForge.SFN_Add,id:8092,x:32249,y:32723,varname:node_8092,prsc:2|A-3097-OUT,B-7588-UVOUT;n:type:ShaderForge.SFN_Slider,id:9665,x:31461,y:32526,ptovrint:False,ptlb:Horizontal Scroll,ptin:_HorizontalScroll,varname:node_9665,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:0.0607545,max:1;n:type:ShaderForge.SFN_Slider,id:3399,x:31461,y:32626,ptovrint:False,ptlb:Vertical Scroll,ptin:_VerticalScroll,varname:node_3399,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:0.04050335,max:1;n:type:ShaderForge.SFN_Color,id:8792,x:32716,y:32464,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_8792,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:3238,x:32944,y:32493,varname:node_3238,prsc:2|A-8792-RGB,B-1179-RGB;n:type:ShaderForge.SFN_Multiply,id:3994,x:33103,y:32804,varname:node_3994,prsc:2|A-1179-A,B-1025-A;n:type:ShaderForge.SFN_Rotator,id:5507,x:32447,y:32723,varname:node_5507,prsc:2|UVIN-8092-OUT,ANG-7746-OUT;n:type:ShaderForge.SFN_Slider,id:7746,x:32076,y:32978,ptovrint:False,ptlb:Rotation Angle,ptin:_RotationAngle,varname:node_7746,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:7;n:type:ShaderForge.SFN_Tex2d,id:1025,x:32864,y:32841,ptovrint:False,ptlb:Opacity Texture,ptin:_OpacityTexture,varname:node_1025,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:859073d2e990fd44a8c0ccb33ba5daf4,ntxv:0,isnm:False;proporder:1179-9665-3399-8792-7746-1025;pass:END;sub:END;*/

Shader "Plaid/PlaidPan with Rotate" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _HorizontalScroll ("Horizontal Scroll", Range(-1, 1)) = 0.0607545
        _VerticalScroll ("Vertical Scroll", Range(-1, 1)) = 0.04050335
        _Color ("Color", Color) = (1,1,1,1)
        _RotationAngle ("Rotation Angle", Range(0, 7)) = 0
        _OpacityTexture ("Opacity Texture", 2D) = "white" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles n3ds wiiu 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _HorizontalScroll;
            uniform float _VerticalScroll;
            uniform float4 _Color;
            uniform float _RotationAngle;
            uniform sampler2D _OpacityTexture; uniform float4 _OpacityTexture_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 projPos : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float2 sceneUVs = (i.projPos.xy / i.projPos.w);
////// Lighting:
////// Emissive:
                float node_5507_ang = _RotationAngle;
                float node_5507_spd = 1.0;
                float node_5507_cos = cos(node_5507_spd*node_5507_ang);
                float node_5507_sin = sin(node_5507_spd*node_5507_ang);
                float2 node_5507_piv = float2(0.5,0.5);
                float4 node_2963 = _Time;
                float2 node_5507 = (mul(((float2(_HorizontalScroll,_VerticalScroll)*node_2963.g)+float2((sceneUVs.x * 2 - 1)*(_ScreenParams.r/_ScreenParams.g), sceneUVs.y * 2 - 1).rg)-node_5507_piv,float2x2( node_5507_cos, -node_5507_sin, node_5507_sin, node_5507_cos))+node_5507_piv);
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_5507, _MainTex));
                float3 emissive = (_Color.rgb*_MainTex_var.rgb);
                float3 finalColor = emissive;
                float4 _OpacityTexture_var = tex2D(_OpacityTexture,TRANSFORM_TEX(i.uv0, _OpacityTexture));
                return fixed4(finalColor,(_MainTex_var.a*_OpacityTexture_var.a));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
