using Godot;
using System;
using System.Collections.Generic;

public static class Rotator {
	
	//Methods
	
	public static Vector3 Rotate3D(Vector3 positions, Vector3 rotations)
	{
		float rXSin = (float)Math.Sin(rotations.X);
		float rXCos = (float)Math.Cos(rotations.X);
		
		float rYSin = (float)Math.Sin(rotations.Y);
		float rYCos = (float)Math.Cos(rotations.Y);
		
		float rZSin = (float)Math.Sin(rotations.Z);
		float rZCos = (float)Math.Cos(rotations.Z);
		
		//
		var v1 = new Vector3(0.0f, 0.0f, 0.0f);
		var v2 = new Vector3(0.0f, 0.0f, 0.0f);
		var v3 = new Vector3(0.0f, 0.0f, 0.0f);
		
		v1.Z = positions.Z * rYCos - positions.X * rYSin;
		v1.X = positions.X * rYCos + positions.Z * rYSin;
		
		v2.Y = positions.Y * rZCos - v1.X * rZSin;
		v2.X = v1.X * rZCos + positions.Y * rZSin;
		
		v3.Z = v1.Z * rXCos - v2.Y * rXSin;
		v3.Y = v2.Y * rXCos + v1.Z * rXSin;
		
		var v4 = new Vector3((float)Math.Round(v2.X / 2.0f), (float)Math.Round(v3.Y / 2.0f), (float)Math.Round(v3.Z / 2.0f));
		
		return v4;
	}
}

