using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Notifications_Controller : MonoBehaviour {

	public InputField notificationHour;
	public InputField notificationMinutes;
	public InputField sleepZoneHour1;
	public InputField sleepZoneHour2;
	public InputField sleepZoneMinutes1;
	public InputField sleepZoneMinutes2;
	public Text inSleepZoneText;
	public Text sleepZoneSave;


	public void Awake(){
		// Set notification time from memory
		DateTime notificationTime = AppControl.control.notificationTime;

		// Get info
		string AM_PM = notificationTime.ToString("tt");
		int hour = notificationTime.Hour;
		int minutes = notificationTime.Minute;

		// Adjust for AM/PM in hours to show 24-hour clock
		if(AM_PM == "PM" && hour < 12){
			hour = hour + 12;
		} 

		// Adjust seconds to show a 0 with a lonely hour.
		if (hour < 10) {
			notificationHour.text = "0" + hour.ToString ();
		} else {
			notificationHour.text = hour.ToString ();
		}

		// Adjust seconds to show a 0 with a lonely minutes.
		if (minutes < 10) {
			notificationMinutes.text = "0" + minutes.ToString ();
		} else {
			notificationMinutes.text = minutes.ToString ();
		}

		// Set Sleep Zone from memory
		DateTime sleepZoneStart = AppControl.control.sleepZoneStart;
		DateTime sleepZoneEnd = AppControl.control.sleepZoneEnd;
		Debug.Log (sleepZoneStart);

		//Get info for start
		AM_PM = sleepZoneStart.ToString("tt");
		hour = sleepZoneStart.Hour;
		minutes = sleepZoneStart.Minute;

		// Adjust for AM/PM in hours to show 24-hour clock
		if(AM_PM == "PM" && hour < 12){
			hour = hour + 12;
		} 

		if (hour < 10) {
			sleepZoneHour1.text = "0" + hour.ToString ();
		} else {
			sleepZoneHour1.text = hour.ToString ();
		}

		// Adjust seconds to show a 0 with a lonely second.
		if (minutes < 10) {
			sleepZoneMinutes1.text = "0" + minutes.ToString ();
		} else {
			sleepZoneMinutes1.text = minutes.ToString ();
		}
	
		//Get info for end
		AM_PM = sleepZoneEnd.ToString("tt");
		hour = sleepZoneEnd.Hour;
		minutes = sleepZoneEnd.Minute;

		// Adjust for AM/PM in hours to show 24-hour clock
		if(AM_PM == "PM" && hour < 12){
			hour = hour + 12;
		} 

		if (hour < 10) {
			sleepZoneHour2.text = "0" + hour.ToString ();
		} else {
			sleepZoneHour2.text = hour.ToString ();
		}

		// Adjust seconds to show a 0 with a lonely second.
		if (minutes < 10) {
			sleepZoneMinutes2.text = "0" + minutes.ToString ();
		} else {
			sleepZoneMinutes2.text = minutes.ToString ();
		}

	}

	public void ReturnToStart(){
		SceneManager.LoadScene ("Settings");
	}

	public void SetNotification(){


		string hour = notificationHour.text;
		string minutes = notificationMinutes.text;
		string year = DateTime.Now.Year.ToString ();
		string month = DateTime.Now.Month.ToString();
		string day = DateTime.Now.Day.ToString ();

		int intHour = 0;
		int intMinutes = 0;

		int.TryParse (hour, out intHour);
		int.TryParse(minutes, out intMinutes);


		// Check if selected time is in the sleep zone
		int start = AppControl.control.sleepZoneStart.Hour;
		int end = AppControl.control.sleepZoneEnd.Hour;
		int currentHour = start;
		bool hourIsInSleep = false;
		bool isInSleepZone = false;

		for(int i = 0; i < 24; i++) {
			if (intHour == currentHour) {
				hourIsInSleep = true;
				Debug.Log ("Is same hour");
				break;
			} else if(currentHour == end){
				hourIsInSleep = false;
				break;
			}

			currentHour++;
			if (currentHour > 23) {
				currentHour = 0;
			}
		}

		if(hourIsInSleep && (intHour == start || intHour == end)){
			hourIsInSleep = false;
			if (intHour == start && intMinutes > AppControl.control.sleepZoneStart.Minute) {
				isInSleepZone = true;
			} else if(intMinutes < AppControl.control.sleepZoneEnd.Minute) {
				isInSleepZone = true;
			}
		} else if(hourIsInSleep){
			hourIsInSleep = false;
			isInSleepZone = true;
		}

		if (isInSleepZone) {
			isInSleepZone = false;
			Debug.Log ("Is in sleep zone");

			inSleepZoneText.text = "I sove zonen";

			StartCoroutine (TimeOut ());
		} else {
			Debug.Log ("Is good");

			// Stop existing notification
			AndroidNotificationManager.Instance.CancelLocalNotification(AppControl.control.notificationId);

			// Create DateTime format
			string date = year + "-" + month + "-" + day + " " + hour + ":" + minutes;
			DateTime notificationTime = DateTime.Parse (date);

			// Store notification time and ID
			AppControl.control.notificationTime = notificationTime;
			AppControl.control.notificationId = SA.Common.Util.IdFactory.NextId;
			AppControl.control.Save ();

			//First notification if available on day of fire
			DateTime now = DateTime.Now;

			TimeSpan diff = notificationTime - now;

			int startFirstNotification = 0;

			if (diff.TotalMinutes < 0) {
				Debug.Log ("Negative");
				startFirstNotification = 10;
			} else {
				Debug.Log ("Positive");
				startFirstNotification = ((diff.Hours * 360) + (diff.Minutes) * 60);
			}

			Debug.Log (startFirstNotification);

			// Start notification
			AndroidNotificationBuilder builder = new AndroidNotificationBuilder(AppControl.control.notificationId, "Scheduled test", "Time for a test", startFirstNotification);

			// Schedule daily repeating notification
			builder.SetRepeating(true);
			TimeSpan delay = notificationTime.AddDays(1.0f) - notificationTime;
			builder.SetRepeatDelay ((int)delay.TotalSeconds);

			AndroidNotificationManager.Instance.ScheduleLocalNotification (builder);

			// Notify
			inSleepZoneText.text = "Gemt";

			StartCoroutine (TimeOut ());
		}

	}

	IEnumerator TimeOut(){
		yield return new WaitForSeconds (1f);
		inSleepZoneText.text = "";
	}

	public void SetSleepZone(){

		string hour = sleepZoneHour1.text;
		string minutes = sleepZoneMinutes1.text;

		string startDate = "0001-01-01 " + hour + ":" + minutes;


		string hour2 = sleepZoneHour2.text;
		string minutes2 = sleepZoneMinutes2.text;

		string endDate = "0001-01-01 " + hour2 + ":" + minutes2;

		Debug.Log (startDate);
		Debug.Log (endDate);

		DateTime sDate = DateTime.Parse (startDate);
		DateTime eDate = DateTime.Parse (endDate);

		// Store sleep zone
		AppControl.control.sleepZoneStart = sDate;
		AppControl.control.sleepZoneEnd = eDate;
		AppControl.control.Save ();

		// Notify
		sleepZoneSave.text = "Gemt";

		StartCoroutine (TimeOut2 ());
	}

	IEnumerator TimeOut2(){
		yield return new WaitForSeconds (1f);
		sleepZoneSave.text = "";
	}

	public void CleanUpHour(int id){

		string toSet = "";
		if (id == 1) {
			toSet = notificationHour.text;
		} else if (id == 2) {
			toSet = sleepZoneHour1.text;
		} else if (id == 3) {
			toSet = sleepZoneHour2.text;
		} 

		int time = -1;
		int.TryParse (toSet, out time);

		if (time > 23) {
			time = 23;
		} else if (time < 0) {
			time = 0;
		}

		toSet = time.ToString ();

		if (toSet.Length < 2) {
			toSet = "0" + toSet;
		} 

		if (id == 1) {
			notificationHour.text = toSet;
		} else if (id == 2) {
			sleepZoneHour1.text = toSet;
		} else if (id == 3) {
			sleepZoneHour2.text = toSet;
		} 
	}

	public void CleanUpSeconds(int id){

		string toSet = "";
		if (id == 1) {
			toSet = notificationMinutes.text;
		} else if (id == 2) {
			toSet = sleepZoneMinutes1.text;
		} else if (id == 3) {
			toSet = sleepZoneMinutes2.text;
		} 

		int time = -1;
		int.TryParse (toSet, out time);

		if (time > 59) {
			time = 59;
		} else if (time < 0) {
			time = 0;
		}

		toSet = time.ToString ();

		if (toSet.Length < 2) {
			toSet = "0" + toSet;
		} 

		if (id == 1) {
			notificationMinutes.text = toSet;
		} else if (id == 2) {
			sleepZoneMinutes1.text = toSet;
		} else if (id == 3) {
			sleepZoneMinutes2.text = toSet;
		} 
	}
}
