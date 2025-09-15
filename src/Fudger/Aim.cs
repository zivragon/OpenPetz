using Godot;
using System;
using System.Collections.Generic;

namespace Fudger 
{
	class Aim : Base
	{				
		public int AimRate = 20;
		public int StepSize = 0;
		
		public int AimTarget {get; private set;} = 0;
		private int Direction = 1; // -1 or 1
		
		//private bool HasTargetChanged = false;
			
		public Aim()
		{	
			Mode = Mode.Angular;
		
			AimTarget = 0;
			CurrentValue = -32768;
		
			if (Mode == Mode.Angular)
				StepSize = Angle.MinDiffAngle(CurrentValue, AimTarget) / AimRate;
			else
				StepSize = (CurrentValue - AimTarget) / AimRate;
			Direction = Math.Sign(StepSize);
			GD.Print(StepSize);
		}
		
		public void SetAimTarget(int _target)
		{
			if (AimTarget == _target)
				return;
			
			AimTarget = _target;
			//re-calculate step size 
			if (Mode == Mode.Angular)
				StepSize = Angle.MinDiffAngle(CurrentValue, AimTarget) / AimRate;
			else
				StepSize = (CurrentValue - AimTarget) / AimRate;
			Direction = Math.Sign(StepSize);
		}
			
		public override void Update()
		{
			if (Math.Sign(CurrentValue + StepSize  - AimTarget) != Direction)
			{
				CurrentValue += StepSize;
			}
		}
	}
	
}