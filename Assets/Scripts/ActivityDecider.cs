using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class ActivityDecider : ScriptableObject
{
    [Header("Parameters")]
    public int bedIsMadeFitness = 10;
    public int hasFoodFitness = 10;
    public int playVideoGameFitness = 2;
    public int workFitnessIfInZone = 10;

    [Header("Dependencies")]
    public Stats stats;
    public ActivityManager activityManager;


    public MainActivity Decide()
    {
        if (activityManager.workIsCalled)
        {
            return MainActivity.Work;
        }
        List<MainActivity> roulette = new List<MainActivity>();

        AddFitnessToRoulette(roulette, MainActivity.PlayVideoGame, playVideoGameFitness);

        if (stats.AllInZone())
        {
            AddFitnessToRoulette(roulette, MainActivity.Work, workFitnessIfInZone);
        }

        if (activityManager.BedIsPrepared())
        {
            AddFitnessToRoulette(roulette, MainActivity.Sleep, bedIsMadeFitness);
        }

        if (activityManager.HasFood())
        {
            AddFitnessToRoulette(roulette, MainActivity.Eat, hasFoodFitness);
        }

        var result = roulette[Random.Range(0, roulette.Count)];

        Logger.Format(result.ToString());
        return result;
    }

    void AddFitnessToRoulette(List<MainActivity> roulette, MainActivity activity, int fitness)
    {
        for (int i = 0; i < fitness; i++)
        {
            roulette.Add(activity);
        }
    }
}
