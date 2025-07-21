using Godot;
using System;
using System.Collections.Generic;

/* A PaintBall object cannot be "directly" rendered.
 * Instead, it contains information needed to render a paintball. It needs to be passed into a PaintBallGroup object in order to be rendered.
 */

public partial class PaintBall
{
	private Vector3 coordinations = new Vector3(1.0f, 0.0f, 0.0f);
	private float size = 1.0f;
	private float colorIndex = 95.0f / 255.0f;
	private float fuzz = 0.0f;
	
	public Vector3 Coordinations { get => coordinations; }
	public float Size { get => size; }
	public float ColorIndex { get => colorIndex; }
	public float Fuzz { get => fuzz; }
	
	public PaintBall()
	{
		
	}
	
	public PaintBall(Vector3 _coords, float _size, float _colorIndex)
	{
		var coords = _coords.Normalized();
		//_colorIndex needs to be converted to the [0, 1] range
		colorIndex = _colorIndex;
		
		coordinations = _coords;
		size = _size;
		
	}
}