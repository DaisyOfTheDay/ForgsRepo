using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelect : MonoBehaviour
{
    [Header("----- Element Type Player Prefabs -----")]
    [SerializeField] GameObject firePlayer;
    [SerializeField] GameObject waterPlayer;
    [SerializeField] GameObject earthPlayer;

    [Header("----- Other Necessary Objects -----")]
    [SerializeField] GameObject levelManager;
    [SerializeField] GameObject UI;

    [Header("----- Do Not Fill/Filled By Script -----")]
    public GameObject player;
    public void SelectedFire()
    {
        PrePlayerElementSetup();
        Instantiate(firePlayer);
        Debug.Log("Selected Fire");
        PostPlayerElementSetup();
    }

    public void SelectedWater()
    {
        PrePlayerElementSetup();
        Instantiate(waterPlayer);
        Debug.Log("Selected Water");
        PostPlayerElementSetup();
    }

    public void SelectedEarth()
    {
        PrePlayerElementSetup();
        Instantiate(earthPlayer);
        Debug.Log("Selected Earth");
        PostPlayerElementSetup();
    }

    public void PrePlayerElementSetup() //must happen before player element setup occurs
    {
        DestroyImmediate(Camera.main.gameObject);
    }

    public void PostPlayerElementSetup() //must happen after player element setup occurs
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Player Spawned");
        Instantiate(levelManager);
        Debug.Log("Level Manager Created");
        Instantiate(UI);
        Debug.Log("UI Created");
        SceneManager.LoadScene("Reception");
    }
}
