using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

	public Canvas Logo, QuitMenu, PlayerMenu, SeedMenu;
	public Button StartText, ExitText;
	public Button one, two, three, four, back;
	public Button yes, no;
	public Button rLow, rMed, rHigh, rUltra, fLow, fMed, fHigh, fUltra,
	        fhLow, fhMed, fhHigh, fhUltra, msLow, msMed, msHigh, msUltra,
			lava, lunar, terra, snow, oneAI, twoAI, threeAI, fourAI, save;
	
	private int roughness, flatness, flatHeight, mountainSize, biome, numCPUs;
	private int numPlayers = 0;
	private int index = 0;
	private int subIndex = 0;
	public float logoDisplay;
	private Button[] startOptions, quitOptions, playerOptions;
	
	private Button[] roughOptions, flatOptions, flatHeightOptions,
        mountSizeOptions, biomeOptions, cpuOptions;

	void Awake() {
		if (PlayerPrefs.HasKey("roughness"))
			roughness = PlayerPrefs.GetInt("roughness");
		if (PlayerPrefs.HasKey("flatness"))
			flatness = PlayerPrefs.GetInt("flatness");
		if (PlayerPrefs.HasKey("flatHeight"))
			flatHeight = PlayerPrefs.GetInt("flatHeight");
		if (PlayerPrefs.HasKey("mountainSize"))
			mountainSize = PlayerPrefs.GetInt("mountainSize");
		if (PlayerPrefs.HasKey("biome"))
			biome = PlayerPrefs.GetInt("biome");
		if (PlayerPrefs.HasKey("numCPUs"))
			numCPUs = PlayerPrefs.GetInt("numCPUs");
	}
		
	void Start () {
		logoDisplay = 3.0f;
		Logo = Logo.GetComponent<Canvas>();
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
		// cpuOptions        = new Button[4] { oneAI, twoAI, threeAI, fourAI};
	}
	
	
	bool upButtonPushed = false;
	bool sideButtonPushed = false;
	bool inSubMenu = false;
	bool seedMenu = false;
	void FixedUpdate() {
		if(logoDisplay>=0){
			logoDisplay -= Time.deltaTime;
		}
		else{
			Logo.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
		}
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
				if ((index == 0) && roughness+1 < roughOptions.Length) {
					roughOptions[roughness++].GetComponent<Text>().color = Color.white;
					sideButtonPushed = true;
				} else if ((index == 1) && flatness+1 < flatOptions.Length) {
					flatOptions[flatness++].GetComponent<Text>().color = Color.white;
					sideButtonPushed = true;
				} else if ((index == 2) && flatHeight+1 < flatHeightOptions.Length) {
					flatHeightOptions[flatHeight++].GetComponent<Text>().color = Color.white;
					sideButtonPushed = true;
				} else if ((index == 3) && mountainSize+1 < mountSizeOptions.Length) {
					mountSizeOptions[mountainSize++].GetComponent<Text>().color = Color.white;
					sideButtonPushed = true;
				} else if ((index == 4) && biome+1 < biomeOptions.Length) {
					biomeOptions[biome++].GetComponent<Text>().color = Color.white;
					sideButtonPushed = true;
				// } else if ((index == 5) && numCPUs+1 < cpuOptions.Length) {
				// 	cpuOptions[numCPUs++].GetComponent<Text>().color = Color.white;
				// 	sideButtonPushed = true;
				} 
			} else if (Input.GetAxis("MenuLR") < 0 && !sideButtonPushed) {
				if ((index == 0) && roughness-1 >= 0) {
					roughOptions[roughness--].GetComponent<Text>().color = Color.white;
					sideButtonPushed = true;
				} else if ((index == 1) && flatness-1 >= 0) {
					flatOptions[flatness--].GetComponent<Text>().color = Color.white;
					sideButtonPushed = true;
				} else if ((index == 2) && flatHeight-1 > 0) {
					flatHeightOptions[flatHeight--].GetComponent<Text>().color = Color.white;
					sideButtonPushed = true;
				} else if ((index == 3) && mountainSize-1 >= 0) {
					mountSizeOptions[mountainSize--].GetComponent<Text>().color = Color.white;
					sideButtonPushed = true;
				} else if ((index == 4) && biome-1 >= 0) {
					biomeOptions[biome--].GetComponent<Text>().color = Color.white;
					sideButtonPushed = true;
				// } else if ((index == 5) && numCPUs-1 >= 0) {
				// 	cpuOptions[numCPUs--].GetComponent<Text>().color = Color.white;
				// 	sideButtonPushed = true;
				}  
			} else if (Input.GetAxis("MenuLR") == 0) { sideButtonPushed = false; }
			
			if (Input.GetAxis("MenuUD") < 0 && !upButtonPushed && index+1<=6) {
				index++;
				upButtonPushed = true;
			} else if (Input.GetAxis("MenuUD") > 0 && !upButtonPushed && index-1>=0) {
				index--;
				upButtonPushed = true;
			} else if (Input.GetAxis("MenuUD") == 0) { upButtonPushed = false; }
			
			if (index == 5)
				save.GetComponent<Text>().color = Color.red;
			else 
				save.GetComponent<Text>().color = Color.white;
			
			roughOptions[roughness].GetComponent<Text>().color = Color.red;
			flatOptions[flatness].GetComponent<Text>().color = Color.red;
			flatHeightOptions[flatHeight].GetComponent<Text>().color = Color.red;
			mountSizeOptions[mountainSize].GetComponent<Text>().color = Color.red;
			biomeOptions[biome].GetComponent<Text>().color = Color.red;
			// cpuOptions[numCPUs].GetComponent<Text>().color = Color.red;
			
			if (Input.GetButton("Submit") && index >= 5)
			{	
				Debug.Log("Starting");
				StartLevel();
			}
			else if (Input.GetButton("Cancel")) { index = 0; NoPress(); }
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
		PlayerPrefs.SetInt("roughness", roughness);
		PlayerPrefs.SetInt("flatness", flatness);
		PlayerPrefs.SetInt("flatHeight", flatHeight);
		PlayerPrefs.SetInt("mountainSize", mountainSize);
		PlayerPrefs.SetInt("biome", biome);
		PlayerPrefs.SetInt("numCPUs", numCPUs);
		Debug.Log("Started");
		if (numPlayers == 1)
			SceneManager.LoadScene ("1 Player");
		// else if (numPlayers == 2)
			// SceneManager.LoadScene ("2P_Kart_Selection");
		// else if (numPlayers == 3)
			// SceneManager.LoadScene ("3P_Kart_Selection");
		else if (numPlayers == 4)
			SceneManager.LoadScene ("4 Player");
		// else
			// Debug.Log("GAME DOES NOT SUPPORT THAT NUMBER OF PLAYERS!!");
	}
	
	public void ExitGame () {
		Application.Quit();
	}
}
