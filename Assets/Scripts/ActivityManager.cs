using System;
using System.Collections.Generic;
using UnityEngine;

public enum MainActivity
{
    Idle, Eat, Sleep, PlayVideoGame, Work
}

public enum SideActivity
{
    Snack
}

[CreateAssetMenu]
public class ActivityManager : ScriptableObject
{
    [Header("States")]

    public MainActivity currentMainActivity;
    public bool IsCurrentMainActivityAtTable()
    {
        return currentMainActivity == MainActivity.PlayVideoGame || currentMainActivity == MainActivity.Work;
    }

    [SerializeField]
    float interruptCount;
    public float InterruptCount
    {
        get { return interruptCount; }
        set
        {
            interruptCount = Mathf.Clamp(value, 0, interruptDelayedDuration);
        }
    }
    public bool IsInterrupted()
    {
        return interruptCount > 0;
    }
    public bool interruptedByMimi = false;
    [SerializeField]
    float foodAmount;
    public float FoodAmount
    {
        get { return foodAmount; }
        set
        {
            foodAmount = Mathf.Clamp(value, 0, value);
        }
    }
    public bool HasFood()
    {
        return FoodAmount > 0;
    }
    [SerializeField]
    float snackAmount;
    public float SnackAmount
    {
        get { return snackAmount; }
        set
        {
            snackAmount = Mathf.Clamp(value, 0, value);
        }
    }
    public bool HasSnack()
    {
        return snackAmount > 0;
    }
    [SerializeField]
    float sleepAmount;
    public float SleepAmount
    {
        get { return sleepAmount; }
        set
        {
            sleepAmount = Mathf.Clamp(value, 0, value);
        }
    }
    public bool CanSleep()
    {
        return sleepAmount > 0;
    }
    public bool BedIsPrepared()
    {
        return sleepAmount > 0;
    }
    public bool workIsCalled;
    public float workProgress;
    public int workMilstoneIndex;
    public float timeRemaining = 0;
    public float TimeRemaining
    {
        get { return timeRemaining; }
        set
        {
            timeRemaining = Mathf.Clamp(value, 0, value);
        }
    }
    public bool IsTimeUp()
    {
        return TimeRemaining <= 0;
    }
    public bool IsWorkDone()
    {
        return workMilstoneIndex >= workMilestoneCount;
    }

    [Header("Parameters")]
    public IntRange naturalHappinessDelta = new IntRange(-400, -200);
    public IntRange naturalFullnessDelta = new IntRange(-400, -200);
    [Tooltip("real delta = factored deta * fullness %")]
    public IntRange naturalSobernessFactoredDelta = new IntRange(-2000, -1000);
    public List<CardType> interruptingCards = new List<CardType>();
    public int foodCardLastDuration = 6;
    public int snackCardLastDuration = 6;
    public int sleepCardLastDuration = 6;
    public int workMilestoneCount = 4;
    public int workProgressAmountForMilestone = 3;
    public float interruptDelayedDuration = 1;
    public float totalTime = 60;

    //Idle, Eat, Sleep, PlayVideoGame, Work
    [Header("Activity - Idle")]
    [Range(0, 1)]
    public float idleInterruptChance = 0.25f;
    [Header("Activity - Eat")]
    [Range(0, 1)]
    public float eatInterruptChance = 0.25f;
    public IntRange eatHappinessDelta = new IntRange(0, 0);
    public IntRange eatFullnessDelta = new IntRange(1000, 2000);
    public IntRange eatSobernessDelta = new IntRange(0, 0);
    [Header("Activity - Sleep")]
    [Range(0, 1)]
    public float sleepInterruptChance = 0.25f;
    public IntRange sleepHappinessDelta = new IntRange(0, 0);
    public IntRange sleepSobernessDelta = new IntRange(1000, 2000);
    public IntRange sleepFullnessDelta = new IntRange(100, 200);
    [Header("Activity - PlayVideoGame")]
    [Range(0, 1)]
    public float playVideoGameInterruptChance = 0.1f;
    public IntRange playVideoHappinessDelta = new IntRange(1000, 2000);
    public IntRange playVideoGameSobernessDelta = new IntRange(-200, -100);
    public IntRange playVideoGameFullnessDelta = new IntRange(-200, -100);
    [Header("Activity - Work")]
    [Range(0, 1)]
    public float workInterruptChanceIfNotInZone = 0.25f;
    public IntRange workHappinessDelta = new IntRange(-200, -100);
    public IntRange workSobernessDelta = new IntRange(-200, -100);
    public IntRange workFullnessDelta = new IntRange(-200, -100);
    [Header("Side Activity - Snack")]
    public IntRange snackHappinessDelta = new IntRange(0, 0);
    public IntRange snackFullnessDelta = new IntRange(500, 1000);
    public IntRange snackSobernessDelta = new IntRange(0, 0);
    [Header("Card - Mimi")]
    public IntRange mimiCardHappinessDelta = new IntRange(0, 0);
    [Header("Card - Work")]
    public IntRange workCardHappinessDeltaIfNotInZone = new IntRange(-200, -100);


    [Header("Dependencies")]
    public Stats stats;
    public MainActivityChangedEvent activityChangedEvent;
    public CardPlayedEvent cardPlayedEvent;
    public ActivityDecider activityDecider;

    public void OnEnable()
    {
        cardPlayedEvent.handler.AddListener(OnCardPlayed);
        Reset();
    }

