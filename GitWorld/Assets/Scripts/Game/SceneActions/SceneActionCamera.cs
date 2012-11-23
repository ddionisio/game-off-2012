using UnityEngine;
using System.Collections;

public class SceneActionCamera : SequencerAction {
	public CameraController.Mode mode = CameraController.Mode.Free;
	public string attachToPath = "";
	
	public override void Start(MonoBehaviour behaviour) {
		Main.instance.cameraController.mode = mode;
		
		if(attachToPath.Length > 0) {
			GameObject go = ((SceneController)behaviour).SearchObject(attachToPath);
			if(go != null) {
				Main.instance.cameraController.attach = go.transform;
			}
		}
	}
}
