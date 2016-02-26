using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

	public Canvas QuitMenu;
	public Button StartText, ExitText;

	void Start () {
		QuitMenu = QuitMenu.GetComponent<Canvas> ();
		StartText = StartText.GetComponent<Button> ();
		ExitText = ExitText.GetComponent<Button> ();
		QuitMenu.enabled = false;
	}
	public void ExitPress () {
		QuitMenu.enabled = true;
		StartText.enabled = false;
		ExitText.enabled = false;
	}
	public void NoPress () {
		QuitMenu.enabled = false;
		StartText.enabled = true;
		ExitText.enabled = true;
	}
	public void StartLevel () {
		SceneManager.LoadScene ("KART_DEV");
	}
	public void ExitGame () {
		Application.Quit();
	}
}
