using UnityEngine;
using System.Collections;

public class SceneActionActivateEntityType : SequencerAction {
	public string type;
	public bool activate = true;
	
	public override void Start(MonoBehaviour behaviour) {
		if(type.Length > 0) {
			System.Type theType = System.Type.GetType(type);
			Component[] comps = ((SceneController)behaviour).GetComponentsInChildren(theType);
			foreach(Component c in comps) {
				c.SendMessage("OnSceneActivate", activate, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}