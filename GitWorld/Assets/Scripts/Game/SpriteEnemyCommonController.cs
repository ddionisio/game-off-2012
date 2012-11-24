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
		
		Vector2 scale;
		
		switch(act) {
		case Entity.Action.idle:
			scale = mSprite.scale;
			scale.y = Mathf.Abs(scale.y);
			mSprite.scale = scale;
			break;
			
		case Entity.Action.stunned:
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
}
