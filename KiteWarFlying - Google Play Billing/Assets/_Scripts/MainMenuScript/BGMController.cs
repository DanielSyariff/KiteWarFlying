using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

[Serializable]
public class AudioData
{
    public AudioClip clip;
    public string clipName;
}

public class BGMController : MonoBehaviour
{
    public AudioData[] BGMList;
    public AudioData[] SFXList;
    public AudioSource bgm;
    public AudioSource sfx;
    public int mute;
    // Start is called before the first frame update

    private void Start()
    {
        mute = PlayerPrefs.GetInt("AudioMute", 1);

        if (mute == 0)
        {
            MuteAudio(1);
        }
        else
        {
            MuteAudio(0);
        }
    }
    public void PlayBGM(string clipName)
    {
        AudioData s = Array.Find(BGMList, sound => sound.clipName == clipName);
        bgm.clip = s.clip;
        bgm.Play();
    }

    public void PlaySFX(string clipName)
    {
        AudioData s = Array.Find(SFXList, sound => sound.clipName == clipName);
        sfx.clip = s.clip;
        sfx.Play();
    }

    public void StopBGM()
    {
        bgm.Pause();
    }

    public void MuteAudio(int value)
    {
        if (value == 0) //Unmute
        {
            mute = 1;
            bgm.mute = false;
            sfx.mute = false;
        }
        else //Mute
        {
            mute = 0;
            bgm.mute = true;
            sfx.mute = true;
        }

        PlayerPrefs.SetInt("AudioMute", mute);
    }
}
