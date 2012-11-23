using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour {
	[System.NonSerialized]
	public PlanetAttach planetAttach;
	
	[System.NonSerialized]
	public Entity entity;
	
	[System.NonSerialized]
	public Vector2 curPlanetDir;
	
	public string startState;
	
	private Sequencer.StateInstance mStateInstance = null;
	
	private bool mHasStarted = false;
	
	public void SequenceStop() {
		if(mStateInstance != null) {
			mStateInstance.terminate = true;
			mStateInstance = null;
		}
	}
	
	public void SequenceSetPause(bool pause) {
		if(mStateInstance != null) {
			mStateInstance.pause = pause;
		}
	}
	
	public void SequenceSetState(string state) {
		SequenceStop();
		
		mStateInstance = AIManager.instance.states.Start(this, state);
	}
		
	void OnEnable() {
		if(mHasStarted) {
			Start();
		}
	}
	
	void OnDisable() {
		SequenceStop();
	}
	
	void Awake() {
		planetAttach = GetComponent<PlanetAttach>();
		entity = GetComponent<Entity>();
	}
	
	void Start() {
		if(!string.IsNullOrEmpty(startState)) {
			SequenceSetState(startState);
		}
		
		mHasStarted = true;
	}
}
