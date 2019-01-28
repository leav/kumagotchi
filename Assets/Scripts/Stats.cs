using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "Stats")]
public class Stats : ScriptableObject
{
    public Stat happiness;
    public Stat fullness;
    public Stat soberness;

    public void OnEnable()
    {
        ResetDefaults();
    }

    public void ResetDefaults()
    {
        happiness.Set(happiness.defaultValue);
        fullness.Set(fullness.defaultValue);
        soberness.Set(soberness.defaultValue);
    }

    public bool AllInZone()
    {
        return happiness.IsInZone() && fullness.IsInZone() && soberness.IsInZone();
    }
}
