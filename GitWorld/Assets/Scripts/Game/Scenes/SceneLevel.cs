using UnityEngine;
using System.Collections;

public class SceneLevel : SceneController {
	
	/// <summary>
	/// Only use this for components that are in-game related. Only available after awake/level loaded.
	/// </summary>
	public static SceneLevel instance {
		get {
			return (SceneLevel)Main.instance.sceneController;
		}
	}
	
	public Planet planet {
		get {
			return mPlanet;
		}
	}
	
	public Player player {
		get {
			return mPlayer;
		}
	}
			
	private Player mPlayer;
	private Planet mPlanet;
	
	protected override void Awake() {
		base.Awake();
		
		mPlayer = GetComponentInChildren<Player>();
		mPlanet = GetComponentInChildren<Planet>();
	}
}
