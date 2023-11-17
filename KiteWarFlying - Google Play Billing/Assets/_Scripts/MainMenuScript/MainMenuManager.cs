using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

[System.Serializable]
public class KiteData
{
    public int status;
}

[System.Serializable]
public class SpriteData
{
    public Sprite sprite; 
    public float cost;
}

[System.Serializable]
public class KiteDataListJSON
{
    public List<int> kiteData;

    public KiteDataListJSON()
    {
        kiteData = new List<int>();
    }
}

public class MainMenuManager : MonoBehaviour
{
    public AdsController adsController;
    public BGMController bgmController;
    public List<SpriteData> kitesprite;
    public List<int> kiteData;

    [Header("Currency")]
    public PlayerInfo playerInfo;
    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI currencyText;
    public Transform notEnoughCurrency;

    [Header("SHOP")]
    public int chooseIndex;
    public GameObject costPanel;
    public Image layanganImage;
    public TextMeshProUGUI coinCostText;
    public GameObject buttonSelect;
    public GameObject buttonbuy;

    [Header("Panel")]
    public GameObject loadingPanel;

    private void Awake()
    {
        playerInfo = FindObjectOfType<PlayerInfo>();
        bgmController = FindObjectOfType<BGMController>();
    }
    private void Start()
    {
        //Reset Player Pref
        PlayerPrefs.SetInt("ChancePlay", 1);

        bgmController.PlayBGM("BGMHome");

        if (PlayerPrefs.GetInt("FirstTime") == 0)
        {
            SaveKiteDataJSON();
            PlayerPrefs.SetInt("FirstTime", 1);
        }
        else
        {
            LoadKiteDataJSON();
        }

        UpdateScoreUI();
        UpdateCurrencyUI();

        chooseIndex = PlayerPrefs.GetInt("SelectedLayangan", 0);
        CheckLayanganStatus();
    }

    #region Local Saving
    public void SaveKiteDataJSON()
    {
        KiteDataListJSON _KiteDataList = new KiteDataListJSON();
        _KiteDataList.kiteData = kiteData;

        //---- save to local
        string userKiteDataPath = Application.persistentDataPath + "/userKiteData.dat";

        FileStream dataStream = new FileStream(userKiteDataPath, FileMode.Create);

        BinaryFormatter converter = new BinaryFormatter();
        converter.Serialize(dataStream, _KiteDataList);
        dataStream.Close();

        Debug.Log("Success Save to Local");
    }

    public void LoadKiteDataJSON()
    {
        KiteDataListJSON _KiteDataList = new KiteDataListJSON();

        //declare save path for binary file
        string userKiteDataPath = Application.persistentDataPath + "/userKiteData.dat";

        if (File.Exists(userKiteDataPath))
        {
            FileStream dataStream = new FileStream(userKiteDataPath, FileMode.Open);
            BinaryFormatter converter = new BinaryFormatter();

            _KiteDataList = converter.Deserialize(dataStream) as KiteDataListJSON;

            kiteData = _KiteDataList.kiteData;

            Debug.Log("Kite Data loaded from local");
            dataStream.Close();

            if (kitesprite.Count > kiteData.Count)
            {
                int tmpNewData = kitesprite.Count - kiteData.Count;
                for (int i = 0; i < tmpNewData; i++)
                {
                    kiteData.Add(0);
                }

                Debug.Log("Terdapat Penambahan JSON");

                SaveKiteDataJSON();
            }
        }
        else
        {
            Debug.Log("There is no saved Kite Data file");
        }
    }

    public void UpdateKiteDataList(int index, int statusValue)
    {
        kiteData[index] = statusValue;
    }

    #endregion

    #region Choose Layangan
    public void RightLeft(int to)
    {
        if (to == 1)
        {
            if (chooseIndex == kitesprite.Count - 1)
            {
                chooseIndex = kitesprite.Count - 1;
            }
            else
            {
                chooseIndex++;
            }
        }
        else
        {
            if (chooseIndex == 0)
            {
                chooseIndex = 0;
            }
            else
            {
                chooseIndex--;
            }
        }

        CheckLayanganStatus();
    }

