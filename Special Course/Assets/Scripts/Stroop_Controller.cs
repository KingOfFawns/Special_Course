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

	private int phase = 1;
	private Color[] colors = {Color.green,Color.red,Color.blue,new Color(180f/255f,0,1f),
		new Color(1f,140f/255f,0), Color.yellow};
	private string[] words = {"Grøn", "Rød", "Blå", "Lilla", "Orange", "Gul"};

	private int ranColor = 0;
	private int ranWord = 0;

	private int correctMatches = 0;
	private int shownWords = 0;

	private bool active = false;

	public void StartButton(){
		check.enabled = false;

		startCanvas.SetActive(false);

		canvas.SetActive (true);

		UpdateWordAndColor ();

		taskText.text = "Match farven på bogstaverne";
		StartCoroutine (PhaseShift ());
	}

	IEnumerator PhaseShift(){

		yield return new WaitForSeconds (20);
		taskText.text = "Match farven der skrives";
		phase = 2;

		yield return new WaitForSeconds (20);
		taskText.text = "Match farven på bogstaverne";
		phase = 3;

		yield return new WaitForSeconds (20);
		// Activate canvas representing the end of the test
		// plus show motivation scores
		endCanvas.SetActive (true);
		shownText.text = shownWords.ToString();
		correctText.text = correctMatches.ToString ();

		yield return new WaitForSeconds(3);

		// Data to be stored
		string name = "Stroop Effect";
		string time = System.DateTime.Now.ToString();
		string numOfWordDisp = shownWords.ToString ();
		string cMatches = correctMatches.ToString ();

		// Store data
		AppControl.control.dataString = "Name: " + name + ", Time: " + time + 
			", Words displayed: " + numOfWordDisp + ", Correct matches: " + cMatches + "\n"; 
		AppControl.control.SaveData ();

		// test end
		SceneManager.LoadScene("MainMenu");
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

		shownWords++;

		ranColor = Random.Range (0, 6);
		ranWord = Random.Range (0, 6);

		colorText.text = words [ranWord];
		colorText.color = colors [ranColor];
	}
}
