using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StopTest : MonoBehaviour {

	public void StopTestButton(){

		// Log stop
		string time = System.DateTime.Now.ToString ();
		AppControl.control.dataString = "Test Stopped, " + "Time: " + time;
		AppControl.control.csvString = "Test Stopped;" + time + ";;;;;;;;;;;;;;;;";
		AppControl.control.SaveData ();


		// Reset AchieveCounter
		AppControl.control.achieveCounter = 0;
		AppControl.control.Save ();

		// Go to main menu
		SceneManager.LoadScene ("MainMenu");
	}
}
