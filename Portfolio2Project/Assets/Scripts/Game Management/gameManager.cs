using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static Skills;
using TMPro;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("------ Player Stuff -----")]

    public GameObject player;
    public PlayerController playerScript;
    public GameObject playerRespawn;
    public GameObject playerSpawn;
    public Skills skillScript;
<<<<<<< HEAD
    /*public GameObject firePlayer;
    public GameObject waterPlayer;
    public GameObject earthPlayer;*/

=======
    NewStaff.Element playerElement;
>>>>>>> branchworks

    [Header("----- UI Stuff -----")]
    public GameObject pauseMenu;
    public GameObject activeMenu;
    public GameObject loseMenu;
    public GameObject winMenu;
    public GameObject settingsMenu;
    public GameObject flashDamage;
    public GameObject inventoryMenu;
    
    [Header("----- Enemy Stuff -----")]
    public int enemiesRemaining;

    [Header("-----Misc Stuff-----")]

    [SerializeField] Slider hpBar;
    [SerializeField] Text hpText;
    [SerializeField] TextMeshProUGUI levelText;
    LevelManager levelManager;
<<<<<<< HEAD
=======
    [SerializeField] int fadeIntensity;
    int currentFade;

>>>>>>> branchworks
    public Image ability1; //Hi-Jump
    public Image ability2; //Dash
    public Image ability3; //Blink
    public Image fadeOutImage;
    public List<Sprite> spriteArray;
    public Image element;
    public bool fadeIn;

    public bool isPaused;
    float timeScaleOrig;
    float waitTime;

    private void Awake()
    {
        instance = this;
        levelManager = LevelManager.instance;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        skillScript = player.GetComponent<Skills>();
<<<<<<< HEAD
        //playerRespawn = GameObject.FindGameObjectWithTag("PlayerRespawn");
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        ResetHpBar();
=======
        ResetHpBar();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        timeScaleOrig = Time.timeScale;
        fadeIn = false;
        currentFade = 0;
    }

    private void Start()
    {
>>>>>>> branchworks
        SetElementIcon();
        enemiesRemaining = 0;
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
        SetElementIcon();
=======
        //StartCoroutine(FadeScreen(fadeIn));

>>>>>>> branchworks
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            activeMenu = pauseMenu;
            showActiveMenu();
            pauseState();
        }
<<<<<<< HEAD
        if (instance.enemiesRemaining <= 0 && levelManager.isInLevel)
        {
            levelManager.levelComplete();
        }
        abilityCooldown();
=======
        AbilityCoolDown();

        if (playerElement != playerScript.playerElement)
        {
            SetElement();
        }
