using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager moneyManagerInstance;

    public Animator coinIconAnimator;
    public Transform finalPosCoin;
    public Transform initialPosCoin;
    public TextMeshProUGUI moneyTMP;
    public TextMeshProUGUI moneyWonTMP;

    private int currentMoneyWonInLevel;
    private string moneyLabel = "Money";

    private int visualMoney;
    public int VisualMoney
    {
        get
        {
            return visualMoney;
        }
        set
        {
            visualMoney = value;
            moneyTMP.text = visualMoney.ToString();
        }
    }

    private int money;
    public int Money
    {
        get
        {
            return money;
        }
        set
        {
            money = value;
            PlayerPrefs.SetInt(moneyLabel, money);
        }
    }

    public void Awake()
    {
        moneyManagerInstance = this;
    }

    private void Start()
    {
        if(PlayerPrefs.HasKey(moneyLabel))
        {
            Money = PlayerPrefs.GetInt(moneyLabel);
            VisualMoney = Money;
        }
        else
        {
            Money = 0;
            VisualMoney = 0;
        }
    }

    public void WinCoins()
    {
        Money += currentMoneyWonInLevel;
        moneyWonTMP.text = "x" + currentMoneyWonInLevel.ToString();

        for (int i = 0; i < currentMoneyWonInLevel; i++)
        {
            OnMoneyWon moneyWonClone = ObjectPooler.objectPoolerInstance.GetPooledObject("Coin").GetComponent<OnMoneyWon>();
            moneyWonClone.finalPos = finalPosCoin.position;
            moneyWonClone.transform.position = initialPosCoin.position + new Vector3(Random.Range(-150f, 150f), Random.Range(-100f, 100f), 0);
            moneyWonClone.gameObject.SetActive(true);
        }
    }

    public void WinCoinDuringLevel()
    {
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        GameObject fxUI = ObjectPooler.objectPoolerInstance.GetPooledObject("+1Coin");
        float randomXPos = Random.Range(-Screen.width / 8f, Screen.width / 8f);
        float randomYPos = Random.Range(-Screen.height / 20f, Screen.height / 20f);
        fxUI.transform.position = GlovesStacking.glovesStackingInstance.fxUIRoot.position + new Vector3(randomXPos, randomYPos, 0);
        fxUI.SetActive(true);
        currentMoneyWonInLevel++;
    }
}
