using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour {
	public float cameraDistanceLimit;
	
	public PlanetBody body {
		get {
			return mPlanetBody;
		}
	}
	
	private PlanetBody mPlanetBody;
	
	void Awake() {
		mPlanetBody = GetComponent<PlanetBody>();
	}
	
	// Use this for initialization
	void Start () {
		Main.instance.cameraController.originMinDistance = cameraDistanceLimit;
		Main.instance.cameraController.origin = transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
