using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StopTest : MonoBehaviour {

	public void StopTestButton(){

		// Log stop
		string time = System.DateTime.Now.ToString ();
		AppControl.control.dataString = "Test Stopped, " + "Time: " + time;
		AppControl.control.SaveData ();

		// Go to main menu
		SceneManager.LoadScene ("MainMenu");
	}
}
