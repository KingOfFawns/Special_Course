using UnityEngine;
using UnityEngine.SceneManagement;

public class Achieve_Controller : MonoBehaviour {

	public void GoBack(){
		SceneManager.LoadScene ("ResultOverview");
	}
}
