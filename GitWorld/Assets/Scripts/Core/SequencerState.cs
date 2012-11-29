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
	
	public void Start(MonoBehaviour behaviour, Sequencer.StateInstance stateData, string stateName) {
		if(stateName == null) {
			stateName = sequenceDefaultState;
		}
		
		if(!string.IsNullOrEmpty(stateName)) {
			if(mSequences != null) {
				
				Sequencer seq;
				if(mSequences.TryGetValue(stateName, out seq)) {
					behaviour.StartCoroutine(seq.Go(stateData, behaviour));
				}
				else {
					Debug.LogError("State not found: "+stateName, behaviour);
				}
			}
		}
	}
}