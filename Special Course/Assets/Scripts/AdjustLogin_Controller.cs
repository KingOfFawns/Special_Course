using System.Collections;
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
		// Get password and split it in spaces
		string[] hexSplit = AppControl.control.password.Split (' ');
		// Calculate password
		string pass = "";
		foreach (string h in hexSplit) {
			int value = Convert.ToInt32 (h, 16);
			string sValue = Char.ConvertFromUtf32 (value);

			pass = pass + sValue;
		}

		// Show password
		showField.text = "Current: " + pass;
	}

	public void ReturnToStart(){
		SceneManager.LoadScene ("Settings");
	}

	public void StorePassword(){
		// If the input password and input repeat password is the same
		if (password.text == passwordRepeat.text) {

			// Turn password into array of chars
			char[] values = password.text.ToCharArray ();
			// Calculate hex of each char (smallest cryptation)
			string hexPass = "";
			for(int i = 0; i < values.Length; i++) {
				int value = Convert.ToInt32 (values[i]);

				if (i < values.Length - 1) {
					hexPass = hexPass + String.Format ("{0:X}", value) + ' ';
				} else {
					hexPass = hexPass + String.Format ("{0:X}", value);
				}
			}

			// Store hex version of password
			AppControl.control.password = hexPass;
			AppControl.control.Save ();

			// Show user that it is confirmed
			saveText.text = "Gemt";
		} 
		// If the input password and input repeat password is not the same
		else {
			// Show user that the passwords are not the same
			saveText.text = "Passwordsne er ikke ens";
		}

		// Start reset of text
		StartCoroutine (TimeOut ());
	}

	IEnumerator TimeOut(){
		// Reset the text used to notify the user of fails or confirms in the input of passwords
		yield return new WaitForSeconds (1);
		saveText.text = "";
	}
}
