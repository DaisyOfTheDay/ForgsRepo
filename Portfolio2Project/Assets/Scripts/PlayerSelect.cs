using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelect : MonoBehaviour
{
    //Animator anim = player.GetComponent<PlayerController>().playerWeaponScript.GetComponent<Animator>();
    [SerializeField] GameObject player;
    [SerializeField] NewStaff staff;
    public void SelectedFire()
    {
        Instantiate(player);
        player.GetComponent<PlayerController>().playerWeaponScript.element = NewStaff.Element.Fire;
        gameManager.instance.SetElementIcon();
        Debug.Log("PlayerSpawned");
        SceneManager.LoadScene("Main Game");
    }

    public void SelectedWater()
    {
        Instantiate(player);
        player.GetComponent<PlayerController>().playerWeaponScript.element = NewStaff.Element.Water;
        staff.GetComponent<Animator>().SetTrigger("SpearMelee");
        gameManager.instance.SetElementIcon();
        Debug.Log("PlayerSpawned");
        SceneManager.LoadScene("Main Game");
    }

    public void SelectedEarth()
    {
        Instantiate(player);
        player.GetComponent<PlayerController>().playerWeaponScript.element = NewStaff.Element.Earth;
        gameManager.instance.SetElementIcon();
        Debug.Log("PlayerSpawned");
        SceneManager.LoadScene("Main Game");
    }
}
