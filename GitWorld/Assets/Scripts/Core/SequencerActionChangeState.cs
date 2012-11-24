using UnityEngine;
using System.Collections;

public class SequencerActionChangeState : SequencerAction {

	public string state = "";
	
	public override void Start(MonoBehaviour behaviour) {
		behaviour.SendMessage("SequenceChangeState", state, SendMessageOptions.RequireReceiver);
	}
}
