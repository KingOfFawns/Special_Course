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

	IEnumerator PhaseShift(){

		// First phase showing
		taskText.text = "Match farven på bogstaverne";
		phase = 1;
		// Timer waits for 20 seconds
		for (int i = 1; i < 21; i++) {
			yield return new WaitForSeconds(1);
			timeBar.size = i / 60f;
			timeBar.transform.GetChild (1).GetComponent<Text> ().text = (60 - i).ToString ();
		}

		// Transition canvas
		transitionCanvas.SetActive(true);
		phaseText.text = "Match farven der skrives";
		yield return new WaitForSeconds (2);
		transitionCanvas.SetActive(false);

		// Show second phase
		taskText.text = "Match farven der skrives";
		phase = 2;
		// Timer waits for 20 seconds
		for (int i = 21; i < 41; i++) {
			yield return new WaitForSeconds(1);
			timeBar.size = i / 60f;
			timeBar.transform.GetChild (1).GetComponent<Text> ().text = (60 - i).ToString ();
		}

		// Transition canvas
		transitionCanvas.SetActive(true);
		phaseText.text = "Match farven på bogstaverne";
		yield return new WaitForSeconds (2);
		transitionCanvas.SetActive(false);

		// Show third phase
		taskText.text = "Match farven på bogstaverne";
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

		// Data to be stored
		string patientNumber = "#" + AppControl.control.patientNumber.ToString().Substring(1);
		string name = "Stroop Effect";
		string time = System.DateTime.Now.ToString();
		string numOfWordDisp = shownWords.ToString ();
		string cMatches = correctMatches.ToString ();

		// Store data
		AppControl.control.dataString = "Patient Number: " + patientNumber + ", Name: " + name + ", Time: " + time + 
			", Words displayed: " + numOfWordDisp + ", Correct matches: " + cMatches; 
		AppControl.control.csvString = patientNumber + ";" + name + ";" + time + ";;;;;;;;;;" + numOfWordDisp + ";;" + cMatches + ";;";
		AppControl.control.SaveData ();

		// test end
		SceneManager.LoadScene("Word_Recog_End");
	}

	public void CheckSelected(int col){
		if (!active) {
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
