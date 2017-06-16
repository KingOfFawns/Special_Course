using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login_Controller : MonoBehaviour {

	public InputField password;
	public Text loginText;

	private string code = "RootAdmin";

	void Start(){
		// get password from storage
		code = AppControl.control.password;
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
