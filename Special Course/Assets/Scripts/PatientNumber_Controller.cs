using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PatientNumber_Controller : MonoBehaviour {

	public InputField patientInput;
	public Text notifyText;
	public Text patientText;
	public GameObject back;
	public GameObject next;


	void Start(){
		// If first time app use
		if (AppControl.control.first_Time_Start) {
			back.SetActive (false);
			next.SetActive (true);
		}
		// Load patient number as text
		patientText.text = AppControl.control.patientNumber.ToString ().Substring (1); 
	}

	public void GoBack(){
		SceneManager.LoadScene ("Settings");
	}

	public void NextButton(){
		// Check if patientNumber is not 0
		if(AppControl.control.patientNumber == 0){
			notifyText.text = "Indtast patient nummer.";
			StartCoroutine (ResetNotifyText ());
		} else {
			SceneManager.LoadScene ("AdjustNotifications");
		}
	}

	public void StorePatientNumber(){
		// if patientNumber is not empty
		if (patientInput.text != "") {
			notifyText.text = "Patient nummer gemt.";

			// Parse patient number and store it (The extra 1 is for storing purpose)
			string patientNumber = "1" + patientInput.text;
			int value;
			int.TryParse (patientNumber, out value);
			AppControl.control.patientNumber = value;
			AppControl.control.Save ();
		} else {
			notifyText.text = "Kan ikke gemme tomt patient nummer.";
		}

		StartCoroutine (ResetNotifyText ());
	}

	IEnumerator ResetNotifyText(){
		// Reset notify text
		patientText.text = AppControl.control.patientNumber.ToString ().Substring (1); 
		patientInput.text = "";
		yield return new WaitForSeconds (1);
		notifyText.text = "";
	}
}
