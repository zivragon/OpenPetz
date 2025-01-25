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
	private float colorIndex = 0.0f;
	
	public Color Coordinations { get => coordinations; }
	public float Size { get => size; }
	public float ColorIndex { get => colorIndex; }
	
	public PaintBall()
	{
	
	}
	
	public PaintBall(Vector3 _coords, float _size, float _colorIndex)
	{
		coordinations = new Color( _coords.X, _coords.Y, _coords.Z, 0);
		size = _size / 100.0f;
		colorIndex = _colorIndex;
	}
}
