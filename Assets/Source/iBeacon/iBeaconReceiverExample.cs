using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class iBeaconReceiverExample : MonoBehaviour {
	private Vector2 scrolldistance;
	private List<Beacon> mybeacons = new List<Beacon>();
	private bool scanning = true;
	public bool beaconOn, blueOn;
	public GameObject BeaconUI;
	public Text BeaconText;
	int changeIn;
	// Use this for initialization
	void Start () {
		iBeaconReceiver.BeaconRangeChangedEvent += OnBeaconRangeChanged;
		iBeaconReceiver.BluetoothStateChangedEvent += OnBluetoothStateChanged;
		iBeaconReceiver.CheckBluetoothLEStatus();
		Debug.Log ("Listening for beacons");
	}
	
	void OnDestroy() {
		iBeaconReceiver.BeaconRangeChangedEvent -= OnBeaconRangeChanged;
		iBeaconReceiver.BluetoothStateChangedEvent -= OnBluetoothStateChanged;
	}
	// Update is called once per frame
	void Update () {
		if (Application.loadedLevel != 1) {
//			Screen.orientation = ScreenOrientation.Portrait;
			if (mybeacons.Count > 0 && blueOn) {
				beaconOn = true;
			} else {
				beaconOn = false;
			
			}
		} else {
//			Screen.orientation = ScreenOrientation.LandscapeLeft;
			BeaconUI.SetActive (true);
//			if(mybeacons.Count != changeIn){
			BeaconText.text = "";
			foreach (Beacon b in mybeacons) {
				BeaconText.text = BeaconText.text+"\nUUID: " + b.UUID + "\nMajor: " + b.major + "\nMinor: " + b.minor + "\nRange: " + b.range.ToString () + "\nRssi: " + b.rssi + "\n=================================";
			}
//			changeIn = mybeacons.Count;
//			}

		}
	}
	private void OnBluetoothStateChanged(BluetoothLowEnergyState newstate) {
		switch (newstate) {
		case BluetoothLowEnergyState.POWERED_ON:
			iBeaconReceiver.Init();
			blueOn = true;
			Debug.Log ("It is on, go searching");
			break;
		case BluetoothLowEnergyState.POWERED_OFF:
			//iBeaconReceiver.EnableBluetooth();
			mybeacons.Clear();
			blueOn = false;
			
			Debug.Log ("It is off, switch it on");
			break;
		case BluetoothLowEnergyState.UNAUTHORIZED:
			Debug.Log("User doesn't want this app to use Bluetooth, too bad");
			break;
		case BluetoothLowEnergyState.UNSUPPORTED:
			Debug.Log ("This device doesn't support Bluetooth Low Energy, we should inform the user");
			break;
		case BluetoothLowEnergyState.UNKNOWN:
		case BluetoothLowEnergyState.RESETTING:
		default:
			Debug.Log ("Nothing to do at the moment");
			break;
		}
	}

	private void OnBeaconRangeChanged(List<Beacon> beacons) { // 
		foreach (Beacon b in beacons) {
			if (mybeacons.Contains(b)) {
				mybeacons[mybeacons.IndexOf(b)] = b;
			} else {
				// this beacon was not in the list before
				// this would be the place where the BeaconArrivedEvent would have been spawned in the the earlier versions
				mybeacons.Add(b);
			}
		}
		foreach (Beacon b in mybeacons) {
			if (b.lastSeen.AddSeconds(10) < DateTime.Now) {
				// we delete the beacon if it was last seen more than 10 seconds ago
				// this would be the place where the BeaconOutOfRangeEvent would have been spawned in the earlier versions
				mybeacons.Remove(b);
			}
		}
	}
	public void beaconClose(){
		iBeaconReceiver.Stop();
		Application.LoadLevel(0);
	}

//	void OnGUI() {
//	if(Application.loadedLevel == 1){
//		Screen.orientation = ScreenOrientation.LandscapeLeft;
//		GUIStyle labelStyle = GUI.skin.GetStyle("Label");
//#if UNITY_ANDROID
//		labelStyle.fontSize = 40;
//#elif UNITY_IOS
//		labelStyle.fontSize = 25;
//#endif
//		float currenty = 10;
//		float labelHeight = labelStyle.CalcHeight(new GUIContent("IBeacons"), Screen.width-20);
//		GUI.Label(new Rect(currenty,10,Screen.width-20,labelHeight),"IBeacons");
//		
//		currenty += labelHeight;
//		scrolldistance = GUI.BeginScrollView(new Rect(10,currenty,Screen.width -20, Screen.height - currenty - 10),scrolldistance,new Rect(0,0,Screen.width - 20,mybeacons.Count*100));
//		GUILayout.BeginVertical("box",GUILayout.Width(Screen.width-20),GUILayout.Height(50));
//		foreach (Beacon b in mybeacons) {
//			GUILayout.Label("UUID: "+b.UUID);
//			GUILayout.Label("Major: "+b.major);
//			GUILayout.Label("Minor: "+b.minor);
//			GUILayout.Label("Range: "+b.range.ToString());
//			GUILayout.Label("Rssi: "+b.rssi);
//		}
//		GUILayout.EndVertical();
//		GUI.EndScrollView();
//	}
//	}
}
