using UnityEngine;
using System.Collections;

public class UIBehaviourCont : MonoBehaviour {

	// Use this for initialization

	public void beaconPage(){
		iBeaconReceiver.Stop();
		Application.LoadLevel(1);
	}
}
