using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class CardManager : ScriptableObject
{
    [Header("States")]
    public List<Card> cardsOnHand = new List<Card>();

    [Header("Parameters")]
    public int maxCardsOnHand = 4;

    [Header("Dependencies")]
    public CardPlayedEvent cardPlayedEvent;

    public void OnEnable()
    {
        Reset();
    }

    public void Reset()
    {
        cardsOnHand.Clear();
        DealToMax();
    }

    public void DealToMax()
    {
        while (cardsOnHand.Count < maxCardsOnHand)
        {
            cardsOnHand.Add(DealCard());
        }
    }

    public Card DealCard()
    {
        var card = ScriptableObject.CreateInstance<Card>();
        var v = System.Enum.GetValues(typeof(CardType));
        card.cardType = (CardType)v.GetValue(Random.Range(0, v.Length));
        Logger.Format("{0}", card.cardType.ToString());
        return card;
    }

    public void PlayAt(int index)
    {
        if (index < 0 || index >= cardsOnHand.Count)
        {
            return;
        }
        var card = cardsOnHand[index];
        cardsOnHand.RemoveAt(index);
        cardsOnHand.Insert(index, DealCard());
        cardPlayedEvent.Publish(card.cardType);
    }

    public void Play(Card card)
    {
        PlayAt(cardsOnHand.IndexOf(card));
    }
}

public enum CardType
{
    Food, Snack, PrepareTheBed, Mimi, Work
}