using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    //Resume the game
   public void resume()
    {
        gameManager.instance.unPauseState();
    }

    //Restarts the level from the beginning
    public void restart()
    {
<<<<<<< HEAD
        Destroy(gameManager.instance.player);
=======
        //LevelManager.instance = null;
        //GameManager.instance = null;

        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Destroy(GameObject.FindGameObjectWithTag("LevelManager"));
        Destroy(GameObject.FindGameObjectWithTag("UI"));        
        Debug.Log("Player Character destroyed");

        GameManager.instance.UnpauseState();
>>>>>>> branchworks
        SceneManager.LoadScene("Character Select");
    }

    //Quits the game; doesn't work unless built
    public void Quit()
    {
        Application.Quit();
    }

    //Respawn player from respawn location
    public void respawnPLayer()
    {
        gameManager.instance.playerScript.spawnPlayer();
        gameManager.instance.unPauseState();
    }

    public void goBackMenu()
    {
        gameManager.instance.goBack();
    }
   
    //Go to next level; doesn't work until next level is made
    public void nextLevel()
    {

    }
}
