using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Flanker_Controller : MonoBehaviour {

	public GameObject arrow;
	public Image check;
	public GameObject startCanvas;
	public GameObject canvas;
	public GameObject endCanvas;
	public Text shownText;
	public Text correctText;

	private int rand_target = -1;
	private int rand_distractors = -1;

	private float xStart = -2.25f;
	private float yStart = 3.7f;

	private int gridsShown = 0;
	private int correctSelections = 0;

	private bool active = false;

	private bool end = false;


	public void StartButton(){
		// Stop start canvas
		startCanvas.SetActive(false);

		canvas.SetActive (true);

		// Set up first time arrows
		SetArrows ();

		// Start a timer
		StartCoroutine (Timer ());
	}

	IEnumerator Timer(){
		// The timer waits for 60 seconds
		yield return new WaitForSeconds (60f);
		end = true;

		// Activate canvas representing the end of the test
		// plus show motivation scores
		endCanvas.SetActive (true);
		shownText.text = gridsShown.ToString();
		correctText.text = correctSelections.ToString ();

		// Wait for 3 seconds before transitioning to the next test
		yield return new WaitForSeconds (3f);

		// Data to be stored
		string name = "Eriksen Flanker";
		string time = System.DateTime.Now.ToString();
		string gridsShowed = gridsShown.ToString ();
		string correctMatches = correctSelections.ToString ();

		// Store data
		AppControl.control.dataString = "Name: " + name + ", Time: " + time + 
			", Grids shown: " + gridsShowed + ", Correct matches: " + correctMatches; 
		AppControl.control.csvString = name + ";" + time + ";;;;;;;;;;;" + gridsShowed + ";" + correctMatches + ";;";
		AppControl.control.SaveData ();

		// Move on
		SceneManager.LoadScene("MainMenu");
	}

	public void CheckSelection(int dir){
		// Active signifies that a new set of arrows are being spawned
		// nullifies button mashing between arrow sets
		if (!active) {
			// If the selected button is the same as the target, show a check mark and count it
			// Else show an x representing failure
			if (dir == rand_target) {
				check.sprite = Resources.Load<Sprite>("checkmark");
				check.enabled = true;
				correctSelections++;
			} else {
				check.sprite = Resources.Load<Sprite>("failure");
				check.enabled = true;
			}

			// Start the spawning of the next set of arrows
			StartCoroutine (NextArrowSet ());

			StartCoroutine (ResetCheck ());
		}
	}

	IEnumerator NextArrowSet(){
		active = true; // Spawning in progress

		RemoveArrows ();

		// Wait half a second
		yield return new WaitForSeconds (0.5f);

		SetArrows ();
		active = false; // Spawning done
	}

	IEnumerator ResetCheck(){
		// Wait 0.3 seconds, then remove checkmark/X
		yield return new WaitForSeconds (0.5f);
		check.enabled = false;
	}

	void SetArrows () {
		if(!end){
			gridsShown++; // Count every set of arrows shown
		}

		// Random directions for target arrow and distractor arrows
		rand_target = Random.Range(0, 4);
		rand_distractors = Random.Range(0, 4);

		// Start position for arrow grid
		float x = xStart;
		float y = yStart;

		int[][] arrowGrid = ArrowSpawning ();

		// Arrow spawnning
		for (int i = 0; i < 7; i++) {
			for (int j = 0; j < 7; j++) {

				// Center arrows
				if(i == 3 && j == 3){
					Vector3 pos = new Vector3 (x, y, 0f);
					Instantiate (arrow, pos, Quaternion.Euler(0,0,90*rand_target));
				}
				// Distractor arrow
				else if(arrowGrid[i][j] == 1){
					
					Vector3 pos = new Vector3 (x, y, 0f);
					Instantiate (arrow, pos, Quaternion.Euler(0,0,90*rand_distractors));
				}

				// Move along grid in y-direction
				y -= 0.75f;

			}
			// Reset y and move to next column
			y = yStart;
			x += 0.75f;
		}
	}

	void RemoveArrows(){
		// Find all arrows spawned
		GameObject[] arrows = GameObject.FindGameObjectsWithTag ("arrow");

		// Remove all found arrows
		foreach (GameObject g in arrows) {
			Destroy (g);
		}
	}

	int[][] ArrowSpawning(){

		int[][] arrowGrid = new int[7][];

		for (int i = 0; i < 7; i++) {
			arrowGrid [i] = new int[7];
		}

		arrowGrid [2] [3] = arrowGrid [3] [2] = arrowGrid [3] [4] = arrowGrid [4] [3] = 1;

		for (int i = 1; i < 9; i++) {

			int ran = Random.Range (0, 2);


			// Spreading up, down, left and right
			if (ran == 1 && i <= 2) {
				if (arrowGrid [4 + (i-1)] [3] == 1){
					arrowGrid [4 + i] [3] = ran;
					arrowGrid [2 - i] [3] = ran;

					arrowGrid [3] [4 + i] = ran;
					arrowGrid [3] [2 - i] = ran;
				}
			} 
			// Second layer: up, down, left, right
			else if (ran == 1 && i <= 5) {
				if(arrowGrid[3 + (i-3)][4] == 1){
					arrowGrid [4 + (i - 3)] [4] = ran;
					arrowGrid [2 - (i - 3)] [4] = ran;

					arrowGrid [4 + (i - 3)] [2] = ran;
					arrowGrid [2 - (i - 3)] [2] = ran;

					arrowGrid [4] [4 + (i - 3)] = ran;
					arrowGrid [4] [2 - (i - 3)] = ran;

					arrowGrid [2] [4 + (i - 3)] = ran;
					arrowGrid [2] [2 - (i - 3)] = ran;
				}

			} 
			// Third layer: up, down, left, right
			else if (ran == 1 && i <= 7) {
				if (arrowGrid[4 + (i - 6)][4 + (i - 6)] == 1) {
					arrowGrid [5 + i - 6] [5] = ran;
					arrowGrid [1 - (i - 6)] [5] = ran;

					arrowGrid [5 + i - 6] [1] = ran;
					arrowGrid [1 - (i - 6)] [1] = ran;

					arrowGrid [5] [5 + i - 6] = ran;
					arrowGrid [5] [1 - (i - 6)] = ran;

					arrowGrid [1] [5 + i - 6] = ran;
					arrowGrid [1] [1 - (i - 6)] = ran;
				}

			} 
			// Corners
			else if(ran == 1 && i <= 8){
				if (arrowGrid [5] [5] == 1) {
					arrowGrid [6] [6] = arrowGrid [0] [6] = arrowGrid [6] [0] = arrowGrid [0] [0] = ran;
				}
			}
		}

		return arrowGrid;
	}
}
