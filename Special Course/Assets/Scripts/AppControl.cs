using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class AppControl : MonoBehaviour {

	public static AppControl control;

	// Storage for word recog start and end
	public int word_Recog_Target = 10; // Should be stored
	public int word_Last_Test = -1; // Should be stored
	public int word_previous_Test = -1; // Should be stored
	public string[] words;
	public string[] chosenWords;
	public int identifiedWords = 0;
	public int falseWords = 0;
	public bool success = false;

	// Storage for N-Back
	public int N = 2; // Should be stored
	public float N_percentage_last = 0f; // Should be stored

	// Storage for Digit Span
	public int digitSpan_DigitLength = 3; // Should be stored

	// Boolean for a first time start up settings screen
	public bool first_Time_Start = true; // Should be stored

	// String used to store logged data
	public string dataString = "";
	public string csvString = "";

	// Variables to control time settings
	public double wordRecogStart_WordTimer = 1.0f; // Should be stored
	public double digitSpan_SequenceTimer = 1.0f; // Should be stored
	public double NBack_Timer = 1.0f; // Should be stored

	// Variables for notification settings
	public int notificationId = 0; // Should be stored
	public DateTime notificationTime; // Should be stored
	public DateTime sleepZoneStart; // Should be stored
	public DateTime sleepZoneEnd; // Should be stored

	// Variables for random notification
	public int[] randomNotificationId = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}; // Should be stored
	public DateTime[] randomNotification;

	// Variable to control time on test and 1 time per test
	public DateTime testStartDate;
	public bool testStarted = false;
	public TimeSpan timer;

	// Settings Login Password
	public string password = "52 6F 6F 74 41 64 6D 69 6E"; // Should be stored

	// Highscore data
	public int maxSequenceLength = 0; // Should be stored

	// Achievements data
	public int achieveCounter = 0; // Should be stored
	public bool firstTestCleared = false; // Should be stored
	public bool tenTestsCleared = false; // Should be stored
	public bool fiftyTestsCleared = false; // Should be stored
	public bool hundredTestsCleared = false; // Should be stored
	public bool hundredfiftyTestsCleared = false; // Should be stored

	// Patient data
	public int patientNumber = 0;


	void Awake () {
		// Make sure that AppControl is a singleton throughout the scenes
		if (control == null) {
			DontDestroyOnLoad (gameObject);
			control = this;
		} else if (control != this) {
			Destroy (gameObject);
		} 
	}

	public void Save(){
		if (File.Exists (Application.persistentDataPath + "/.appData.dat")) {
			File.SetAttributes (Application.persistentDataPath + "/.appData.dat", FileAttributes.Normal);
		}
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/.appData.dat");

		// Data to store
		AppData data = new AppData();

		// Word Recog data
		data.word_Recog_Target = word_Recog_Target;
		data.word_Last_Test = word_Last_Test;
		data.word_previous_Test = word_previous_Test;

		// N-back data
		data.N = N;
		data.N_percentage_last = N_percentage_last;

		// Digit Span data
		data.digitSpan_DigitLength = digitSpan_DigitLength;

		// First time boolean
		data.first_Time_Start = first_Time_Start;

		// Timer variables for settings - data
		data.wordRecogStart_WordTimer = wordRecogStart_WordTimer;
		data.digitSpan_SequenceTimer = digitSpan_SequenceTimer;
		data.NBack_Timer = NBack_Timer;

		// Notification and sleepzone data
		data.notificationId = notificationId;
		data.notificationTime = notificationTime;
		data.sleepZoneStart = sleepZoneStart;
		data.sleepZoneEnd = sleepZoneEnd;

		// Random notification data
		data.randomNotificationId = randomNotificationId;
		data.randomNotification = randomNotification;

		// Test control data
		data.testStartDate = testStartDate;
		data.testStarted = testStarted;
		data.timer = timer;

		// Store settings login password
		data.password = password;

		// Store highscore data
		data.maxSequenceLength = maxSequenceLength;

		// Store achievement data
		data.achieveCounter = achieveCounter;
		data.firstTestCleared = firstTestCleared;
		data.tenTestsCleared = tenTestsCleared;
		data.fiftyTestsCleared = fiftyTestsCleared;
		data.hundredTestsCleared = hundredTestsCleared;
		data.hundredfiftyTestsCleared = hundredfiftyTestsCleared;

		// Store patient number
		data.patientNumber = patientNumber;

		// Store data
		bf.Serialize (file, data);
		file.Close();

		File.SetAttributes (Application.persistentDataPath + "/.appData.dat", FileAttributes.Hidden);
	}

	public void Load(){
		if (File.Exists (Application.persistentDataPath + "/.appData.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/.appData.dat", FileMode.Open);
			AppData data = (AppData)bf.Deserialize (file);
			file.Close ();

			// Load data
			word_Recog_Target = data.word_Recog_Target;
			word_Last_Test = data.word_Last_Test;
			word_previous_Test = data.word_previous_Test;

			N = data.N;
			N_percentage_last = data.N_percentage_last;

			digitSpan_DigitLength = data.digitSpan_DigitLength;

			first_Time_Start = data.first_Time_Start;

			wordRecogStart_WordTimer = data.wordRecogStart_WordTimer;
			digitSpan_SequenceTimer = data.digitSpan_SequenceTimer;
			NBack_Timer = data.NBack_Timer;

			notificationId = data.notificationId;
			notificationTime = data.notificationTime;
			sleepZoneStart = data.sleepZoneStart;
			sleepZoneEnd = data.sleepZoneEnd;

			randomNotificationId = data.randomNotificationId;
			randomNotification = data.randomNotification;

			testStartDate = data.testStartDate;
			testStarted = data.testStarted;
			timer = data.timer;

			password = data.password;

			maxSequenceLength = data.maxSequenceLength;

			achieveCounter = data.achieveCounter;
			firstTestCleared = data.firstTestCleared;
			tenTestsCleared = data.tenTestsCleared;
			fiftyTestsCleared = data.fiftyTestsCleared;
			hundredTestsCleared = data.hundredTestsCleared;
			hundredfiftyTestsCleared = data.hundredfiftyTestsCleared;

			patientNumber = data.patientNumber;
		}
	}

	public void SaveData(){
		File.AppendAllText (Application.persistentDataPath + "/.dat2.dat", dataString + Environment.NewLine);
		File.AppendAllText (Application.persistentDataPath + "/.dat1.dat", csvString + Environment.NewLine);

		File.SetAttributes (Application.persistentDataPath + "/.dat2.dat", FileAttributes.Hidden);
		File.SetAttributes (Application.persistentDataPath + "/.dat1.dat", FileAttributes.Hidden);
	}
}

