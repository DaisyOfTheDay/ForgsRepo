using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("----- Level Indexes -----")]
    [SerializeField] int repeatableLevelsMinIndex; //list repeatable levels contiguously in Build Settings
    [SerializeField] int repeatableLevelsMaxIndex;
    [SerializeField] int hubSceneIndex;
    [SerializeField] int characterSelectIndex;
    [SerializeField] int bossLevelOne;
    [SerializeField] int maxPlayableLevel;

    [Header("----- Spawner Settings -----")]
    [SerializeField] int baseNumberOfEnemiesToSpawn;
    [Range(0f, 1f)] public float numberOfEnemiesScaling;
    [SerializeField] float timeBetweenSpawns;
    public int maxEnemiesAtOneTime;
  

    [Header("----- For Spawners To Know (Ignore)-----")]
    public int currentLevel;
    public int totalEnemiesToSpawn; //total enemies to spawn
    public int enemiesRemaining; //goes up when an enemyAI Start()'s and goes down on enemy death
    public int currentEnemiesSpawned;
    public int currentEnemiesAlive;
    public bool isSpawning;
    private int currentSpawner;
    GameObject[] spawners;

    [Header("----- Level Transition Stuff (Ignore)-----")]
    public bool inElevator; //player is in elevator
    public bool levelLoading;
    public bool levelStarted; //player successfully teleported/close enough to spawn
    public bool levelCompleted; //for use by other scripts, makes life easier -> if levelStarted, no enemies, and player in elevator -> load new level
    //public bool tutorialBeaten;
    public bool endlessMode;//to determine if the player is currently playing Endless Mode

    [Header("----- High Score Stuff (Ignore)-----")]
    public int highestLevelCompleted;
    public int totalEnemiesDefeated;

    private UIManager uiManager;
    private AudioManager audioManager;

    void Awake()//occurs before start
    {
        if (LevelManager.instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        currentSpawner = 0;
        currentLevel = 1;
        totalEnemiesDefeated = 0;
    }
    void Start()//occurs before the first frame
    {
        if(UIManager.instance != null) 
        {
            uiManager = UIManager.instance;        
        }
        if(AudioManager.instance != null)
        {
            audioManager = AudioManager.instance;
        }
        //setting starting variables
        currentLevel = 1;
        levelLoading = false;
        totalEnemiesDefeated = 0;
        endlessMode = false;
        levelStarted = false;
        enemiesRemaining = 0;
        currentEnemiesSpawned = 0;
        inElevator = false;
    }

    void Update()
    {
        if (uiManager != null)
        {
            uiManager.UpdateLevelCount();//make sure we have the right level number displayed
        
        }        

        if (levelLoading == false)
        {
            currentEnemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;
            if (!isSpawning && !levelCompleted && (totalEnemiesToSpawn > currentEnemiesSpawned) && (currentEnemiesAlive < maxEnemiesAtOneTime))
            {
                StartCoroutine(SpawnersSpawn());//if there are more enemies that can be spawned, then begin spawning another
            }
            TrackLevelCompletion();
        }
    }

    public void TrackLevelCompletion()
    {
        if (levelStarted && enemiesRemaining <= 0) //if level is started and all enemies are dead level is considered completed
        {
            levelCompleted = true;
            if (inElevator && !levelLoading)
            {
                levelLoading = true;
                if(currentLevel >= 5)//update the player on how well they're doing after each level after the tutorial
                {
                    uiManager.ShowPostRunStats();
                }
                else
                {
                    LevelTransitionSequence();
                }
            }
        }
        else
        {
            levelCompleted = false;
        }
    }
    public void LevelTransitionSequence() //if levelStarted, no enemies, and player in elevator -> load new level
    {
        StartCoroutine(uiManager.FadeScreen());
        //loads a new level != the current level index
    }

    public int GetRandomLevelIndex()//get a random, repeatable level that isn't the current one
    {
        int randomIndex = Random.Range(repeatableLevelsMinIndex, repeatableLevelsMaxIndex + 1);
        while (randomIndex == SceneManager.GetActiveScene().buildIndex)
        {
            randomIndex = Random.Range(repeatableLevelsMinIndex, repeatableLevelsMaxIndex + 1);
            if (randomIndex != SceneManager.GetActiveScene().buildIndex)
            {
                break;
            }
        }
        return randomIndex;
    }

    void ScaleSpawners() //Scales Number of enemies per level
    {
        int level = currentLevel - 5;
        if (level < 0)
        {
            level = 0;
        }
        totalEnemiesToSpawn = (int)(baseNumberOfEnemiesToSpawn * ((level * numberOfEnemiesScaling) + 1));
    }

    public void LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == hubSceneIndex || SceneManager.GetActiveScene().buildIndex == characterSelectIndex)
        {//chcking if the current scene is a hub or character select scene
            if (currentLevel == bossLevelOne)
            {//if the current level should be the boss
                LoadLevelVariableReset();
                SceneManager.LoadScene("HR");//load the boss level
            }
            else
            {
                LoadLevelVariableReset();
                if (currentLevel == 1)
                {
                    SceneManager.LoadScene("Home");
                }
                else
                {
                    if(SceneManager.GetActiveScene().buildIndex == hubSceneIndex)
                    {
                        SceneManager.LoadScene(GetRandomLevelIndex());
                    }
                    else
                    {
                        SceneManager.LoadScene("HUB");
                    }
                }
            }
        }
        else
        {//if it's a normal level
            if (currentLevel % 3 == 0)
            {
                audioManager.ChangeSong();
            }

            
            if (currentLevel % 5 == 0)
            {
                ++currentLevel; //ups difficulty
                ScaleSpawners();
                LoadLevelVariableReset();
                SceneManager.LoadScene(hubSceneIndex);
            }
            else
            {
                ++currentLevel; //ups difficulty
                if (currentLevel > maxPlayableLevel && !endlessMode)
                {
                    ++highestLevelCompleted;
                    uiManager.ShowEndLetter();
                }
                else
                {
                    ScaleSpawners();
                    if (currentLevel < 6) 
                    {
                        if (currentLevel > highestLevelCompleted)
                        {
                            highestLevelCompleted = currentLevel;
                        }
                        LoadLevelVariableReset();
                        switch (currentLevel)
                        {
                            case 1:
                                {
                                    SceneManager.LoadScene("Home");
                                    break;
                                }
                            case 2:
                                {
                                    SceneManager.LoadScene("UpTheCliffs");
                                    break;
                                }
                            case 3:
                                {
                                    SceneManager.LoadScene("AcrossTheGap");
                                    break;
                                }
                            case 4:
                                {
                                    SceneManager.LoadScene("RoadBlock");
                                    break;
                                }
                            case 5:
                                {
                                    enemiesRemaining = totalEnemiesToSpawn;
                                    SceneManager.LoadScene("Reception");                                    
                                    break;
                                }
                        }
                    }
                    else
                    {
                        LoadLevelVariableReset();
                        enemiesRemaining = totalEnemiesToSpawn;
                        if (currentLevel > highestLevelCompleted)
                        {
                            highestLevelCompleted = currentLevel;
                        }
                        SceneManager.LoadScene(GetRandomLevelIndex());
                    }
                }                
            }
        }
    }

    IEnumerator SpawnersSpawn()
    {
        isSpawning = true;

        spawners = GameObject.FindGameObjectsWithTag("Spawner");
        if(spawners != null && spawners.Length > 0)
        {
            if(currentSpawner >= spawners.Length)
            {
                currentSpawner = 0;
            }
            spawners[currentSpawner].GetComponent<EnemySpawner>().SpawnEnemies();
        }

        yield return new WaitForSeconds(timeBetweenSpawns);
        ++currentSpawner;
        isSpawning = false;
    }

    public void SetCurrentLevel(int levelToSetTo)
    {
        currentLevel = levelToSetTo;
        ScaleSpawners();
    }

    private void LoadLevelVariableReset()
    {
        enemiesRemaining = 0;
        currentEnemiesSpawned = 0;
        inElevator = false;
        levelLoading = false;
        levelStarted = false;
        levelCompleted = false;   
        StopAllCoroutines();
    }

    public void TutorialBeatenGoToLevelSix()
    {
        if (highestLevelCompleted >= 5)
        {
            currentLevel = 6;
        }
        else
        {
            currentLevel = 1;
        }
    }
}
