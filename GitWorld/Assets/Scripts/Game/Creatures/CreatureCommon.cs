using UnityEngine;
using System.Collections;

public class CreatureCommon : Entity, Entity.IListener {
	public string afterSpawnAIState; //set ai state to this after spawning
	
	private AIController mAI;
	
	protected override void Awake() {
		base.Awake();
		
		mAI = GetComponent<AIController>();
	}
	
	protected override void OnEnable() {
		base.OnEnable();
	}
	
	// Use this for initialization
	protected override void Start () {
		mCollideLayerMask = Main.layerMaskPlayerProjectile;
		
		base.Start();
	}
			
	//void OnGrabStart(PlayerGrabber grabber) {
	//}
	
	protected virtual void OnGrabDone(PlayerGrabber grabber) {
		planetAttach.enabled = false;
		grabber.Retract(true);
	}
	
	protected virtual void OnGrabRetractStart(PlayerGrabber grabber) {
		//call proper state as 'grabbed'
	}
	
	protected virtual void OnGrabRetractEnd(PlayerGrabber grabber) {
		//get eaten
	}
	
	public void OnEntityAct(Action act) {
	}
	
	public void OnEntityInvulnerable(bool yes) {
		mCollideLayerMask = yes ? 0 : Main.layerMaskPlayerProjectile;
	}
	
	public void OnEntityCollide(Entity other, bool youAreReceiver) {
		GameObject go = other.gameObject;
		if(youAreReceiver && go.layer == Main.layerPlayerProjectile) {
		}
	}
	
	public void OnEntitySpawnFinish() {
		if(mAI != null && !string.IsNullOrEmpty(afterSpawnAIState)) {
			mAI.SequenceSetState(afterSpawnAIState);
		}
	}
	
	/*void OnGrabThrow(PlayerGrabber grabber) {
		transform.parent = mPrevParent;
		mPrevParent = null;
		
		planetAttach.enabled = true;
		planetAttach.RefreshPos();
		mThrown = true;
		
		//mPlanetAttach.applyGravity = false;
		
		//compute velocity in planet space
		Vector2 dir = planetAttach.ConvertToPlanetDir(grabber.head.up);
		
		mThrowVel = dir*throwSpeed;
		if(Mathf.Sign(dir.x) == Mathf.Sign(grabber.thePlayer.planetAttach.planetDir.x)) {
			mThrowVel += grabber.thePlayer.planetAttach.velocity;
		}
		
		if(grabber.thePlayer.planetAttach.GetCurYVel() > 0) {
			mThrowVel.y += grabber.thePlayer.planetAttach.GetCurYVel();
		}
	}*/
	
	void LateUpdate () {
	}
}
