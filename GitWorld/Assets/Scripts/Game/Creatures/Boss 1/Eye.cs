using UnityEngine;
using System.Collections;

public class Eye : EntityBase {
	[System.NonSerialized]
	public BossLevelOne boss = null;
	
	void Awake() {
		mReticle = Reticle.Type.Eat;
	}
	
	void OnDestroy() {
		boss = null;
	}
	
	void OnGrabStart(PlayerGrabber grabber) {
	}
	
	void OnGrabDone(PlayerGrabber grabber) {
		grabber.Retract(true);
	}
	
	void OnGrabRetractStart(PlayerGrabber grabber) {
	}
	
	void OnGrabRetractEnd(PlayerGrabber grabber) {
		//make something happen
		grabber.DetachGrab();
		boss.EyeEaten(this);
	}
}
