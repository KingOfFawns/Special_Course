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
		// Load local stored data
		AppControl.control.Load ();
		AppControl.control.Save (); // This creates file if not existing

		if (AppControl.control.first_Time_Start) {
			// Set DateTime array
			AppControl.control.randomNotification = new DateTime[10];
			AppControl.control.timer = new TimeSpan ();
			AppControl.control.Save ();

			SceneManager.LoadScene ("AdjustPatientNumber");
		} else {
			// Get list of pending notifications
			List<LocalNotificationTemplate> scheduled = AndroidNotificationManager.Instance.LoadPendingNotifications (true);
			// Get the ids.
			List<int> ids = new List<int>();
			foreach (LocalNotificationTemplate s in scheduled) {
				ids.Add (s.id);
			}

			// Find the latest date in which a notification is set
			DateTime max = DateTime.Now;
			foreach (DateTime d in AppControl.control.randomNotification) {
				max = d > max ? d : max; 
			}
				
				
			// Set random notification
			for (int i = 0; i < 10; i++) {
				if(!ids.Contains(AppControl.control.randomNotificationId[i])) {
					AndroidNotificationManager.Instance.CancelLocalNotification(AppControl.control.randomNotificationId[i]);
					max = StartRandomNotification(max,i);
					AppControl.control.randomNotification [i] = max;
				}
			}
			AppControl.control.Save ();
		}
	}

	void Update(){
		// Get current time
		DateTime now = DateTime.Now;

		// Get set notification time
		DateTime notiTime = AppControl.control.notificationTime;
		notiTime = new DateTime (DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, notiTime.Hour, notiTime.Minute, notiTime.Second);

		TimeSpan diff = now.Subtract(notiTime);

		// Calculate seconds from set notification
		double secondsFromNotification = diff.TotalSeconds;
		bool isInSet = false;
		if (secondsFromNotification >= 0 && secondsFromNotification < 3600) {
			isInSet = true;
		}

		// Get all random notification
		DateTime[] randoms = AppControl.control.randomNotification;
		DateTime random = new DateTime ();
		// Check if any random notification is active
		bool isInRandom = false;
		foreach (DateTime r in randoms) {
			diff = now.Subtract (r);
			double secondsFromRandom = diff.TotalSeconds;

			if (secondsFromRandom >= 0 && secondsFromRandom < 3600) {
				isInRandom = true;
				random = r;
				break;
			}
		}

		// Set boolean from test
		bool isStart = AppControl.control.testStarted;
		DateTime testStart = AppControl.control.testStartDate;

		// Get time from test start until next test can appear
		TimeSpan timeLeft = AppControl.control.timer;

		if (isInSet) {
			timeLeft = notiTime.AddHours (1).Subtract (testStart);
		} else if (isInRandom) {
			timeLeft = random.AddHours (1).Subtract (testStart);
		}
		AppControl.control.timer = timeLeft;
		AppControl.control.Save ();

		// Check if the next test can appear and ready up for the next test
		Debug.Log("Time left :" + timeLeft.TotalSeconds);
		Debug.Log("Timer: " + now.Subtract (testStart).TotalSeconds);

		if (now.Subtract (testStart).TotalSeconds >= timeLeft.TotalSeconds) {
			AppControl.control.testStarted = false;
			isStart = false;
		}

		// Activate 'Start test' button and 'Skip' Button
		if ((isInSet || isInRandom) && !isStart) {
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
		AppControl.control.testStartDate = DateTime.Now;

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


	DateTime StartRandomNotification(DateTime max, int randomNotificationNumber){
		// Get set notification
		int notiHour = AppControl.control.notificationTime.Hour;
		int notiMinutes = AppControl.control.notificationTime.Minute;

		// Generate Random time
		int intHour = UnityEngine.Random.Range (0, 24);
		int intMinutes = UnityEngine.Random.Range (0, 60);

		// Start and end hour of the sleep zone
		int sleepStart = AppControl.control.sleepZoneStart.Hour;
		int sleepEnd = AppControl.control.sleepZoneEnd.Hour;

		// list to contain hours that are acceptable for notifications
		List<int> goodHours = new List<int> ();

		// Calculate the good hours
		int currentHour = sleepEnd;

		int loopEnd = sleepStart + 1;
		if(loopEnd > 23){
			loopEnd = 0;
		}
		for (int i = 0; i < 24; i++) {
			goodHours.Add (currentHour);
			currentHour++;
			if (currentHour > 23) {
				currentHour = 0;
			}
			if (currentHour == loopEnd) {
				break;
			}
		}

		// Boolean to keep track of when the notification is in the sleep zone
		bool isInSleepZone = false;

		// The notification is in between the sleep zone hours
		if (!goodHours.Contains(intHour)){
			isInSleepZone = true;
		}

		// Cases when the notification is the same hour as the sleep zone start or end
		if (!isInSleepZone) {
			int diffStart = intMinutes - AppControl.control.sleepZoneStart.Minute;
			int diffEnd = intMinutes - AppControl.control.sleepZoneEnd.Minute;

			// Same hour as sleep zone start
			if(intHour == sleepStart){
				// If the notification time is after or on the sleep zone start time
				if (diffStart >= 0) {
					isInSleepZone = true;
				}
			} 
			// Same hour as sleep zone end
			else if(intHour == sleepEnd){
				// If the notification time is before or on the sleep zone end time
				if (diffEnd <= 0) {
					isInSleepZone = true;
				}
			}
		}

		while (isInSleepZone) {
			isInSleepZone = false;

			intHour = UnityEngine.Random.Range (0, 24);
			intMinutes = UnityEngine.Random.Range (0, 60);

			// The notification is in between the sleep zone hours
			if (!goodHours.Contains(intHour)){
				isInSleepZone = true;
			}

			// Cases when the notification is the same hour as the sleep zone start or end
			if (!isInSleepZone) {
				int diffStart = intMinutes - AppControl.control.sleepZoneStart.Minute;
				int diffEnd = intMinutes - AppControl.control.sleepZoneEnd.Minute;

				// Same hour as sleep zone start
				if(intHour == sleepStart){
					// If the notification time is after or on the sleep zone start time
					if (diffStart >= 0) {
						isInSleepZone = true;
					}
				} 
				// Same hour as sleep zone end
				else if(intHour == sleepEnd){
					// If the notification time is before or on the sleep zone end time
					if (diffEnd <= 0) {
						isInSleepZone = true;
					}
				}
			}
		}

		int day = max.Day;
		int month = max.Month;
		int year = max.Year;

		if (max.Month == 1 || max.Month == 3 || max.Month == 5 || max.Month == 7 || max.Month == 8 || max.Month == 10 || max.Month == 12) {
			if (max.Day + 1 > 31) {
				day = 0;
				month += 1;
			}
		} else if (max.Month == 4 || max.Month == 6 || max.Month == 9 || max.Month == 11) {
			if (max.Day + 1 > 30) {
				day = 0;
				month += 1;
			}
		} else {
			if (max.Day + 1 > 28) {
				day = 0;
				month += 1;
			}
		}
		if (month > 12) {
			month = 1;
			year += 1;
		}


		DateTime randomTime = new DateTime (year, month, day + 1, intHour, intMinutes, 0);

		// Store notification id
		AppControl.control.randomNotificationId[randomNotificationNumber] = SA.Common.Util.IdFactory.NextId;
		AppControl.control.Save ();

		//First notification if available on day of fire
		TimeSpan diff = randomTime - DateTime.Now;

		int startNotification = 0;

		if (diff.TotalMinutes < 0) {
			// First notification is the next day
			startNotification = (int)(24 * 60 * 60 + diff.TotalSeconds); 
		} else {
			// First notification is on the same day
			startNotification = (int)(diff.TotalSeconds);
		}

		// Create notification
		AndroidNotificationBuilder builder = new AndroidNotificationBuilder(AppControl.control.randomNotificationId[randomNotificationNumber], "Tilfældig Test", "Det er tid til at teste dig selv. Testen er aktiv i 1 time fra nu.", startNotification);

		// Launch notifications
		AndroidNotificationManager.Instance.ScheduleLocalNotification (builder);

		return randomTime;

	}


}