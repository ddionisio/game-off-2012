using UnityEngine;
using System.Collections;

//this only works when spawning
public class ItemStar : Entity, Entity.IListener {
	public enum LifeState {
		None,
		Active,
		Grabbed,
		Thrown,
		Dying
	}
	
	public Color deathColor;
	public float grabScale;
	
	public float throwSpeed;
	public float throwRotateSpeed;
	
	public int numBounce;
	
	public float throwFadeOffDelay; //decay after thrown
	public float fadeOffDelay; //when do we start disappearing?
	public float dyingDelay; //duration of disappear
	
	private tk2dBaseSprite mSprite;
	private int mCurBounce = 0;
	private float mCurFadeTime = 0;
	
	private LifeState mLifeState = LifeState.None;
			
	protected override void Awake() {
		base.Awake();
		
		mSprite = GetComponent<tk2dBaseSprite>();
	}
	
	protected override void OnEnable() {
		base.OnEnable();
	}
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
	}
			
	void OnGrabStart(PlayerGrabber grabber) {
		mLifeState = LifeState.Grabbed;
		planetAttach.enabled = false;
		mCurFadeTime = 0;
		mCurBounce = 0;
		InvulnerableOff();
	}
	
	void OnGrabDone(PlayerGrabber grabber) {
		Vector3 scale = mSprite.scale;
		scale.y = scale.x = grabScale;
		mSprite.scale = scale;
		
		mSprite.color = Color.white;
		
		//particle?
		
		
		grabber.Retract(true);
	}
	
	void OnGrabRetractStart(PlayerGrabber grabber) {
		//call proper state as 'grabbed'
	}
	
	void OnGrabRetractEnd(PlayerGrabber grabber) {
		//get eaten
	}
	
	void OnGrabDetach(PlayerGrabber grabber) {
		transform.parent = EntityManager.instance.transform;
		
		planetAttach.enabled = true;
		planetAttach.applyGravity = true;
	}
	
	void OnGrabThrow(PlayerGrabber grabber) {
		mLifeState = LifeState.Thrown;
		mCurBounce = 0;
		mCurFadeTime = 0;
		
		gameObject.layer = Main.layerPlayerProjectile;
		
		planetAttach.applyOrientation = false;
		
		//compute velocity in planet space
		Vector2 dir = planetAttach.ConvertToPlanetDir(grabber.head.up);
		
		Vector2 throwVel = dir*throwSpeed;
		if(Mathf.Sign(dir.x) == Mathf.Sign(grabber.thePlayer.planetAttach.planetDir.x)) {
			throwVel += grabber.thePlayer.planetAttach.velocity;
		}
		
		if(grabber.thePlayer.planetAttach.GetCurYVel() > 0) {
			throwVel.y += grabber.thePlayer.planetAttach.GetCurYVel();
		}
		
		planetAttach.velocity = throwVel;
	}
	
	void OnPlanetLand(PlanetAttach pa) {
		Vector2 vel = planetAttach.velocity;
		vel.y = Mathf.Abs(vel.y);
		planetAttach.velocity = vel;
		planetAttach.ResetCurYVel();
		
		if(mCurBounce < numBounce) {
			mCurBounce++;
		}
	}
	
	public void OnEntityAct(Action act) {
		switch(act) {
		case Action.spawning:
			mLifeState = LifeState.None;
			mCurFadeTime = 0.0f;
			planetAttach.applyOrientation = true;
			planetAttach.applyGravity = false;
			gameObject.layer = Main.layerItem;
			mReticle = Reticle.Type.Grab;
			mSprite.color = Color.white;
			mSprite.scale = Vector3.one;
			break;
		}
	}
	
	public void OnEntityInvulnerable(bool yes) {
	}
	
	public void OnEntityCollide(Entity other, bool youAreReceiver) {
		//bouncing off enemy who received our 'star'tling blow! (har har!)
		if(!youAreReceiver && mCurBounce < numBounce && mLifeState == LifeState.Thrown) {
			Vector2 vel = planetAttach.velocity;
			vel.x *= -1;
			planetAttach.velocity = vel;
			planetAttach.ResetCurYVel();
			
			mCurBounce++;
		}
	}
		
	public void OnEntitySpawnFinish() {
		//start fading out
		mLifeState = LifeState.Active;
		
		action = Entity.Action.idle;
	}
	
	void Decay(float delay) {
		mCurFadeTime += Time.deltaTime;
		if(mCurFadeTime >= delay || mCurBounce >= numBounce) {
			mCurFadeTime = 0;
			mSprite.color = deathColor;
			mLifeState = LifeState.Dying;
		}
	}
	
	void LateUpdate () {
		switch(mLifeState) {
		case LifeState.Active:
			Decay(fadeOffDelay);
			break;
			
		case LifeState.Thrown:
			
			
			Decay(throwFadeOffDelay);
			break;
			
		case LifeState.Dying:
			mCurFadeTime += Time.deltaTime;
			if(mCurFadeTime >= dyingDelay) {
				mLifeState = LifeState.None;
				EntityManager.instance.Release(transform);
			}
			break;
		}
		
		if(!planetAttach.applyOrientation) {
			transform.up = Util.Vector2DRot(transform.up, throwRotateSpeed*Time.deltaTime*Mathf.Deg2Rad);
		}
	}
}
