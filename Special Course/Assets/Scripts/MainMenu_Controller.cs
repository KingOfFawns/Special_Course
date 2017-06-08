using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_Controller : MonoBehaviour {

	public void StartTest(){
		SceneManager.LoadScene ("Temp_Test_Selector");
	}

	public void GoToSettings(){

	}

	public void GoToHelp(){
		AppControl.control.ClearData ();
	}

	public void Skip(){
		Application.Quit ();
	}

}
