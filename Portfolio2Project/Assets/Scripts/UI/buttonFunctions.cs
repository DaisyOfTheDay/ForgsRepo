using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    [SerializeField] GameObject playerPrefabToSpawn;
    public AudioSource buttonAudio;

    private GameObject player;

    private UIManager uiManager;

    PlayerController playerController;

    private void Start()
    {
        uiManager = UIManager.instance;
    }

    //Resume the game
    public void Resume()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.volumeScale);
        gameManager.instance.UnpauseState();
    }

    //Restarts the level from the beginning
    public void Restart()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.volumeScale);
        Debug.Log("Player Character destroyed");

        uiManager.HideActiveMenu();
        uiManager.HUD.SetActive(false);
        uiManager.activeMenu = uiManager.playerSelect;

        LevelManager.instance.totalEnemiesDefeated = 0;

        AudioManager.instance.currSong = 1;
        AudioManager.instance.PlaySong();
        uiManager.ShowActiveMenu();
        Time.timeScale = gameManager.instance.timeScaleOriginal;

        Destroy(GameObject.FindGameObjectWithTag("Player"));
        LevelManager.instance.endlessMode = false;
        SceneManager.LoadScene("Character Select");
    }

    //Quits the game; doesn't work unless built
    public void QuitCheck()
    {
        //buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.volumeScale);
        AudioManager.instance.MenuTransition();
        uiManager.quitCheckMenu.SetActive(true);
    }

    public void GoBackMenu()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.volumeScale);
        uiManager.settingsMenu.SetActive(false);
       
    }
    public void GoToSettings() //goes to settings menu
    {
        AudioManager.instance.MenuTransition();
        uiManager.settingsMenu.SetActive(true);
    }

    public void GoToCredits()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.volumeScale);

        uiManager.creditsMenu.SetActive(true);
    }
    //Go back to Main Menu
    public void GoToMainMenu()
    {
        AudioManager.instance.MenuTransition();
        uiManager.saveMenu.SetActive(true);
        
        LevelManager.instance.endlessMode = false;
    }

    public void MainFromChar()
    {
        AudioManager.instance.MenuTransition();
        uiManager.HideActiveMenu();
        uiManager.playerSelect.SetActive(false);
        uiManager.activeMenu = uiManager.mainMenu;
        uiManager.ShowActiveMenu();
        SceneManager.LoadScene("Main Menu");
    }

    //If they want to save their game
    public void YesSave()
    {
        AudioManager.instance.MenuTransition();
        fileManager.save();
        uiManager.saveMenu.SetActive(false);
        uiManager.HideActiveMenu();
        uiManager.HUD.SetActive(false);
        uiManager.activeMenu = uiManager.mainMenu;
        AudioManager.instance.currSong = 0;
        AudioManager.instance.PlaySong();
        uiManager.ShowActiveMenu();

        LevelManager.instance.totalEnemiesDefeated = 0;

        Debug.Log("Player Character destroyed");
        Time.timeScale = gameManager.instance.timeScaleOriginal;

        Destroy(GameObject.FindGameObjectWithTag("Player"));
        SceneManager.LoadScene("Main Menu");
    }

    //If they don't want to save their game
    public void NoSave()
    {
        AudioManager.instance.MenuTransition();
        fileManager.resetData();
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        uiManager.saveMenu.SetActive(false);
        uiManager.HideActiveMenu();
        uiManager.HUD.SetActive(false);
        uiManager.activeMenu = uiManager.mainMenu;
        AudioManager.instance.currSong = 0;
        AudioManager.instance.PlaySong();
        uiManager.ShowActiveMenu();

        LevelManager.instance.totalEnemiesDefeated = 0;

        Debug.Log("Player Character destroyed");
        Time.timeScale = gameManager.instance.timeScaleOriginal;

        if (LevelManager.instance != null)
        {
            LevelManager.instance.TutorialBeatenGoToLevelSix();
        }

        SceneManager.LoadScene("Main Menu");
    }
    public void ExitSave()
    {

        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.volumeScale);
        uiManager.saveMenu.SetActive(false);
    }
    public void PlaySaveGame() //Takes player to character select scene
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.volumeScale);

        //Debug.Log("Play Button Pressed");
        uiManager.HideActiveMenu();
        uiManager.activeMenu = uiManager.playerSelect;
        AudioManager.instance.ChangeSong();
        uiManager.ShowActiveMenu();
        //LevelManager.instance.currentLevel = fileManager.level;
        SceneManager.LoadScene("Character Select");
    }

    public void PlayNewGame() //Takes player to character select scene
    {

        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.volumeScale);
        fileManager.resetData();
        //Debug.Log("Play Button Pressed");
        uiManager.HideActiveMenu();
        uiManager.activeMenu = uiManager.playerSelect;
        AudioManager.instance.ChangeSong();
        uiManager.ShowActiveMenu();
        //LevelManager.instance.tutorialBeaten = false;
        LevelManager.instance.highestLevelCompleted = 0;
        LevelManager.instance.totalEnemiesDefeated = 0;

        SceneManager.LoadScene("Character Select");
    }

    public void SelectedFire()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.switchStaffAud, AudioManager.instance.volumeScale);

        PrePlayerElementSetup();
        playerController.playerElement = NewStaff.Element.Fire;
        PostPlayerElementSetup();
    }

    public void SelectedWater()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.switchStaffAud, AudioManager.instance.volumeScale);
        PrePlayerElementSetup();
        playerController.playerElement = NewStaff.Element.Water;
        PostPlayerElementSetup();
    }

    public void SelectedEarth()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.switchStaffAud, AudioManager.instance.volumeScale);

        PrePlayerElementSetup();
        playerController.playerElement = NewStaff.Element.Earth;
        PostPlayerElementSetup();
    }

    public void PrePlayerElementSetup() //must happen before player element setup occurs
    {
        DestroyImmediate(Camera.main.gameObject);
        uiManager.HideActiveMenu();
        uiManager.HUD.SetActive(true);
        player = Instantiate(playerPrefabToSpawn);
        //Debug.Log("Player Spawned");
        playerController = player.GetComponent<PlayerController>();
        //Debug.Log("Player Controller Set");
    }

    public void PostPlayerElementSetup() //must happen after player element setup occurs
    {
        //Debug.Log("Player Element Set");
        AudioManager.instance.ChangeSong();
        gameManager.instance.isPaused = false;
        if(LevelManager.instance != null)
        {
            LevelManager.instance.TutorialBeatenGoToLevelSix();
            LevelManager.instance.LoadNextLevel();
        }
        
    }

    public void EndLetterOKButton()
    {
        uiManager.HideActiveMenu();
        uiManager.ShowPostRunStats();
    }

    public void YesQuit()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.volumeScale);

        Application.Quit();
    }

    public void NoQuit()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.volumeScale);
        uiManager.quitCheckMenu.SetActive(false);
    }

    public void Continue()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.volumeScale);
        gameManager.instance.UnpauseState();
        
    }

    public void GamePlayRecapOKButton()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.volumeScale);
        if (uiManager.loseMenu.activeSelf) //if you lose
        {
            uiManager.gamePlayRecap.SetActive(false);
        }
        else if(LevelManager.instance.currentLevel == 22) //if you win
        {
            uiManager.gamePlayRecap.SetActive(false);
            uiManager.creditsMenu.SetActive(true);        }
        else //after each level
        {
            uiManager.gamePlayRecap.SetActive(false);
            gameManager.instance.UnpauseState();
            LevelManager.instance.LevelTransitionSequence();
        }
        
    }
}
