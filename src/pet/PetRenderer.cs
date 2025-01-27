using Godot;
using System;
using System.Collections.Generic;
using OpenPetz;

//To Do: re-think if this class should inherit from Node2D
public partial class PetRenderer : Node2D
{
	public Vector3 rotation = new Vector3(0, 0, 0);

	//Geometry containers
	private List<Ball> ballz = new List<Ball> (); //store ballz
	private List<Line> linez = new List<Line> (); //store ballz

	//this member is temporary 
	private string[] texturePaths = new string[] { /*"./art/textures/flower.bmp"*/ "./Resource/textures/ziverre/ribbon.bmp" };
	
	private List<Texture2D> textureList = new List<Texture2D>();

	private Texture2D palette;
	private Bhd catBhd;
	private Bhd.FrameGroup animation;
	private int currentFrame = 0;

	private TextureAtlas textureAtlas = null;
	//Methods


	Lnz linezData = new Lnz();

	public override void _Ready()
	{
		
		catBhd = AnimationManager.FetchCatBhd();
		animation = catBhd.GetAnimation(0);
		
		linezData.Parse("./Resource/linez/calico-petz3.lnz"); // @todo configure the file
		
		var frame = animation.m_Frames[currentFrame];
		
		LoadTextures();
		//Prepare the Textures
		var texture = textureList[0];

		Texture2D palette = PaletteManager.FetchPalette("petz");
		
		//Ignore until texture atlas is implemented
		/*textureAtlas = new TextureAtlas();
		
		AddChild(textureAtlas);*/

		//Create dummy ballz for now.
		for (int i = 0; i < catBhd.NumBallz; i++)
		{
			var orien = frame.BallOrientation(i);

			Lnz.BallzInfo ballInfo = linezData.BallzInfoz[i];
			int color = ballInfo.Color;// 40;
			
			Ball dummyBall = new Ball(texture, palette, (int)catBhd.GetDefaultBallSize(i), color, (int)ballInfo.Fuzz, 1, ballInfo.OutlineColor);

			Vector2 dummyCoord = new Vector2(orien.Position.X, orien.Position.Y);
			
			dummyBall.Position = dummyCoord;

			dummyBall.ZIndex = (int)-orien.Position.Z;

			//add them to the lists
			this.ballz.Add(dummyBall);
			AddChild(dummyBall);
			
			List <PaintBall> paintBallz = new List<PaintBall>();
			foreach(var pb in linezData.PaintBallz)
			{
				if (pb.BaseBall == i)
				{
					paintBallz.Add(new PaintBall(new Vector3(pb.Direction.X, pb.Direction.Y, pb.Direction.Z),
						pb.Diameter, pb.Color));
				}
			}
			PaintBallGroup pbg = new PaintBallGroup(dummyBall, paintBallz);
			dummyBall.AddChild(pbg);
		}

		//ignore for now
		foreach(var line in linezData.Linez)
		{
			if (line.StartBall >= this.ballz.Count)
			{
				GD.Print(String.Format("Line.StartBall index out of range. {0}/{1}",line.StartBall,this.ballz.Count));
				continue;
			}
			
			if (line.EndBall >= this.ballz.Count)
			{
				GD.Print(String.Format("Line.EndBall index out of range. {0}/{1}",line.EndBall,this.ballz.Count));
				continue;
			}
			
			Line newLine = new Line(
				null, 
				null, 
				this.ballz[line.StartBall],
				this.ballz[line.EndBall],
				line.Color, // -1,
				1,
				line.RightOutlineColor,//39,
				line.LeftOutlineColor//39
			);

			//add them to the lists
			this.linez.Add(newLine);
			AddChild(newLine);
		}
	}

	public override void _Process(double delta)
	{
		rotation.Y += (float)0.05; 
		UpdateGeometries();
	}

	// CUSTOM Methods

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

		currentFrame += 0;
		GD.Print(animation.NumFrames);
		
		if (currentFrame >= animation.NumFrames)
			currentFrame = 0;
		
		var frame = animation.m_Frames[currentFrame];
	
		float rYSin = (float)Math.Sin(rotation.Y);
		float rYCos = (float)Math.Cos(rotation.Y);
		
		float rZSin = (float)Math.Sin(rotation.Z);
		float rZCos = (float)Math.Cos(rotation.Z);
		
		for (int index = 0; index < this.ballz.Count; index++)
		{

			var orien = frame.BallOrientation(index);

			float xf = orien.Position.X;
			float yf = orien.Position.Y;
			float zf = orien.Position.Z;
			
			float zz = zf;

			zf = (zz * rYCos) - (xf * rYSin);
			xf = (xf * rYCos) + (zz * rYSin);
			
			float yf2 = (yf * rZCos) - (xf * rZSin);
			float xf2 = (xf * rZCos) + (yf * rZSin);

			float z = (float)Math.Round(zf);
			float y = (float)Math.Round(yf2);
			float x = (float)Math.Round(xf2);

			Vector2 v = new Vector2(x, y);

			ballz[index].Position = v;
			//Since Godot renders Nodes with highest Z on top of others unlike original petz l, we set negative of it
			this.ballz[index].ZIndex = (int)-z;
		}
	}
	
	private void UpdateLinez(){
		foreach (Line line in this.linez){
			line.ZIndex = Math.Min(line.start.ZIndex, line.end.ZIndex) - 1;
		}
	}

}
