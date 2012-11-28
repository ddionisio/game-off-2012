using UnityEngine;
using System.Collections;

public class Player : Entity, Entity.IListener {
	public float hurtInvulDelay;
	public float hurtDelay;
	
	/// <summary> The speed at which we get knocked back when hurt. </summary>
	public float hurtSpeed;
	/// <summary> The speed at which we jump when we get hurt. </summary>
	public float hurtJumpSpeed;
	
	/// <summary> For moving left/right (angle/s) </summary>
	public float moveSpeed;
	
	/// <summary> For jumping (m/s) </summary>
	public float jumpSpeed;
	
	public float deathDelay = 2.0f; //delay to bring game over menu up
	
	private PlayerController mController;
	
	private float mPlayerCurTime;
	
	///// implements
	
	protected override void Awake() {
		base.Awake();
		
		mController = GetComponent<PlayerController>();
	}
	
	protected override void OnEnable() {
		base.OnEnable();
	}

	protected override void SceneStart() {
		SetDefaultCollideMask();
		
		base.SceneStart();
	}
	
	void SetDefaultCollideMask() {
		mCollideLayerMask = Main.layerMaskEnemy | Main.layerMaskEnemyComplex 
			| Main.layerMaskEnemyNoPlayerProjectile | Main.layerMaskProjectile;
	}
				
	public void OnGrabStart() {
		mController.enabled = false;
	}
	
	public void OnGrabDone() {
		mController.enabled = true;
	}
	
	public void OnGrabRetractStart() {
	}
	
	public void OnGrabRetractEnd() {
	}
	
	public void OnGrabThrow() {
	}
	
	///// internal
	
	// Update is called once per frame
	void LateUpdate () {
		switch(action) {
		case Action.hurt:
			mPlayerCurTime += Time.deltaTime;
			if(mPlayerCurTime >= hurtDelay) {
				mController.enabled = true;
				action = Entity.Action.idle;
			}
			break;
			
		case Action.die:
			mPlayerCurTime += Time.deltaTime;
			if(mPlayerCurTime >= deathDelay) {
				Main.instance.uiManager.ModalOpen(UIManager.Modal.GameOver);
			}
			break;
		}
	}
	
	void OnUIModalActive() {
		planetAttach.velocity.x = 0;
		mController.enabled = false;
	}
	
	void OnUIModalInactive() {
		mController.enabled = true;
	}
	
	public void OnEntityAct(Action act) {
		switch(act) {
		case Action.idle:
			planetAttach.velocity = Vector2.zero;
			break;
			
		case Action.hurt:
			_SetActionDisablePlayer();
			Invulnerable(hurtInvulDelay);
			break;
			
		case Action.die:
			_SetActionDisablePlayer();
			BroadcastMessage("OnPlayerDeath", null, SendMessageOptions.DontRequireReceiver);
			break;
			
		case Action.victory:
			_SetActionDisablePlayer();
			break;
		}
	}
	
	void _SetActionDisablePlayer() {
		mPlayerCurTime = 0.0f;
		planetAttach.velocity = Vector2.zero;
		mController.enabled = false;
	}
	
	public void OnEntityInvulnerable(bool yes) {
		if(yes) {
			mCollideLayerMask = 0;
		}
		else {
			SetDefaultCollideMask();
		}
	}
	
	public void OnEntityCollide(Entity other, bool youAreReceiver) {
		if(youAreReceiver && other.CanHarmPlayer()) {
			stats.ApplyDamage(other.stats);
			if(stats.curHP == 0) {
				//dead!
				action = Entity.Action.die;
				
				planetAttach.velocity = Vector2.zero;
			}
			else {
				//get hurt
				action = Entity.Action.hurt;
				
				//knock back
				switch(planetAttach.CheckSide(other.planetAttach)) {
				case Util.Side.Left:
				case Util.Side.None:
					planetAttach.velocity = new Vector2(hurtSpeed, 0);
					break;
				case Util.Side.Right:
					planetAttach.velocity = new Vector2(-hurtSpeed, 0);
					break;
				}
				
				planetAttach.Jump(hurtJumpSpeed, false);
			}
		}
	}
	
	public void OnEntitySpawnFinish() {
		//players don't really spawn...unless we implement lives, or restart, or whatever
	}
	
	void OnSceneActivate(bool activate) {
		mController.enabled = activate;
		planetAttach.applyGravity = activate;
		
		if(!activate) {
			planetAttach.velocity = Vector2.zero;
			planetAttach.ResetCurYVel();
		}
	}
}
