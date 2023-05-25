using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        //fileManager.save();
       // fileManager.load();
    }
    public void PlayGame() //Takes player to character select scene
    {
        //Debug.Log("Play Button Pressed");
        SceneManager.LoadScene("Character Select");
    }

    public void QuitGame()
    {
        //Debug.Log("Quit Button Pressed");
        Application.Quit();
    }
}
