using UnityEngine;
using System.Collections.Generic;

public class StatsUI : MonoBehaviour
{
    public GameObject statusTab;
    public GameObject statePanel;

    public RectTransform happinessBar;
    public RectTransform happinessZone;
    public RectTransform fullnessBar;
    public RectTransform fullnessZone;
    public RectTransform sobernessBar;
    public RectTransform sobernessZone;

    public Stats stats;
    public RxTest rx;

    void Update()
    {
        statusTab.SetActive(!rx.ShouldShowTitle());
        statePanel.SetActive(!rx.ShouldShowTitle());

        happinessBar.anchorMax = new Vector2(stats.happiness.GetRatio(), 0.5f);
        happinessZone.anchorMin = new Vector2(stats.happiness.GetZoneMinRatio(), 0.5f);
        happinessZone.anchorMax = new Vector2(stats.happiness.GetZoneMaxRatio(), 0.5f);

        fullnessBar.anchorMax = new Vector2(stats.fullness.GetRatio(), 0.5f);
        fullnessZone.anchorMin = new Vector2(stats.fullness.GetZoneMinRatio(), 0.5f);
        fullnessZone.anchorMax = new Vector2(stats.fullness.GetZoneMaxRatio(), 0.5f);

        sobernessBar.anchorMax = new Vector2(stats.soberness.GetRatio(), 0.5f);
        sobernessZone.anchorMin = new Vector2(stats.soberness.GetZoneMinRatio(), 0.5f);
        sobernessZone.anchorMax = new Vector2(stats.soberness.GetZoneMaxRatio(), 0.5f);
    }
}