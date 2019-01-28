using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IntRange
{
    public int min;
    public int max;

    public IntRange(int min, int max)
    {
        this.min = min;
        this.max = max;
    }

    public int Get()
    {
        return Random.Range(min, max + 1);
    }

    public int GetWithFactor(float factor)
    {
        return (int)(Get() * factor);
    }
}