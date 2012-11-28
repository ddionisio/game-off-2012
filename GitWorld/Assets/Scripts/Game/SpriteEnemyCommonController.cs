using UnityEngine;
using System.Collections;

public class SpriteEnemyCommonController : SpriteEntityController {

	protected override void Awake() {
		base.Awake();
	}
	
	protected override void Start() {
		base.Start();
	}
	
	protected override void Update() {
		base.Update();
	}

	public override void OnEntityAct(Entity.Action act) {
		base.OnEntityAct(act);
		
		switch(act) {
		case Entity.Action.spawning:
		case Entity.Action.idle:
			ResetCommonData();
			break;
			
		case Entity.Action.stunned:
			Vector2 scale;
			scale = mSprite.scale;
			scale.y = -Mathf.Abs(scale.y);
			mSprite.scale = scale;
			break;
		}
	}
	
	public override void OnEntityInvulnerable(bool yes) {
		base.OnEntityInvulnerable(yes);
	}
	
	public override void OnEntityCollide(Entity other, bool youAreReceiver) {
		base.OnEntityCollide(other, youAreReceiver);
	}
	
	public override void OnEntitySpawnFinish() {
		base.OnEntitySpawnFinish();
	}
	
	void ResetCommonData() {
		Vector2 scale;
		scale = mSprite.scale;
		scale.y = Mathf.Abs(scale.y);
		mSprite.scale = scale;
	}
}
