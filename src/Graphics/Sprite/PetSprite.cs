using Godot;
using System;
using System.Collections.Generic;
using OpenPetz;

//To Do: re-think if this class should inherit from Node2D
public partial class PetSprite : Sprite3D
{
	private Pet parent = null;
	
	private Vector3 HeadRotation = new Vector3(0f, 0f, 0f);

	//Methods

	public PetSprite (Pet _p){
		parent = _p;
		
		List<TextureParams> textureList = new List<TextureParams>();
		textureList.Add(new TextureParams {
			Path = "./art/textures/trianglespink.bmp"
		});
		textureList.Add(new TextureParams {
			Path = "./art/textures/bee2.bmp"
		});
		textureList.Add(new TextureParams {
			Path = "./art/textures/wizard.bmp"
		});
		textureList.Add(new TextureParams {
			Path = "./art/textures/bubblesb.bmp"
		});
		textureList.Add(new TextureParams {
			Path = "./art/textures/quiltblue.bmp"
		});
		
		Texture2D palette = PaletteManager.FetchPalette("catz");
		textureAtlas = new TextureAtlas(palette, Guid.Empty, textureList);

		AddChild(textureAtlas);
		
		Visible = false;
	}
	
	public override void _Ready()
	{
		HeadRotation.Y = -3.14f - 1.57f;
		Rotation3D.Y = 3.14f + 1.57f; 
		
		RenderingServer.FramePostDraw += SetupSprite;
	}

	public override void _Process(double delta)
	{
		//HeadRotation.Y -= 0.1f;
		//Rotation3D.Y += 0.1f;
		foreach (var ball in BallzList){
			ball.Rotation3D = Rotation3D;
		}
		
		UpdateGeometries();
	}

	// CUSTOM Methods
	
	public void SetFrame(BallzModel.Frame frame){
		currentFrame = frame;
	}
	
	public void SetHeadRotation(Vector3 _rotation)
	{
		HeadRotation = _rotation;
	}
	
	public void PointHeadAt(Vector2 _point)
	{
		//HeadRotation = _rotation;
	}
	
	private void SetupSprite()
	{
		//shamelessly stolen from Catz 1 :)
		//int[] colors = {102,102,82,102,102,102,72,42,82,72,82,112,82,72,15,15,72,72,72,112,112,112,82,72,82,82,82,112,112,102,82,102,102,72,72,102,102,112,82,102,42,82,82,112,72,82,112,82,42,112,112,112,42,42,42,118,118,0,0,0,0,0,0,72,112,9,9};
		int[] colors = {32,32,72,72,72,72,72,32,72,32,72,32,72,72,15,15,32,32,32,32,32,32,32,32,72,72,72,0,0,72,32,32,72,72,32,32,72,0,72,72,72,32,32,72,72,72,72,28,34,32,32,32,32,32,32,18,118,0,0,0,0,0,0,32,32,7,7};
		
		GD.Print(colors.Length);
		
		
		for (int i = 0; i < 67; i++)
		{
			int color = colors[i];
			int pcolor = 110;
			int tex = -1;
			
			Ball dummyBall = new Ball(textureAtlas, new BallParams {
				Diameter = parent.catBhd.GetDefaultBallSize(i),// / 2,
				ColorIndex = color,
				Fuzz = 4,
				OutlineType = 1,
				OutlineColor = 0,
				TextureIndex = tex
			});

			Vector2 dummyCoord = new Vector2(0.0f, 0.0f);
			
			dummyBall.Position = dummyCoord;

			dummyBall.ZIndex = (int)0;

			//add them to the lists
			BallzList.Add(dummyBall);
			AddChild(dummyBall);
			
			/*List<PaintBall> pbz = new List<PaintBall> (); //store ballz
			pbz.Add(new PaintBall(new PaintBallParams {
				Diameter = 30,
				Direction = new Vector3(0.0f, 0.0f, -1.0f),
				ColorIndex = pcolor
			}));
			pbz.Add(new PaintBall(new PaintBallParams {
				Diameter = 30,
				Direction = new Vector3(0.0f, 0.0f, 1.0f),
				ColorIndex = pcolor
			}));
			
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
				End = BallzList[membs.Y],
				LeftColor = 0,
				RightColor = 0
			});
			
			/*LinezList.Add(dummyLine);
			AddChild(dummyLine);*/
		}
		
		Visible = true;
		
		RenderingServer.FramePostDraw -= SetupSprite;
	}

	//NOTE: Order of updating matters!
	private void UpdateGeometries(){
		if (!Visible)
			return;
		
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
		
		int[] headballz = {4, 5, 7, 8, 9, 10, 11, 14, 15, 27, 28, 29, 24, 30, 31, 36, 37, 40, 55, 56, 57, 58, 59, 60, 61, 62};
		
		foreach (var index in headballz)
		{		
			var headball = new Vector3(BallzList[index].Position.X, BallzList[index].Position.Y, -BallzList[index].ZIndex);
			var chestball = new Vector3(BallzList[6].Position.X, BallzList[6].Position.Y, -BallzList[6].ZIndex);
			
			var rotMat2 = Rotator.Rotate3D(headball - chestball, HeadRotation);
			
			rotMat2 += chestball;
			
			Vector2 v2 = new Vector2(rotMat2.X, rotMat2.Y);
			BallzList[index].Position = v2;
			//Since Godot renders Nodes with highest Z on top of others unlike original petz l, we set negative of it
			BallzList[index].ZIndex = (int)-rotMat2.Z;
		}
		
	}
	
	private void UpdateLinez(){
		foreach (Line line in LinezList){
			line.ZIndex = Math.Min(line.Info.Start.ZIndex, line.Info.End.ZIndex) - 1;
		}
	}

}
