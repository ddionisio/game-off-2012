using UnityEngine;
using System.Collections;

public class CreatureBoss : CreatureCommon {
	public override void Spawn() {
		base.Spawn();
		
		Main.instance.uiManager.hud.bossStatus.gameObject.SetActiveRecursively(true);
		Main.instance.uiManager.hud.bossStatus.SetStats(stats);
		
		//TODO: more stuff here for special bosses, e.g. super big boss when we need to adjust the camera settings
	}
}
