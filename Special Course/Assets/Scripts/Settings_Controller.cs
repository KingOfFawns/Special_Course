using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class Settings_Controller : MonoBehaviour {

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
		if (!Directory.Exists ("/storage/emulated/0/Download/SEE-COG")) {
			Directory.CreateDirectory ("/storage/emulated/0/Download/SEE-COG");
		}
		if(File.Exists("/storage/emulated/0/Download/SEE-COG/SaveData.csv")){
			File.Delete("/storage/emulated/0/Download/SEE-COG/SaveData.csv");
		}
		if(File.Exists("/storage/emulated/0/Download/SEE-COG/SaveData.txt")){
			File.Delete("/storage/emulated/0/Download/SEE-COG/SaveData.txt");
		}
			
		if (File.Exists (Application.persistentDataPath + "/.dat2.dat")) {
			File.Copy (Application.persistentDataPath + "/.dat2.dat", "/storage/emulated/0/Download/SEE-COG/SaveData.txt");
		}
		File.Copy (Application.persistentDataPath + "/.dat1.dat", "/storage/emulated/0/Download/SEE-COG/SaveData.csv");

		AndroidToast.ShowToastNotification ("Data sendt til 'Download/See-COG'", AndroidToast.LENGTH_SHORT);
	}

	public void ResetResultData(){
		if (Directory.Exists ("/storage/emulated/0/Download/SEE-COG")) {
			Directory.Delete ("/storage/emulated/0/Download/SEE-COG");
		}

		if (File.Exists (Application.persistentDataPath + "/.dat2.dat")) {
			File.Delete (Application.persistentDataPath + "/.dat2.dat");
		}
		if (File.Exists (Application.persistentDataPath + "/.dat1.dat")) {
			File.Delete (Application.persistentDataPath + "/.dat1.dat");
		}
	}

	public void ResetAppData(){
		if (File.Exists (Application.persistentDataPath + "/.appData.dat")) {
			File.Delete (Application.persistentDataPath + "/.appData.dat");
		}

		AndroidNotificationManager.Instance.CancelLocalNotification (AppControl.control.notificationId);

		AppControl.control.word_Recog_Target = 10;
		AppControl.control.word_Last_Test = -1;
		AppControl.control.word_previous_Test = -1;

		AppControl.control.N = 2;
		AppControl.control.N_percentage_last = 0f;

		AppControl.control.digitSpan_DigitLength = 3;

		AppControl.control.first_Time_Start = true;

		AppControl.control.wordRecogStart_WordTimer = 1.0f;
		AppControl.control.digitSpan_SequenceTimer = 1.0f;
		AppControl.control.NBack_Timer = 1.0f;

		AppControl.control.password = "52 6F 6F 74 41 64 6D 69 6E";

		AppControl.control.maxSequenceLength = 0;

		AppControl.control.firstTestCleared = false;
		AppControl.control.tenTestsCleared = false;
		AppControl.control.fiftyTestsCleared = false;
		AppControl.control.hundredTestsCleared = false;
		AppControl.control.hundredfiftyTestsCleared = false;
		AppControl.control.achieveCounter = 0;

		AppControl.control.patientNumber = 0;

		AppControl.control.Save ();

		SceneManager.LoadScene ("MainMenu");
	}
}
