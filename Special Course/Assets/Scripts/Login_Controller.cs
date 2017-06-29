using System.Collections;
using System.Collections.Generic;
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
		if (password.text == code) {
			password.text = "";
			SceneManager.LoadScene ("Settings");
		} else {
			password.text = "";
			loginText.text = "Forkert Kode";

			StartCoroutine (TimeOut ());
		}
	}

	IEnumerator TimeOut(){
		yield return new WaitForSeconds (1);
		loginText.text = "";
	}


}
