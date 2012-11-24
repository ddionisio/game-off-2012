using UnityEngine;
using System.Collections;

public class SceneActionSetWave : SequencerAction {
	public int current = 0;
	public int max = 0;

	public override void Start(MonoBehaviour behaviour) {	
		Main.instance.uiManager.hud.wave.SetWave(current, max);
	}
}
