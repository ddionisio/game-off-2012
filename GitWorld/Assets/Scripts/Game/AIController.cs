using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour {
	[System.NonSerialized]
	public PlanetAttach planetAttach;
	
	[System.NonSerialized]
	public Entity entity;
	
	[System.NonSerialized]
	public Vector2 curPlanetDir;
	
	[System.NonSerialized]
	public Vector2 velocityHolder;
	
	[System.NonSerialized]
	public float lastTime; //use this for waiting
	
	[System.NonSerialized]
	public int counter; //some sort of counting
	
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
	
	public void SequenceRestart() {
		if(!string.IsNullOrEmpty(mCurState)) {
			SequenceSetState(mCurState);
		}
	}
	
	void SequenceChangeState(string state) {
		SequenceSetState(state);
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
