using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

	public partial class Ball : Node2D
	{
		private MeshInstance2D meshInstance;
		public ImmediateMesh immediateMesh;
		public ShaderMaterial material;

		public Texture2D texture;
		public Texture2D palette;

		int radius;
		int color_index;
		int fuzz;
		int outline_width;
		int outline_color;

	public Ball()
	{

	}

	public Ball(Texture2D texture, Texture2D palette, int radius, int color_index, int fuzz, int outline_width, int outline_color)
	{
		this.texture = texture;
		this.palette = palette;
		this.radius = radius;
		this.color_index = color_index;
		this.fuzz = fuzz;
		this.outline_width = outline_width;
		this.outline_color = outline_color;
	}

	public override void _Ready()
	{
		meshInstance = new MeshInstance2D();
		AddChild(meshInstance);

		immediateMesh = new ImmediateMesh();
		meshInstance.Mesh = immediateMesh;

		// need to copy material for each ball or else they overwrite eachother's parameters
		material = (ShaderMaterial)GD.Load<ShaderMaterial>("res://shaders/ball_shader.tres").Duplicate(true);

		material.SetShaderParameter("fuzz", fuzz);
		material.SetShaderParameter("radius", radius);
		material.SetShaderParameter("outline_width", outline_width);

		material.SetShaderParameter("color_index", color_index);
		material.SetShaderParameter("outline_color", outline_color);

		material.SetShaderParameter("tex", texture);
		material.SetShaderParameter("palette", palette);


		material.SetShaderParameter("center", this.GlobalPosition);
	}


	public override void _Process(double dt)
	{
		material.SetShaderParameter("center", this.GlobalPosition);

		immediateMesh.ClearSurfaces();
		immediateMesh.SurfaceBegin(Mesh.PrimitiveType.Triangles);

		drawBall();

		immediateMesh.SurfaceEnd();

		immediateMesh.SurfaceSetMaterial(0, material);
		meshInstance.Material = material;
	}



	private void drawBall()
	{
		int size = radius + fuzz;

		immediateMesh.SurfaceSetUV(new Vector2(0, 0));
		immediateMesh.SurfaceAddVertex(new Vector3(-1 * size, -1 * size, 0));

		immediateMesh.SurfaceSetUV(new Vector2(0, 128));
		immediateMesh.SurfaceAddVertex(new Vector3(-1 * size, size, 0));

		immediateMesh.SurfaceSetUV(new Vector2(128, 128));
		immediateMesh.SurfaceAddVertex(new Vector3(size, size, 0));


		immediateMesh.SurfaceSetUV(new Vector2(0, 128));
		immediateMesh.SurfaceAddVertex(new Vector3(size, -1 * size, 0));

		immediateMesh.SurfaceSetUV(new Vector2(0, 0));
		immediateMesh.SurfaceAddVertex(new Vector3(-1 * size, -1 * size, 0));

		immediateMesh.SurfaceSetUV(new Vector2(128, 128));
		immediateMesh.SurfaceAddVertex(new Vector3(size, size, 0));

	}


}
