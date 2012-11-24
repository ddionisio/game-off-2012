using UnityEngine;
using System.Collections;

public class AISetAnimation : SequencerAction {
	
	public AIAnimKey[] animations;
	
	public float wait;
	
	public override void Start(MonoBehaviour behaviour) {
		AIController ai = (AIController)behaviour;
		ai.BroadcastMessage("OnAIAnimation", animations, SendMessageOptions.DontRequireReceiver);
		ai.lastTime = Time.time;
	}
	
	public override bool Update(MonoBehaviour behaviour) {
		//take into account time since animations were played so we are still in sync even after pause/resume of the sequencer
		AIController ai = (AIController)behaviour;
		return Time.time - ai.lastTime >= wait;
	}
}
