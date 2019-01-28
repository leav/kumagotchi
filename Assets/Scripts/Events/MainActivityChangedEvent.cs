using System;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu]
public class MainActivityChangedEvent : ScriptableObject
{
    [System.Serializable]
    public class Event : UnityEvent<MainActivity>
    {
    }

    public Event handler;

    public void Publish(MainActivity activity)
    {
        Logger.Format(activity.ToString());
        handler.Invoke(activity);
    }
}