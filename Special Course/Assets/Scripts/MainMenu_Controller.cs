using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainMenu_Controller : MonoBehaviour {

	public GameObject MainCamera;
	public Button startTest;
	public Button skip;
	public Text test;

	void Start(){
		if (AppControl.control.first_Time_Start) {
			SceneManager.LoadScene ("AdjustPatientNumber");
		}
	}

	void Update(){
		// Get current time
		DateTime now = DateTime.Now;

		// Compare to controlled notification
		TimeSpan diff = now - AppControl.control.notificationTime;
		int secondsFromNotification = (diff.Hours * 60 + diff.Minutes) * 60 + diff.Seconds + 360;

		// Compare to random notification
		diff = now - AppControl.control.randomNotificationTime;
		int secondsFromRandom = (diff.Hours * 60 + diff.Minutes) * 60 + diff.Seconds + 360;

		// Activate 'Start test' button and 'Skip' Button
		if (secondsFromNotification < 360 || secondsFromRandom < 360) {

			test.text = "Active";

			/*
			startTest.enabled = true;
			skip.enabled = true;
			*/
		} else {

			test.text = "Inactive";

			/*
			startTest.enabled = false;
			skip.enabled = false;
			*/
		}
	}

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
		string patientNumber = "#" + AppControl.control.patientNumber.ToString().Substring(1);
		string time = System.DateTime.Now.ToString ();

		AppControl.control.dataString = "Patient Number: " + patientNumber + ", Skipped" + ", Time: " + time;
		AppControl.control.csvString = patientNumber + ";Skipped;" + time + ";;;;;;;;;;;;;;;;";
		AppControl.control.SaveData ();

		// Reset AchieveCounter
		AppControl.control.achieveCounter = 0;
		AppControl.control.Save ();

		Application.Quit ();
	}

	public void Exit(){
		Application.Quit ();
	}
}