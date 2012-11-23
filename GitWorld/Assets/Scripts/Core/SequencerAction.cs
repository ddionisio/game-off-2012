using UnityEngine;
using System.Collections;

public class SequencerAction  {
	public float delay;
	
	//store any non-readonly fields to given behaviour, don't put them here, store them in behaviour
	//these sequence actions can be shared by different behaviours
	
	public virtual void Start(MonoBehaviour behaviour) {
	}
	
	/// <summary>
	/// Periodic update, return true if done.
	/// </summary>
	public virtual bool Update(MonoBehaviour behaviour) {
		return true;
	}
	
	public virtual void Finish(MonoBehaviour behaviour) {
	}
}
