using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class PaintBallGroup: MeshInstance2D
{
    private Ball baseBall = null;
    private List <PaintBall> paintBallz = null;
    private Mesh meshBuffer = null;
    private Material material = null;
    

    PaintBallGroup(Ball _base, List < PaintBall > _paintBallz) 
    {
        baseBall = _base;
        paintBallz = _paintBallz;

        Position = new Vector2(0.0, 0.0);
        Rotation = new Vector3(0.0, 0.0, 0.0);
    }

    public override void _Ready() 
    {
        material = ShaderManager.FetchShaderMaterial("paintball");
        
        this.Material = material;
        
        var st = new SurfaceTool();

        st.SetColorFormat(0, SurfaceTool.CustomFormat.CustomRFloat);

        st.Begin(Mesh.PrimitiveType.Triangles);

        foreach (paintBall in paintBallz)
        {
            // "color" (it is infact, not color)
            var info = new Color(paintBall.Size, paintBall.ColorIndex, 0.0, 0.0);
            
            st.SetNormal(paintBall.Coordinations);
            st.SetCustom(0, info);
            st.AddVertex(new Vector3(-1, -1, 0));
            
            st.SetNormal(paintBall.Coordinations);
            st.SetCustom(0, info);
            st.AddVertex(new Vector3(-1, 1, 0));
            
            st.SetNormal(paintBall.Coordinations);
            st.SetCustom(0, info);
            st.AddVertex(new Vector3(1, 1, 0));
            
            st.SetNormal(paintBall.Coordinations);
            st.SetCustom(0, info);
            st.AddVertex(new Vector3(-1, -1, 0));
            
            st.SetNormal(paintBall.Coordinations);
            st.SetCustom(0, info);
            st.AddVertex(new Vector3(1, -1, 0));
            
            st.SetNormal(paintBall.Coordinations);
            st.SetCustom(0, info);
            st.AddVertex(new Vector3(1, 1, 0));

        }
        // Commit to a mesh.
        meshBuffer = st.Commit();
        
        this.Mesh = meshBuffer;
        
        //Set uniform variables
        this.material.SetShaderParameter("fuzz", baseBall.fuzz);
		this.material.SetShaderParameter("diameter", baseBall.diameter);

		this.material.SetShaderParameter("color_index", 95);
		material.SetShaderParameter("transparent_color_index", 0);

		this.material.SetShaderParameter("tex", baseBall.texture);
		this.material.SetShaderParameter("palette", baseBall.palette);

		this.material.SetShaderParameter("center", this.GlobalPosition);
    }
    
    public override void _Process()
    {
        this.material.SetShaderParameter("center", this.GlobalPosition);
    }
}