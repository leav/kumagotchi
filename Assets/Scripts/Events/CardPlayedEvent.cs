using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class CardPlayedEvent : ScriptableObject
{
    [System.Serializable]
    public class Event : UnityEvent<CardType>
    {
    }

    public Event handler;

    public void Publish(CardType card)
    {
        Logger.Format(card.ToString());
        handler.Invoke(card);
    }
}