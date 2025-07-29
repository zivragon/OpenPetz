using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct BallInfo {
	public int Fuzz {get; set;} = 0;
	public int Diameter {get; set;} = 1;
	public int ColorIndex {get; set;} = 0;
	public int OutlineType {get; set;} = 0;
	public int OutlineColor {get; set;} = 0;
	public int TextureIndex {get; set;} = -1;
	public BallInfo(){}
}

public partial class Ball : MeshInstance2D
{
	private Mesh ballMesh;
	//To do: Merge this with this.Material
	private ShaderMaterial material;
	private List<PaintBallGroup> paintBallGroups = null;

	public Texture2D texture;
	public Texture2D palette;

	public TextureAtlas atlas {get; private set;} = null;
	
	public BallInfo Info {get; private set;} = new BallInfo();
	
	public Vector3 rotation = new Vector3(0.0f, 0.0f, 0.0f);

	private SubTextureCoordinations atlasCoords = new SubTextureCoordinations(0.0f, 0.0f, 1.0f, 1.0f); 

	public Ball()
	{

	}

	//public Ball(TextureAtlas _atlas, Texture2D texture, Texture2D palette, int diameter, int color_index, int fuzz, int outline_width, int outline_color)
	public Ball(TextureAtlas _atlas, BallInfo _info)
	{
		Info = _info;

		atlas = _atlas;
		
		this.ballMesh = MeshManager.FetchDefaultMesh();

		this.material = ShaderManager.FetchShaderMaterial("ball");

		this.Mesh = this.ballMesh;
		this.Material = this.material;
	}

	public override void _Ready()
	{
		//Set Material uniform parameters

		this.material.SetShaderParameter(StringManager.S("fuzz"), Info.Fuzz);
		this.material.SetShaderParameter(StringManager.S("diameter"), Info.Diameter);
		this.material.SetShaderParameter(StringManager.S("outline_width"), Info.OutlineType);

		this.material.SetShaderParameter(StringManager.S("outline_color"), Info.OutlineColor);
		
		this.material.SetShaderParameter(StringManager.S("tex"), TextureManager.FetchEmptyTexture());
		this.material.SetShaderParameter(StringManager.S("palette"), atlas.Palette);

		this.material.SetShaderParameter(StringManager.S("center"), this.GlobalPosition);
		
		SetTextureAtlas();
	}


	public override void _Process(double dt)
	{
		material.SetShaderParameter(StringManager.S("center"), this.GlobalPosition);
	}
	
	// CUSTOM METHODS
	
	public void AddPaintBalls(List<PaintBall> _paintBalls)
	{
		if (paintBallGroups == null)
			paintBallGroups = new List<PaintBallGroup>();
		
		var pbg = new PaintBallGroup(atlas, this, _paintBalls);
		paintBallGroups.Add(pbg);
		AddChild(pbg);
	}
	
	public void SetTextureAtlas()
	{
		//atlas = _atlas;
		
		if (atlas.TextureData != null)
		{
			atlasCoords = atlas.GetSubTextureCoords(0, Info.ColorIndex);

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
