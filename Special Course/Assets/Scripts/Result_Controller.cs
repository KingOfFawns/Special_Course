using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class Result_Controller : MonoBehaviour {

	public GameObject resultLine;
	public GameObject compareLine;
	public Text[] dateText;
	public Text RLegend;
	public Text CLegend;

	private float x = 2.16f;
	private float y = -3.4f;
	private string[] fileLines;
	private LineRenderer resultGraph;
	private LineRenderer compareGraph;


	public void ReturnToStart(){
		SceneManager.LoadScene ("MainMenu");
	}

	void Awake(){
		// Read data file into string array fileLines
		fileLines = File.ReadAllLines (Application.persistentDataPath + "/.dat1.dat");

		resultGraph = resultLine.GetComponent<LineRenderer> ();
		compareGraph = compareLine.GetComponent<LineRenderer> ();
	}

	public void WordRecognitionGraph(){
		RLegend.text = "Korrekte kontra falske viste";
		CLegend.text = "Ord vist i del 1";


	}

	public void NBackGraph(){
		RLegend.text = "Korrekte valgte af viste";
		CLegend.text = "N ud af 15";

		GraphIt2 (15, "N-Back", 15, 7, 14);
	}

	public void DigitSpanGraph(){
		RLegend.text = "Korrekte valgte af viste";
		CLegend.text = "Sekvens længde af 15";

		GraphIt2 (15, "Digit Span", 10, 13, 9);
	}

	public void EriksenFlankerGraph(){
		RLegend.text = "Korrekte valgte af viste";
		CLegend.text = "Pile sæt vist af 60";

		GraphIt (60, "Eriksen Flanker", 12, 13);
	}

	public void StroopEffectGraph(){
		RLegend.text = "Korrekte valgte af viste";
		CLegend.text = "Ord vist af 50";

		GraphIt (50, "Stroop Effect", 11, 13);
	}

	void GraphIt (int maxCompare, string test, int shownThings, int correctOfShown){
		clearGraph ();

		// Counter to select the amount of dates needed
		int count = -1;

		// Go through data from latest date
		for (int i = fileLines.Length - 1; i > 0; i--) {

			// If 10 dates found
			if (count > 8) {
				break;
			}

			// Is the line one that can be used
			if (fileLines [i].Contains (test)) {
				count++;

				// Extract date
				string[] current = fileLines [i].Split (';');
				string[] date = current [1].Split (' ');
				date = date [0].Split ('/');

				// Fix date
				if (date [0].Length < 2 && date [1].Length < 2) {
					dateText [count].text = "0" + date [0] + "- 0" + date [1] + "-" + date [2];
				} 
				else if (date [0].Length < 2) {
					dateText [count].text = "0" + date [0] + "-" + date [1] + "-" + date [2];
				} 
				else if (date [1].Length < 2) {
					dateText [count].text = date [0] + "- 0" + date [1] + "-" + date [2];
				} 
				else {
					dateText [count].text = date [0] + "-" + date [1] + "-" + date [2];
				}

				// Get relevant data
				float shown, corrects, ratio, compare = 0f;
				float.TryParse (current [shownThings], out shown);
				float.TryParse (current [correctOfShown], out corrects);

				// Calculate ratio and compare
				ratio = 2.5f * (corrects / shown);
				compare = 2.5f * (shown / maxCompare);

				// Create graph points
				resultGraph.SetPosition (count, new Vector3 (x, y + ratio, 0f));
				compareGraph.SetPosition (count, new Vector3 (x, y + compare, 0f));

				// Increment x position in graph
				x = x - 0.48f;
			}
		}
		// Reset x
		x = 2.16f;
	}

	void GraphIt2 (int maxCompare, string test, int shownThings, int correctOfShown, int compareValue){
		clearGraph ();

		// Counter to select the amount of dates needed
		int count = -1;

		// Go through data from latest date
		for (int i = fileLines.Length - 1; i > 0; i--) {

			// If 10 dates found
			if (count > 8) {
				break;
			}

			// Is the line one that can be used
			if (fileLines [i].Contains (test)) {
				count++;

				// Extract date
				string[] current = fileLines [i].Split (';');
				string[] date = current [1].Split (' ');
				date = date [0].Split ('/');

				// Fix date
				if (date [0].Length < 2 && date [1].Length < 2) {
					dateText [count].text = "0" + date [0] + "- 0" + date [1] + "-" + date [2];
				} 
				else if (date [0].Length < 2) {
					dateText [count].text = "0" + date [0] + "-" + date [1] + "-" + date [2];
				} 
				else if (date [1].Length < 2) {
					dateText [count].text = date [0] + "- 0" + date [1] + "-" + date [2];
				} 
				else {
					dateText [count].text = date [0] + "-" + date [1] + "-" + date [2];
				}

				// Get relevant data
				float shown, corrects, ratio, compare = 0f;
				int compareValueInt = 0;
				float.TryParse (current [shownThings], out shown);
				float.TryParse (current [correctOfShown], out corrects);
				int.TryParse (current[compareValue], out compareValueInt);

				// Calculate ratio and compare
				ratio = 2.5f * (corrects / shown);
				compare = 2.5f * ((float)compareValueInt / maxCompare);

				// Create graph points
				resultGraph.SetPosition (count, new Vector3 (x, y + ratio, 0f));
				compareGraph.SetPosition (count, new Vector3 (x, y + compare, 0f));

				// Increment x position in graph
				x = x - 0.48f;
			}
		}
		// Reset x
		x = 2.16f;
	}

	void clearGraph(){
		// Reset all the points in the graph.
		for (int i = 0; i < 10; i++) {
			resultGraph.SetPosition (i, new Vector3 (x, y, 0f));
			compareGraph.SetPosition (i, new Vector3 (x, y, 0));
			x = x - 0.48f;
		}

		x = 2.16f;
	}
}
