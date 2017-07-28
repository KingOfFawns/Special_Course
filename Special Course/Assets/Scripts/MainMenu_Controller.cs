using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class MainMenu_Controller : MonoBehaviour {

	public GameObject startTest;
	public GameObject skip;
	public GameObject exit;
	public TextAsset textFile;

	private bool isInSet = false;
	private bool isInRandom = false;
	private DateTime random;
	private DateTime notiTime;
	private DateTime[] randoms;

	// Use this for initialization
	void Awake () {

		// Load local stored data
		AppControl.control.Load ();
		AppControl.control.Save (); // This creates file if not existing

		// Check if storage file exists, create if not
		if (!File.Exists (Application.persistentDataPath + "/.dat1.dat")) {
			string header = "PatientNumber;Name;Time;Target words;Identified words;Falsely identified words;Time used in seconds;False negatives;" +
				"True positives;False positives;Length of sequences;Number of sequences;Words displayed;Grids showed;" +
				"Correct matches; N; Images shown";
			File.AppendAllText (Application.persistentDataPath + "/.dat1.dat", header + Environment.NewLine);
			File.SetAttributes (Application.persistentDataPath + "/.dat1.dat", FileAttributes.Hidden);
		}

		// Load textFile with words for word_recog tests
		string text = textFile.text;
		// Split all words in an array
		string[] words = text.Split ("\n" [0]);
		// Store the array
		AppControl.control.words = words;
		AppControl.control.Save ();
	}

	void Start(){
		// Load notification time
		notiTime = AppControl.control.notificationTime;
		notiTime = new DateTime (DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, notiTime.Hour, notiTime.Minute, notiTime.Second);

		// If first time start up
		if (AppControl.control.first_Time_Start) {

			ResetAppData ();

			// Set DateTime array for random notifications
			AppControl.control.randomNotification = new DateTime[10];
			// Set timer to 0
			AppControl.control.timer = new TimeSpan (0, 0, 0);
			AppControl.control.Save ();

			// Send to adjust patient number
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
					Debug.Log("Random " + i + ": " + max);
				}
			}
			AppControl.control.Save ();
		}

		// Load the random notifications
		randoms = AppControl.control.randomNotification;
	}

	static void ResetAppData ()
	{
		// Reset app data
		AppControl.control.word_Recog_Target = 8;
		AppControl.control.word_Last_Test = -1;
		AppControl.control.word_previous_Test = -1;
		AppControl.control.N = 2;
		AppControl.control.N_percentage_last = 0f;
		AppControl.control.digitSpan_DigitLength = 3;
		AppControl.control.first_Time_Start = true;
		AppControl.control.wordRecogStart_WordTimer = 1.0f;
		AppControl.control.digitSpan_SequenceTimer = 1.0f;
		AppControl.control.NBack_Timer = 1.0f;
		AppControl.control.testStartDate = new System.DateTime (System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, System.DateTime.Now.Hour - 1, System.DateTime.Now.Minute, System.DateTime.Now.Second);
		AppControl.control.password = "52 6F 6F 74 41 64 6D 69 6E";
		AppControl.control.maxSequenceLength = 0;
		AppControl.control.firstTestCleared = false;
		AppControl.control.tenTestsCleared = false;
		AppControl.control.fiftyTestsCleared = false;
		AppControl.control.hundredTestsCleared = false;
		AppControl.control.hundredfiftyTestsCleared = false;
		AppControl.control.achieveCounter = 0;
		AppControl.control.patientNumber = 0;
	}

	void Update(){
		// Get current time
		DateTime now = DateTime.Now;

		// Get time from notification
		TimeSpan diff = now.Subtract(notiTime);

		// Calculate seconds from set notification
		double secondsFromNotification = diff.TotalSeconds;
		isInSet = false;
		if (secondsFromNotification >= 0 && secondsFromNotification < 3600) {
			isInSet = true;
		}

		// Check if any random notification is active
		isInRandom = false;
		foreach (DateTime r in randoms) {
			diff = now.Subtract(r);
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

		// Check if the next test can appear and ready up for the next test
		if (now.Subtract (testStart).TotalSeconds >= timeLeft.TotalSeconds) {
			AppControl.control.testStarted = false;
			isStart = false;
		}

		// Activate or deactivate 'Start test' button and 'Skip' Button
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
		// Get now
		DateTime now = DateTime.Now;

		// Create notification time today
		notiTime = new DateTime (DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, notiTime.Hour, notiTime.Minute, notiTime.Second);

		// Check which kind of notification and set timer accordingly
		if (isInSet) {
			AppControl.control.timer = notiTime.AddHours (1).Subtract (now);
		} else if (isInRandom) {
			AppControl.control.timer = random.AddHours (1).Subtract (now);
		}

		// Save test start time
		AppControl.control.testStartDate = now;
		AppControl.control.Save ();

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
		// Get notification time
		DateTime now = DateTime.Now;
		notiTime = new DateTime (DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, notiTime.Hour, notiTime.Minute, notiTime.Second);

		// Calculate timer for notifications
		if (isInSet) {
			AppControl.control.timer = notiTime.AddHours (1).Subtract (now);
		} else if (isInRandom) {
			AppControl.control.timer = random.AddHours (1).Subtract (now);
		}

		// Store start time
		AppControl.control.testStartDate = now;

		// End test
		AppControl.control.testStarted = true;
		AppControl.control.Save ();

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
		DateTime notification = AppControl.control.notificationTime;
		int notiHour = notification.Hour;
		int notiMinutes = notification.Minute;

		// Generate Random time
		int intHour = UnityEngine.Random.Range (0, 24);
		int intMinutes = UnityEngine.Random.Range (0, 60);
		DateTime r = new DateTime (notification.Year, notification.Month, notification.Day, intHour, intMinutes, notification.Second);

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

		// Boolean to keep track of when the notification is in the sleep zone or too close to the set notification
		bool isTooClose = false;

		// The notification is in between the sleep zone hours
		if (!goodHours.Contains(intHour)){
			isTooClose = true;
		}
		// The notification is too close to the set notification
		if ((r - notification).TotalSeconds < 3599 && (r - notification).TotalSeconds > -3599) {
			isTooClose = true;
		}

		// Cases when the notification is the same hour as the sleep zone start or end
		if (!isTooClose) {
			int diffStart = intMinutes - AppControl.control.sleepZoneStart.Minute;
			int diffEnd = intMinutes - AppControl.control.sleepZoneEnd.Minute;

			// Same hour as sleep zone start
			if(intHour == sleepStart){
				// If the notification time is after or on the sleep zone start time
				if (diffStart >= 0) {
					isTooClose = true;
				}
			} 
			// Same hour as sleep zone end
			else if(intHour == sleepEnd){
				// If the notification time is before or on the sleep zone end time
				if (diffEnd <= 0) {
					isTooClose = true;
				}
			}
		}

		// As long as the notification is in the sleep zone or too close to the set notification
		while (isTooClose) {
			isTooClose = false;

			// Calculate new notification time
			intHour = UnityEngine.Random.Range (0, 24);
			intMinutes = UnityEngine.Random.Range (0, 60);
			r = new DateTime (notification.Year, notification.Month, notification.Day, intHour, intMinutes, notification.Second);

			// The notification is in between the sleep zone hours
			if (!goodHours.Contains(intHour)){
				isTooClose = true;
			}
			// The notification is too close to the set notification 
			if ((r - notification).TotalSeconds < 3599 && (r - notification).TotalSeconds > -3599) {
				isTooClose = true;
			}

			// Cases when the notification is the same hour as the sleep zone start or end
			if (!isTooClose) {
				int diffStart = intMinutes - AppControl.control.sleepZoneStart.Minute;
				int diffEnd = intMinutes - AppControl.control.sleepZoneEnd.Minute;

				// Same hour as sleep zone start
				if(intHour == sleepStart){
					// If the notification time is after or on the sleep zone start time
					if (diffStart >= 0) {
						isTooClose = true;
					}
				} 
				// Same hour as sleep zone end
				else if(intHour == sleepEnd){
					// If the notification time is before or on the sleep zone end time
					if (diffEnd <= 0) {
						isTooClose = true;
					}
				}
			}
		}

		// Get the date of the latest set random notification
		int day = max.Day;
		int month = max.Month;
		int year = max.Year;

		// Calculate change of date
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

		// Set dateTime for random notification
		DateTime randomTime = new DateTime (year, month, day + 1, intHour, intMinutes, 0);

		// Store notification id
		AppControl.control.randomNotificationId[randomNotificationNumber] = SA.Common.Util.IdFactory.NextId;
		AppControl.control.Save ();

		//First notification if available on day of fire
		TimeSpan diff = randomTime - DateTime.Now;

		int startNotification = (int)(diff.TotalSeconds);

		// Create notification
		AndroidNotificationBuilder builder = new AndroidNotificationBuilder(AppControl.control.randomNotificationId[randomNotificationNumber], "Tilfældig Test", "Det er tid til at teste dig selv. Testen er aktiv i 1 time fra nu.", startNotification);

		// Launch notification
		AndroidNotificationManager.Instance.ScheduleLocalNotification (builder);

		return randomTime;
	}
}