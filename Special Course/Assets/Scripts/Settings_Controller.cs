using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
}
