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
	public GameObject back;
	public GameObject next;
	public Text notifyText;

	private bool notiSet = false;
	private bool sleepSet = false;
	private bool upDown = false;


	void Start(){
		if (AppControl.control.first_Time_Start) {
			back.SetActive (false);
			next.SetActive (true);
		}

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

	public void NextButton(){
		if (!notiSet) {
			notifyText.text = "Notifikation mangler";
			StartCoroutine (ResetNotifyText ());
		} else if (!sleepSet) {
			notifyText.text = "Sove zone mangler";
			StartCoroutine (ResetNotifyText ());
		}
		else {
			AppControl.control.first_Time_Start = false;
			AppControl.control.Save ();

			SceneManager.LoadScene ("MainMenu");
		}
	}

	IEnumerator ResetNotifyText(){
		yield return new WaitForSeconds (1);
		notifyText.text = "";
	}

	public void SetNotification(){

		if (!sleepSet && AppControl.control.first_Time_Start) {
			notifyText.text = "Indtast sove zone først";
			StartCoroutine (ResetNotifyText ());
			return;
		}

		string hour = notificationHour.text;
		string minutes = notificationMinutes.text;

		int intHour = 0;
		int intMinutes = 0;

		int.TryParse (hour, out intHour);
		int.TryParse(minutes, out intMinutes);

		Debug.Log ("Noti Time: " + intHour + "," + intMinutes); 

		// Start and end hour of the sleep zone
		int sleepStart = AppControl.control.sleepZoneStart.Hour;
		int sleepEnd = AppControl.control.sleepZoneEnd.Hour;

		Debug.Log ("Start: " + sleepStart);
		Debug.Log ("End :" + sleepEnd);

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
			Debug.Log ("Hour = " + currentHour);
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
			Debug.Log ("In sleep zone");
		}

		// Cases when the notification is the same hour as the sleep zone start or end
		if (!isInSleepZone) {
			int diffStart = intMinutes - AppControl.control.sleepZoneStart.Minute;
			int diffEnd = intMinutes - AppControl.control.sleepZoneEnd.Minute;

			// Same hour as sleep zone start
			if(intHour == sleepStart){
				Debug.Log ("In same hour as start");
				// If the notification time is after or on the sleep zone start time
				if (diffStart >= 0) {
					isInSleepZone = true;
				}
			} 
			// Same hour as sleep zone end
			else if(intHour == sleepEnd){
				Debug.Log ("In same Hour as end");
				// If the notification time is before or on the sleep zone end time
				if (diffEnd <= 0) {
					isInSleepZone = true;
				}
			}
		}

		if (isInSleepZone) {
			isInSleepZone = false;

			inSleepZoneText.text = "I sove zonen";

			StartCoroutine (TimeOut ());
		} else {
			// Stop existing notification
			AndroidNotificationManager.Instance.CancelLocalNotification(AppControl.control.notificationId);
			AndroidNotificationManager.Instance.CancelAllLocalNotifications();

			// Create DateTime format
			string year = DateTime.Now.Year.ToString ();
			string month = DateTime.Now.Month.ToString();
			string day = DateTime.Now.Day.ToString ();
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
				// First notification is the next day
				startFirstNotification = (int)(24 * 60 * 60 + diff.TotalMinutes * 60); 
			} else {
				// First notification is on the same day
				startFirstNotification = (int)(diff.TotalMinutes * 60);
			}

			// Create first
			AndroidNotificationBuilder builder = new AndroidNotificationBuilder(AppControl.control.notificationId, "Planlagt Test", "Det er tid til at teste dig selv. Testen er aktiv i 1 time fra nu.", startFirstNotification);

			// Schedule daily repeating notification
			builder.SetRepeating(true);
			TimeSpan delay = DateTime.Now.AddDays(1.0f) - DateTime.Now;
			builder.SetRepeatDelay ((int)delay.TotalSeconds);

			// Launch notifications
			AndroidNotificationManager.Instance.ScheduleLocalNotification (builder);

			// Notify
			inSleepZoneText.text = "Gemt";
			notiSet = true;

			AppControl.control.randomNotification = new DateTime[10];
			AppControl.control.Save ();

			StartCoroutine (TimeOut ());
		}


	}

	IEnumerator TimeOut(){
		// Resets displayed text
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

		Debug.Log ("Start: " + startDate);
		Debug.Log ("End: " + endDate);

		DateTime sDate = DateTime.Parse (startDate);
		DateTime eDate = DateTime.Parse (endDate);

		// Store sleep zone
		AppControl.control.sleepZoneStart = sDate;
		AppControl.control.sleepZoneEnd = eDate;
		AppControl.control.Save ();

		// Notify
		sleepZoneSave.text = "Gemt";
		sleepSet = true;

		StartCoroutine (TimeOut2 ());
	}

	IEnumerator TimeOut2(){
		yield return new WaitForSeconds (1f);
		sleepZoneSave.text = "";
	}
		
	public void UpDown(bool up){
		upDown = up;
	}

	public void AdjustHour(int id){
		string hour = "";
		if (id == 1) {
			hour = notificationHour.text;
		} else if (id == 2) {
			hour = sleepZoneHour1.text;
		} else {
			hour = sleepZoneHour2.text;
		}

		int intHour;
		int.TryParse (hour, out intHour);

		if (upDown) {
			intHour++;
			if (intHour > 23) {
				intHour = 0;
			}
		} else {
			intHour--;
			if (intHour < 0) {
				intHour = 23;
			}
		}

		if (id == 1) {
			notificationHour.text = intHour.ToString ();
		} else if (id == 2) {
			sleepZoneHour1.text = intHour.ToString ();
		} else {
			sleepZoneHour2.text = intHour.ToString ();
		}
		CleanUpHour (id);
	}

	public void AdjustMinute(int id){
		string minutes = "";
		if (id == 1) {
			minutes = notificationMinutes.text;
		} else if (id == 2) {
			minutes = sleepZoneMinutes1.text;
		} else {
			minutes = sleepZoneMinutes2.text;
		}

		int intMinutes;
		int.TryParse (minutes, out intMinutes);

		if (upDown) {
			intMinutes++;
			if (intMinutes > 59) {
				intMinutes = 0;
			}
		} else {
			intMinutes--;
			if (intMinutes < 0) {
				intMinutes = 59;
			}
		}

		if (id == 1) {
			notificationMinutes.text = intMinutes.ToString ();
		} else if (id == 2) {
			sleepZoneMinutes1.text = intMinutes.ToString ();
		} else {
			sleepZoneMinutes2.text = intMinutes.ToString ();
		}
		CleanUpMinute (id);
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

	public void CleanUpMinute(int id){

		string toSet = "";
		if (id == 1) {
			toSet = notificationMinutes.text;
		} else if (id == 2) {
			toSet = sleepZoneMinutes1.text;
		} else if (id == 3) {
			toSet = sleepZoneMinutes2.text;
		} 

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
