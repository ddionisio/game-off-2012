using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public const int maxJump = 2;
	
	private Player mPlayer;
	
	void Awake() {
		mPlayer = GetComponent<Player>();
	}
	
	void Update() {
		PlanetAttach planetAttach = mPlayer.planetAttach;
		
		float xS = Input.GetAxis("Horizontal");
		if(xS > 0.0f) {
			xS = -1.0f;
		}
		else if(xS < 0.0f) {
			xS = 1.0f;
		}
		
		if(planetAttach.jumpCounter < maxJump) {
			if(Input.GetButtonDown("Jump")) {
				planetAttach.Jump(mPlayer.jumpSpeed);
				
				if(xS != 0.0f) {
					planetAttach.velocity.x = xS*mPlayer.moveSpeed;
				}
			}
		}
		
		if(planetAttach.jumpCounter == 0 && planetAttach.isGround) {
			planetAttach.velocity.x = xS*mPlayer.moveSpeed;
		}
		
		planetAttach.velocity.y = 0;
	}
}
