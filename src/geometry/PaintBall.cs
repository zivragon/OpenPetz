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
	private float colorIndex = 95.0f;
	private float fuzz = 4.0f;
	
	public Color Coordinations { get => coordinations; }
	public float Size { get => size; }
	public float ColorIndex { get => colorIndex; }
	public float Fuzz { get => fuzz; }
	
	public PaintBall()
	{
	    coordinations.a = colorIndex;
	}
	
	public PaintBall(Vector3 _coords, float _size, float _colorIndex)
	{
	    var coords = _coords.Normalize();
	    
		coordinations = new Color( coords.X, coords.Y, coords.Z, _colorIndex);
		size = _size / 100.0f;
		colorIndex = _colorIndex;
	}
}
