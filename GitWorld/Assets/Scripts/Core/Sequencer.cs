using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sequencer {
	[System.Serializable]
	public class StateData {
		public string name;
		public TextAsset source;
	}
	
	public class StateInstance {
		public bool pause=false;
		public bool terminate=false;
	}
	
	public bool loop = false;
	public List<SequencerAction> actions = null;
			
	public static Dictionary<string, Sequencer> Load(StateData[] sequences) {
		fastJSON.JSON.Instance.UseSerializerExtension = true;
		
		Dictionary<string, Sequencer> ret = new Dictionary<string, Sequencer>(sequences.Length);
		
		foreach(StateData dat in sequences) {
			if(dat.source != null) {
				Sequencer newSequence = (Sequencer)fastJSON.JSON.Instance.ToObject(dat.source.text, typeof(Sequencer));
				ret[dat.name] = newSequence;
			}
		}
		
		return ret;
	}
		
	public IEnumerator Go(StateInstance stateInstance, MonoBehaviour behaviour) {
		if(actions != null) {
			int i = 0;
			int len = actions.Count;
			while(!stateInstance.terminate && i < len) {
				if(stateInstance.pause) {
					yield return new WaitForFixedUpdate();
					continue;
				}
				
				SequencerAction action = actions[i];
				
				if(action.delay > 0) {
					yield return new WaitForSeconds(action.delay);
				}
				
				action.Start(behaviour);
				
				while(!stateInstance.terminate && !action.Update(behaviour)) {
					yield return new WaitForFixedUpdate();
				}
				
				action.Finish(behaviour);
				
				i++;
				if(loop && i == len) {
					i = 0;
				}
			}
		}
		
		yield break;
	}
}
