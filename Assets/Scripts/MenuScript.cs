using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

	public Canvas QuitMenu, PlayerMenu, SeedMenu;
	public Button StartText, ExitText;
	public Button one, two, three, four, back;
	public Button yes, no;
	public Button rLow, rMed, rHigh, rUltra, fLow, fMed, fHigh, fUltra,
	        fhLow, fhMed, fhHigh, fhUltra, msLow, msMed, msHigh, msUltra,
			lava, lunar, terra, snow, oneAI, twoAI, threeAI, fourAI, cancel;
	
	private int roughness, flatness, flatHeight, mountainSize, biome, numCPUs;
	private int numPlayers = 0;
	private int index = 0;
	private int subIndex = 0;
	private Button[] startOptions, quitOptions, playerOptions;
	
	private Button[] roughOptions, flatOptions, flatHeightOptions,
        mountSizeOptions, biomeOptions, cpuOptions;

	void Start () {
		QuitMenu   = QuitMenu.GetComponent<Canvas> ();
		PlayerMenu = PlayerMenu.GetComponent<Canvas>();
		SeedMenu   = SeedMenu.GetComponent<Canvas>();
		StartText  = StartText.GetComponent<Button> ();
		ExitText   = ExitText.GetComponent<Button> ();
		QuitMenu.enabled   = false;
		PlayerMenu.enabled = false;
		SeedMenu.enabled   = false;
		
		startOptions  = new Button[2] {StartText, ExitText};
		quitOptions   = new Button[2] {yes, no};
		playerOptions = new Button[5] { one, two, three, four, back};
		
		roughOptions      = new Button[4] { rLow, rMed, rHigh, rUltra};
		flatOptions       = new Button[4] { fLow, fMed, fHigh, fUltra};
		flatHeightOptions = new Button[4] { fhLow, fhMed, fhHigh, fhUltra};
		mountSizeOptions  = new Button[4] { msLow, msMed, msHigh, msUltra};
		biomeOptions      = new Button[4] { lava, lunar, terra, snow};
		cpuOptions        = new Button[4] { oneAI, twoAI, threeAI, fourAI};
	}
	
	
	bool upButtonPushed = false;
	bool sideButtonPushed = false;
	bool inSubMenu = false;
	bool seedMenu = false;
	void FixedUpdate() {
		if (!seedMenu)
		{
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
		else
		{
			if (Input.GetAxis("MenuLR") > 0 && !sideButtonPushed) {
				if ((index == 0) && subIndex+1 < roughOptions.Length) {
					roughOptions[subIndex].GetComponent<Text>().color = Color.white;
					roughOptions[++subIndex].GetComponent<Text>().color = Color.red;
					sideButtonPushed = true;
				} else if ((index == 1) && subIndex+1 < flatOptions.Length) {
					flatOptions[subIndex].GetComponent<Text>().color = Color.white;
					flatOptions[++subIndex].GetComponent<Text>().color = Color.red;
					sideButtonPushed = true;
				} else if ((index == 2) && subIndex+1 < flatHeightOptions.Length) {
					flatHeightOptions[subIndex].GetComponent<Text>().color = Color.white;
					flatHeightOptions[++subIndex].GetComponent<Text>().color = Color.red;
					sideButtonPushed = true;
				} else if ((index == 3) && subIndex+1 < mountSizeOptions.Length) {
					mountSizeOptions[subIndex].GetComponent<Text>().color = Color.white;
					mountSizeOptions[++subIndex].GetComponent<Text>().color = Color.red;
					sideButtonPushed = true;
				} else if ((index == 4) && subIndex+1 < biomeOptions.Length) {
					biomeOptions[subIndex].GetComponent<Text>().color = Color.white;
					biomeOptions[++subIndex].GetComponent<Text>().color = Color.red;
					sideButtonPushed = true;
				} else if ((index == 5) && subIndex+1 < cpuOptions.Length) {
					cpuOptions[subIndex].GetComponent<Text>().color = Color.white;
					cpuOptions[++subIndex].GetComponent<Text>().color = Color.red;
					sideButtonPushed = true;
				} 
			} else if (Input.GetAxis("MenuLR") < 0 && !sideButtonPushed) {
				if ((index == 0) && subIndex-1 >= 0) {
					roughOptions[subIndex].GetComponent<Text>().color = Color.white;
					roughOptions[--subIndex].GetComponent<Text>().color = Color.red;
					sideButtonPushed = true;
				} else if ((index == 1) && subIndex-1 >= 0) {
					flatOptions[subIndex].GetComponent<Text>().color = Color.white;
					flatOptions[--subIndex].GetComponent<Text>().color = Color.red;
					sideButtonPushed = true;
				} else if ((index == 2) && subIndex-1 >= 0) {
					flatHeightOptions[subIndex].GetComponent<Text>().color = Color.white;
					flatHeightOptions[--subIndex].GetComponent<Text>().color = Color.red;
					sideButtonPushed = true;
				} else if ((index == 3) && subIndex-1 >= 0) {
					mountSizeOptions[subIndex].GetComponent<Text>().color = Color.white;
					mountSizeOptions[--subIndex].GetComponent<Text>().color = Color.red;
					sideButtonPushed = true;
				} else if ((index == 4) && subIndex-1 >= 0) {
					biomeOptions[subIndex].GetComponent<Text>().color = Color.white;
					biomeOptions[--subIndex].GetComponent<Text>().color = Color.red;
					sideButtonPushed = true;
				} else if ((index == 5) && subIndex-1 >= 0) {
					cpuOptions[subIndex].GetComponent<Text>().color = Color.white;
					cpuOptions[--subIndex].GetComponent<Text>().color = Color.red;
					sideButtonPushed = true;
				} 
			} else if (Input.GetAxis("MenuLR") == 0) { sideButtonPushed = false; }
			
			if (Input.GetButton("Submit"))
			{	
				switch (index) {
					case 0:
						roughPress(subIndex);
						break;
					case 1:
						flatPress(subIndex);
						break;
					case 2:
						flatHeightPress(subIndex);
						break;
					case 3:
						mountSizePress(subIndex);
						break;
					case 4:
						biomePress(subIndex);
						break;
					case 5:
						cpuPress(subIndex);
						break;
				}
				subIndex = 0;
			}
			else if (Input.GetButton("Cancel")) 
			{ 
				if (index != 0)
					index--;
				else
					NoPress(); 
			}
		}
	}
	
	public void ExitPress () {
		QuitMenu.enabled = true;
		PlayerMenu.enabled = false;
		SeedMenu.enabled = false;
		StartText.enabled = false;
		ExitText.enabled = false;
	}
	
	public void PlayerPress () {
		PlayerMenu.enabled = true;
		QuitMenu.enabled = false;
		SeedMenu.enabled = false;
		StartText.enabled = false;
		ExitText.enabled = false;
	}
	
	public void SeedPress () {
		index = 0;
		seedMenu = true;
		SeedMenu.enabled = true;
		PlayerMenu.enabled = false;
		QuitMenu.enabled = false;
		StartText.enabled = false;
		ExitText.enabled = false;
	}
	
	public void roughPress(int level) {
		index++;
		if (level == 0)
			roughness = level;
		else if (level == 1)
			roughness = level;
		else if (level == 2)
			roughness = level;
		else if (level == 3)
			roughness = level;
		return;
	}
	
	public void flatPress(int level) {
		index++;
		if (level == 0)
			flatness = level;
		else if (level == 1)
			flatness = level;
		else if (level == 2)
			flatness = level;
		else if (level == 3)
			flatness = level;
		return;
	}
	
	public void flatHeightPress(int level) {
		index++;
		if (level == 0)
			flatHeight = level;
		else if (level == 1)
			flatHeight = level;
		else if (level == 2)
			flatHeight = level;
		else if (level == 3)
			flatHeight = level;
		return;
	}
	
	public void mountSizePress(int level) {
		index++;
		if (level == 0)
			mountainSize = level;
		else if (level == 1)
			mountainSize = level;
		else if (level == 2)
			mountainSize = level;
		else if (level == 3)
			mountainSize = level;
		return;
	}
	
	public void biomePress(int level) {
		index++;
		if (level == 0)
			biome = level;
		else if (level == 1)
			biome = level;
		else if (level == 2)
			biome = level;
		else if (level == 3)
			biome = level;
		return;
	}
	
	public void cpuPress(int level) {
		if (level == 0)
			numCPUs = level;
		else if (level == 1)
			numCPUs = level;
		else if (level == 2)
			numCPUs = level;
		else if (level == 3)
			numCPUs = level;
		return;
	}
	
	public void NoPress () {
		seedMenu = false;
		inSubMenu = false;
		QuitMenu.enabled = false;
		PlayerMenu.enabled = false;
		SeedMenu.enabled = false;
		StartText.enabled = true;
		ExitText.enabled = true;
	}
	
	public void P1Press () { numPlayers = 1; SeedPress (); }
	public void P2Press () { numPlayers = 2; SeedPress (); }
	public void P3Press () { numPlayers = 3; SeedPress (); }
	public void P4Press () { numPlayers = 4; SeedPress (); }
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
