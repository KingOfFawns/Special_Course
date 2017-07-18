using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WordRecogEnd_Controller : MonoBehaviour {

	public GameObject startCanvas;
	public GameObject canvas;
	public GameObject choiceCanvas;
	public GameObject endCanvas;
	public Text startText;
	public Button button;
	public Text wordText;
	public GameObject content;
	public Text targetWords;
	public Text foundWords;
	public Text textWord;
	public Scrollbar timeBar;
	public Text wordsLeft;

	private int wordLength;
	private int numOFWords = 10;
	private string[] chosenWords;
	private string[] distractorWords;
	private int timer = 0;

	void Awake () {
		// Reset values
		AppControl.control.success = false;
		AppControl.control.identifiedWords = 0;
		AppControl.control.falseWords = 0;

		// Load needed data from storage
		wordLength = AppControl.control.words.Length;
		numOFWords = AppControl.control.word_Recog_Target;
		chosenWords = AppControl.control.chosenWords;

		// Array for distractor words
		distractorWords = new string[numOFWords];

		// Choose needed amount of distractor words
		int ran = 0;
		string word = "";
		for (int i = 0; i < numOFWords; i++) {
			ran = Random.Range (0, wordLength);

			word = AppControl.control.words [ran];

			// Check if word is already chosen, and if it is choose a new one
			bool check = false;
			while (!check) {
				check = true;
				foreach (string w in chosenWords) {
					if (w == word) {
						check = false;
						ran = Random.Range (0, wordLength);
						word = AppControl.control.words [ran];
						break;
					}
				}

				foreach (string d in distractorWords) {
					if (d == word) {
						check = false;
						ran = Random.Range (0, wordLength);
						word = AppControl.control.words [ran];
						break;
					}
				}
			}

			// Store distractor word
			distractorWords [i] = word;
		}

	}

	void Start(){
		// Set text
		startText.text = numOFWords.ToString();

		// Merge target and distractor words
		string[] allWords = new string[numOFWords*2];
		chosenWords.CopyTo(allWords,0);
		distractorWords.CopyTo (allWords, numOFWords);

		Shuffle (allWords);

		for(int i = 0; i < numOFWords*2; i++) {

			wordText.text = allWords[i];
			Instantiate (button, content.transform);
		}
	}

	void Update(){
		System.DateTime now = System.DateTime.Now;
		System.DateTime testStart = AppControl.control.testStartDate;

		if (now.Subtract (testStart).TotalSeconds >= 600) {
			AppControl.control.testStarted = true;

			// Log stop
			string patientNumber = "#" + AppControl.control.patientNumber.ToString().Substring(1);
			string time = System.DateTime.Now.ToString ();

			AppControl.control.dataString = "Patient Number: " + patientNumber + ", Test udløbet" + ", Time: " + time;
			AppControl.control.csvString = patientNumber + ";Test Stopped;" + time + ";;;;;;;;;;;;;;;;";
			AppControl.control.SaveData ();

			SceneManager.LoadScene ("MainMenu");
		}
	}

	void Shuffle<T>(T[] arr){
		// Fisher-Yates shuffle algorithm
		for(int i = arr.Length-1; i > 0; i--) {
			int r = Random.Range (0, i);
			T tmp = arr [i];
			arr [i] = arr [r];
			arr [r] = tmp;
		}
	}


	public void StartButton(){
		startCanvas.SetActive(false);

		canvas.SetActive (true);

		StartCoroutine (Timer ());
	}

	public void clickGo(){
		StartCoroutine (More ());
	}
	public void clickStop(){
		StartCoroutine (Stop ());
	}

	IEnumerator Timer(){
		wordsLeft.text = numOFWords + " ord";

		// Timer runs 1 second per word to guess (minimum is 30)
		int timeEnd = numOFWords * 2;

		if (timeEnd < 30) {
			timeEnd = 30;
		}

		// Run the timer + Update the timer bar alongside it
		while (timer < timeEnd) {
			if (AppControl.control.success) {
				choiceCanvas.SetActive (true);
				break;
			}
			yield return new WaitForSeconds (1);
			timer++;

			// Timer bar
			timeBar.size = timer / (float)timeEnd;
			timeBar.transform.GetChild (1).GetComponent<Text> ().text = (timeEnd - timer).ToString ();
		}

		if (!AppControl.control.success) {
			StartCoroutine (Stop ());
		}
	}

	IEnumerator More(){
		choiceCanvas.SetActive (false);
		int timer2 = 0;
		timeBar.transform.GetChild (1).GetComponent<Text> ().text = (5 - timer2).ToString ();
		while (timer2 < 5) {
			yield return new WaitForSeconds (1);
			timer2++;

			// Timer bar
			timeBar.size = timer2 / 5f;
			timeBar.transform.GetChild (1).GetComponent<Text> ().text = (5 - timer2).ToString ();
		}
		StartCoroutine (Stop ());
	}

	IEnumerator Stop(){
		choiceCanvas.SetActive (false);
		// Activate end canvas ansd set visual data
		endCanvas.SetActive (true);
		targetWords.text = numOFWords.ToString ();
		foundWords.text = AppControl.control.identifiedWords.ToString ();

		// Special data setting
		string numOfTargets = numOFWords.ToString ();

		// Calculate data
		int last = AppControl.control.word_Last_Test;
		int previous = AppControl.control.word_previous_Test;

		bool fail = false;

		if (numOFWords > 20 && AppControl.control.falseWords >= 4) {
			fail = true;
		} else if(numOFWords >= 12 && AppControl.control.falseWords >= 3){
			fail = true;
		} else if(numOFWords >= 7 && AppControl.control.falseWords >= 2){
			fail = true;
		} else if (AppControl.control.falseWords >= 1){
			fail = true;
		}

		if (fail && previous == 1 && last == 1) {
			numOFWords--;

			previous = -1;
			last = -1;
		} else if (!fail && last == 0 && AppControl.control.identifiedWords == numOFWords) {
			numOFWords++;

			previous = -1;
			last = -1;
		} else {
			previous = last;

			if (fail) {
				last = 1;
			} else if (AppControl.control.identifiedWords == numOFWords) {
				last = 0;
			} else {
				last = -1;
			}
		}

		if (numOFWords < 2) {
			numOFWords = 2;
		}

		textWord.text = numOFWords.ToString ();

		// Achievement calculations
		AppControl.control.achieveCounter = AppControl.control.achieveCounter + 1;

		if (!AppControl.control.firstTestCleared) {
			AppControl.control.firstTestCleared = true;
			AndroidToast.ShowToastNotification ("Achievement opnået", AndroidToast.LENGTH_LONG);
		} else if (AppControl.control.achieveCounter == 10 && !AppControl.control.tenTestsCleared) {
			AppControl.control.tenTestsCleared = true;
			AndroidToast.ShowToastNotification ("Achievement opnået", AndroidToast.LENGTH_LONG);
		} else if (AppControl.control.achieveCounter == 50  && !AppControl.control.fiftyTestsCleared) {
			AppControl.control.firstTestCleared = true;
			AndroidToast.ShowToastNotification ("Achievement opnået", AndroidToast.LENGTH_LONG);
		} else if (AppControl.control.achieveCounter == 100  && !AppControl.control.hundredTestsCleared) {
			AppControl.control.hundredTestsCleared = true;
			AndroidToast.ShowToastNotification ("Achievement opnået", AndroidToast.LENGTH_LONG);
		} else if (AppControl.control.achieveCounter == 150  && !AppControl.control.hundredfiftyTestsCleared) {
			AppControl.control.hundredfiftyTestsCleared = true;
			AndroidToast.ShowToastNotification ("Achievement opnået", AndroidToast.LENGTH_LONG);
		}

		//Store local data
		AppControl.control.word_Recog_Target = numOFWords;
		AppControl.control.word_Last_Test = last;
		AppControl.control.word_previous_Test = previous;
		AppControl.control.testStarted = true;
		AppControl.control.Save ();

		yield return new WaitForSeconds (3);

		// Data to be stored
		string patientNumber = "#" + AppControl.control.patientNumber.ToString().Substring(1);
		string name = "Word Recognition";
		string time = System.DateTime.Now.ToString();
		string numOfIdentified = AppControl.control.identifiedWords.ToString ();
		string numOfFalse = AppControl.control.falseWords.ToString ();
		string timerTime = timer.ToString ();


		// Store data
		AppControl.control.dataString = "Patient Number: " + patientNumber + ", Name: " + name + ", Time: " + time + 
			", Target words: " + numOfTargets + ", Identified words: " + numOfIdentified + ", Falsely identified words: " 
			+ numOfFalse + ", Time used: " + timerTime + " seconds"; 
		AppControl.control.csvString = patientNumber + ";" + name + ";" + time + ";" + numOfTargets + ";" + numOfIdentified + ";" + numOfFalse + ";" + timerTime + ";;;;;;;;;;";

		AppControl.control.SaveData ();

		// test end
		SceneManager.LoadScene ("MainMenu");
	}
}
