using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StopTest : MonoBehaviour {

	private bool press = false;


	public void StopTestButton(){
		if (press) {
			// Log stop
			string patientNumber = "#" + AppControl.control.patientNumber.ToString ().Substring (1);
			string time = System.DateTime.Now.ToString ();

			AppControl.control.dataString = "Patient Number: " + patientNumber + ", Test Stopped" + ", Time: " + time;
			AppControl.control.csvString = patientNumber + ";Test Stopped;" + time + ";;;;;;;;;;;;;;;;";
			AppControl.control.SaveData ();

			// Reset AchieveCounter
			AppControl.control.achieveCounter = 0;

			// End Test
			AppControl.control.testStarted = true;
			AppControl.control.Save ();

			// Go to main menu
			SceneManager.LoadScene ("MainMenu");
		} else {
			Button back = GameObject.Find ("Button (9)").GetComponent<Button> ();
			back.transform.GetChild(0).GetComponent<Text>().text = "Sikker?";
			press = true;
		}
	}
}
