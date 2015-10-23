using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameController : MonoBehaviour {
	
	iBeaconReceiverExample	beaconController;
	// Use this for initialization
	void Start () {
		beaconController = gameObject.GetComponent<iBeaconReceiverExample>();
	}
	
	// Update is called once per frame
	void Update () {



	if(beaconController.beaconOn)
			gameObject.GetComponent<Animation>().Play("Horse_Run");
		else if(!beaconController.beaconOn)
			gameObject.GetComponent<Animation>().Play("Horse_Walk");
	}  
}
