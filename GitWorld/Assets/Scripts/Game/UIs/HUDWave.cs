using UnityEngine;
using System.Collections;

public class HUDWave : MonoBehaviour {
	[SerializeField] UILabel label;
	[SerializeField] string waveFormat;
	
	public void SetWave(int cur, int max) {
		if(max == 0) {
			label.enabled = false;
		}
		else {
			label.enabled = true;
			label.text = string.Format(waveFormat, cur, max);
		}
	}
	
	void Awake() {
		label.enabled = false;
	}
}