[Serializable]
class AppData {
	public int word_Recog_Target = 10;
	public int word_Last_Test = -1;
	public int word_previous_Test = -1;

	public int N = 2;
	public float N_percentage_last = 0f;

	public int digitSpan_DigitLength = 3;

	public bool first_Time_Start = true;

	public double wordRecogStart_WordTimer = 1.0f;
	public double digitSpan_SequenceTimer = 1.0f;
	public double NBack_Timer = 1.0f;

	public int notificationId = 0;
	public DateTime notificationTime;
	public DateTime sleepZoneStart;
	public DateTime sleepZoneEnd;

	public int[] randomNotificationId = {0 ,0 ,0 ,0 ,0 , 0, 0, 0, 0, 0}; // Should be stored
	public DateTime[] randomNotification;

	public DateTime testStartDate;
	public bool testStarted = false;
	public TimeSpan timer;

	public string password = "52 6F 6F 74 41 64 6D 69 6E";

	public int maxSequenceLength = 0;

	public bool firstTestCleared = false;
	public bool tenTestsCleared = false; // Should be stored
	public bool fiftyTestsCleared = false; // Should be stored
	public bool hundredTestsCleared = false; // Should be stored
	public bool hundredfiftyTestsCleared = false; // Should be stored
	public int achieveCounter = 0;

	public int patientNumber = 0;
}