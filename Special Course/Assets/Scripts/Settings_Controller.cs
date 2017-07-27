using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class Settings_Controller : MonoBehaviour {

	private bool resetData = false;
	private bool resetAppData = false;

	public void ReturnToStart(){
		SceneManager.LoadScene ("MainMenu");
	}

	public void GoToAdjust(){
		SceneManager.LoadScene("AdjustTests");
	}

	public void GoToNotifications(){
		SceneManager.LoadScene ("AdjustNotifications");
	}

	public void GoToLoginAdjust(){
		SceneManager.LoadScene ("AdjustSettingsLogin");
	}

	public void GoToPatientNumber(){
		SceneManager.LoadScene ("AdjustPatientNumber");
	}

	public void SendData(){
		// If folder does not exist, create it
		if (!Directory.Exists ("/storage/emulated/0/Download/SEE-COG")) {
			Directory.CreateDirectory ("/storage/emulated/0/Download/SEE-COG");
		}
		// If csv file exists, delete it
		if(File.Exists("/storage/emulated/0/Download/SEE-COG/SaveData.csv")){
			File.Delete("/storage/emulated/0/Download/SEE-COG/SaveData.csv");
		}
		// If txt file exists, delete it
		if(File.Exists("/storage/emulated/0/Download/SEE-COG/SaveData.txt")){
			File.Delete("/storage/emulated/0/Download/SEE-COG/SaveData.txt");
		}
		// Copy data files to folder if they exists
		if (File.Exists (Application.persistentDataPath + "/.dat2.dat")) {
			File.Copy (Application.persistentDataPath + "/.dat2.dat", "/storage/emulated/0/Download/SEE-COG/SaveData.txt");
		}
		File.Copy (Application.persistentDataPath + "/.dat1.dat", "/storage/emulated/0/Download/SEE-COG/SaveData.csv");

		// Show toast message to user, informing them that the data is sent
		AndroidToast.ShowToastNotification ("Data sendt til 'Download/See-COG'", AndroidToast.LENGTH_SHORT);
	}

	public void ResetResultData(){

		if (resetData) {
			resetData = false;
			// Delete folder SEE-COG if it exists
			if (Directory.Exists ("/storage/emulated/0/Download/SEE-COG")) {
				if (File.Exists ("/storage/emulated/0/Download/SEE-COG/SaveData.txt")) {
					File.Delete ("/storage/emulated/0/Download/SEE-COG/SaveData.txt");
				}
				if (File.Exists ("/storage/emulated/0/Download/SEE-COG/SaveData.csv")) {
					File.Delete ("/storage/emulated/0/Download/SEE-COG/SaveData.csv");
				}
				Directory.Delete ("/storage/emulated/0/Download/SEE-COG");
			}
			// Delete txt data file if it exists
			if (File.Exists (Application.persistentDataPath + "/.dat2.dat")) {
				File.Delete (Application.persistentDataPath + "/.dat2.dat");
			}
			// Delete csv data file if it exists
			if (File.Exists (Application.persistentDataPath + "/.dat1.dat")) {
				File.Delete (Application.persistentDataPath + "/.dat1.dat");
			}
			
			AndroidNotificationManager.Instance.ShowToastNotification ("Resultat data slettet", AndroidToast.LENGTH_LONG);
		} else {
			// First press of button, ready up for second press
			resetData = true;
			// Set button text
			Button rr = GameObject.Find ("Button (5)").GetComponent<Button> ();
			rr.transform.GetChild(0).GetComponent<Text>().text = "Sikker?";
			// Start reset of button
			StartCoroutine (ResetDataReset ());
		}
	}

	IEnumerator ResetDataReset(){
		// Reset button after 3 seconds
		yield return new WaitForSeconds (3);
		resetData = false;
		Button rr = GameObject.Find ("Button (5)").GetComponent<Button> ();
		rr.transform.GetChild(0).GetComponent<Text>().text = "Reset resultat data";
	}

	public void ResetAppData(){
		// If second press of button
		if (resetAppData) {
			resetAppData = false;

			resetData = true;
			ResetResultData ();

			// If appData file exists, delete it
			if (File.Exists (Application.persistentDataPath + "/.appData.dat")) {
				File.Delete (Application.persistentDataPath + "/.appData.dat");
			}

			// Reset app data
			AppControl.control.word_Recog_Target = 8;
			AppControl.control.word_Last_Test = -1;
			AppControl.control.word_previous_Test = -1;

			AppControl.control.N = 2;
			AppControl.control.N_percentage_last = 0f;

			AppControl.control.digitSpan_DigitLength = 3;

			AppControl.control.first_Time_Start = true;

			AppControl.control.wordRecogStart_WordTimer = 1.0f;
			AppControl.control.digitSpan_SequenceTimer = 1.0f;
			AppControl.control.NBack_Timer = 1.0f;

			AppControl.control.testStartDate = 
			new System.DateTime (
				System.DateTime.Now.Year,
				System.DateTime.Now.Month,
				System.DateTime.Now.Day,
				System.DateTime.Now.Hour - 1, 
				System.DateTime.Now.Minute, 
				System.DateTime.Now.Second);

			AppControl.control.password = "52 6F 6F 74 41 64 6D 69 6E";

			AppControl.control.maxSequenceLength = 0;

			AppControl.control.firstTestCleared = false;
			AppControl.control.tenTestsCleared = false;
			AppControl.control.fiftyTestsCleared = false;
			AppControl.control.hundredTestsCleared = false;
			AppControl.control.hundredfiftyTestsCleared = false;
			AppControl.control.achieveCounter = 0;

			AppControl.control.patientNumber = 0;

			// Save new app data file
			AppControl.control.Save ();

			// Go to main menu
			SceneManager.LoadScene ("MainMenu");
		} else {
			// First press of button, ready up for second press
			resetAppData = true;
			// Set button text
			Button ra = GameObject.Find ("Button (6)").GetComponent<Button> ();
			ra.transform.GetChild(0).GetComponent<Text>().text = "Sikker?";
			// Start reset of button
			StartCoroutine (ResetAppReset ());
		}
	}

	IEnumerator ResetAppReset(){
		//Reset button after 3 seconds
		yield return new WaitForSeconds (3);
		resetAppData = false;
		Button ra = GameObject.Find ("Button (6)").GetComponent<Button> ();
		ra.transform.GetChild(0).GetComponent<Text>().text = "Reset app data";
	}
}
