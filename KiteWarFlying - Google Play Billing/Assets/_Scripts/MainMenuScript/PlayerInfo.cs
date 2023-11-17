using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [Header("Currency")]
    public float highscore;
    public float currency;

    private static PlayerInfo instance;

    private void Awake()
    {
        if (instance == null)
        {
            // Objek belum ada, jadi jadikan objek ini instance yang aktif
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Objek sudah ada sebelumnya, hancurkan objek yang baru dibuat
            Destroy(gameObject);
        }

        highscore = PlayerPrefs.GetFloat("PlayerHighscore", 0);
        currency = PlayerPrefs.GetFloat("PlayerCurrency", 1000);
    }

    public void CheckCurrencyAndSave(float toCurrency)
    {
        currency += toCurrency;
        PlayerPrefs.SetFloat("PlayerCurrency", currency);
    }

    public void CheckHighscoreAndSave(float score)
    {
        if (highscore < score)
        {
            PlayerPrefs.SetFloat("PlayerHighscore", score);
            highscore = PlayerPrefs.GetFloat("PlayerHighscore", 0);
        }
    }
}
