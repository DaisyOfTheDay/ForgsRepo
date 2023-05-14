using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("-----Components-----")]
    [SerializeField] Animator doorAnim;
    [SerializeField] GameObject doorLight;


    [Header("-----Levels------")]
    [SerializeField] Transform tutorialLevel;
    [SerializeField] Transform[] levelPrefabs;

    [Header("-----Misc------")]
    [SerializeField] Material lightOnMat;
    public bool isInLevel;
    public int level = 0;

    Material lightOffMat;
    bool levelIsComplete;
    bool inElevator;
    Transform currLevel;

    // Start is called before the first frame update
    void Start()
    {
        lightOffMat = doorLight.GetComponent<MeshRenderer>().material;
    }



    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && !inElevator) 
        {
            Debug.Log("Player is in Elevator");
            inElevator= true;
            Debug.Log(other.name);
            StartCoroutine(nextLevelCoroutine());
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (levelIsComplete)
            {
                StopCoroutine(nextLevelCoroutine());
                inElevator = false;
            }
            else
            {
                if (doorAnim != null)
                {
                    doorAnim.SetBool("Open", false);
                }
            }
        }


    }

    public void levelComplete()
    {
        if (!levelIsComplete)
        {
            doorLight.GetComponent<MeshRenderer>().material = lightOnMat;
            levelIsComplete = true;
            if (doorAnim != null)
            {
                Debug.Log("OpeningDoor");
                doorAnim.SetBool("Open", true);
            }
        }
    }

    IEnumerator nextLevelCoroutine()
    {
        isInLevel = false;
        Debug.Log("Next level!");
        yield return new WaitForSeconds(2);
        if (doorAnim != null)
        {
            doorAnim.SetBool("Open", false);
        }
        if (level == 0)
        {
            currLevel = Instantiate(tutorialLevel);
            level++;
        }
        else
        {
            int rand = Random.Range(0, levelPrefabs.Length);
            currLevel = Instantiate(levelPrefabs[rand]);
            level++;
        }
        levelIsComplete = false;
        yield return new WaitForSeconds(1);
        if (doorAnim != null)
        {
            doorAnim.SetBool("Open", true);
        }
        isInLevel= true;
        StopCoroutine(nextLevelCoroutine());


    }





}
