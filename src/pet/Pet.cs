using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;

using OpenPetz;

public partial class Pet : Node2D
{
	private PetSprite petSprite;

	private Texture2D palette;
	public BallzModel catBhd;
	private BallzModel.FrameGroup animation;
	private int currentFrame = 0;
	
	public List<Fudger> Fudgers = new List<Fudger>();
	
	public Vector3 Rotation3D = new Vector3(0.0f, 0.0f, 0.0f);
	
	public Pet()
	{
		
	}

	public override void _Ready()
	{
		Rotation3D.X -= 0.125f;
		
		Fudgers.Add(new Fudger(Fudger.EDirectiveType.Decay));
		
		Rotation3D.Y = Fudgers[(int)Fudger.EType.Rotation].GetCurrentAngle();
		
		World.pets.Add(this);
		
		catBhd = AnimationManager.FetchCatBhd();
		animation = catBhd.GetAnimation(0); //104
		
		var frame = animation.Frames[currentFrame];

		petSprite = new PetSprite(this);
		
		AddChild(petSprite);
		
		petSprite.SetFrame(frame);
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
		
		var fudger2 = Fudgers[(int)Fudger.EType.Rotation];
		
		//What?
		//var cursor = GetViewport().GetMousePosition();
		//WHAT?
		//var angle = (int)(Math.Atan2((double)(GlobalPosition.X - cursor.X), 128d) * 128d / Math.PI);
		
		//fudger2.SetAimTarget(angle);
		
		Rotation3D.Y = fudger2.GetCurrentAngle();
		
		currentFrame += 1;
		if (currentFrame >= animation.NumFrames)
			currentFrame = 0;
		
		var frame = animation.Frames[currentFrame];
		
		//temp commented
		petSprite.SetFrame(frame);
		
		//petSprite.PointHeadAt(GetViewport().GetMousePosition());
	}

	public override void _Draw()
	{

	}

}
