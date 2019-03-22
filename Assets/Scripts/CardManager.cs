using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class CardManager : ScriptableObject
{
    [Header("States")]
    public List<Card> cardsOnHand = new List<Card>();
    public float dealCardCountDown = 0;

    [Header("Parameters")]
    public int maxCardsOnHand = 4;
    public float dealCardSeconds = 3;

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
        if (!isDealingCard()) {
            dealCardCountDown = dealCardSeconds;
        }
        var card = cardsOnHand[index];
        cardsOnHand.RemoveAt(index);
        cardPlayedEvent.Publish(card.cardType);
    }

    public void Play(Card card)
    {
        PlayAt(cardsOnHand.IndexOf(card));
    }

    public void OnUpdate() {
        if (isDealingCard()) {
            dealCardCountDown -= Time.fixedDeltaTime;
            if (dealCardCountDown <= 0) {
                cardsOnHand.Add(DealCard());
                dealCardCountDown = dealCardSeconds;
            }
        }
    }

    private bool isDealingCard() {
        return cardsOnHand.Count < maxCardsOnHand;
    }
}

public enum CardType
{
    Food, Snack, PrepareTheBed, Mimi, Work
}