using UnityEngine;
using System.Collections.Generic;

public class CardUI : MonoBehaviour
{
    public GameObject actionsTab;
    public GameObject cardDeck;

    public Transform slotsParent;

    public GameObject foodPrefab;
    public GameObject miPrefab;
    public GameObject sleepPrefab;
    public GameObject snackPrefab;
    public GameObject yellingPrefab;

    List<Dictionary<CardType, GameObject>> slots = new List<Dictionary<CardType, GameObject>>();
    

    public CardManager cardManager;
    public RxTest rx;

    private void Start()
    {
        slots.Clear();

        for (int i = 0; i < slotsParent.childCount; i++)
        {
            var slotTransform = slotsParent.GetChild(i);
       
            var slotDict = new Dictionary<CardType, GameObject>();
            InstantiateToDict(slotTransform, slotDict, CardType.Food, foodPrefab);
            InstantiateToDict(slotTransform, slotDict, CardType.Mimi, miPrefab);
            InstantiateToDict(slotTransform, slotDict, CardType.PrepareTheBed, sleepPrefab);
            InstantiateToDict(slotTransform, slotDict, CardType.Snack, snackPrefab);
            InstantiateToDict(slotTransform, slotDict, CardType.Work, yellingPrefab);
            slots.Add(slotDict);

            var handler = slotTransform.gameObject.AddComponent<ClickHandler>();
            var index = i; // Clone to local var.
            handler.handler.AddListener(() => OnClicked(index));
        }
    }

    void InstantiateToDict(Transform parent, Dictionary<CardType, GameObject> dict, CardType cardType, GameObject prefab)
    {
        var o = Instantiate(prefab, parent);
        o.SetActive(false);
        dict.Add(cardType, o);
    }

    void Update()
    {
        actionsTab.SetActive(!rx.ShouldShowTitle());
        cardDeck.SetActive(!rx.ShouldShowTitle());

        for (int i = slots.Count - 1; i >= 0; i--)
        {
            bool showCard = false;
            CardType cardType = CardType.Food;
            if (i < cardManager.cardsOnHand.Count)
            {
                showCard = true;
                cardType = cardManager.cardsOnHand[i].cardType;
            }
            foreach (var keyPair in slots[i])
            {
                var active = showCard && keyPair.Key == cardType;
                keyPair.Value.SetActive(active);
            }

        }
    }

    void OnClicked(int index)
    {
        Logger.Format("OnClicked {0}", index);
        cardManager.PlayAt(index);
    }
}