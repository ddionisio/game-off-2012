using UnityEngine;
using System.Collections;

public class AISetMaxVelocity : SequencerAction {
	public float velocity;
	
	public override void Start(MonoBehaviour behaviour) {
		AIController ai = (AIController)behaviour;
		PlanetAttach pa = ai.planetAttach;
		pa.maxVelocity = velocity;
	}
}
