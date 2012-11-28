using UnityEngine;
using System.Collections;

public class AIJump : SequencerAction {

	public float speedMin=0;
	public float speedMax=0;
	
	public override void Start(MonoBehaviour behaviour) {
		AIController ai = (AIController)behaviour;
		PlanetAttach pa = ai.planetAttach;
		pa.Jump(speedMin < speedMax ? Random.Range(speedMin, speedMax) : speedMin);
		ai.entity.action = Entity.Action.jump;
	}
	
	public override bool Update(MonoBehaviour behaviour) {
		PlanetAttach pa = ((AIController)behaviour).planetAttach;
		return pa.isGround;
	}
	
	public override void Finish(MonoBehaviour behaviour) {
	}
}
