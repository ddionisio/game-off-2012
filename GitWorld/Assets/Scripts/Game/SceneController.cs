using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Scene controller. Make sure this is the root of all your objects in the scene, there should only be one of these.
/// </summary>
public class SceneController : MonoBehaviour {
	public SequencerState sequencer;
	
	private string mRootPath;
	
	//only call these during inits
	public GameObject SearchObject(string path) {
		return GameObject.Find(mRootPath+path);
	}
	
	protected virtual void Start() {
		if(sequencer != null) {
			//Sequencer.StateInstance
			sequencer.Start(this);
		}
	}
	
	protected virtual void Awake() {
		mRootPath = "/"+gameObject.name+"/";
		
		sequencer.Load();
	}
}
