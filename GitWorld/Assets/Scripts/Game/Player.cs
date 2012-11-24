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
		mCollideLayerMask = Main.layerMaskEnemy | Main.layerMaskProjectile;
		
		base.SceneStart();
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
		}
	}
	
	void OnUIModalActive() {
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
			Invulnerable(hurtInvulDelay);
			mController.enabled = false;
			break;
		}
	}
	
	public void OnEntityInvulnerable(bool yes) {
		mCollideLayerMask = yes ? 0 : Main.layerMaskEnemy | Main.layerMaskProjectile;
	}
	
	public void OnEntityCollide(Entity other, bool youAreReceiver) {
		GameObject go = other.gameObject;
		if(youAreReceiver && ((go.layer == Main.layerEnemy && other.action != Entity.Action.hurt) || go.layer == Main.layerProjectile)) {
			mPlayerCurTime = 0.0f;
			
			stats.ApplyDamage(other.stats);
			if(stats.curHP == 0) {
				//dead!
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
}
