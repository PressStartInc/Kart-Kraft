using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

	public Canvas QuitMenu, PlayerMenu;
	public Button StartText, ExitText;
	public Button one, two, three, four, back;
	public Button yes, no;
	
	private int numPlayers = 0;
	private int index = 0;
	private int subIndex = 0;
	private Button[] startOptions = new Button[3];
	private Button[] quitOptions = new Button[3];
	private Button[] playerOptions = new Button[5];

	void Start () {
		QuitMenu = QuitMenu.GetComponent<Canvas> ();
		PlayerMenu = PlayerMenu.GetComponent<Canvas>();
		StartText = StartText.GetComponent<Button> ();
		ExitText = ExitText.GetComponent<Button> ();
		QuitMenu.enabled = false;
		PlayerMenu.enabled = false;
		
		startOptions[0] = StartText;
		startOptions[1] = ExitText;
		
		playerOptions[0] = one;
		playerOptions[1] = two;
		playerOptions[2] = three;
		playerOptions[3] = four;
		playerOptions[4] = back;
		
		quitOptions[0] = yes;
		quitOptions[1] = no;
	}
	
	
	bool upButtonPushed = false;
	bool sideButtonPushed = false;
	bool inSubMenu = false;
	void Update() {
		if ((Input.GetAxis("MenuUD") < 0 && !upButtonPushed) && (index+1 < 2)) {
			startOptions[index].GetComponent<Text>().color = Color.white;
			startOptions[++index].GetComponent<Text>().color = Color.red;
			Debug.Log("MENU UP"); 
			upButtonPushed = true;
		} else if ((Input.GetAxis("MenuUD") > 0 && !upButtonPushed) && (index-1 >= 0)) {
			startOptions[index].GetComponent<Text>().color = Color.white;
			startOptions[--index].GetComponent<Text>().color = Color.red; 
			Debug.Log("MENU DOWN");
			upButtonPushed = true;
		} else if (Input.GetAxis("MenuUD") == 0) { upButtonPushed = false; }
		
		if (Input.GetAxis("MenuLR") > 0 && !sideButtonPushed) {
			if ((index == 0) && subIndex+1 < playerOptions.Length) {
				playerOptions[subIndex].GetComponent<Text>().color = Color.white;
				playerOptions[++subIndex].GetComponent<Text>().color = Color.red;
				sideButtonPushed = true;
			} else if ((index == 1) && subIndex+1 < quitOptions.Length) {
				quitOptions[subIndex].GetComponent<Text>().color = Color.white;
				quitOptions[++subIndex].GetComponent<Text>().color = Color.red;
				sideButtonPushed = true;
			}
		} else if (Input.GetAxis("MenuLR") < 0 && !sideButtonPushed) {
			if ((index == 0) && subIndex-1 >= 0) {
				playerOptions[subIndex].GetComponent<Text>().color = Color.white;
				playerOptions[--subIndex].GetComponent<Text>().color = Color.red;
				sideButtonPushed = true;
			} else if ((index == 1) && subIndex-1 >= 0) {
				quitOptions[subIndex].GetComponent<Text>().color = Color.white;
				quitOptions[--subIndex].GetComponent<Text>().color = Color.red;
				sideButtonPushed = true;
			}
		} else if (Input.GetAxis("MenuLR") == 0) { sideButtonPushed = false; }
		
		
		if (Input.GetButton("Submit"))
		{	
			if (!inSubMenu) {
				subIndex = 0;
				inSubMenu = true;
				switch (index) {
					case 0:
						PlayerPress();
						break;
					case 1:
						ExitPress();
						break;
				}
			} else if (inSubMenu) {
				if (index == 0) {
					switch (subIndex) {
						case 0:
							P1Press();
							break;
						case 1:
							P2Press();
							break;
						case 2:
							P3Press();
							break;
						case 3:
							P4Press();
							break;
						case 4:
							NoPress();
							break;
					}
				} else if (index == 1) {
					switch (subIndex) {
						case 0:
							ExitGame();
							break;
						case 1:
							NoPress();
							break;
					}
				}
			}
		}
		else if (Input.GetButton("Cancel")) { NoPress(); }
	}
	
	public void ExitPress () {
		QuitMenu.enabled = true;
		PlayerMenu.enabled = false;
		StartText.enabled = false;
		ExitText.enabled = false;
	}
	
	public void PlayerPress () {
		PlayerMenu.enabled = true;
		QuitMenu.enabled = false;
		StartText.enabled = false;
		ExitText.enabled = false;
	}
	
	public void NoPress () {
		inSubMenu = false;
		QuitMenu.enabled = false;
		PlayerMenu.enabled = false;
		StartText.enabled = true;
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
