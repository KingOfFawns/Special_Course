using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Stroop_Controller : MonoBehaviour {

	public Text taskText;
	public Text colorText;
	public Image check;
	public GameObject startCanvas;
	public GameObject canvas;
	public GameObject endCanvas;
	public Text shownText;
	public Text correctText;
	public GameObject transitionCanvas;
	public Text phaseText;
	public Scrollbar timeBar;

	private int phase = 0;
	private Color[] colors = {Color.green,Color.red,Color.blue,new Color(180f/255f,0,1f),
		new Color(1f,140f/255f,0), Color.yellow};
	private string[] words = {"Grøn", "Rød", "Blå", "Lilla", "Orange", "Gul"};
	private int ranColor = 0;
	private int ranWord = 0;
	private int correctMatches = 0;
	private int shownWords = 0;
	private bool active = false;
	private bool end = false;


	public void StartButton(){
		check.enabled = false;

		startCanvas.SetActive(false);

		canvas.SetActive (true);

		UpdateWordAndColor ();

		StartCoroutine (PhaseShift ());
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

	IEnumerator PhaseShift(){

		// First phase showing
		taskText.text = "Tryk på den farve, som bogstaverne er skrevet med.";
		phase = 1;
		// Timer waits for 20 seconds
		for (int i = 1; i < 21; i++) {
			yield return new WaitForSeconds(1);
			timeBar.size = i / 60f;
			timeBar.transform.GetChild (1).GetComponent<Text> ().text = (60 - i).ToString ();
		}

		// Transition canvas
		transitionCanvas.SetActive(true);
		phaseText.text = "Tryk på den farve, som ordet beksriver.";
		yield return new WaitForSeconds (2);
		transitionCanvas.SetActive(false);

		// Show second phase
		taskText.text = "Tryk på den farve, som ordet beksriver.";
		phase = 2;
		// Timer waits for 20 seconds
		for (int i = 21; i < 41; i++) {
			yield return new WaitForSeconds(1);
			timeBar.size = i / 60f;
			timeBar.transform.GetChild (1).GetComponent<Text> ().text = (60 - i).ToString ();
		}

		// Transition canvas
		transitionCanvas.SetActive(true);
		phaseText.text = "Tryk på den farve, som bogstaverne er skrevet med.";
		yield return new WaitForSeconds (2);
		transitionCanvas.SetActive(false);

		// Show third phase
		taskText.text = "Tryk på den farve, som bogstaverne er skrevet med.";
		phase = 3;
		// Timer waits for 20 seconds
		for (int i = 41; i < 61; i++) {
			yield return new WaitForSeconds(1);
			timeBar.size = i / 60f;
			timeBar.transform.GetChild (1).GetComponent<Text> ().text = (60 - i).ToString ();
		}

		// Activate canvas representing the end of the test
		// plus show motivation scores
		endCanvas.SetActive (true);
		end = true;
		shownText.text = shownWords.ToString();
		correctText.text = correctMatches.ToString ();

		yield return new WaitForSeconds(3);

		// If not in help's test mode
		if (!AppControl.control.testOfTest) {
			// Data to be stored
			string patientNumber = "#" + AppControl.control.patientNumber.ToString ().Substring (1);
			string name = "Stroop Effect";
			string time = System.DateTime.Now.ToString ();
			string numOfWordDisp = shownWords.ToString ();
			string cMatches = correctMatches.ToString ();

			// Store data
			AppControl.control.dataString = "Patient Number: " + patientNumber + ", Name: " + name + ", Time: " + time +
			", Words displayed: " + numOfWordDisp + ", Correct matches: " + cMatches; 
			AppControl.control.csvString = patientNumber + ";" + name + ";" + time + ";;;;;;;;;;" + numOfWordDisp + ";;" + cMatches + ";;";
			AppControl.control.SaveData ();

			// test end
			SceneManager.LoadScene ("Word_Recog_End");
		} else {
			SceneManager.LoadScene ("Help");
		}
	}

	public void CheckSelected(int col){
		// If change is not active
		if (!active) {
			// check if selected is correct according to phase
			if (phase == 1 || phase == 3) {
				if (col == ranColor) {
					correctMatches++;
					check.sprite = Resources.Load<Sprite> ("checkmark");
				} else {
					check.sprite = Resources.Load<Sprite> ("failure");
				}
			} else {
				if (col == ranWord) {
					correctMatches++;
					check.sprite = Resources.Load<Sprite> ("checkmark");
				} else {
					check.sprite = Resources.Load<Sprite> ("failure");
				}
			}
			check.enabled = true;

			StartCoroutine (NextWord ());

			StartCoroutine (ResetCheck ());
		}
	}

	IEnumerator NextWord(){
		active = true; // Spawning in progress

		colorText.text = "";
		// Wait half a second
		yield return new WaitForSeconds (0.5f);

		UpdateWordAndColor ();
		active = false; // Spawning done
	}

	IEnumerator ResetCheck(){
		// Wait 0.3 seconds, then remove checkmark/X
		yield return new WaitForSeconds (0.3f);
		check.enabled = false;
		check.sprite = Resources.Load<Sprite> ("None"); 
	}
	
	public void UpdateWordAndColor(){

		if (!end) {
			shownWords++;
		}

		// Chose color and word
		ranColor = Random.Range (0, 6);
		ranWord = Random.Range (0, 6);

		// Show color and word
		colorText.text = words [ranWord];
		colorText.color = colors [ranColor];
	}
}
