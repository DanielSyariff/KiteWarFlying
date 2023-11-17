using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    public BGMController bgmController;
    public string sfxName;

    private void Awake()
    {
        bgmController = FindObjectOfType<BGMController>();
    }

    public void Tap()
    {
        bgmController.PlaySFX(sfxName);
    }
}
