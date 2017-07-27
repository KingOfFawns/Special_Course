using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Login_Controller : MonoBehaviour {

	public InputField password;
	public Text loginText;

	private string code = "";

	void Start(){
		// get password from storage + De-Hexify
		string[] hexSplit = AppControl.control.password.Split (' ');
		string pass = "";
		foreach (string h in hexSplit) {
			int value = Convert.ToInt32 (h, 16);
			string sValue = Char.ConvertFromUtf32 (value);

			pass = pass + sValue;
		}

		code = pass;
	}

	public void ReturnToStart(){
		SceneManager.LoadScene ("MainMenu");
	}

	public void confirmButton(){
		// Check if password is correct
		if (password.text == code) {
			// Correct, go to settings
			password.text = "";
			SceneManager.LoadScene ("Settings");
		} else {
			// Incorrect password, notify
			password.text = "";
			loginText.text = "Forkert Kode";

			StartCoroutine (TimeOut ());
		}
	}

	IEnumerator TimeOut(){
		// Reset notify text
		yield return new WaitForSeconds (1);
		loginText.text = "";
	}


}
