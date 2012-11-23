using UnityEngine;
using System.Collections;

public class Reticle : MonoBehaviour {
	public enum Type {
		Grab,
		Hit,
		Eat,
		
		NumType
	}
	
	private static string[] mTypeClips = {"grab", "hit", "eat"};
	
	public tk2dAnimatedSprite animSprite;
	
	public void Activate(Type type) {
		animSprite.Play(mTypeClips[(int)type]);
	}
	
	void Awake() {
		if(animSprite == null) {
			animSprite = GetComponentInChildren<tk2dAnimatedSprite>();
		}
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
