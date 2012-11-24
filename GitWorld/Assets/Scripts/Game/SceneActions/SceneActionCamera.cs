using UnityEngine;
using System.Collections;

public class SceneActionCamera : SequencerAction {
	public CameraController.Mode mode = CameraController.Mode.Free;
	public string attachToPath = "";
	public string attachToWaypoint = ""; //waypoint name, not path!
	public bool immediate=true;
	
	public override void Start(MonoBehaviour behaviour) {
		Main.instance.cameraController.mode = mode;
		
		if(attachToPath.Length > 0) {
			GameObject go = ((SceneController)behaviour).SearchObject(attachToPath);
			if(go != null) {
				Main.instance.cameraController.attach = go.transform;
			}
			else {
				Debug.LogWarning("Path not found: "+attachToPath);
			}
		}
		else if(attachToWaypoint.Length > 0) {
			Transform t = WaypointManager.instance.GetWaypoint(attachToWaypoint);
			if(t != null) {
				Main.instance.cameraController.attach = t;
			}
			else {
				Debug.LogWarning("Can't find waypoint: "+attachToWaypoint);
			}
		}
	}
}