    public void CheckLayanganStatus()
    {
        layanganImage.sprite = kitesprite[chooseIndex].sprite;

        if (kiteData[chooseIndex] == 0)
        {
            costPanel.SetActive(true);
            buttonbuy.SetActive(true);
            buttonSelect.SetActive(false);
            coinCostText.text = kitesprite[chooseIndex].cost.ToString();
        }
        else
        {
            costPanel.SetActive(false);
            buttonbuy.SetActive(false);
            buttonSelect.SetActive(true);
        }
    }
    #endregion

    #region Buy and Select Layangan
    public void SelectLayangan()
    {
        PlayerPrefs.SetInt("SelectedLayangan", chooseIndex);
        notEnoughCurrency.gameObject.SetActive(true);
        StartCoroutine(StartAnimatingNotEnoughCurrency("SELECTED", false));
    }
    public void BuyLayangan()
    {
        if (playerInfo.currency >= kitesprite[chooseIndex].cost)
        {
            bgmController.PlaySFX("SFXTransaction");
            kiteData[chooseIndex] = 1;
            SaveKiteDataJSON();
            CheckLayanganStatus();
            CalculateCurrency(-kitesprite[chooseIndex].cost);
            adsController.CallInterstitialAds();
        }
        else
        {
            bgmController.PlaySFX("SFXButton");
            notEnoughCurrency.gameObject.SetActive(true);
            StartCoroutine(StartAnimatingNotEnoughCurrency("Not Enough Currency", true));
            Debug.Log("Not Enough Currency");
        }
    }
    IEnumerator StartAnimatingNotEnoughCurrency(string text, bool notEnough)
    {
        if (notEnough)
        {
            notEnoughCurrency.GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        else
        {
            notEnoughCurrency.GetComponent<TextMeshProUGUI>().color = Color.white;
        }
        notEnoughCurrency.GetComponent<TextMeshProUGUI>().text = text;
        Vector3 originalPosition = notEnoughCurrency.position;
        notEnoughCurrency.DOShakePosition(0.5f, 10f);//.OnComplete(ResetPosition);
        yield return new WaitForSeconds(0.5f);
        notEnoughCurrency.position = originalPosition;
        yield return new WaitForSeconds(2f);
        notEnoughCurrency.gameObject.SetActive(false);
    }

    #endregion

    #region Currecny Increase and Decrease
    public void CalculateCurrency(float toCurrency) //to Currecy diisi dengan Berapa banyak Penambahan ataupun Pengurangan Ex(1000 / -1000)
    {
        playerInfo.CheckCurrencyAndSave(toCurrency);
        UpdateCurrencyUI();
    }

    public void UpdateScoreUI()
    {
        highscoreText.text = playerInfo.highscore.ToString();
    }
    public void UpdateCurrencyUI()
    {
        currencyText.text = playerInfo.currency.ToString();
    }
    #endregion

    #region Buy Bundle
    public void BuyBundle()
    {
        playerInfo.CheckCurrencyAndSave(5000);

        bgmController.PlaySFX("SFXTransaction");
        for (int i = 0; i < kiteData.Count; i++)
        {
            kiteData[i] = 1;
        }

        SaveKiteDataJSON();
        CheckLayanganStatus();

        UpdateCurrencyUI();
    }
    #endregion

    #region Play
    public void PlayGame()
    {
        //adsController.RewardedPlayGames();
        StartCoroutine(LoadingPlayNoAds());
    }

    public IEnumerator LoadingPlayNoAds()
    {
        loadingPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadSceneAsync("Gameplay");
    }

    public void SuccessAds_PlayGame()
    {
        StartCoroutine(StartingSceneGameplay());
    }

    IEnumerator StartingSceneGameplay()
    {
        loadingPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadSceneAsync("Gameplay");
    }
    #endregion

    #region Rate Us
    public void RateUs()
    {
//#if UNITY_ANDROID
        string packageName = "com.JakBoStudio.KiteWarFlying";
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + packageName);
//#endif
    }
    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveKiteDataJSON();
        }
    }
}
