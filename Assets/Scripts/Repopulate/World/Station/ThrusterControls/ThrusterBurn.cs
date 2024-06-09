using System;
using UnityEngine;

[Serializable]
public class ThrusterBurn {
	
	[SerializeField] private float thrusterAngle;
	[SerializeField] private int burnTime;
	[SerializeField] private float burnStrength;

	public int preBurnTime { get; private set; } = 50;
	public int postBurnTime { get; private set; } = 50;
	public int resetTime { get; private set; } = 50;

	private int preBurnCount = 0;
	private int resetCount = 0;
	
	//Burn
	//Angle = -1 to 1
	//Time in seconds
	//Strength 0 to 1
	public ThrusterBurn(float angle, float time, float strength) {
		thrusterAngle = angle;
		burnTime = SecondsToFrames(time);
		burnStrength = strength;
	}
	
	//Burn
	//Angle = -1 to 1
	//Preburn in fixed ticks
	//Time in seconds
	//Postburn in fixed ticks
	//Reset in fixed ticks
	//Strength 0 to 1
	public ThrusterBurn(float angle, int preTime, float time, int postTime, int reset, float strength) {
		thrusterAngle = angle;
		preBurnTime = preTime;
		burnTime = SecondsToFrames(time);
		postBurnTime = postTime;
		resetTime = reset;
		burnStrength = strength;
	}
	
	//Wait
	public ThrusterBurn(float time) {
		thrusterAngle = 0;
		burnTime = SecondsToFrames(time);
		burnStrength = 0;
	}

	private int SecondsToFrames(float time) {
		return (int) Mathf.Ceil(time * 50f);
	}

	public BurnState GetBurnState() {
		if (preBurnTime > preBurnCount) {
			preBurnCount++;
			return BurnState.PRE_BURN;
		}

		if (burnTime > 0) {
			burnTime--;
			return BurnState.BURN;
		}

		if (postBurnTime > 0) {
			postBurnTime--;
			return BurnState.POST_BURN;
		} 

		if (resetTime > resetCount) {
			resetCount++;
			return BurnState.RESET;
		}

		return BurnState.FINISH;
	}

	public float GetThrusterAngle() => thrusterAngle;
	public int GetBurnTime() => burnTime;
	public float GetBurnStrength() => burnStrength;

	public float PreBurnTimeStep() => (float)preBurnCount / preBurnTime;
	public float ResetTimeStep() => (float)resetCount / resetTime;
}

public enum BurnState {
	PRE_BURN,
	BURN,
	POST_BURN,
	RESET,
	FINISH
}