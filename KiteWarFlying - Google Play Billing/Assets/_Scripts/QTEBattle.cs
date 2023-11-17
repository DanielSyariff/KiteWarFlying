using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QTEBattle : MonoBehaviour
{
    public Slider slider;
    public GameObject qtePanel;
    public EnemyKite enemyKite;
    public KiteMovement kiteMovement;

    public float reductionRate = 0.5f; 
    public float incrementRate = 0.2f;

    private float currentValue;

    public bool qteActive;

    [Header("Countdown")]
    public TextMeshProUGUI countdownText; 
    public float countdownDuration = 5f; 
    private float countdownTimer;

    public bool isCounting;

    public void StartQTE(EnemyKite enemy)
    {
        reductionRate = enemy.reductionPower;
        enemyKite = enemy;
        qtePanel.SetActive(true);
        currentValue = slider.maxValue / 2;
        qteActive = true;
        isCounting = true;

        countdownTimer = countdownDuration;
    }

    public void EndQTE(bool winCondition)
    {
        qtePanel.SetActive(false);
        qteActive = false;

        if (winCondition)
        {
            enemyKite.Death();
        }
        else
        {
            kiteMovement.PlayerDeath();
        }
    }

    private void Update()
    {
        if (qteActive)
        {
            currentValue -= reductionRate * Time.deltaTime;

            currentValue = Mathf.Max(currentValue, 0f);

            slider.value = currentValue;

            //Timer

            if (isCounting)
            {
                countdownTimer -= Time.deltaTime;
                countdownTimer = Mathf.Max(countdownTimer, 0f);

                int countdownInt = Mathf.FloorToInt(countdownTimer);

                countdownText.text = countdownInt.ToString();

                if (countdownTimer == 0)
                {
                    CheckQTECondition();
                    isCounting = false;
                }
            }
        }
    }

    public void CheckQTECondition()
    {
        if (slider.value > slider.maxValue / 2)
        {
            Debug.Log("WIN");
            EndQTE(true);
        }
        else
        {
            Debug.Log("Lose");
            EndQTE(false);
        }
    }

    public void Attack()
    {
        currentValue += incrementRate;
        slider.value = currentValue;
    }
}
