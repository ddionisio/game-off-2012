using UnityEngine;
using System.Collections;

public class CreatureCommon : Entity, Entity.IListener {
	public string afterSpawnAIState; //set ai state to this after spawning
	
	public float hurtDelay = 1.0f;
	
	public float stunDelay = 3; //set to zero for no stun, die right away. use for certain enemies
	public float reviveDelay = 2; 
	public float dieDelay = 2;
	
	public bool applyGravity = true;
	public bool isComplex = false; //for bosses or large enemies with collision other than sphere
	public bool customLayer = false;
	public bool pauseAIOnHurt = true;
	
	protected AIController mAI;
	
	private string mLastAIState;
	
	private float mCurEnemyTime = 0;
	
	private Vector2 mPrevVelocity;
	private Vector2 mPrevAccel;
	
	protected override void Awake() {
		base.Awake();
		
		mAI = GetComponent<AIController>();
	}
	
	protected override void OnEnable() {
		base.OnEnable();
	}
	
	// Use this for initialization
	protected override void SceneStart() {
		ResetCommonData();
		
		base.SceneStart();
	}
			
	//void OnGrabStart(PlayerGrabber grabber) {
	//}
	
	protected virtual void OnGrabDone(PlayerGrabber grabber) {
		planetAttach.enabled = false;
		grabber.Retract(true);
		
		action = Entity.Action.grabbed;
	}
	
	protected virtual void OnGrabRetractStart(PlayerGrabber grabber) {
		//call proper state as 'grabbed'
	}
	
	protected virtual void OnGrabRetractEnd(PlayerGrabber grabber) {
		grabber.DetachGrab();
		
		planetAttach.enabled = true;
		
		//get eaten, let player know
		Release();
	}
	
	public virtual void OnEntityAct(Action act) {
		//Debug.Log("enemy: "+name+" acting: "+act);
		
		switch(act) {
		case Action.spawning:
			ResetCommonData();
			break;
			
		case Action.idle:
			//caution: we are also set here after revived, don't really need to do anything here...
			break;
			
		case Action.hurt:
			Invulnerable(hurtDelay);
			
			mPrevVelocity = planetAttach.velocity;
			mPrevAccel = planetAttach.accel;
			planetAttach.velocity = Vector2.zero;
			planetAttach.accel = Vector2.zero;
			
			if(pauseAIOnHurt)
				mAI.SequenceSetPause(true);
			break;
			
		case Action.reviving:
			mCurEnemyTime = 0;
			break;
			
		case Action.revived:
			string playAIState = mLastAIState;
			
			ResetCommonData();
			
			if(stats != null) {
				stats.ResetStats();
			}
			
			action = Entity.Action.idle;
			
			if(!string.IsNullOrEmpty(playAIState)) {
				mAI.SequenceSetState(playAIState);
			}
			break;
			
		case Action.stunned:
			//stop activity and become edible
			mLastAIState = mAI.curState;
			mAI.SequenceStop();
			planetAttach.velocity = Vector2.zero;
			planetAttach.accel = Vector2.zero;
			planetAttach.applyGravity = true;
						
			if(!customLayer) {
				gameObject.layer = Main.layerItem;
				mCollideLayerMask = 0;
			}
			
			mReticle = Reticle.Type.Eat;
			
			mCurEnemyTime = 0;
			break;
			
		case Action.die:
			mCurEnemyTime = 0;
			break;
		}
	}
	
	//call this to change ai state after getting hit, return the state to change to
	public virtual string AIToStateAfterHurt() {
		return null;
	}
	
	public virtual void OnEntityInvulnerable(bool yes) {
		if(!customLayer) {
			if(isComplex) {
				gameObject.layer = yes ? Main.layerEnemyNoPlayerProjectile : Main.layerEnemyComplex;
			}
			else {
				mCollideLayerMask = yes ? 0 : Main.layerMaskPlayerProjectile;
			}
		}
		
		//after invul wears off and we are hurt, resume activity
		if(!yes && action == Entity.Action.hurt) {
			action = prevAction;
			planetAttach.velocity = mPrevVelocity;
			planetAttach.accel = mPrevAccel;
			
			string aiChangeState = AIToStateAfterHurt();
			
			if(!string.IsNullOrEmpty(aiChangeState) && mAI.curState != aiChangeState) {
				mAI.SequenceSetState(aiChangeState);
			}
			else if(pauseAIOnHurt) {
				mAI.SequenceSetPause(false);
			}
		}
	}
	
	public void DoHurt() {
		if(stats.curHP == 0) {
			if(stunDelay > 0) {
				action = Entity.Action.stunned;
			}
			else {
				action = Entity.Action.die;
			}
		}
		else {
			action = Entity.Action.hurt;
		}
	}
	
	public void OnEntityCollide(Entity other, bool youAreReceiver) {
		GameObject go = other.gameObject;
		bool doIt = isComplex ? !youAreReceiver : youAreReceiver;
		doIt = doIt && go.layer == Main.layerPlayerProjectile && !FlagsCheck(Entity.Flag.Invulnerable);
		
		if(doIt) {
			if(stats != null && other.stats != null) {
				stats.ApplyDamage(other.stats);
				if(stats.curHP == 0) {
					if(stunDelay > 0) {
						action = Entity.Action.stunned;
					}
					else {
						action = Entity.Action.die;
					}
				}
				else {
					action = Entity.Action.hurt;
				}
			}
		}
	}
	
	public virtual void OnEntitySpawnFinish() {
		if(mAI != null && !string.IsNullOrEmpty(afterSpawnAIState)) {
			mAI.SequenceSetState(afterSpawnAIState);
		}
	}
	
	public override bool CanHarmPlayer() {
		return action != Entity.Action.hurt;
	}
	
	void ResetCommonData() {
		if(!customLayer) {
			mCollideLayerMask = isComplex ? 0 : Main.layerMaskPlayerProjectile;
			gameObject.layer = isComplex ? Main.layerEnemyComplex : Main.layerEnemy;
		}
		
		mReticle = Reticle.Type.NumType;
		mCurEnemyTime = 0;
		mPrevVelocity = Vector2.zero;
		mPrevAccel = Vector2.zero;
		mLastAIState = null;
		planetAttach.applyGravity = applyGravity;
	}
	
	protected virtual void LateUpdate () {
		switch(action) {
		case Action.stunned:
			mCurEnemyTime += Time.deltaTime;
			if(mCurEnemyTime >= stunDelay) {
				action = Entity.Action.reviving;
			}
			break;
			
		case Action.reviving:
			mCurEnemyTime += Time.deltaTime;
			if(mCurEnemyTime >= reviveDelay) {
				action = Entity.Action.revived;
			}
			break;
			
		case Action.die:
			mCurEnemyTime += Time.deltaTime;
			if(mCurEnemyTime >= dieDelay) {
				Release();
			}
			break;
		}
	}
}
