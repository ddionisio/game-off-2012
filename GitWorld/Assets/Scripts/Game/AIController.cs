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
	
	private string mCurState;
	
	private bool mHasStarted = false;
	
	public string curState {
		get {
			return mCurState;
		}
	}
	
	public void SequenceStop() {
		if(mStateInstance != null) {
			mStateInstance.terminate = true;
			mStateInstance = null;
			
			mCurState = null;
		}
	}
	
	public void SequenceSetPause(bool pause) {
		if(mStateInstance != null) {
			mStateInstance.pause = pause;
		}
	}
	
	public void SequenceSetState(string state) {
		SequenceStop();
		
		mCurState = state;
		
		mStateInstance = AIManager.instance.states.Start(this, state);
	}
		
	void OnEnable() {
		if(mHasStarted) { //during spawn
			SceneStart();
		}
	}
	
	void OnDisable() {
		SequenceStop();
	}
	
	void Awake() {
		planetAttach = GetComponent<PlanetAttach>();
		entity = GetComponent<Entity>();
	}
	
	void SceneStart() {
		if(!string.IsNullOrEmpty(startState)) {
			SequenceSetState(startState);
		}
		
		mHasStarted = true;
	}
}
