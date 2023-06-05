using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class fileManager// : MonoBehaviour
{
    public static float masterVolume;
    public static float effectVolume;
    public static float musicVolume;
    //public static int level;

    public static bool invertY;



    public static void save()
    {
        PlayerPrefs.SetFloat("MV", masterVolume);
        PlayerPrefs.SetFloat("EV", effectVolume);
        PlayerPrefs.SetFloat("MuV", musicVolume);
        //PlayerPrefs.SetInt("LvL", level);
        PlayerPrefs.SetString("inv", invertY.ToString());
    }

    public static void load()
    {
        masterVolume = PlayerPrefs.GetFloat("MV");
        effectVolume = PlayerPrefs.GetFloat("EV");
        musicVolume = PlayerPrefs.GetFloat("MuV");
        //level = PlayerPrefs.GetInt("LvL");
        invertY = bool.Parse( PlayerPrefs.GetString("inv"));

    }

    public static void resetData()
    {
        PlayerPrefs.DeleteAll();
        masterVolume = 0.5f;
        effectVolume = 0.5f;
        AudioManager.instance.soundEffectsVolume.value = effectVolume;
        musicVolume = 0.5f;
        AudioManager.instance.volumeValue.value = musicVolume;
        //level = 1;
        invertY = false;
        UIManager.instance.invert.isOn = invertY;

    }

}
