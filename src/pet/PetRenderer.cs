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


	LnzData linezData = new LnzData();

	//
	// Ballz and AddBallz can be Ommited
	//

	Lnz.AddBall GetAddballInfo(int ballID)
	{
		// Assuming main ballz are never ommited
		
		int index = ballID - (int)catBhd.NumBallz; // @todo make this not so hacky

		return linezData.AddBallz[index];
	}

	Ball GetBallByID(int ballID)
	{
		Ball result = ballz.Find(ball => ball.id == ballID);
		
		if (result == null)
		{
			result = addBallz.Find(ball => ball.id == ballID);
		}

		return result;
	}

	public override void _Ready()
	{
		
		catBhd = AnimationManager.FetchCatBhd();
		animation = catBhd.GetAnimation(0);
		
		// @todo this file should be specified by [Default Linez File]
		// @todo this file has [Num Ballz]
		// @todo have a way for Parse to override, not only append
		linezData.ParseFile("./Resource/linez/catmaster.lnz"); // @todo configure the file
		//
		linezData.ParseFile("./Resource/linez/calico-petz3.lnz"); // @todo configure the file
		
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
			
			
			if (linezData.IsBallOmmited(i))
			{
				continue;
			}
			
			var orien = frame.BallOrientation(i);

			Lnz.BallzInfo ballInfo = linezData.BallzInfoz[i];
			
			Ball dummyBall = new Ball(i, texture, palette, (int)catBhd.GetDefaultBallSize(i), ballInfo.Color, (int)ballInfo.Fuzz, 1, ballInfo.OutlineColor);

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
			var addBallInfo = linezData.AddBallz[index];
			var parentBall = this.ballz[addBallInfo.ParentBallID];
			int addBallId = (int)catBhd.NumBallz + index;

			if (linezData.IsBallOmmited(addBallId))
			{
				continue;
			}

			Ball dummyBall = new Ball(
					addBallId, texture, palette, (int)addBallInfo.BallSize, addBallInfo.Color, (int)addBallInfo.Fuzz,
					addBallInfo.Outline, addBallInfo.OutlineColor
				);
			dummyBall.Position = parentBall.Position + new Vector2(addBallInfo.Offset.X, addBallInfo.Offset.Y);
			dummyBall.ZIndex = (int)-addBallInfo.Offset.Z;
			this.addBallz.Add(dummyBall);
			AddChild(dummyBall);
		}

		foreach (var whisker in linezData.Whiskers)
		{
			// Either ball or add ball
			
			Ball startBall = GetBallByID( whisker.StartBallID );
			Ball endBall = GetBallByID( whisker.EndBallID );

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
				whisker.Color,
				1,
				-1,
				-1
			);

			//add them to the lists
			this.linez.Add(newLine);
			AddChild(newLine);
		}

		foreach(var line in linezData.Linez)
		{
			// Either ball or add ball
			
			Ball startBall = GetBallByID( line.StartBall );
			Ball endBall = GetBallByID( line.EndBall);

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

		currentFrame += 1;
		GD.Print(animation.NumFrames);
		
		if (currentFrame >= animation.NumFrames)
			currentFrame = 0;

		UpdateMainBallz();
		UpdateAddBallz();
		UpdateLinez();
	}
	
	//To Do: implement the rotation vector math for x rotation
	private void UpdateMainBallz()
	{
		var frame = animation.m_Frames[currentFrame];
	
		float rYSin = (float)Math.Sin(rotation.Y);
		float rYCos = (float)Math.Cos(rotation.Y);
		
		float rZSin = (float)Math.Sin(rotation.Z);
		float rZCos = (float)Math.Cos(rotation.Z);
		
		for (int index = 0; index < this.ballz.Count; index++)
		{

			var orien = frame.BallOrientation(this.ballz[index].id);

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

		GD.Print(animation.NumFrames);
		
		var frame = animation.m_Frames[currentFrame];
	
		float rYSin = (float)Math.Sin(rotation.Y);
		float rYCos = (float)Math.Cos(rotation.Y);
		
		float rZSin = (float)Math.Sin(rotation.Z);
		float rZCos = (float)Math.Cos(rotation.Z);
		
		foreach (var addBall in this.addBallz)
		{
			Lnz.AddBall addBallInfo = GetAddballInfo(addBall.id);
			
			var orien = frame.BallOrientation(addBallInfo.ParentBallID);

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

			addBall.Position = v;
			//Since Godot renders Nodes with highest Z on top of others unlike original petz l, we set negative of it
			addBall.ZIndex = (int)-z;
		}
	}

	
	private void UpdateLinez(){
		foreach (Line line in this.linez){
			line.ZIndex = Math.Min(line.start.ZIndex, line.end.ZIndex) - 1;
		}
	}

}
