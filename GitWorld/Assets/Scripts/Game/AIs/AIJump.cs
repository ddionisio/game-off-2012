using UnityEngine;
using System.Collections;

public class AIJump : SequencerAction {

	public float speedMin;
	public float speedMax;
	
	public override void Start(MonoBehaviour behaviour) {
		AIController ai = (AIController)behaviour;
		PlanetAttach pa = ai.planetAttach;
		pa.Jump(Random.Range(speedMin, speedMax));
		ai.entity.action = Entity.Action.jump;
	}
	
	public override bool Update(MonoBehaviour behaviour) {
		PlanetAttach pa = ((AIController)behaviour).planetAttach;
		return pa.isGround;
	}
	
	public override void Finish(MonoBehaviour behaviour) {
	}
}
