using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationController : MonoBehaviour {
	
	[SerializeField] protected Animator anim;

	public abstract void TriggerAnimation();

}