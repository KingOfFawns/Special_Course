﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	public void OnClick(){

		Button b = this.GetComponent<Button> ();

		string word = b.GetComponentInChildren<Text>().text;

		ColorBlock cb = b.colors;

		if (IsChosen (word)) {
			cb.normalColor = Color.green;
			AppControl.control.identifiedWords++;
		} else {
			cb.normalColor = Color.red;
			AppControl.control.falseWords++;
		}

		if (AppControl.control.identifiedWords >= AppControl.control.word_Recog_Target) {
			AppControl.control.success = true;
		}

		b.colors = cb;

	}

	bool IsChosen(string word){
		string[] chosenWords = AppControl.control.chosenWords;

		foreach (string w in chosenWords) {
			if (w == word) {
				return true;
			}
		}
		return false;
	}
}
