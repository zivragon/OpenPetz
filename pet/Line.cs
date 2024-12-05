using Godot;
using System;

public partial class Line : Node2D
{
	private MeshInstance2D meshInstance;
	public ImmediateMesh immediateMesh;
	public ShaderMaterial material;


	public override void _Ready()
	{
		meshInstance = new MeshInstance2D();
		AddChild(meshInstance);

		immediateMesh = new ImmediateMesh();
		meshInstance.Mesh = immediateMesh;

		// need to copy material for each ball or else they overwrite eachother's parameters
		material = (ShaderMaterial)GD.Load<ShaderMaterial>("res://shaders/line_shader.tres").Duplicate(true);

		//TODO setup line parameters
	}


	public override void _Process(double delta)
	{
		immediateMesh.ClearSurfaces();
		immediateMesh.SurfaceBegin(Mesh.PrimitiveType.Triangles);

		drawVetices();

		immediateMesh.SurfaceEnd();

		immediateMesh.SurfaceSetMaterial(0, material);
		meshInstance.Material = material;
	}

	private void drawVetices()
	{
		int size = 1000;

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
