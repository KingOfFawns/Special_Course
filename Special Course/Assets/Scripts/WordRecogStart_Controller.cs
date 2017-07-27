using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WordRecogStart_Controller : MonoBehaviour {

	public GameObject startCanvas;
	public GameObject canvas;
	public GameObject endCanvas;
	public Text wordsText;
	public Text word;
	public Text wordsLeft;

	private int numOFWords = 10;
	private string[] chosenWords;

	// Use this for initialization
	void Start () {
		// Load local data for number of words
		numOFWords = AppControl.control.word_Recog_Target;

		wordsText.text = "Du skal huske " + numOFWords +" ord.";

		// Setup chosen words
		chosenWords = new string[numOFWords];

		// Get amount of words
		int wordLength = AppControl.control.words.Length;

		// Array to save random numbers for choosing words
		int[] randoms = new int[numOFWords];

		// Choose needed amount of random words
		for (int i = 0; i < numOFWords; i++) {
			int ran = Random.Range (0, wordLength);

			// Check if word is already chosen, and if it is choose a new one
			bool check = false;
			while (!check) {
				check = true;
				foreach (int r in randoms) {
					if (ran == r) {
						check = false;
						ran = Random.Range (0, wordLength);
						break;
					}
				}
			}
			randoms [i] = ran;

			// Store chosen word
			chosenWords [i] = AppControl.control.words [ran];
		}
	}

	void Update(){
		// Get now and test start time
		System.DateTime now = System.DateTime.Now;
		System.DateTime testStart = AppControl.control.testStartDate;

		// If the test started more than 10 minutes ago and the test is not in help's test mode
		if (now.Subtract (testStart).TotalSeconds >= 600 && !AppControl.control.testOfTest) {
			// Set the boolean controlling whether or not the test has been finished
			AppControl.control.testStarted = true;
			AppControl.control.Save ();

			// Log stop
			string patientNumber = "#" + AppControl.control.patientNumber.ToString().Substring(1);
			string time = System.DateTime.Now.ToString ();

			AppControl.control.dataString = "Patient Number: " + patientNumber + ", Test udløbet" + ", Time: " + time;
			AppControl.control.csvString = patientNumber + ";Test Stopped;" + time + ";;;;;;;;;;;;;;;;";
			AppControl.control.SaveData ();

			// Go to main menu
			SceneManager.LoadScene ("MainMenu");
		}
	}

	public void StartButton(){
		startCanvas.SetActive(false);

		canvas.SetActive (true);

		StartCoroutine (ShowWords ());
	}

	IEnumerator ShowWords(){

		// Set start of info text
		wordsLeft.text = "Ord 1 af " + numOFWords.ToString();

		for (int i = 0; i < numOFWords; i++) {
			yield return new WaitForSeconds (0.3f);

			// Show info text
			wordsLeft.text = "Ord " + (i+1).ToString() + " af " + numOFWords.ToString();
			// Show word
			word.text = chosenWords[i];

			// Wait designated time
			yield return new WaitForSeconds ((float)AppControl.control.wordRecogStart_WordTimer);

			// Show blank
			word.text = "";
		}
			
		// Test end
		endCanvas.SetActive (true);

		yield return new WaitForSeconds (3);

		//Store chosen words
		AppControl.control.chosenWords = chosenWords;

		int ran = Random.Range (0, 2);

		if (AppControl.control.testOfTest) {
			SceneManager.LoadScene ("Word_Recog_End");
		}
		else if (ran == 0) {
			SceneManager.LoadScene ("N_Back");	
		} else {
			SceneManager.LoadScene ("Digit_Span");
		}

	}

}
