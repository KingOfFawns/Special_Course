using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PatientNumber_Controller : MonoBehaviour {

	public InputField patientInput;
	public Text notifyText;
	public Text patientText;

	void Start(){
		patientText.text = AppControl.control.patientNumber.ToString ().Substring (1); 
	}

	public void GoBack(){
		SceneManager.LoadScene ("Settings");
	}

	public void StorePatientNumber(){
		if (patientInput.text != "") {
			notifyText.text = "Patient nummer gemt.";

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
		patientText.text = AppControl.control.patientNumber.ToString ().Substring (1); 
		patientInput.text = "";
		yield return new WaitForSeconds (3);
		notifyText.text = "";
	}
}
