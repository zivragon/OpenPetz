using Godot;
using System;
using System.Collections.Generic;

using OpenPetz;

public partial class Pet : LinezObject
{
	private PetSprite petSprite;

	private Texture2D palette;
	public BallzModel catBhd;
	private BallzModel.FrameGroup animation;
	private int currentFrame = 0;
	
	//public Vector3 Rotation3D = new Vector3(-0.125f, 0.0f, 0.0f);
	
	public Pet()
	{
		
	}

	public override void _Ready()
	{
		//temp
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(45,	244,	0,	1,	-1,	-3,	3,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(45,	244,	0,	1,	-1,	-3,	3,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(45,	244,	0,	2,	-1,	-5,	1,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(45,	244,	0,	2,	-1,	-12,	3,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(45,	244,	0,	2,	-2,	-16,	0,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(45,	244,	0,	2,	0,	-16,	0,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(45,	244,	0,	3,	-1,	3,	1,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(45,	244,	0,	0,	1,	-2,	0,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(45,	244,	0,	1,	-1,	-22,	0,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(35,	244,	0,	1,	-1,	15,	2,	0));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(45,	244,	0,	1,	-1,	-22,	0,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(35,	244,	0,	1,	-1,	15,	2,	0));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(45,	244,	0,	1,	-1,	-8,	1,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(45,	244,	0,	1,	-1,	-8,	1,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(201,	244,	0,	0,	1,	3,	-1,	-1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(201,	244,	0,	0,	1,	3,	-1,	-1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(35,	244,	0,	0,	-2,	7,	2,	0));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(35,	244,	0,	0,	0,	7,	2,	0));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(35,	244,	0,	0,	-1,	7,	2,	0));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(35,	244,	0,	0,	0,	7,	2,	0));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(35,	244,	0,	0,	-2,	7,	2,	0));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(35,	244,	0,	0,	-1,	7,	2,	0));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(35,	244,	0,	0,	0,	4,	2,	0));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(35,	244,	0,	0,	0,	4,	2,	0));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(45,	244,	0,	1,	-1,	-9,	0,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(45,	244,	0,	1,	-1,	-21,	3,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(45,	244,	0,	1,	-1,	-21,	3,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(244,	203,	0,	0,	3,	9,	-1,	-1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(244,	203,	0,	0,	3,	9,	-1,	-1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(45,	244,	0,	0,	-1,	19,	0,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(35,	244,	0,	1,	1,	-9,	2,	0));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(35,	244,	0,	1,	1,	-9,	2,	0));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(45,	244,	0,	1,	0,	-14,	3,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(45,	244,	0,	1,	-2,	-14,	3,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(35,	244,	0,	0,	0,	-2,	2,	0));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(35,	244,	0,	0,	0,	-2,	2,	0));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(45,	244,	0,	1,	-1,	8,	0,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(244,	244,	0,	0,	-1,	-5,	-1,	0));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(45,	244,	0,	1,	-1,	-2,	1,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(45,	244,	0,	1,	-1,	-2,	1,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(45,	244,	0,	0,	-1,	-12,	0,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(35,	244,	0,	0,	0,	-5,	2,	0));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(35,	244,	0,	0,	0,	-5,	2,	0));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(45,	244,	0,	1,	-1,	-7,	4,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(35,	244,	0,	1,	-1,	-9,	2,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(35,	244,	0,	1,	-1,	-11,	2,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(35,	244,	0,	1,	-1,	-9,	2,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(35,	244,	0,	1,	-1,	-8,	2,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(35,	244,	0,	1,	-1,	-6,	2,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(35,	244,	0,	0,	-2,	5,	2,	0));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(35,	244,	0,	0,	0,	5,	2,	0));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(35,	244,	0,	0,	-1,	5,	2,	0));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(35,	244,	0,	0,	0,	5,	2,	0));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(35,	244,	0,	0,	-2,	5,	2,	0));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(35,	244,	0,	0,	-1,	5,	2,	0));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(75,	244,	0,	0,	-1,	5,	-1,	0));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(75,	244,	0,	0,	-1,	5,	-1,	0));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(25,	244,	0,	0,	0,	-15,	-1,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(25,	244,	0,	0,	0,	-15,	-1,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(25,	244,	0,	0,	0,	-15,	-1,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(25,	244,	0,	0,	0,	-15,	-1,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(25,	244,	0,	0,	0,	-15,	-1,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(25,	244,	0,	0,	0,	-15,	-1,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(45,	244,	0,	0,	-1,	-8,	1,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(45,	244,	0,	0,	-1,	-8,	1,	1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(7,	244,	0,	0,	0,	5,	-1,	-1));
		Linez.BallzInfo.Add(new OpenPetz.Linez.Entries.BallInfo(7,	244,	0,	0,	0,	5,	-1,	-1));
		
		Fudgers.Add(new Fudger(Fudger.EDirectiveType.Decay));
		
		Rotation3D.Y = 1.57f/2f;
		
		catBhd = AnimationManager.FetchCatBhd();
		animation = catBhd.GetAnimation(0); //104
		
		var frame = animation.Frames[currentFrame];

		petSprite = new PetSprite(this);
		
		AddChild(petSprite);
		
		petSprite.SetFrame(frame);
		
		World.pets.Add(this);
	}

	public override void _ExitTree()
	{
		World.pets.Remove(this);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		foreach (var fudger in Fudgers)
			fudger.Update();
		
		//var fudger2 = Fudgers[(int)Fudger.EType.Rotation];
		
		//What?
		//var cursor = GetViewport().GetMousePosition();
		//WHAT?
		//var angle = (int)(Math.Atan2((double)(GlobalPosition.X - cursor.X), 128d) * 128d / Math.PI);
		
		//fudger2.SetAimTarget(angle);
		
		//Rotation3D.Y = 1.57f;
		
		currentFrame += 1;
		if (currentFrame >= animation.NumFrames)
			currentFrame = 0;
		
		var frame = animation.Frames[currentFrame];
		
		//temp commented
		petSprite.SetFrame(frame);
		
		//petSprite.PointHeadAt(GetViewport().GetMousePosition());
	}
}
