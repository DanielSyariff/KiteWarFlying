using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToOpenAndClose : MonoBehaviour
{
    public GameObject[] objectToOpenOrClose;
    public bool OpenClose;
    public void OpenAndClose()
    {
        for (int i = 0; i < objectToOpenOrClose.Length; i++)
        {
            if (OpenClose)
            {
                objectToOpenOrClose[i].SetActive(true);
            }
            else
            {
                objectToOpenOrClose[i].SetActive(false);
            }
        }
    }
}
