using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

	public Canvas QuitMenu, OptionsMenu, PlayerMenu;
	public Button StartText, OptionsText, ExitText;
	
	private int numPlayers = 0;

	void Start () {
		QuitMenu = QuitMenu.GetComponent<Canvas> ();
		OptionsMenu = OptionsMenu.GetComponent<Canvas> ();
		PlayerMenu = PlayerMenu.GetComponent<Canvas>();
		StartText = StartText.GetComponent<Button> ();
		OptionsText = OptionsText.GetComponent<Button> ();
		ExitText = ExitText.GetComponent<Button> ();
		QuitMenu.enabled = false;
		OptionsMenu.enabled = false;
		PlayerMenu.enabled = false;
	}
	public void ExitPress () {
		QuitMenu.enabled = true;
		OptionsMenu.enabled = false;
		PlayerMenu.enabled = false;
		StartText.enabled = false;
		OptionsText.enabled = false;
		ExitText.enabled = false;
	}
	
	public void OptionsPress () {
		OptionsMenu.enabled = true;
		PlayerMenu.enabled = false;
		QuitMenu.enabled = false;
		StartText.enabled = false;
		OptionsText.enabled = false;
		ExitText.enabled = false;
	}
	
	public void PlayerPress () {
		PlayerMenu.enabled = true;
		OptionsMenu.enabled = false;
		QuitMenu.enabled = false;
		StartText.enabled = false;
		OptionsText.enabled = false;
		ExitText.enabled = false;
	}
	
	public void NoPress () {
		QuitMenu.enabled = false;
		OptionsMenu.enabled = false;
		PlayerMenu.enabled = false;
		StartText.enabled = true;
		OptionsText.enabled = true;
		ExitText.enabled = true;
	}
	
	public void P1Press () { numPlayers = 1; StartLevel (); }
	public void P2Press () { numPlayers = 2; StartLevel (); }
	public void P3Press () { numPlayers = 3; StartLevel (); }
	public void P4Press () { numPlayers = 4; StartLevel (); }
	public void StartLevel () {
		// if (numPlayers == 1)
			// SceneManager.LoadScene ("1P_Kart_Selection");
		// else if (numPlayers == 2)
			// SceneManager.LoadScene ("2P_Kart_Selection");
		// else if (numPlayers == 3)
			// SceneManager.LoadScene ("3P_Kart_Selection");
		// else if (numPlayers == 4)
			// SceneManager.LoadScene ("4P_Kart_Selection");
		// else
			// Debug.Log("GAME DOES NOT SUPPORT THAT NUMBER OF PLAYERS!!");
	}
	
	public void ExitGame () {
		Application.Quit();
	}
}
