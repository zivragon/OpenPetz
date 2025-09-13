using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

using OpenPetz;

public partial class Pet : Node2D
{
	private PetSprite petSprite;

	private Texture2D palette;
	public BallzModel catBhd;
	private BallzModel.FrameGroup animation;
	private int currentFrame = 0;
	
	public Pet()
	{
		
	}

	public override void _Ready()
	{
		World.pets.Add(this);
		
		catBhd = AnimationManager.FetchCatBhd();
		animation = catBhd.GetAnimation(104);
		
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
		currentFrame += 1;
		if (currentFrame >= animation.NumFrames)
			currentFrame = 0;
		
		var frame = animation.Frames[currentFrame];
		
		//temp commented
		petSprite.SetFrame(frame);
		
		petSprite.PointHeadAt(GetViewport().GetMousePosition());
		//double rot = Math.Atan2(10.0, /*(double)petSprite.GlobalPosition.X - */(double)GetViewport().GetMousePosition().X);
		//double rot = Math.Atan2(-10.0, 10.0 - (double)petSprite.GlobalPosition.X);
		
		//petSprite.SetHeadRotation(new Vector3(0f, (float)rot, 0f));
	}

	public override void _Draw()
	{

	}

}
