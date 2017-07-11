using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainMenu_Controller : MonoBehaviour {

	public GameObject startTest;
	public GameObject skip;
	public GameObject exit;

	void Start(){
		if (AppControl.control.first_Time_Start) {
			SceneManager.LoadScene ("AdjustPatientNumber");
		} else {
			List<LocalNotificationTemplate> scheduled = AndroidNotificationManager.Instance.LoadPendingNotifications (true);
			List<int> ids = new List<int>();

			foreach (LocalNotificationTemplate s in scheduled) {
				ids.Add (s.id);
			}
				
			for (int i = 0; i < 5; i++) {
				if(!ids.Contains(AppControl.control.randomNotificationId[i])) {
					AndroidNotificationManager.Instance.CancelLocalNotification(AppControl.control.randomNotificationId[i]);
				}
				StartRandomNotification (i);
			}
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
			startTest.SetActive(true);
			skip.SetActive(true);
			exit.SetActive(false);
		} else {
			startTest.SetActive(false);
			skip.SetActive(false);
			exit.SetActive(true);
		}
	}

	public void StartTest(){
		SceneManager.LoadScene ("Word_Recog_Start");
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


	void StartRandomNotification(int days){
		// Get set notification
		int notiHour = AppControl.control.notificationTime.Hour;
		int notiMinutes = AppControl.control.notificationTime.Minute;

		// Generate Random time
		int intHour = UnityEngine.Random.Range (0, 24);
		int intMinutes = UnityEngine.Random.Range (0, 60);

		DateTime noti = DateTime.Parse("0001-01-01 " + notiHour + ":" + notiMinutes);
		DateTime rando = DateTime.Parse("0001-01-01 " + intHour + ":" + intMinutes);

		double minutes = (noti - rando).TotalMinutes;

		// Check if random notification is too close to the set notification
		bool onNoti = true;
		while (onNoti) {
			if (minutes >= 60) {
				onNoti = false;
			} else {
				// Generate Random time
				intHour = UnityEngine.Random.Range (0, 24);
				intMinutes = UnityEngine.Random.Range (0, 60);
				rando = new DateTime (0001, 1, 1, intHour, intMinutes, 0);

				minutes = (noti - rando).TotalMinutes;
			}
		}


		// Check if selected time is in the sleep zone
		bool isInSleepZone = false;
		CheckSleepZone (intHour, intMinutes, ref isInSleepZone);

		if (isInSleepZone) {
			StartRandomNotification (days);
		} else {
			// Store notification id
			AppControl.control.randomNotificationId[days] = SA.Common.Util.IdFactory.NextId;
			AppControl.control.Save ();

			//First notification if available on day of fire
			DateTime now = DateTime.Now;

			DateTime notificationTime = new DateTime (DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, intHour, intMinutes, 0);

			TimeSpan diff = notificationTime - now;

			int startFirstNotification = 0;

			if (diff.TotalMinutes < 0) {
				// First notification is the next day
				startFirstNotification = (int)(24 * 60 * 60 + diff.TotalMinutes * 60) + (days * 24*60*60); 
			} else {
				// First notification is on the same day
				startFirstNotification = (int)(diff.TotalMinutes * 60) + (days * 24*60*60);
			}

			// Create notification
			AndroidNotificationBuilder builder = new AndroidNotificationBuilder(AppControl.control.randomNotificationId[days], "Tilfældig Test", "Det er tid til at teste dig selv.", startFirstNotification);

			// Launch notifications
			AndroidNotificationManager.Instance.ScheduleLocalNotification (builder);
		}
	}

	void CheckSleepZone (int intHour, int intMinutes, ref bool isInSleepZone)
	{
		int start = AppControl.control.sleepZoneStart.Hour;
		int end = AppControl.control.sleepZoneEnd.Hour;
		int currentHour = start;
		bool hourIsInSleep = false;

		for (int i = 0; i < 24; i++) {
			if (intHour == currentHour) {
				hourIsInSleep = true;
				break;
			}
			else if (currentHour == end) {
				hourIsInSleep = false;
				break;
			}
			currentHour++;
			if (currentHour > 23) {
				currentHour = 0;
			}
		}
		if (hourIsInSleep && (intHour == start || intHour == end)) {
			hourIsInSleep = false;
			if (intHour == start && intMinutes > AppControl.control.sleepZoneStart.Minute) {
				isInSleepZone = true;
			}
			else if (intMinutes < AppControl.control.sleepZoneEnd.Minute) {
				isInSleepZone = true;
			}
		}
		else if (hourIsInSleep) {
			hourIsInSleep = false;
			isInSleepZone = true;
		}
	}
}