>>>>>>> branchworks
    }

    public void pauseState()
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        flashDamage.SetActive(false);

    }

    public void unPauseState()
    {
        isPaused = false;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        hideActiveMenu();
    }
    public void goBack() //Go back to pause menu
    {
        pauseState();
        settingsMenu.SetActive(false);
        activeMenu = pauseMenu;
        showActiveMenu();
    }

    public void youLose()
    {
        pauseState();
        activeMenu = loseMenu;
        showActiveMenu();
    }

    public void goToSettings() //goes to settings menu
    {
        pauseState();
        activeMenu = settingsMenu;
        showActiveMenu();
    }

    public void showActiveMenu() //shows active menu if there is one.
    {
        if (activeMenu != null)
        {
            activeMenu.SetActive(true);
        }
    }

    public void hideActiveMenu() //hides active menu and sets it to null
    {
        if (activeMenu != null)
        {
            activeMenu.SetActive(false);
            activeMenu = null;
        }
    }

    public void showDamage()
    {
        StartCoroutine(flashRed());
    }

    IEnumerator flashRed()
    {
        flashDamage.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        flashDamage.SetActive(false);
    }
    public void youWin()
    {
        activeMenu = winMenu;
        showActiveMenu();
        pauseState();
    }
 
    //displays the correct element based on character type
    public void SetElementIcon()
    {
        //Debug.Log(playerScript.GetWeapon());
        element.sprite = spriteArray[(int) playerScript.playerElement];
    }

    public void UpdateHealthBar()
    {
        hpBar.maxValue = playerScript.getOriginalHealth();
        hpBar.value = playerScript.getHealth();
        if (playerScript.getHealth() <= 0)
        {
            hpText.text = "HP: 0";
        }
        else
        {
            hpText.text = "HP: " + playerScript.getHealth();
        }
    }
    //update level counter in UI
    public void updateLevelCount()
    {
        int level = levelManager.getlevel();
        levelText.text = level.ToString("F0");
    }
    //cooldownImage
    public void abilityCooldown()
    {
        if(skillScript.isDashCooldown())
        {
            waitTime = skillScript.getCooldown(skill.Dash);
           for(int i = 0; i < waitTime; i++)
            {
                ability2.fillAmount += (1.0f / waitTime) * Time.deltaTime;
            }
        }
           
        else if(skillScript.isJumpCooldown())
        {
            waitTime = skillScript.getCooldown(skill.HiJump);
            for (int i = 0; i < waitTime; i++)
            {
                ability1.fillAmount += (1.0f / waitTime) * Time.deltaTime;
            }
            
        }
        else if(skillScript.isBlinkCooldown())
        {
            waitTime = skillScript.getCooldown(skill.Blink);
            for (int i = 0; i < waitTime; i++)
                ability3.fillAmount += (1.0f / waitTime) * Time.deltaTime;
        }
        else
        {
            ability1.fillAmount = 0;
            ability2.fillAmount = 0;
            ability3.fillAmount = 0;
        }


    }

    public void ResetHpBar()
    {
        //hpBar.maxValue = playerScript.getOriginalHealth();
        //hpBar.value = playerScript.getHealth();
        hpBar.maxValue = 1;
        hpBar.value = 1;
        hpText.text = "HP: " + playerScript.getHealth();
    }

<<<<<<< HEAD
    IEnumerator fadeScreen(bool toFade)
=======
    public IEnumerator FadeScreen(bool toFadeIn)
>>>>>>> branchworks
    {
       if(toFadeIn == true)   //Fade into level
        {
            if (currentFade < 255)
            {
<<<<<<< HEAD
                fadeOutImage.color = new Color(1, 1, 1, i);
                yield return null;
=======
                fadeInFadeOutImage.color = new Color(0, 0, 0, currentFade);
                currentFade += fadeIntensity;
>>>>>>> branchworks
            }
            else
            {
                currentFade = 255;
            }
            yield return null;
        }
        else           //Fade out of level
        {
            if (currentFade > 0)
            {
<<<<<<< HEAD
                fadeOutImage.color = new Color(1, 1, 1, i);
                yield return null;
=======
                fadeInFadeOutImage.color = new Color(0, 0, 0, currentFade);
                currentFade -= fadeIntensity;
>>>>>>> branchworks
            }
            else
            {
                currentFade = 0;
            }
            yield return null;
        }
    }
<<<<<<< HEAD

    //public void StartGame()
    //{
    //    player = GameObject.FindGameObjectWithTag("Player");
    //    //Looking for which element the player has
    //    waterPlayer = GameObject.Find("Water Player");
    //    firePlayer = GameObject.Find("Fire Player");
    //    earthPlayer = GameObject.Find("Earth Player");
    //    timeScaleOrig = Time.timeScale;
    //    playerScript = player.GetComponent<PlayerController>();
    //    playerRespawn = GameObject.FindGameObjectWithTag("PlayerRespawn");
    //    levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
    //    ResetHpBar();
    //    SetElementIcon();
    //    enemiesRemaining = 0;
    //    gameStarted = true;
    //}

    //public void SelectedFire()
    //{
    //    SceneManager.LoadScene("Main Game");
    //    playerSpawn = GameObject.FindGameObjectWithTag("Player Spawn");
    //    playerType = Instantiate(playerTypeFire);
    //    playerType.transform.SetPositionAndRotation(playerSpawn.transform.position, playerSpawn.transform.rotation);
    //    StartGame();
    //}

    //public void SelectedWater()
    //{

    //}

    //public void SelectedEarth()
    //{

    //}
=======
    public void SetElement()
    {
        playerElement = playerScript.playerElement;
    }
>>>>>>> branchworks
}
