using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class AdjustLogin_Controller : MonoBehaviour {

	public Text showField;
	public InputField password;
	public InputField passwordRepeat;
	public Text saveText;


	void Start(){
		string[] hexSplit = AppControl.control.password.Split (' ');
		string pass = "";
		foreach (string h in hexSplit) {
			int value = Convert.ToInt32 (h, 16);
			string sValue = Char.ConvertFromUtf32 (value);

			pass = pass + sValue;
		}

		showField.text = "Current: " + pass;
	}

	public void ReturnToStart(){
		SceneManager.LoadScene ("Settings");
	}

	public void StorePassword(){
		if (password.text == passwordRepeat.text) {

			Debug.Log (password.text);

			char[] values = password.text.ToCharArray ();
			string hexPass = "";
			for(int i = 0; i < values.Length; i++) {
				int value = Convert.ToInt32 (values[i]);

				if (i < values.Length - 1) {
					hexPass = hexPass + String.Format ("{0:X}", value) + ' ';
				} else {
					hexPass = hexPass + String.Format ("{0:X}", value);
				}
			}

			Debug.Log (hexPass);

			AppControl.control.password = hexPass;
			AppControl.control.Save ();

			saveText.text = "Gemt";
		} else {
			saveText.text = "Passwordsne er ikke ens";
		}

		StartCoroutine (TimeOut ());
	}

	IEnumerator TimeOut(){
		yield return new WaitForSeconds (1);
		saveText.text = "";
	}
}
