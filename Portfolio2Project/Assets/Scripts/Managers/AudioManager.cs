using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    [Header("----- Volume Stuff -----")]
    public Slider volumeValue;
    public Slider soundEffectsVolume;
    public Toggle bgToggle;
    public Toggle seToggle;

    [Header("----- Audio Stuff -----")]
    [SerializeField] AudioSource aud;
    [SerializeField] List<AudioClip> bgms;
    [SerializeField] AudioClip mainMenu;
    [SerializeField] AudioClip charSelect;
    [SerializeField] float volume;
    [SerializeField] int currSong;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this; 
    }

    private void Start()
    {
        PlaySong();
    }
    // Update is called once per frame
    void Update()
    {
        UpdateBGVolume();
        UpdateToggles();
    }
    public void UpdateToggles()
    {
        if (soundEffectsVolume.value == 0)
        {
            seToggle.isOn = true;
        }
        else
        {
            seToggle.isOn = false;
        }

        if (volumeValue.value == 0)
        {
            bgToggle.isOn = true;
        }
        else
        {
            bgToggle.isOn = false;
        }
    }

    public void StopSong()
    {
        aud.Stop();
    }

    void PlaySong()
    {
        aud.Stop();
        if(UIManager.instance.activeMenu = UIManager.instance.mainMenu)
        {
            aud.clip = mainMenu;
        }
        else if(UIManager.instance.activeMenu = UIManager.instance.playerSelect)
        {
            aud.clip = charSelect;
        }
        else
        {
            aud.clip = bgms[currSong];
        }
        aud.Play();
        aud.loop = true;
    }
    public void ChangeSong()
    {
        currSong++;
        if (currSong >= bgms.Count)
        {
            currSong = 0;
        }
        PlaySong();
    }
    public void UpdateBGVolume()
    {
        volume = volumeValue.value;
        aud.volume = volume;
        if (volumeValue.value == 0)
        {
            bgToggle.isOn = true;
        }
        else
        {
            bgToggle.isOn = false;
        }
    }
}
