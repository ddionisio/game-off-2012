using UnityEngine;
using System.Collections;

public class AIFollowPlayerAccel : SequencerAction {
	public bool horizontalOnly = true;
	
	public float accel = 0;
	
	public int doneAfterNumChangeDir = 1;
	
	public float breakSpeed = 0; //cheap
	public bool useBreakSpeed = true;
			
	public override void Start(MonoBehaviour behaviour) {
		AIController ai = (AIController)behaviour;
		ai.counter = 0;
	}
	
	public override bool Update(MonoBehaviour behaviour) {
		AIController ai = (AIController)behaviour;
		
		PlanetAttach pa = ai.planetAttach;
		Player player = SceneLevel.instance.player;
		
		//Debug.Log("side: "+pa.CheckSide(player.planetAttach));
		Vector2 prevDir = ai.curPlanetDir;
		ai.curPlanetDir = pa.GetDirTo(player.planetAttach, horizontalOnly);
		
		bool done = true;
		
		if(doneAfterNumChangeDir == 0 || ai.counter < doneAfterNumChangeDir) {
			//cap speed on opposite dir
			if(Vector2.Dot(prevDir, ai.curPlanetDir) < 0.0f) {			
				if(useBreakSpeed) {
					pa.velocity = prevDir*breakSpeed;
				}
				
				if(doneAfterNumChangeDir > 0) {
					ai.counter++;
				}
			}
		}
		
		if(doneAfterNumChangeDir > 0) {
			done = doneAfterNumChangeDir == ai.counter && Vector2.Dot(pa.velocity, ai.curPlanetDir) > 0.0f;
		}
		
		//pa.velocity = Vector2.zero;
		pa.accel = ai.curPlanetDir*accel;
				
		return done;
	}
}
