using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [Header("Main Menu Element")]
    public string[] menuVoice = {"START GAME","SKIN","OPTIONS","EXIT"};
    public string[] resolutionVoice = {"1024x576","1280x720","1920x1080"};

    [Header("Skin Shop")]
    public Image      currImage;
    public Sprite[]   skinImages;
    public int   []   CostSkin;
    public bool  []   IsAcquired;
    public string[]   skinname;
    public RuntimeAnimatorController[] PlayerAnimators;

    [Header("UI Menu Element")]
    public Text level_Text;
    public Text score_text;
    public Text speed_text;
    public Text egg_text;
    public Text menu_text;
    public Text cost_skin_text;
    public Text SkinText;
    public Text ResolutionText;
    public GameObject panelSkin;
    public GameObject panelSetting;
    public GameObject MenuPanel;
    public GameObject GameOverPanel;

    [Header("Game Element")]
    public GameObject player;
    public GameObject[] gamePlayElement;
    public string[] SPD = {"LOW","HIGH"};
    public int   Level       = 1;
    public int   Egg         = 0;
    public float Score       = 0f;
    public float WaitingTime = 1f;
    public bool  endGame     = true;

    [Header("Audio Element")]
    public Sprite[] audioSprite;
    public Image audioimg;
    public bool  bgon     = true;
    public AudioSource bginput;
    public AudioSource audioinput;

    int currentIndex     = 0, currentIndexSkin = 0, currentIndexResolution = 0;
    bool skinmenu        = false;

    public void Awake()
    {
        ConfirmResolution(); LoadGame();
      this.GetComponent<PlayerMover>().enabled = false;   
    }

    public void Update()
    {
        level_Text.text = "" + Level;
        score_text.text = "" + Score;
        speed_text.text = "" + SPD[0];
        egg_text  .text = "" + Egg.ToString("00");

        if(menu_text.text == "CONFIRM&BACK") ResolutionText.text = resolutionVoice[currentIndexResolution];
        if(menu_text.text == "BUY&BACK" || menu_text.text == "USE&BACK") UpdateSkin();
        if(menu_text.text != "BUY&BACK" && menu_text.text != "USE&BACK" && menu_text.text != "CONFIRM&BACK" && menu_text.text != "ENDGAME" && menu_text.text != "INGAME") menu_text.text  = menuVoice[currentIndex];

        if (Input.GetKeyDown(KeyCode.A)) Left();
        if (Input.GetKeyDown(KeyCode.D)) Right();
        if (Input.GetKeyDown(KeyCode.Return)) Action();

        Debug.Log(menu_text.text);
    }

    public void ResetGame()
    {
        Level = 1; Score = 0f;WaitingTime = 1f;
    }

    public void IncrementEgg(int increment)
    {
        Egg += increment;
        SaveGame();
    }

    public void IncrementScore(int increment)
    {
        Score += increment;
        SaveGame(); 
        if (Score % 25 == 0)
         IncrementLevel();
    }

    public void IncrementLevel()
    {
        Level +=1; DecrementWaitingTime();
    }

    public void DecrementWaitingTime()
    {
        WaitingTime -= 0.1f;
        if (WaitingTime < 0.4f) WaitingTime -= 0.005f;
        else if (WaitingTime < 0.3f) WaitingTime = 0.35f;
    }

    //FUNZIONE DESTRA
     public void Right()
    {
       audioinput.Play();
       switch (menu_text.text)
       {
         case "CONFIRM&BACK":
            currentIndexResolution = (currentIndexResolution + 1) % resolutionVoice.Length; 
         break;

         case "BUY&BACK":
            currentIndexSkin = (currentIndexSkin + 1) % skinImages.Length;
            UpdateSkin();
         break;

         case "USE&BACK":
            currentIndexSkin = (currentIndexSkin + 1) % skinImages.Length;
            UpdateSkin();
         break;

         default:
            currentIndex = (currentIndex + 1) % menuVoice.Length;
         break;
       }
    }

    //FUNZIONE SINISTRA
    public void Left()
    {
        audioinput.Play();
       switch (menu_text.text)
       {
         case "CONFIRM&BACK":
           currentIndexResolution = (currentIndexResolution - 1 + resolutionVoice.Length) % resolutionVoice.Length;
         break;

         case "BUY&BACK":
            currentIndexSkin = (currentIndexSkin - 1 + skinImages.Length) % skinImages.Length; UpdateSkin();
         break;

         case "USE&BACK":
             currentIndexSkin = (currentIndexSkin - 1 + skinImages.Length) % skinImages.Length; UpdateSkin();
         break;

         default:
            currentIndex = (currentIndex - 1 + menuVoice.Length) % menuVoice.Length;
         break;
       } 
    }

    //FUNZIONE CHIAMATA AL TASTO INVIO 
    public void Action()
    {
        audioinput.Play();

        switch(menu_text.text)
        {
            case "START GAME":
                StartGame();
            break;

            case "SKIN":
              panelSkin.SetActive(true);
              menu_text.text = "BUY&BACK";
            break;

            case "OPTIONS":
               panelSetting.SetActive(true);
               menu_text.text = "CONFIRM&BACK";
            break;

            case "BUY&BACK":
              panelSkin .SetActive(false);
              menu_text.text = "SKIN";
              BuySkin();
            break;

            case "USE&BACK":
              panelSkin .SetActive(false);
              menu_text.text = "SKIN";
              player.GetComponent<Animator>().runtimeAnimatorController = PlayerAnimators[currentIndexSkin];
              //UseSkin();
            break;

            case "CONFIRM&BACK":
                panelSetting.SetActive(false);
                menu_text.text = "OPTIONS";
                ConfirmResolution();
            break;

            case "EXIT":
              Application.Quit();
            break;

            case "ENDGAME":
                EndGame();
            break;

            default:
            break;
        }
    }
    
    public void BgSet()
    {
        if (bgon == true) 
        {
            bgon = false;
            bginput.Stop();
            audioimg.sprite =  audioSprite[1];

        } else if (bgon == false)
        {
            bgon = true;
            bginput.Play();
            audioimg.sprite =  audioSprite[0];
        }
        
    }

    //FUNZIONE CHE AGGIORNA TUTTO IL REPARTO SKINBOX
    private void UpdateSkin()
    {
        if (currImage != null)
        {
            currImage.sprite =  skinImages[currentIndexSkin];
            SkinText.text = "" + skinname[currentIndexSkin];
            cost_skin_text.text = "COST x " + CostSkin  [currentIndexSkin];
            if (IsAcquired[currentIndexSkin]) 
            {
                menu_text.text = "USE&BACK";
                currImage.color = Color.white;
            }
            else
            {
                menu_text.text = "BUY&BACK";
                currImage.color = Color.grey;
            }
        }
    }

    //FUNZIONE CONFERMA RISOLUZIONE GIOCO
    public void ConfirmResolution()
    {
        string[] dimensions = resolutionVoice[currentIndexResolution].Split('x');
        int width  = int.Parse(dimensions[0]); int height = int.Parse(dimensions[1]);    
        Screen.SetResolution(width, height, false);
        //Debug.Log($"Width: {width}, Height: {height}");
    }

    //FUNZIONE COMPRA SKIN
    public void BuySkin()
    {
        if(Egg >= CostSkin[currentIndexSkin] && !IsAcquired[currentIndexSkin]) 
        {
            Egg -= CostSkin[currentIndexSkin];
            IsAcquired[currentIndexSkin] = true;
            SaveGame();
            //Debug.Log("BUYED");
        }
    }

    //FUNZIONE CHE INIZIA IL GIOCO - ABILITA SCRIPT GIOCATORE SPAWN E DISABILITA LAYOUT MENU
    public void StartGame()
    {
        MenuPanel.SetActive(false);
        endGame = false;
        this.GetComponent<PlayerMover>().enabled = true;
        menu_text.text = "INGAME";
        for(int i = 0; i < gamePlayElement.Length; i++)  gamePlayElement[i].SetActive(true);
    }

    //FUNZIONE CHE FINISCE IL GIOCO, FERMANDO LO SCRIPT PLAYER E LO SPAWN DANDO IL GAMEOVER
    public void GameOver()
    {
        DestroyObjectsByTags();
        ResetGame();
        this.GetComponent<PlayerMover>().enabled = false;
        endGame = true;
        GameOverPanel.SetActive(true);
        menu_text.text = "ENDGAME";
    }

    //FUNZION CHE SEGNA LA FINE DEL GIOCO - ABILITA IL LAYOUT MAIN MENU
    public void EndGame()
    {
        //currentIndex = 0;
        MenuPanel.SetActive(true);
        panelSkin.SetActive(false);
        panelSetting.SetActive(false);
        GameOverPanel.SetActive(false);
        menu_text.text = "START GAME";
        for(int i = 0; i < gamePlayElement.Length; i++)  gamePlayElement[i].SetActive(false);       
    }

    //FUNZIONE CHETrova tutti gli oggetti con il tag "element" e "PlatformElement"
    public void DestroyObjectsByTags()
    {
        GameObject[] elements = GameObject.FindGameObjectsWithTag("element");
        GameObject[] platformElements = GameObject.FindGameObjectsWithTag("PlatformElement");

        // Distruggi tutti gli oggetti trovati
        foreach (GameObject obj in elements) Destroy(obj);
        foreach (GameObject obj in platformElements) Destroy(obj);   
    }

    // Salvataggio dei dati
    public void SaveGame()
    {
        PlayerPrefs.SetInt("Egg", Egg);
        //PlayerPrefs.SetFloat("Score", Score);
        PlayerPrefs.Save();

        string isAcquiredString = "";
        foreach (bool acquired in IsAcquired) isAcquiredString += acquired ? "1" : "0";
        PlayerPrefs.SetString("IsAcquired", isAcquiredString);
    }

    // Caricamento dei dati
    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("Egg"))
        {
            Egg = PlayerPrefs.GetInt("Egg");
            //Debug.Log("Dati caricati Uova : " + Egg);
        }

        /* if (PlayerPrefs.HasKey("Score"))
        {
            Score = PlayerPrefs.GetFloat("Score");
            //Debug.Log("Dati caricati Score : " + Score);
        }
 */
        if (PlayerPrefs.HasKey("IsAcquired"))
        {
            string isAcquiredString = PlayerPrefs.GetString("IsAcquired");
            for (int i = 0; i < isAcquiredString.Length && i < IsAcquired.Length; i++) IsAcquired[i] = isAcquiredString[i] == '1';
        }
    }


}
