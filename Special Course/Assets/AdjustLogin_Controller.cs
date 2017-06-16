using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AdjustLogin_Controller : MonoBehaviour {

	public InputField password;
	public InputField passwordRepeat;
	public Text saveText;

	public void ReturnToStart(){
		SceneManager.LoadScene ("Settings");
	}

	public void StorePassword(){
		if (password.text == passwordRepeat.text) {

			AppControl.control.password = password.text;
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
