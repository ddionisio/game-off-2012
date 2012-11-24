using UnityEngine;
using System.Collections;

public class HUDHitPoint : MonoBehaviour {
	public GameObject onWidget;
	public GameObject offWidget;
	
	public void SetOn(bool on) {
		onWidget.SetActiveRecursively(on);
		offWidget.SetActiveRecursively(!on);
	}
}
