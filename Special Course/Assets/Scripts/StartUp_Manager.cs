using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUp_Manager : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		AppControl.control.Load ();
		AppControl.control.Save ();

		Debug.Log (Application.persistentDataPath);
	}
}
