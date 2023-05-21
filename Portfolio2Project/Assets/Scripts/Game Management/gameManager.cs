using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static Skills;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("------ Player Stuff -----")]

    public GameObject player;
    public PlayerController playerScript;
    public GameObject playerSpawn;
    public Skills skillScript;
    NewStaff.Element playerElement;

    [Header("----- UI Stuff -----")]
    public GameObject pauseMenu;
    public GameObject activeMenu;
    public GameObject loseMenu;
    public GameObject winMenu;
    public GameObject settingsMenu;
    public GameObject flashDamage;
    public GameObject inventoryMenu;
    public Image fadeInFadeOutImage;
    public Image playerHealthBar;
    public TextMeshProUGUI levelText;

    [Header("-----Misc Stuff-----")]

    LevelManager levelManager;
    [SerializeField] int fadeIntensity;
    int currentFade;

    public Image ability1; //Hi-Jump
    public Image ability2; //Dash
    public Image ability3; //Blink
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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        timeScaleOrig = Time.timeScale;
        fadeIn = false;
        currentFade = 0;
    }

    private void Start()
    {
        SetElementIcon();
    }

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine(FadeScreen(fadeIn));

        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            activeMenu = pauseMenu;
            ShowActiveMenu();
            PauseState();
        }
        AbilityCoolDown();

        if (playerElement != playerScript.playerElement)
        {
            SetElement();
        }
    }

    public void PauseState()
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        flashDamage.SetActive(false);

    }

    public void UnpauseState()
    {
        isPaused = false;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        HideActiveMenu();
    }
    public void GoBack() //Go back to pause menu
    {
        PauseState();
        settingsMenu.SetActive(false);
        activeMenu = pauseMenu;
        ShowActiveMenu();
    }

    public void YouLose()
    {
        PauseState();
        activeMenu = loseMenu;
        ShowActiveMenu();
    }

    public void GoToSettings() //goes to settings menu
    {
        PauseState();
        activeMenu = settingsMenu;
        ShowActiveMenu();
    }

    public void ShowActiveMenu() //shows active menu if there is one.
    {
        if (activeMenu != null)
        {
            activeMenu.SetActive(true);
        }
    }

    public void HideActiveMenu() //hides active menu and sets it to null
    {
        if (activeMenu != null)
        {
            activeMenu.SetActive(false);
            activeMenu = null;
        }
    }

    public void ShowDamage()
    {
        StartCoroutine(FlashRed());
    }

    IEnumerator FlashRed()
    {
        flashDamage.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        flashDamage.SetActive(false);
    }
    public void YouWin()
    {
        activeMenu = winMenu;
        ShowActiveMenu();
        PauseState();
    }
 
    //displays the correct element based on character type
    public void SetElementIcon()
    {
        //Debug.Log(playerScript.GetWeapon());
        element.sprite = spriteArray[(int) playerScript.playerElement];
    }


    //update level counter in UI
    public void UpdateLevelCount()
    {
        int level = levelManager.currentLevel;
        levelText.text = level.ToString("F0");
    }
    //cooldownImage
    public void AbilityCoolDown()
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

    public IEnumerator FadeScreen(bool toFadeIn)
    {
       if(toFadeIn == true)   //Fade into level
        {
            if (currentFade < 255)
            {
                fadeInFadeOutImage.color = new Color(0, 0, 0, currentFade);
                currentFade += fadeIntensity;
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
                fadeInFadeOutImage.color = new Color(0, 0, 0, currentFade);
                currentFade -= fadeIntensity;
            }
            else
            {
                currentFade = 0;
            }
            yield return null;
        }
    }
    public void SetElement()
    {
        playerElement = playerScript.playerElement;
    }
}
