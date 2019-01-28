using UnityEngine;
using System.Collections.Generic;

public class TimeUI : MonoBehaviour
{
    public RectTransform bar;
    public UnityEngine.UI.Text text;

    public ActivityManager activity;

    void Update()
    {
        var timeElapsed = activity.totalTime - activity.TimeRemaining;
        bar.anchorMax = new Vector2(timeElapsed / activity.totalTime, 0.5f);

        float minutes = Mathf.Floor(activity.timeRemaining / 60);
        float seconds = Mathf.Floor(activity.timeRemaining % 60);
        text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}