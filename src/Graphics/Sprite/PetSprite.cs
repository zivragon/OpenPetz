using Godot;
using System;
using System.Collections.Generic;
using OpenPetz;

//To Do: re-think if this class should inherit from Node2D
public partial class PetSprite : Sprite3D
{
	private Pet parent = null;

	//this member is temporary 
	private string[] texturePaths = new string[] { /*"./art/textures/flower.bmp"*/ "./Resource/textures/ziverre/ribbon.bmp" };
	
	private List<Texture2D> textureList = new List<Texture2D>();

	//Methods

	public PetSprite (Pet _p){
		parent = _p;
		
		Texture2D palette = PaletteManager.FetchPalette("oddballz");
		textureAtlas = new TextureAtlas(palette, Guid.Empty, null);

		AddChild(textureAtlas);
		
		Visible = false;
	}
	
	public override void _Ready()
	{
		Rotation3D.Y = (float)(1.57/2.0); 
		
		LoadTextures();
		
		RenderingServer.FramePostDraw += SetupSprite;
	}

	public override void _Process(double delta)
	{
		Rotation3D.Y += 0.05f;
		foreach (var ball in BallzList){
			ball.Rotation3D = Rotation3D;
		}
		
		UpdateGeometries();
	}

	// CUSTOM Methods
	
	public void SetFrame(BallzModel.Frame frame){
		currentFrame = frame;
	}
	
	private void SetupSprite()
	{
		for (int i = 0; i < 67; i++)
		{
			int color = 10 + (i % 2) * 80;
			
			Ball dummyBall = new Ball(textureAtlas, new BallParams {
				Diameter = parent.catBhd.GetDefaultBallSize(i),// / 2,
				ColorIndex = color,
				Fuzz = 4,
				OutlineType = 1,
				OutlineColor = 39,
				TextureIndex = -1
			});

			Vector2 dummyCoord = new Vector2(0.0f, 0.0f);
			
			dummyBall.Position = dummyCoord;

			dummyBall.ZIndex = (int)0;

			//add them to the lists
			BallzList.Add(dummyBall);
			AddChild(dummyBall);
			
			/*List<PaintBall> pbz = new List<PaintBall> (); //store ballz
			pbz.Add(new PaintBall(new Vector3(0.0f, 0.0f, -1.0f), 0.3f, pcolor));
			pbz.Add(new PaintBall(new Vector3(0.0f, 0.0f, 1.0f), 0.4f, pcolor));
			pbz.Add(new PaintBall(new Vector3(0.0f, 1.0f, 0.0f), 0.5f, pcolor));
			pbz.Add(new PaintBall(new Vector3(0.0f, -1.0f, 0.0f), 0.4f, pcolor));
			pbz.Add(new PaintBall(new Vector3(1.0f, 0.0f, 0.0f), 0.3f, pcolor));
			pbz.Add(new PaintBall(new Vector3(-1.0f, 0.0f, 0.0f), 0.4f, pcolor));
			
			dummyBall.AddPaintBalls(pbz);*/
		}
		
		List<Vector2I> arr = new List<Vector2I>();
		arr.Add(new Vector2I(6,36));
		arr.Add(new Vector2I(2,3));
		arr.Add(new Vector2I(6,2));
		arr.Add(new Vector2I(8,9));
		arr.Add(new Vector2I(10,11));
		arr.Add(new Vector2I(3, 43));
		arr.Add(new Vector2I(43,44));
		arr.Add(new Vector2I(44,45));	
		arr.Add(new Vector2I(45,46));	
		arr.Add(new Vector2I(46,47));	
		arr.Add(new Vector2I(47,48));
		arr.Add(new Vector2I(32,0));
		arr.Add(new Vector2I(33,1));
		arr.Add(new Vector2I(41,0));
		arr.Add(new Vector2I(42,1));
		arr.Add(new Vector2I(25,32));
		arr.Add(new Vector2I(26,33));
		arr.Add(new Vector2I(12,63));
		arr.Add(new Vector2I(13,64));
		arr.Add(new Vector2I(12,38));
		arr.Add(new Vector2I(13,39));
		arr.Add(new Vector2I(55,56));
		
		foreach (var membs in arr)
		{
			var dummyLine = new Line(this, textureAtlas, new LineParams {
				Start = BallzList[membs.X],
				End = BallzList[membs.Y]
			});
			
			/*LinezList.Add(dummyLine);
			AddChild(dummyLine);*/
		}
		
		Visible = true;
		
		RenderingServer.FramePostDraw -= SetupSprite;
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
	
		for (int index = 0; index < BallzList.Count; index++)
		{

			var orien = frame.BallOrientation(index);
			
			var rotMat = Rotator.Rotate3D(orien.Position, Rotation3D);

			Vector2 v = new Vector2(rotMat.X, rotMat.Y);

			BallzList[index].Position = v;
			//Since Godot renders Nodes with highest Z on top of others unlike original petz l, we set negative of it
			BallzList[index].ZIndex = (int)-rotMat.Z;
		}
	}
	
	private void UpdateLinez(){
		foreach (Line line in LinezList){
			line.ZIndex = Math.Min(line.Info.Start.ZIndex, line.Info.End.ZIndex) - 1;
		}
	}

}
