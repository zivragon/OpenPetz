using Godot;
using System;
using System.Collections.Generic;

namespace Fudger 
{
	public enum Mode 
	{
		Unknown0 = 0,
		Angular  = 1,
		Unknown2 = 2
	}
		
	public enum DirectiveType 
	{
		Deactivated = 0,
		Suspended   = 1,
		Aim         = 2,
		Drift       = 3,
		Align       = 4,
		Decay       = 5,
		SwingTrue   = 6,
		SwingFalse  = 7
	}
	
	public enum Type 
	{
		Rotation = 0,
		Roll     = 1,
		Tilt     = 2,
		Num //used for allocating fudger list
	}
	
	public class Base
	{
		
		public Mode Mode = Mode.Unknown0;
		public DirectiveType Type = DirectiveType.Deactivated; // ?
		public DirectiveType TypeToResume = DirectiveType.Deactivated;
		
		public bool UpdateWhenDeactivated = false;
		
		public int UpperLimit = 10;
		public int LowerLimit = 0;
		
		public int CurrentValue = 0;
		
		public Base()
		{
			
		}
		
		public float GetCurrentAngle()
		{
			return CurrentValue / 32768f * (float)Math.PI;
		}
		
		public virtual void Update()
		{
			
		}
	}
	
	public static class Method 
	{
		public static Fudger.Base InitFudger(DirectiveType _type)
		{
			switch (_type)
			{
				case (DirectiveType.Aim) :
					return new Fudger.Aim();
				default:
					return null;
			}
		}
	}
	
}