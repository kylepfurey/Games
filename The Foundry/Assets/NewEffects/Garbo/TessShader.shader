Shader "Custom/TessShader" {
  //  ...
        SubShader{
        //    ...
            Pass {
               // ...
                #pragma target 5.0 // 5.0 required for tessellation
              //  ...
                #pragma vertex Vertex
                #pragma hull Hull
                #pragma domain Domain
                #pragma fragment Fragment
           //     ...
            }
    }

        struct Attributes
    {
        float3 positionOS : POSITION;
        float3 normalOS : NORMAL;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    struct TessellationControlPoint
    {
        float3 positionWS : INTERNALTESSPOS;
        float3 normalWS : NORMAL;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    TessellationControlPoint Vertex(Attributes input)
    {
        TessellationControlPoint output;

        UNITY_SETUP_INSTANCE_ID(input);
        UNITY_TRANSFER_INSTANCE_ID(input, output);

        VertexPositionInputs posnInputs = GetVertexPositionInputs(input.positionOS);
        VertexNormalInputs normalInputs = GetVertexNormalInputs(input.normalOS);

        output.positionWS = posnInputs.positionWS;
        output.normalWS = normalInputs.normalWS;
        return output;
    }


    // The hull function runs once per vertex. You can use it to modify vertex
    // data based on values in the entire triangle
    [domain("tri")] // Signal we're inputting triangles
    [outputcontrolpoints(3)] // Triangles have three points
    [outputtopology("triangle_cw")] // Signal we're outputting triangles
    [patchconstantfunc("PatchConstantFunction")] // Register the patch constant function
    [partitioning("integer")] // Select a partitioning mode: integer, fractional_odd, fractional_even or pow2
    TessellationControlPoint Hull(
        InputPatch<TessellationControlPoint, 3> patch, // Input triangle
        uint id : SV_OutputControlPointID)
    { // Vertex index on the triangle

        return patch[id];
    }


    struct TessellationFactors
    {
        float edge[3] : SV_TessFactor;
        float inside : SV_InsideTessFactor;
        float3 bezierPoints[NUM_BEZIER_CONTROL_POINTS] : BEZIERPOS;
    };
}