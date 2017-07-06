using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StopTest : MonoBehaviour {

	public void StopTestButton(){

		// Log stop
		string patientNumber = "#" + AppControl.control.patientNumber.ToString().Substring(1);
		string time = System.DateTime.Now.ToString ();

		AppControl.control.dataString = "Patient Number: " + patientNumber + ", Test Stopped" + ", Time: " + time;
		AppControl.control.csvString = patientNumber + ";Test Stopped;" + time + ";;;;;;;;;;;;;;;;";
		AppControl.control.SaveData ();


		// Reset AchieveCounter
		AppControl.control.achieveCounter = 0;
		AppControl.control.Save ();

		// Go to main menu
		SceneManager.LoadScene ("MainMenu");
	}
}
