using UnityEngine;
using System.Collections;

public class PlayerStats : EntityStats {
	[System.NonSerialized]
	public int score = 0;
	
	
	public override void ResetStats () {
		base.ResetStats ();
		
		score = 0;
	}
}
