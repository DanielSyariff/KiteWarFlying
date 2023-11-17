using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreAndResultManager : MonoBehaviour
{
    private PlayerInfo playerInfo;
    public AdsController adsController;
    public BGMController bgmController;

   [Header("On Gameplay")]
    public TextMeshProUGUI scoreGameplayText;
    public TextMeshProUGUI coinGameplayText;

    [Header("Result Screen")]
    public float score;
    public float coin;
    public int kill;
    public GameObject resultPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinText;

    [Header("Pause")]
    public GameObject pausePanel;
    public GameObject tipsPanel;

    public Transform enemyKiteParent;
    public List<GameObject> enemyKiteList = new List<GameObject>();

    public static ScoreAndResultManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        playerInfo = FindObjectOfType<PlayerInfo>();
        bgmController = FindObjectOfType<BGMController>();
        adsController = FindObjectOfType<AdsController>();
    }

    private void Start()
    {
        bgmController.PlayBGM("BGMIngame");

        foreach (Transform tmpEnemy in enemyKiteParent)
        {
            if (tmpEnemy.gameObject.activeInHierarchy)
            {
                enemyKiteList.Add(tmpEnemy.gameObject);
            }
        }

        if (PlayerPrefs.GetInt("FirstTimeTips", 0) == 0)
        {
            PlayerPrefs.SetInt("FirstTimeTips", 1);
            tipsPanel.SetActive(true);
        }

        UpdateGameplayScoreAndCoin();
    }

    public void Pause(bool pause)
    {
        if (pause)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void IncreaseScoreAndCheckKill(float KillScore, float coinValue)
    {
        score += KillScore;
        kill++;

        coin += coinValue;

        UpdateGameplayScoreAndCoin();

        //Cek Jika Sudah Kill Semua
        //if (kill == enemyKiteList.Count)
        //{
        //    ShowScore(true);
        //}
    }

    public void UpdateGameplayScoreAndCoin()
    {
        scoreGameplayText.text = "Score : " + score.ToString();
        coinGameplayText.text = coin.ToString();
    }


    public void ShowScore(bool condition)
    {
        //if (condition)
        //{
        //    headerText.text = "YOU WIN!";
        //}
        //else
        //{
        //    headerText.text = "YOU LOSE";   
        //}

        bgmController.StopBGM();
        bgmController.PlaySFX("SFXFinalResult");

        scoreText.text = score.ToString();
        coinText.text = coin.ToString();
        resultPanel.SetActive(true);

        //playerInfo.currency += coin;

        playerInfo.CheckCurrencyAndSave(coin);
        playerInfo.CheckHighscoreAndSave(score);
    }
    public int chanceReplay;
    public void PlayAgain()
    {
        chanceReplay = PlayerPrefs.GetInt("ChancePlay");

        Time.timeScale = 1;
        if (chanceReplay > 0)
        {
            chanceReplay--;
            PlayerPrefs.SetInt("ChancePlay", chanceReplay);
            SuccessAds_PlayAgain();
        }
        else
        {
            adsController.RewardedPlayAgain();
        }
    }

    public void SuccessAds_PlayAgain()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void Exit()
    {
        Time.timeScale = 1;
        adsController.RewardedExitGames();
    }

    public void SuccessAds_ExitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
