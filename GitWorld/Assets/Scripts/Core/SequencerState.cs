using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SequencerState {
	public Sequencer.StateData[] sequences;
	public string sequenceDefaultState;
	
	private Dictionary<string, Sequencer> mSequences;
	
	public void Load() {
		mSequences = Sequencer.Load(sequences);
	}
	
	public Sequencer.StateInstance Start(MonoBehaviour behaviour, string stateName=null) {
		Sequencer.StateInstance ret = null;
		
		if(stateName == null) {
			stateName = sequenceDefaultState;
		}
		
		if(!string.IsNullOrEmpty(stateName)) {
			if(mSequences != null) {
				
				Sequencer seq;
				if(mSequences.TryGetValue(stateName, out seq)) {
					ret = new Sequencer.StateInstance();
					behaviour.StartCoroutine(seq.Go(ret, behaviour));
				}
				else {
					Debug.LogError("State not found: "+stateName, behaviour);
				}
			}
		}
		
		return ret;
	}
}