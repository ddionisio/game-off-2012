using UnityEngine;
using System.Collections;

public class AISetDirToPlayer : SequencerAction {
	public bool horizontalOnly = true;
			
	public override void Start(MonoBehaviour behaviour) {
		AIController ai = (AIController)behaviour;
		PlanetAttach pa = ai.planetAttach;
		Player player = SceneLevel.instance.player;
		
		ai.curPlanetDir = pa.GetDirTo(player.planetAttach, horizontalOnly);
	}
}
