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
    public void SelectedFire()
    {
        Instantiate(player);
        staff.ChangeElement("Fire");
        Debug.Log("Selected Fire");
        Debug.Log("PlayerSpawned");
        SceneManager.LoadScene("Main Game");
    }

    public void SelectedWater()
    {
        Instantiate(player);
        player.GetComponent<PlayerController>().playerWeaponScript.element = NewStaff.Element.Water;
        Debug.Log("Selected Water");
        Debug.Log("PlayerSpawned");
        SceneManager.LoadScene("Main Game");
    }

    public void SelectedEarth()
    {
        Instantiate(player);
        staff.ChangeElement("Earth");
        //player.GetComponent<PlayerController>().playerWeaponScript.element = NewStaff.Element.Earth;
        Debug.Log("Selected Earth");
        Debug.Log("PlayerSpawned");
        SceneManager.LoadScene("Main Game");
    }
}
