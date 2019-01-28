using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class RxTest : MonoBehaviour
{
    public enum GameState
    {
        Title,
        Main,
        Ending,
    }

    [Header("States")]
    public GameState gameState = GameState.Main;

    [Header("Parameters")]
    public bool showDebugUI = true;

    [Header("Dependencies")]
    public ActivityManager activityManager;
    public CardManager cardManager;
    public Stats stats;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.Title;

        var updateStream = Observable.EveryFixedUpdate();
        var mainUpdateStream = updateStream.Where(_ => gameState == GameState.Main);

        mainUpdateStream.Subscribe(_ => activityManager.OnUpdate());

        activityManager.Reset();
    }

    public void StartGame()
    {
        gameState = GameState.Main;
        stats.ResetDefaults();
        activityManager.Reset();
    }

    public bool ShouldShowTitle()
    {
        return gameState == GameState.Title;
    }

    public bool IsGameWon()
    {
        return gameState == GameState.Ending && activityManager.IsWorkDone();
    }

    public bool IsGameFailed()
    {
        return gameState == GameState.Ending && !activityManager.IsWorkDone();
    }

    public void Update()
    {
        switch (gameState)
        {
            case GameState.Main:
                if (activityManager.IsTimeUp())
                {
                    gameState = GameState.Ending;
                }
                break;
        }
    }

    private void OnGUI()
    {
        if (Debug.isDebugBuild && showDebugUI)
        {
            DrawDebugMenu();
        }
    }

    private void DrawDebugMenu()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("hap " + stats.happiness.Get());
        GUILayout.Label("ful " + stats.fullness.Get());
        GUILayout.Label("sob " + stats.soberness.Get());
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Add Work Milestone"))
        {
            activityManager.workMilstoneIndex += 1;
        }

        if (GUILayout.Button("-10s"))
        {
            activityManager.TimeRemaining -= 10;
        }

        if (GUILayout.Button("Restart"))
        {
            StartGame();
        }
    }
}
