using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelect : MonoBehaviour
{
    [SerializeField] GameObject player;
<<<<<<< HEAD:Portfolio2Project/Assets/Scripts/PlayerSelect.cs
    [SerializeField] NewStaff staff;
    public void SelectedFire()
    {
        Instantiate(player);
        staff.ChangeElement("Fire");
        Debug.Log("Selected Fire");
        Debug.Log("PlayerSpawned");
        SceneManager.LoadScene("Main Game");
=======
    [SerializeField] GameObject levelManager;
    [SerializeField] GameObject UI;
    public void SelectedFire()
    {
        PrePlayerElementSetup();
        player.GetComponent<PlayerController>().playerWeaponScript.element = NewStaff.Element.Fire;
        PostPlayerElementSetup();
>>>>>>> abceae162203a3ed79a4796f92c902f46baf60ce:Portfolio2Project/Assets/Scripts/Player Select/PlayerSelect.cs
    }

    public void SelectedWater()
    {
        PrePlayerElementSetup();
        player.GetComponent<PlayerController>().playerWeaponScript.element = NewStaff.Element.Water;
<<<<<<< HEAD:Portfolio2Project/Assets/Scripts/PlayerSelect.cs
        Debug.Log("Selected Water");
        Debug.Log("PlayerSpawned");
        SceneManager.LoadScene("Main Game");
=======
        PostPlayerElementSetup();
>>>>>>> abceae162203a3ed79a4796f92c902f46baf60ce:Portfolio2Project/Assets/Scripts/Player Select/PlayerSelect.cs
    }

    public void SelectedEarth()
    {
<<<<<<< HEAD:Portfolio2Project/Assets/Scripts/PlayerSelect.cs
        Instantiate(player);
        staff.ChangeElement("Earth");
        //player.GetComponent<PlayerController>().playerWeaponScript.element = NewStaff.Element.Earth;
        Debug.Log("Selected Earth");
=======
        PrePlayerElementSetup();
        player.GetComponent<PlayerController>().playerWeaponScript.element = NewStaff.Element.Earth;
        PostPlayerElementSetup();
    }

    public void PrePlayerElementSetup() //must happen before player element setup occurs
    {
        DestroyImmediate(Camera.main.gameObject);
        Instantiate(player);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void PostPlayerElementSetup() //must happen after player element setup occurs
    {
>>>>>>> abceae162203a3ed79a4796f92c902f46baf60ce:Portfolio2Project/Assets/Scripts/Player Select/PlayerSelect.cs
        Debug.Log("PlayerSpawned");
        SceneManager.LoadScene("Reception");
        Instantiate(levelManager);
        Instantiate(UI);
    }
}
