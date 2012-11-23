using UnityEngine;
using System.Collections;

public class SceneActionSpawnEntity : SequencerAction {
	public string type;
	public string name;
	public string waypoint;
	
	public override void Start(MonoBehaviour behaviour) {
		EntityManager.instance.Spawn(type, name, null, waypoint);
	}
}
