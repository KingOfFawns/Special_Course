using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordButtonScript : MonoBehaviour {

	private bool active = false;

	public void OnClick(){

		// Get text used to show how many words are left to be chosen
		Text wordsLeft = GameObject.Find ("WordsText").GetComponent<Text>();

		// Get this button
		Button b = this.GetComponent<Button> ();

		// Get buttons word
		string word = b.GetComponentInChildren<Text>().text;

		// Get buttons color
		ColorBlock cb = b.colors;

		// If button has not been selected
		if (!active) {
			// If the word is chosen, count it as identified and change color of button to gray
			if (IsChosen (word)) {
				cb.normalColor = Color.gray;
				AppControl.control.identifiedWords++;
			} 
			// If word is not chosen, count it as false identified and change color of button to gray
			else {
				cb.normalColor = Color.gray;
				AppControl.control.falseWords++;
			}

			// If the amount of word to be chosen is reached
			if (AppControl.control.identifiedWords + AppControl.control.falseWords >= AppControl.control.word_Recog_Target) {
				// Set app succes to true
				AppControl.control.success = true;
			}

			active = true;
		} 
		// If button ahs been selected
		else {
			// Reset color to white
			cb.normalColor = Color.white;

			// Remove from counted accordingly to type
			if (IsChosen (word)) {
				AppControl.control.identifiedWords--;
			} else {
				AppControl.control.falseWords--;
			}

			active = false;
		}

		// Update text showing how many words left to chose
		if ((AppControl.control.identifiedWords + AppControl.control.falseWords) > AppControl.control.word_Recog_Target) {
			wordsLeft.text = "0 ord";
		} else {
			wordsLeft.text = (AppControl.control.word_Recog_Target - (AppControl.control.identifiedWords + AppControl.control.falseWords)) + " ord";
		}

		// Set color of button
		b.colors = cb;

	}

	bool IsChosen(string word){
		// Get all chosen words from word_recog start
		string[] chosenWords = AppControl.control.chosenWords;

		// Check if word is in chosen words
		foreach (string w in chosenWords) {
			if (w == word) {
				return true;
			}
		}
		return false;
	}
}
