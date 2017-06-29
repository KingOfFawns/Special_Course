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

		AndroidToast.ShowToastNotification ("Filer downloadet", AndroidToast.LENGTH_SHORT);
	}
}
