using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_Controller : MonoBehaviour {

	public void StartTest(){
		SceneManager.LoadScene ("Temp_Test_Selector");
	}

	public void GoToSettings(){
		SceneManager.LoadScene ("Settings_Login");
	}

	public void GoToHelp(){
		SceneManager.LoadScene ("Help");
	}

	public void GoToResults(){
		SceneManager.LoadScene ("ResultOverview");
	}

	public void Skip(){

		// Log skip
		string time = System.DateTime.Now.ToString ();
		AppControl.control.dataString = "Skipped, " + "Time: " + time;
		AppControl.control.SaveData ();


		Application.Quit ();
	}

}
