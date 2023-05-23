using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer instance;
    [SerializeField] AudioSource aud;
    [SerializeField] List<AudioClip> bgms;
    [SerializeField] float volume;
    [SerializeField] int currSong;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
       PlaySong();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PlaySong()
    {
        aud.clip = bgms[currSong];
        aud.Play();
        aud.loop = true;
    }
    public void ChangeSong()
    {
        currSong++;
        aud.Stop();
        PlaySong();
    }
}