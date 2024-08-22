using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;




    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
    }


    public void PlayMusic(string name)
    {
        //Sound musicSound = Array.Find(musicSounds, x => x.name == name);

        Sound musicSound = null;
        foreach (Sound sound in musicSounds)
        {
            if (sound.name == name)
            {
                musicSound = sound;
                break;
            }
        }

        if (musicSound == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            musicSource.clip = musicSound.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound sfxSound = Array.Find(sfxSounds, x => x.name == name);

        if (sfxSound == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            sfxSource.PlayOneShot(sfxSound.clip);
        }
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;

        if (volume == 0)
        {

            musicSource.mute = true;
        }
        else
        {
            musicSource.mute = false;
        }
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;

        if (volume == 0)
        {
            sfxSource.mute = true;
        }
        else
        {
            sfxSource.mute = false;
        }
    }
}
