using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_Controller : MonoBehaviour {

	public void StartTest(){
		SceneManager.LoadScene ("Temp_Test_Selector");
	}

	private bool active = false;

	public void GoToSettings(){
		AndroidNotificationBuilder builder = new AndroidNotificationBuilder (SA.Common.Util.IdFactory.NextId, 
				                                     "Test Notification", "The text is here", 5);
		AndroidNotificationManager.Instance.ScheduleLocalNotification (builder);
	}

	public void GoToHelp(){
		AppControl.control.ClearData ();
	}

	public void Skip(){
		Application.Quit ();
	}

}
