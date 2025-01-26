using Godot;
using System;
using System.Collections.Generic;

/* A PaintBall object cannot be "directly" rendered.
 * Instead, it contains information needed to render a paintball. It needs to be passed into a PaintBallGroup object in order to be rendered.
 */

public partial class PaintBall
{
	private Color coordinations = new Color(1.0f, 0.0f, 0.0f, 0.0f);
	private float size = 1.0f;
	private float colorIndex = 95.0f / 255.0f;
	private float fuzz = 4.0f;
	
	public Color Coordinations { get => coordinations; }
	public float Size { get => size; }
	public float ColorIndex { get => colorIndex; }
	public float Fuzz { get => fuzz; }
	
	public PaintBall()
	{
		coordinations.A = colorIndex;
	}
	
	public PaintBall(Vector3 _coords, float _size, float _colorIndex)
	{
		var coords = _coords.Normalized();
		//_colorIndex needs to be converted to the [0, 1] range
		colorIndex = _colorIndex / 255.0f;
		
		coordinations = new Color(
			ColorValueFromCoord(coords.X),
			ColorValueFromCoord(coords.Y),
			ColorValueFromCoord(coords.Z),
			colorIndex
		);
		size = _size;
		
	}
	
	// OpenGL or Godot clamps COLOR inside shader to be 0,1
 	float ColorValueFromCoord(float coord)
 	{
 		// -1,1 to 0,1
 		float result = (coord + 1.0f ) / 2.0f;

 		return result; 
 	}	
}