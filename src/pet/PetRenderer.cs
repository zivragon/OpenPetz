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
	private List<Ball> addBallz = new List<Ball> ();
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
		
		for (int i = 0; i < catBhd.NumBallz; i++)
		{
			var orien = frame.BallOrientation(i);

			Lnz.BallzInfo ballInfo = linezData.BallzInfoz[i];
			int color = ballInfo.Color;// 40;
			
			Ball dummyBall = new Ball(i, texture, palette, (int)catBhd.GetDefaultBallSize(i), color, (int)ballInfo.Fuzz, 1, ballInfo.OutlineColor);

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

		for (var index = 0; index < linezData.AddBallz.Count; index++)
		{
			var addBall = linezData.AddBallz[index];
			var parent = this.ballz[addBall.ParentBallID];
			int addBallId = (int)catBhd.NumBallz + index;

			if (linezData.IsOmmited(addBallId))
			{
				continue;
			}

			Ball dummyBall = new Ball(
					addBallId, texture, palette, (int)addBall.BallSize, addBall.Color, (int)addBall.Fuzz,
					addBall.Outline, addBall.OutlineColor
				);
			dummyBall.Position = parent.Position + new Vector2(addBall.Offset.X, addBall.Offset.Y);
			dummyBall.ZIndex = (int)-addBall.Offset.Z;
			this.addBallz.Add(dummyBall);
			AddChild(dummyBall);
		}

		foreach(var line in linezData.Linez)
		{
			// Either ball or add ball
			
			Ball startBall = ballz.Find(ball => ball.id == line.StartBall);
			if (startBall == null)
			{
				startBall = addBallz.Find(ball => ball.id == line.StartBall);
			}
			
			Ball endBall = ballz.Find(ball => ball.id == line.EndBall);
			if (endBall == null)
			{
				endBall = addBallz.Find(ball => ball.id == line.EndBall);
			}

			// omitted?
			
			if (startBall == null || endBall == null)
			{
				continue;
			}
			
			Line newLine = new Line(
				null, 
				null, 
				startBall,
				endBall,
				line.Color, // -1,
				1,
				line.RightOutlineColor,//39,
				line.LeftOutlineColor//39
			);

			//add them to the lists
			this.linez.Add(newLine);
			AddChild(newLine);
		}

		//ignore for now
		/*for (int l = 0; l < 2; l++)
		{

			Line dummyLine = new Line(null, null, this.ballz[l], this.ballz[l + 1], -1, 1, 39, 39);

			//add them to the lists
			this.linez.Add(dummyLine);
			AddChild(dummyLine);
		}*/
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
		UpdateAddBallz();
		UpdateLinez();
	}
	
	//To Do: implement the rotation vector math for x rotation
	private void UpdateMainBallz()
	{

		currentFrame += 1;
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
	
	private void UpdateAddBallz()
	{

		currentFrame += 1;
		GD.Print(animation.NumFrames);
		
		if (currentFrame >= animation.NumFrames)
			currentFrame = 0;
		
		var frame = animation.m_Frames[currentFrame];
	
		float rYSin = (float)Math.Sin(rotation.Y);
		float rYCos = (float)Math.Cos(rotation.Y);
		
		float rZSin = (float)Math.Sin(rotation.Z);
		float rZCos = (float)Math.Cos(rotation.Z);
		
		for (int index = 0; index < this.addBallz.Count; index++)
		{
			Ball addBall = this.addBallz[index];
			
			int addBallId = addBall.id - (int)catBhd.NumBallz; // @todo make this not so hacky
			
			Lnz.AddBall addBallInfo = linezData.AddBallz[addBallId];
			int parentBallID = linezData.AddBallz[addBallId].ParentBallID;

			var orien = frame.BallOrientation(parentBallID);

			float xf = orien.Position.X + addBallInfo.Offset.X;
			float yf = orien.Position.Y + addBallInfo.Offset.Y;
			float zf = orien.Position.Z + addBallInfo.Offset.Z;
			
			float zz = zf;

			zf = (zz * rYCos) - (xf * rYSin);
			xf = (xf * rYCos) + (zz * rYSin);
			
			float yf2 = (yf * rZCos) - (xf * rZSin);
			float xf2 = (xf * rZCos) + (yf * rZSin);

			float z = (float)Math.Round(zf);
			float y = (float)Math.Round(yf2);
			float x = (float)Math.Round(xf2);

			Vector2 v = new Vector2(x, y);

			this.addBallz[index].Position = v;
			//Since Godot renders Nodes with highest Z on top of others unlike original petz l, we set negative of it
			this.addBallz[index].ZIndex = (int)-z;
		}
	}

	
	private void UpdateLinez(){
		foreach (Line line in this.linez){
			line.ZIndex = Math.Min(line.start.ZIndex, line.end.ZIndex) - 1;
		}
	}

}
