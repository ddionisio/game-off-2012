using UnityEngine;
using System.Collections;

public class SceneActionAnnounce : SequencerAction {
	public int colorR = 255;
	public int colorG = 255;
	public int colorB = 255;
	
	public HUDAnnounce.State state = HUDAnnounce.State.None;
	
	public string message = "";
	
	public override void Start(MonoBehaviour behaviour) {
		HUDAnnounce ha = Main.instance.uiManager.hud.announce;
		
		if(state != HUDAnnounce.State.None) {
			if(state != HUDAnnounce.State.FadeOut || message.Length > 0) {
				ha.color = new Color(((float)colorR)/255.0f, ((float)colorG)/255.0f, ((float)colorB)/255.0f);
				ha.message = message;
			}
		}
		
		ha.state = state;
	}
}
