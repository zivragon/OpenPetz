// *_*
// TO DO: REFACTOR THIS 
// REFACTOR REFACTOR **REFACTOR**

using Godot;
using System;
using System.Collections.Generic;

public struct LineInfo {
	public Ball Start {get; set;} = null;
	public Ball End {get; set;} = null;
	public LineInfo(){}
}
public partial class Line : MeshInstance2D
{
	public TextureAtlas atlas {get; private set;} = null;
	
	public LineInfo Info {get; private set;} = new LineInfo();
	
	public bool Renderable {get; private set;} = false;
	
	//Do I have to...? GODOT?
	private Godot.Collections.Array<Vector2> coords = new Godot.Collections.Array<Vector2>();
	private Godot.Collections.Array<float> diameters = new Godot.Collections.Array<float>();
	
	private Mesh lineMesh;
	//To do: Merge this with this.Material
	private ShaderMaterial material;
	
	private SubTextureCoordinations atlasCoords = new SubTextureCoordinations(0.0f, 0.0f, 1.0f, 1.0f); 
	
	public Line(TextureAtlas _atlas, LineInfo _info)
	{
		Visible = false;
		if (_info.Start == null || _info.End == null)
			return;
		
		Info = _info;

		atlas = _atlas;
		
		this.lineMesh = MeshManager.FetchDefaultLineMesh();

		this.material = ShaderManager.FetchShaderMaterial("line");

		this.Mesh = this.lineMesh;
		this.Material = this.material;
		
		Renderable = true;
	}	
	
	public override void _Ready()
	{
		if (!Renderable)
			return;
		
		coords.Add(Info.Start.Position);
		coords.Add(Info.End.Position);
		
		diameters.Add(Info.Start.Info.Diameter);
		diameters.Add(Info.End.Info.Diameter);
		
		this.material.SetShaderParameter("ball_coords", coords);
		this.material.SetShaderParameter("ball_diameters", diameters);
		this.material.SetShaderParameter("angle_to", Info.Start.Position.AngleToPoint(Info.End.Position));
		
		this.material.SetShaderParameter(StringManager.S("palette"), atlas.Palette);
		
		SetTextureAtlas();
		
		Visible = true;
	}
	
	public override void _Process(double dt)
	{
		if (!Renderable)
			return;
		
		coords[0] = Info.Start.Position;
		coords[1] = Info.End.Position;
		
		this.material.SetShaderParameter("ball_coords", coords);
		this.material.SetShaderParameter("angle_to", Info.Start.Position.AngleToPoint(Info.End.Position));
	}
	
	public void SetTextureAtlas()
	{
		//atlas = _atlas;
		if (!Renderable)
			return;
		
		if (atlas.TextureData != null)
		{
			atlasCoords = atlas.GetSubTextureCoords(0, Info.Start.Info.ColorIndex);

   			this.material.SetShaderParameter(StringManager.S("atlas_position"), atlasCoords.Position);
      		this.material.SetShaderParameter(StringManager.S("atlas_size"), atlasCoords.Size);
			this.material.SetShaderParameter(StringManager.S("tex"), atlas.TextureData);
		} else {
     		this.material.SetShaderParameter(StringManager.S("atlas_position"), new Vector2(0.0f, 0.0f));
      		this.material.SetShaderParameter(StringManager.S("atlas_size"), new Vector2(1.0f, 1.0f));
			this.material.SetShaderParameter(StringManager.S("tex"), TextureManager.FetchEmptyTexture());
		}
	}
}