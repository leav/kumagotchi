using UnityEngine;
using System.Collections.Generic;
using UniRx;

public class GameEndingUI : MonoBehaviour
{
    public GameObject winningScreen;
    public GameObject failedScreen;

    public RxTest rxTest;

    public float restartDelay = 1;
    private float restartCount = 0;

    private void Start()
    {
        winningScreen.AddComponent<ClickHandler>().handler.AddListener(SignalRestart);
        failedScreen.AddComponent<ClickHandler>().handler.AddListener(SignalRestart);
    }

    private void SignalRestart()
    {
        if (restartCount <= 0)
        {
            restartCount = restartDelay;
        }
    }

    void Update()
    {
        winningScreen.SetActive(rxTest.IsGameWon());
        failedScreen.SetActive(rxTest.IsGameFailed());

        if (restartCount > 0)
        {
            restartCount -= Time.deltaTime;
            if (restartCount <= 0)
            {
                restartCount = 0;
                rxTest.StartGame();
            }
        }
    }
}