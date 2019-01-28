using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Stat", order = 1)]
public class Stat : ScriptableObject
{
    public int min = 0;
    public int max = 10000;
    public int zoneMin = 3000;
    public int zoneMax = 7000;
    public int defaultValue = 5000;
    public int value = 5000;

    public int Get()
    {
        return value;
    }

    public float GetRatio()
    {
        return (value - min) / GetRangef();
    }

    public float GetZoneMinRatio()
    {
        return (zoneMin - min) / GetRangef();
    }

    public float GetZoneMaxRatio()
    {
        return (zoneMax - min) / GetRangef();
    }

    public void Set(int newValue)
    {
        this.value = Mathf.Clamp(newValue, min, max);
    }

    public void Add(int delta)
    {
        Set(this.value + delta);
    }

    public bool IsInZone()
    {
        return value >= zoneMin && value <= zoneMax;
    }

    public float GetZoneDistance()
    {
        if (value < zoneMin)
        {
            return (zoneMin - value) / GetRangef();
        }
        if (value > zoneMax)
        {
            return (value - zoneMax) / GetRangef();
        }
        return 0;
    }

    float GetRangef()
    {
        return (float)max - min;
    }
}