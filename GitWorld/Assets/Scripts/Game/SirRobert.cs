using UnityEngine;
using System.Collections;

//place this guy in the level, don't spawn
public class SirRobert : Entity, Entity.IListener {
	public Transform heartHolder;
	
	public float heartRegenDelay = 3.0f;
	
	public int healthCriteria = 3;
	
	public float minPlayerDistance = 60; //based on planet space
	public float minPlayerNeedHeartDistance = 60; //based on planet space
	
	public float acceleration;
	
	private ItemHeart mHeart;
	private float mCurTime = 0;
	private ItemHeart.State mCurHeartState;
	private Player mPlayer = null;
	private float mMinDist;
			
	protected override void Awake() {
		base.Awake();
		
		mHeart = heartHolder.GetComponentInChildren<ItemHeart>();
		mHeart.stateCallback = OnHeartStateChange;
		
		mMinDist = minPlayerDistance;
	}

	// Use this for initialization
	protected override void Start () {
		base.Start();
		
		mHeart.Activate(false);
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(mPlayer == null) {
			SceneLevel sl = SceneLevel.instance;
			if(sl != null) {
				mPlayer = sl.player;
			}
		}
		else {
			bool needHeal = mPlayer.stats.curHP < healthCriteria;
			
			//follow player
			if(planetAttach.GetDistanceHorizontal(mPlayer.planetAttach) > mMinDist) {
				Vector2 dir = planetAttach.GetDirTo(mPlayer.planetAttach, true);
				planetAttach.accel = dir*acceleration;
				action = Entity.Action.move;
			} 
			else if(planetAttach.velocity.x != 0) {
				planetAttach.velocity = Vector2.zero;
				planetAttach.accel = Vector2.zero;
				action = Entity.Action.idle;
			}
			
			//update heart
			switch(mCurHeartState) {
			case ItemHeart.State.Inactive:
				if(needHeal) {
					mCurTime += Time.deltaTime;
					if(mCurTime >= heartRegenDelay) {
						mHeart.Activate(true);
					}
					
					mMinDist = minPlayerNeedHeartDistance;
				}
				else {
					mMinDist = minPlayerDistance;
				}
				break;
			}
		}
	}
	
	void OnDestroy() {
		mHeart = null;
		mPlayer = null;
	}
	
	public void OnEntityAct(Action act) {
	}
	
	public void OnEntityInvulnerable(bool yes) {
	}
	
	public void OnEntityCollide(Entity other, bool youAreReceiver) {
	}
	
	public void OnEntitySpawnFinish() {
	}
			
	void OnHeartStateChange(ItemHeart heart, ItemHeart.State state) {
		mCurTime = 0;
		
		mCurHeartState = state;
		switch(mCurHeartState) {
		case ItemHeart.State.Inactive:
			heartHolder.gameObject.SetActiveRecursively(false);
			break;
		case ItemHeart.State.Active:
			heartHolder.gameObject.SetActiveRecursively(true);
			break;
		case ItemHeart.State.Grabbed:
			break;
		case ItemHeart.State.Eaten:
			heart.transform.parent = heartHolder;
			heart.transform.localPosition = Vector3.zero;
			heart.transform.localRotation = Quaternion.identity;
			heart.transform.localScale = Vector3.one;
			heart.Activate(false); //will set state to inactive
			break;
		}
	}
}