using UnityEngine;
using System.Collections;

public class CreatureSheep : Entity, Entity.IListener {
	public float throwSpeed = 10;
	public float moveSpeed = 15;
			
	private bool mThrown = false;
	private Vector2 mThrowVel;
	private Transform mPrevParent;
	
	protected override void Awake() {
		base.Awake();
		
		mReticle = Reticle.Type.Grab;
	}

	// Use this for initialization
	protected override void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		
		if(!mThrown) {
			planetAttach.velocity.x = moveSpeed;
			planetAttach.velocity.y = 0;
		}
		else {
			planetAttach.velocity = mThrowVel;
		}
	}
	
	void OnPlanetLand(PlanetAttach planetAttach) {
		mThrown = false;
	}
	
	void OnGrabStart(PlayerGrabber grabber) {
	}
	
	void OnGrabDone(PlayerGrabber grabber) {
		mPrevParent = transform.parent;
		
		planetAttach.enabled = false;
		grabber.Retract(true);
	}
	
	void OnGrabRetractStart(PlayerGrabber grabber) {
	}
	
	void OnGrabRetractEnd(PlayerGrabber grabber) {
		//make something happen
	}
	
	void OnGrabThrow(PlayerGrabber grabber) {
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
	}
	
	public void OnEntityAct(Action act) {
	}
	
	public void OnEntityInvulnerable(bool yes) {
	}
	
	public void OnEntityCollide(Entity other, bool youAreReceiver) {
	}
	
	public void OnEntitySpawnFinish() {
	}
}
