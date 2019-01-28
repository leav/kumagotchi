using UnityEngine;
using System.Collections.Generic;

public class ActivityUI : MonoBehaviour
{
    // Idle, Eat, Sleep, PlayVideoGame, Work
    public GameObject idleEvent;
    public GameObject eatEvent;
    public GameObject sleepEvent;
    public GameObject playVideoGameEvent;
    public GameObject workEvent;

    public GameObject food;
    public GameObject snack;
    public GameObject bedFolded;
    public GameObject bedSetUp;
    public GameObject idleMimi;

    public string interruptObjectName = "interrupt";
    public string mimiObjectName = "mimi";
    public ActivityManager activityManager;
    public RxTest rx;

    [SerializeField]
    List<GameObject> interrupts = new List<GameObject>();
    List<GameObject> interruptingMimis = new List<GameObject>();

    private void Start()
    {
        interrupts.Clear();
        AddInterruptGameObject(idleEvent);
        AddInterruptGameObject(eatEvent);
        AddInterruptGameObject(sleepEvent);
        AddInterruptGameObject(playVideoGameEvent);
        AddInterruptGameObject(workEvent);

        UpdateMainActivityActive();
    }

    void AddInterruptGameObject(GameObject parent)
    {
        var o = parent.transform.Find(interruptObjectName);
        if (o != null)
        {
            interrupts.Add(o.gameObject);
        }
        o = parent.transform.Find(mimiObjectName);
        if (o != null)
        {
            interruptingMimis.Add(o.gameObject);
        }
    }

    void Update()
    {
        UpdateMainActivityActive();

        food.SetActive(activityManager.HasFood());

        snack.SetActive(activityManager.HasSnack());

        if (activityManager.currentMainActivity == MainActivity.Sleep)
        {
            bedFolded.SetActive(false);
            bedSetUp.SetActive(false);
        }
        else if (activityManager.CanSleep())
        {
            bedFolded.SetActive(false);
            bedSetUp.SetActive(true);

        }
        else
        {
            bedFolded.SetActive(true);
            bedSetUp.SetActive(false);
        }

        foreach (var interrupt in interrupts)
        {
            interrupt.SetActive(activityManager.IsInterrupted());
        }
        foreach (var mimi in interruptingMimis)
        {
            mimi.SetActive(activityManager.IsInterrupted() && activityManager.interruptedByMimi);
        }
        idleMimi.SetActive(!activityManager.interruptedByMimi);
    }

    void UpdateMainActivityActive()
    {
        if (rx.ShouldShowTitle())
        {
            idleEvent.SetActive(false);
            eatEvent.SetActive(true);
            sleepEvent.SetActive(true);
            playVideoGameEvent.SetActive(false);
            workEvent.SetActive(true);
            return;
        }

        var activity = activityManager.currentMainActivity;
        idleEvent.SetActive(activity == MainActivity.Idle);
        eatEvent.SetActive(activity == MainActivity.Eat);
        sleepEvent.SetActive(activity == MainActivity.Sleep);
        playVideoGameEvent.SetActive(activity == MainActivity.PlayVideoGame);
        workEvent.SetActive(activity == MainActivity.Work);
    }
}