using UnityEngine;
using System.Collections.Generic;

public class MilestoneUI : MonoBehaviour
{
    public Transform container;

    public ActivityManager activity;

    List<GameObject> milestones = new List<GameObject>();

    void Start()
    {
        milestones.Clear();
        foreach (Transform child in container)
        {
            milestones.Add(child.gameObject);
        }
    }

    void Update()
    {
        for (int i = 0; i < milestones.Count; i++)
        {
            var milestone = milestones[i];
            milestone.SetActive(i < activity.workMilstoneIndex);
        }
    }
}