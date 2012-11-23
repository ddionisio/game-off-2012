using UnityEngine;
using System.Collections;

public class UserData : MonoBehaviour {
	public enum LevelState {
		Locked,
		Unlocked
	}
	
	public LevelState GetLevelState(int level) {
		//TODO: get from data
		return LevelState.Unlocked;
	}

	void Awake() {
		//load from preferences
	}
}
