using Godot;
using System;
using System.Collections.Generic;
using OpenPetz;

//To Do: re-think if this class should inherit from Node2D
public partial class PetRenderer : Node2D
{
	private Pet parent = null;
	
	public Vector3 rotation = new Vector3(0, 0, 0);
	
	BallzModel.Frame currentFrame = null;

	//Geometry containers
	private List<Ball> ballz = new List<Ball> (); //store ballz
	private List<Line> linez = new List<Line> (); //store ballz

	//this member is temporary 
	private string[] texturePaths = new string[] { /*"./art/textures/flower.bmp"*/ "./Resource/textures/ziverre/ribbon.bmp" };
	
	private List<Texture2D> textureList = new List<Texture2D>();

	private TextureAtlas textureAtlas = null;
	//Methods

	public PetRenderer (Pet p){
		parent = p;
	}
	
	public override void _Ready()
	{
		LoadTextures();
		//Prepare the Textures
		var texture = textureList[0];

		Texture2D palette = PaletteManager.FetchPalette("petz");
		
		textureAtlas = new TextureAtlas(Guid.Empty, null);

		//Create dummy ballz for now.
		for (int i = 0; i < 67; i++)
		{
			//var orien = frame.BallOrientation(i);
			int color = 40;
			
			Ball dummyBall = new Ball(texture, palette, parent.catBhd.GetDefaultBallSize(i) / 2, color, 4, 1, 39);

			Vector2 dummyCoord = new Vector2(0.0f, 0.0f);
			
			dummyBall.Position = dummyCoord;

			dummyBall.ZIndex = (int)0;

			//add them to the lists
			this.ballz.Add(dummyBall);
			AddChild(dummyBall);
		}
	}

	public override void _Process(double delta)
	{
		rotation.Y = (float)1.57; 
		UpdateGeometries();
	}

	// CUSTOM Methods

	public void SetFrame(BallzModel.Frame frame){
		currentFrame = frame;
	}
	
	private void LoadTextures(){
		//start with adding the empty texture for the sake of texture index of -1
		textureList.Add(TextureManager.FetchEmptyTexture());
		
		foreach (string texturePath in texturePaths)
		{
			Texture2D fetchedTexture = TextureManager.FetchTexture(texturePath);
			
			textureList.Add(fetchedTexture);
		}
	}

	//NOTE: Order of updating matters!
	private void UpdateGeometries(){
		UpdateMainBallz();
		UpdateLinez();
	}
	
	//To Do: implement the rotation vector math for x rotation
	private void UpdateMainBallz()
	{
		if (currentFrame == null)
            return;
		
		var frame = currentFrame;
	
		for (int index = 0; index < this.ballz.Count; index++)
		{

			var orien = frame.BallOrientation(index);
			
			var rotMat = Rotator.Rotate3D(orien.Position, rotation);

			Vector2 v = new Vector2(rotMat.X, rotMat.Y);

			ballz[index].Position = v;
			//Since Godot renders Nodes with highest Z on top of others unlike original petz l, we set negative of it
			ballz[index].ZIndex = (int)-rotMat.Z;
		}
	}
	
	private void UpdateLinez(){
		foreach (Line line in this.linez){
			line.ZIndex = Math.Min(line.start.ZIndex, line.end.ZIndex) - 1;
		}
	}

}
