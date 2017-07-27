using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StopTest : MonoBehaviour {

	private bool press = false;

	public void StopTestButton(){
		// Second press of button
		if (press) {
			// If in test mode, go back to help
			if (AppControl.control.testOfTest) {
				SceneManager.LoadScene ("Help");
			} else {
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
			}
		} else {
			// First button press
			// Set the text of the button and ready up for second press
			Button back = GameObject.Find ("Button (9)").GetComponent<Button> ();
			back.transform.GetChild(0).GetComponent<Text>().text = "Sikker?";
			press = true;
			// Start reset of button
			StartCoroutine (Reset ());
		}
	}

	IEnumerator Reset(){
		// Reset button
		yield return new WaitForSeconds (3);
		press = false;
		Button back = GameObject.Find ("Button (9)").GetComponent<Button> ();
		back.transform.GetChild(0).GetComponent<Text>().text = "Stop test";
	}
}
