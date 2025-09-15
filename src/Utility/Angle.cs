using Godot;
using System;
using System.Collections.Generic;

public static class Angle 
{
	// -32,768 to +32,767
	public static int MinDiffAngle(int _from, int _to)
	{
		if (_to < _from)
			_to += ((_from - _to + 65535) / 65536) * 65536;
		
		int diff = _to - _from;
		
		if (diff > 32768)
			diff -= 65536;
		
		return diff;
	}
}


