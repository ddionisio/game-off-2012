using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Scene controller. Make sure this is the root of all your objects in the scene, there should only be one of these.
/// </summary>
public class SceneController : MonoBehaviour {
	public SequencerState sequencer;
	
	private string mRootPath;
	private Sequencer.StateInstance mStateInstance = null;
	
	//only call these during inits
	public GameObject SearchObject(string path) {
		return GameObject.Find(mRootPath+path);
	}
	
	void SequenceChangeState(string state) {
		if(mStateInstance != null) {
			mStateInstance.terminate = true;
		}
		mStateInstance = sequencer.Start(this, state);
	}
	
	void Start() {
		if(sequencer != null) {
			//Sequencer.StateInstance
			mStateInstance = sequencer.Start(this);
		}
	}
	
	void Awake() {
		mRootPath = "/"+gameObject.name+"/";
		
		sequencer.Load();
	}
}
