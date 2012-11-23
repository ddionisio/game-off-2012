using UnityEngine;
using System.Collections;

public class AIChangeState : SequencerAction {
	public string toState;
	
	public override void Start(MonoBehaviour behaviour) {
		if(!string.IsNullOrEmpty(toState)) {
			((AIController)behaviour).SequenceSetState(toState);
		}
	}
}
