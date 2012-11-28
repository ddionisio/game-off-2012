using UnityEngine;
using System.Collections;

public class AISetVelocity : SequencerAction {
	public float speedMin = 0;
	public float speedMax = 0;
	public float angle = 0; //0 to 360, counterclockwise from right
	public bool useDir = false;
	public bool resetAccel = true;
	
	public override void Start(MonoBehaviour behaviour) {
		AIController ai = (AIController)behaviour;
		PlanetAttach pa = ai.planetAttach;
		
		float speed = speedMin < speedMax ? Random.Range(speedMin, speedMax) : speedMin;
		if(speed > 0) {
			Vector2 dir;
			if(useDir) {
				dir = ai.curPlanetDir;
			}
			else {
				ai.curPlanetDir = dir = Util.Vector2DRot(new Vector2(-1, 0), angle*Mathf.Deg2Rad);
			}
			
			pa.velocity = dir*speed;
			
			ai.entity.action = Entity.Action.move;
		}
		else {
			pa.velocity = Vector2.zero;
			
			ai.entity.action = Entity.Action.idle;
		}
		
		if(resetAccel) {
			pa.accel = Vector2.zero;
		}
	}
}
