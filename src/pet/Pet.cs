using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

using OpenPetz;

public partial class Pet : Node2D
{
	private PetRenderer petRenderer;

	private Texture2D palette;
	public BallzModel catBhd;
	private BallzModel.FrameGroup animation;
	private int currentFrame = 0;

	public override void _Ready()
	{
		World.pets.Add(this);
		
		catBhd = AnimationManager.FetchCatBhd();
		animation = catBhd.GetAnimation(1);
		
		var frame = animation.Frames[currentFrame];

		petRenderer = new PetRenderer(this);
		
		AddChild(petRenderer);
		
		petRenderer.SetFrame(frame);
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
		
		this.petRenderer.SetFrame(frame);
	}

	public override void _Draw()
	{

	}

}
