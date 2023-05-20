using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelect : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] NewStaff staff;
    [SerializeField] GameObject levelManager;
    [SerializeField] GameObject UI;
    public void SelectedFire()
    {
        PrePlayerElementSetup();
        staff.ChangeElement("Fire");
        player.GetComponent<PlayerController>().playerWeaponScript.element = NewStaff.Element.Fire;
        PostPlayerElementSetup();
    }

    public void SelectedWater()
    {
        PrePlayerElementSetup();
        //player.GetComponent<PlayerController>().playerWeaponScript.element = NewStaff.Element.Water;
        Debug.Log("Selected Water");
        PostPlayerElementSetup();
    }

    public void SelectedEarth()
    {
        PrePlayerElementSetup();
        staff.ChangeElement("Earth");
        Debug.Log("Selected Earth");
        player.GetComponent<PlayerController>().playerWeaponScript.element = NewStaff.Element.Earth;
        //player.GetComponent<PlayerController>().playerWeaponScript.element = NewStaff.Element.Earth;
        PostPlayerElementSetup();
    }

    public void PrePlayerElementSetup() //must happen before player element setup occurs
    {
        DestroyImmediate(Camera.main.gameObject);
        Instantiate(player);
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Player Spawned");
    }

    public void PostPlayerElementSetup() //must happen after player element setup occurs
    {

        Instantiate(levelManager);
        Debug.Log("Level Manager Created");
        Instantiate(UI);
        Debug.Log("UI Created");
        SceneManager.LoadScene("Reception");
    }
}
