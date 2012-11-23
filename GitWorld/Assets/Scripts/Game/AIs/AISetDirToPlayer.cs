using UnityEngine;
using System.Collections;

public class AISetDirToPlayer : SequencerAction {
	public bool horizontalOnly = true;
	
	public override void Start(MonoBehaviour behaviour) {
		AIController ai = (AIController)behaviour;
		PlanetAttach pa = ai.planetAttach;
		Player player = SceneLevel.instance.player;
		
		if(horizontalOnly) {
			ai.curPlanetDir = Vector2.zero;
			switch(pa.CheckSide(player.planetAttach)) {
			case Util.Side.Left:
				ai.curPlanetDir.x = -1;
				break;
			case Util.Side.Right:
				ai.curPlanetDir.x = 1;
				break;
			}
		}
		else {
			ai.curPlanetDir = pa.GetDirTo(player.planetAttach);
		}
	}
}