    public void Reset()
    {
        SetMainActivity(MainActivity.Idle);

        interruptCount = 0;
        FoodAmount = 0;
        SnackAmount = 0;
        SleepAmount = 0;

        workIsCalled = false;
        workProgress = 0;
        workMilstoneIndex = 0;

        timeRemaining = totalTime;
    }

    public void SetMainActivity(MainActivity activity)
    {
        var lastActivity = currentMainActivity;
        currentMainActivity = activity;
        if (!currentMainActivity.Equals(lastActivity))
        {
            if (lastActivity == MainActivity.Work)
            {
                workProgress = 0;
            }
            activityChangedEvent.Publish(currentMainActivity);
        }
    }

    //Idle, Eat, Sleep, PlayVideoGame, Work
    public void OnUpdate()
    {
        int happinessDelta = 0;
        int fullnessDelta = 0;
        int sobernessDelta = 0;

        if (!IsInterrupted())
        {
            happinessDelta = naturalHappinessDelta.Get();
            fullnessDelta = naturalFullnessDelta.Get();
            sobernessDelta = naturalSobernessFactoredDelta.GetWithFactor(stats.fullness.GetRatio());
        }

        switch (currentMainActivity)
        {
            case MainActivity.Idle:
                InterruptByChance(idleInterruptChance);
                break;
            case MainActivity.Eat:
                if (!IsInterrupted())
                {
                    happinessDelta += eatHappinessDelta.Get();
                    fullnessDelta += eatFullnessDelta.Get();
                    sobernessDelta += eatSobernessDelta.Get();
                    FoodAmount -= Time.fixedDeltaTime;
                }
                if (!HasFood())
                {
                    Interrupt();
                }
                else
                {
                    InterruptByChance(eatInterruptChance);
                }
                break;
            case MainActivity.Sleep:
                if (!IsInterrupted())
                {
                    happinessDelta += sleepHappinessDelta.Get();
                    fullnessDelta += sleepFullnessDelta.Get();
                    sobernessDelta += sleepSobernessDelta.Get();

                    SleepAmount -= Time.fixedDeltaTime;
                }
                if (!CanSleep())
                {
                    Interrupt();
                }
                else
                {
                    InterruptByChance(sleepInterruptChance);
                }
                break;
            case MainActivity.PlayVideoGame:
                if (!IsInterrupted())
                {
                    happinessDelta += playVideoHappinessDelta.Get();
                    fullnessDelta += playVideoGameFullnessDelta.Get();
                    sobernessDelta += playVideoGameSobernessDelta.Get();
                }

                InterruptByChance(playVideoGameInterruptChance);
                break;
            case MainActivity.Work:
                if (!IsInterrupted())
                {
                    happinessDelta += workHappinessDelta.Get();
                    fullnessDelta += workFullnessDelta.Get();
                    sobernessDelta += workSobernessDelta.Get();

                    workProgress += Time.fixedDeltaTime;
                    if (workProgress >= workProgressAmountForMilestone)
                    {
                        workProgress = 0;
                        workMilstoneIndex += 1;
                    }
                }
                if (!stats.AllInZone())
                {
                    InterruptByChance(workInterruptChanceIfNotInZone);
                }
                break;
        }
        if (SnackAmount > 0 && IsCurrentMainActivityAtTable())
        {
            if (!IsInterrupted())
            {
                happinessDelta += snackHappinessDelta.Get();
                fullnessDelta += snackFullnessDelta.Get();
                sobernessDelta += snackSobernessDelta.Get();

                SnackAmount -= Time.fixedDeltaTime;
            }
        }

        stats.happiness.Add((int)(happinessDelta * Time.fixedDeltaTime));
        stats.fullness.Add((int)(fullnessDelta * Time.fixedDeltaTime));
        stats.soberness.Add((int)(sobernessDelta * Time.fixedDeltaTime));

        TimeRemaining -= Time.fixedDeltaTime;

        if (InterruptCount > 0)
        {
            InterruptCount -= Time.fixedDeltaTime;
            if (InterruptCount <= 0)
            {
                SetMainActivity(activityDecider.Decide());
                workIsCalled = false;
                interruptedByMimi = false;
            }
        }
    }

    void OnCardPlayed(CardType card)
    {
        if (interruptingCards.Contains(card))
        {
            Interrupt();
        }
        switch (card)
        {
            case CardType.Food:
                FoodAmount = foodCardLastDuration;
                break;
            case CardType.Mimi:
                interruptedByMimi = true;
                stats.happiness.Add(mimiCardHappinessDelta.Get());
                break;
            case CardType.PrepareTheBed:
                SleepAmount = sleepCardLastDuration;
                break;
            case CardType.Snack:
                SnackAmount = snackCardLastDuration;
                break;
            case CardType.Work:
                if (!stats.AllInZone())
                {
                    stats.happiness.Add(workCardHappinessDeltaIfNotInZone.Get());
                }
                workIsCalled = true;
                break;
            default:
                break;
        }
    }

    public void Interrupt()
    {
        if (!IsInterrupted())
        {
            InterruptCount = interruptDelayedDuration;
        }
        Logger.Format("interrupted");
    }

    /// <summary>
    /// Interrupts the by chance.
    /// The chance is for second.
    /// </summary>
    /// <param name="chance">Chance.</param>
    public void InterruptByChance(float chance)
    {
        if (UnityEngine.Random.value < chance * Time.fixedDeltaTime)
        {
            Interrupt();
        }
    }
}