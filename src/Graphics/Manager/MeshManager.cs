//TO DO: RE DO

using Godot;
using System;
using System.Collections.Generic;

public static class MeshManager {
	
	private static Dictionary<string, Mesh> avialableMeshes = new Dictionary<string, Mesh>();
	
	private static Mesh defaultMesh = null;
	private static Mesh defaultLineMesh = null;
	private static Mesh normalMesh = null;
	
	//Methods
	
	private static Mesh CreateDefaultMesh()
	{
		var st = new SurfaceTool();

		st.Begin(Mesh.PrimitiveType.Triangles);
			
		st.AddVertex(new Vector3(-1, -1, 0));
			
		st.AddVertex(new Vector3(-1, 1, 0));
			
		st.AddVertex(new Vector3(1, 1, 0));
			
		st.AddVertex(new Vector3(-1, -1, 0));
			
		st.AddVertex(new Vector3(1, -1, 0));
			
		st.AddVertex(new Vector3(1, 1, 0));

		// And finally, generate the mesh.
		var meshBuffer = st.Commit();
		defaultMesh = meshBuffer;
		
		return defaultMesh;
	}
	
	private static Mesh CreateNormalMesh()
	{
		var st = new SurfaceTool();

		st.Begin(Mesh.PrimitiveType.Triangles);
			
		st.AddVertex(new Vector3(0, 0, 0));
			
		st.AddVertex(new Vector3(1, 0, 0));
			
		st.AddVertex(new Vector3(1, 1, 0));
			
		st.AddVertex(new Vector3(0, 0, 0));
			
		st.AddVertex(new Vector3(0, 1, 0));
			
		st.AddVertex(new Vector3(1, 1, 0));

		// And finally, generate the mesh.
		var meshBuffer = st.Commit();
		normalMesh = meshBuffer;
		
		return normalMesh;
	}
	
	private static Mesh CreateDefaultLineMesh()
	{
		var st = new SurfaceTool();

		st.Begin(Mesh.PrimitiveType.Triangles);
			
		st.AddVertex(new Vector3(1, -1, 0));
			
		st.AddVertex(new Vector3(0, -1, 0));
			
		st.AddVertex(new Vector3(0, 1, 0));
			
		st.AddVertex(new Vector3(0, 1, 0));
			
		st.AddVertex(new Vector3(1, 1, 0));
			
		st.AddVertex(new Vector3(1, -1, 0));

		// And finally, generate the mesh.
		var meshBuffer = st.Commit();
		defaultLineMesh = meshBuffer;
		
		return defaultLineMesh;
	}
	
	public static Mesh FetchDefaultMesh()
	{
		if (defaultMesh == null)
			defaultMesh = CreateDefaultMesh();
		return defaultMesh; 
	}
	
	public static Mesh FetchDefaultLineMesh()
	{
		if (defaultLineMesh == null)
			defaultLineMesh = CreateDefaultLineMesh();
		return defaultLineMesh; 
	}
	
	public static Mesh FetchNormalMesh()
	{
		if (normalMesh == null)
			normalMesh = CreateNormalMesh();
		return normalMesh; 
	}
	
	public static Mesh FetchMesh(string name)
	{
		//not for now : )
		return null;
	}

}

