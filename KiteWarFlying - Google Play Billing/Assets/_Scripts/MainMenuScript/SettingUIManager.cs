using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUIManager : MonoBehaviour
{
    public BGMController bgmController;
    public GameObject onImage;
    public GameObject offImage;

    private void Awake()
    {
        bgmController = FindObjectOfType<BGMController>();
    }
    private void OnEnable()
    {
        CheckSoundStatus();
    }
    public void CheckSoundStatus()
    {
        if (bgmController.mute == 1)
        {
            onImage.SetActive(true);
            offImage.SetActive(false);
        }
        else
        {
            onImage.SetActive(false);
            offImage.SetActive(true);
        }
    }

    public void SendMuteToController(int value)
    {
        bgmController.MuteAudio(value);
        CheckSoundStatus();
    }
}
