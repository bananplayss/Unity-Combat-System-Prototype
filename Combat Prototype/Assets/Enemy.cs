using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public enum States {
		None,
		hit_stun,
		block_stun,
	}

	private float hitStunDuration;
	private float hitStunDurationMax = 1f; //TEST

	public States states;

	private void Update() {
		switch (states) {
			case States.hit_stun:
				DebugUI.Instance.SetText("Enemy's stunned...");
				hitStunDuration += Time.deltaTime;
				if(hitStunDuration >= hitStunDurationMax ) {
					states = States.None;
					hitStunDuration= 0f;

					DebugUI.Instance.SetText("Enemy's normal...");
				}
			break;
		}
	}

	private void OnCollisionEnter(Collision collision) {
		Debug.Log(collision.collider.tag);
		if(collision.collider.tag == "Sword") {
			states= States.hit_stun;
		}
	}
}
