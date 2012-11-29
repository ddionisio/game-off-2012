using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Scene controller. Make sure this is the root of all your objects in the scene, there should only be one of these.
/// </summary>
public class SceneController : MonoBehaviour {
	public class StateData : Sequencer.StateInstance {
		public Entity entity; //use this to hold a entity for during update
	}
	
	public SequencerState sequencer;
	
	private string mRootPath;
	private StateData mStateInstance = null;
	
	//only call these during inits
	public GameObject SearchObject(string path) {
		return GameObject.Find(mRootPath+path);
	}
	
	protected virtual void SequenceChangeState(string state) {
		if(mStateInstance != null) {
			mStateInstance.terminate = true;
		}
		mStateInstance = new StateData();
		sequencer.Start(this, mStateInstance, state);
	}
	
	protected virtual void OnDestroy() {
		mStateInstance = null;
	}
	
	protected virtual void Start() {
		if(sequencer != null) {
			//Sequencer.StateInstance
			mStateInstance = new StateData();
			sequencer.Start(this, mStateInstance, null);
		}
	}
	
	protected virtual void Awake() {
		mRootPath = "/"+gameObject.name+"/";
		
		sequencer.Load();
	}
}
