using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("----- LevelIndexes -----")]
    [SerializeField] int repeatableLevelsMinIndex; //list levels contiguously
    [SerializeField] int repeatableLevelsMaxIndex;

    public static LevelManager instance;

    public int currentLevel;
    public int enemiesRemaining;

    public bool inElevator; //player is in elevator
    public bool levelStarted; //player successfully teleported/close enough to spawn
    public bool levelCompleted; //for use by other scripts, makes life easier -> if levelStarted, no enemies, and player in elevator -> load new level

    public bool loadingLevel;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        NewGame();
    }

    private void Update()
    {
        GameManager.instance.UpdateLevelCount();
        if (loadingLevel == false)
        {
            LevelCompletionTracker();
        }

    }
    
    public void NewGame()
    {
        loadingLevel = false;
        levelCompleted = false;
        levelStarted = false;
        currentLevel = 1;
        enemiesRemaining = 0;
        inElevator = false;

    }

    public void NewLevel()
    {
        levelCompleted = false;
        levelStarted = false;
        enemiesRemaining = 0;
        inElevator = false;
    }

    public void LevelCompletionTracker()
    {
        if(loadingLevel == false)
        {
            if (levelStarted == true && enemiesRemaining <= 0) //if level is started and all enemies are dead level is considered completed
            {
                if (levelCompleted == false)
                {
                    Debug.Log("levelStarted True + enemies < 0, level is completed");
                    levelCompleted = true;
                }

                if (inElevator == true)
                {
                    GoToNextLevel();
                }
            }
            else
            {
                levelCompleted = false;
            }
        }
    }
    public void GoToNextLevel() //if levelStarted, no enemies, and player in elevator -> load new level
    {
        NewLevel();
        ++currentLevel; //ups difficulty
        SceneManager.LoadScene(GetRandomLevelIndex()); //loads a new level != the current level index
    }

    public int GetRandomLevelIndex()
    {
        int randomIndex = Random.Range(repeatableLevelsMinIndex, repeatableLevelsMaxIndex + 1);
        while (randomIndex == SceneManager.GetActiveScene().buildIndex)
        {
            randomIndex = Random.Range(repeatableLevelsMinIndex, repeatableLevelsMaxIndex + 1);
            if(randomIndex != SceneManager.GetActiveScene().buildIndex)
            {
                break;
            }
        }
        Debug.Log($"Random Index is {randomIndex}");
        return randomIndex;
    }
}
