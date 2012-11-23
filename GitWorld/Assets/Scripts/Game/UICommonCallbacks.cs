using UnityEngine;
using System.Collections;

public class UICommonCallbacks {
	public static void OnButtonClose(GameObject go) {
		Main.instance.uiManager.ModalCloseTop();
	}
}
