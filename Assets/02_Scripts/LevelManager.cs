using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager levelManagerInstance;

    public TextMeshProUGUI currentLevelTMP;
    public TextMeshProUGUI currentLevelWinTMP;
    public TextMeshProUGUI currentLevelLoseTMP;
    public GameObject winRoot;
    public GameObject loseRoot;
    public GameObject moneyRoot;
    public GameObject winContinueButton;
    public GameObject loseContinueButton;
    public GameObject tutoRoot;

    [HideInInspector] public bool levelFinished;
    private bool touchedScreen;

    private int currentLevelToShow;
    public int CurrentLevelToShow
    {
        get
        {
            return currentLevelToShow;
        }
        set
        {
            currentLevelToShow = value;
            currentLevelTMP.text = "LEVEL " + currentLevelToShow.ToString();
            currentLevelWinTMP.text = "LEVEL " + currentLevelToShow.ToString();
            currentLevelLoseTMP.text = "LEVEL " + currentLevelToShow.ToString();
        }
    }

    public void Awake()
    {
        levelManagerInstance = this;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("CurrentLevel"))
            CurrentLevelToShow = PlayerPrefs.GetInt("CurrentLevel");
        else
            CurrentLevelToShow = 1;
    }

    private void Update()
    {
        if(!touchedScreen)
        {
            if(Input.GetMouseButtonDown(0))
            {
                touchedScreen = true;
                Invoke("DisableTuto", 1.5f);
            }
        }
    }

    private void DisableTuto()
    {
        tutoRoot.SetActive(false);
    }

    public void Win()
    {
        levelFinished = true;

        winRoot.SetActive(true);
        moneyRoot.SetActive(true);
        MoneyManager.moneyManagerInstance.WinCoins();

        PlayerPrefs.SetInt("CurrentLevel", CurrentLevelToShow + 1);

        Invoke("ActivatedContinueButton", 1f);
    }

    public void Lose()
    {
        levelFinished = true;

        loseRoot.SetActive(true);
        moneyRoot.SetActive(true);

        Invoke("ActivatedContinueButton", 1f);
    }

    private void ActivatedContinueButton()
    {
        loseContinueButton.SetActive(true);
        winContinueButton.SetActive(true);
    }
}
