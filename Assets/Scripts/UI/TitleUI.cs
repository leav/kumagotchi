using UnityEngine;
using System.Collections.Generic;
using UniRx;

public class TitleUI : MonoBehaviour
{
    public GameObject title;
    public UnityEngine.UI.Button startButton;

    public RxTest rxTest;

    private void Start()
    {
        startButton.onClick.AddListener(SignalStart);
    }

    private void SignalStart()
    {
        rxTest.StartGame();
    }

    void Update()
    {
        title.SetActive(rxTest.ShouldShowTitle());
    }
}