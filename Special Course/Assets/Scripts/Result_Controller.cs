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

	private float x = 2.16f;
	private float y = -3f;

	private string[] fileLines;


	public void ReturnToStart(){
		SceneManager.LoadScene ("MainMenu");
	}

	void Awake(){
		fileLines = File.ReadAllLines (Application.persistentDataPath + "/saveData.csv");

		EriksenFlankerGraph ();
	}

	public void EriksenFlankerGraph(){
		LineRenderer eFR = resultLine.GetComponent<LineRenderer> ();
		LineRenderer eFC = compareLine.GetComponent<LineRenderer> ();

		int count = -1;

		for(int i = fileLines.Length - 1; i > 0; i--){
			if (count > 8) {
				break;
			}

			if (fileLines [i].Contains ("Eriksen Flanker")) {
				count++;

				string[] current = fileLines [i].Split (';');

				string[] date = current [1].Split(' ');
				date = date [0].Split ('/');

				if (date [0].Length < 2) {
					dateText [count].text = "0" + date [0] + "-" + date [1] + "-" + date [2];
				} else {
					dateText [count].text = date [0] + "-" + date [1] + "-" + date [2];
				}


				float shownGrids, corrects, ratio, compare = 0f;

				float.TryParse (current [12], out shownGrids);
				float.TryParse (current [13], out corrects);

				ratio = 2*(corrects / shownGrids);
				eFR.SetPosition (count, new Vector3 (x, y + ratio, 0f));

				compare = 2 * (shownGrids / 100);
				eFC.SetPosition(count, new Vector3 (x, y + compare, 0f));

				Debug.Log (compare);

				x = x - 0.48f;
			}
		}
		x = 2.16f;
	}

}
