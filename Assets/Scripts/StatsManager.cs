using System;
using UnityEngine;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;

public class StatsManager : MonoBehaviour
{
    private UpgradesManager upgradesManager;
    
    private StatusManager statusManager;

    [SerializeField]
    private SerializedDictionary<Stat, float> BaseStats = new SerializedDictionary<Stat, float>();

    private void Start()
    {
        upgradesManager = GetComponent<UpgradesManager>();
        statusManager = GetComponent<StatusManager>();
        
        // Ensure we have no NullReferenceExceptions 
        foreach (Stat stat in Enum.GetValues(typeof(Stat)))
        {
            if (!BaseStats.ContainsKey(stat))
            {
                BaseStats.Add(stat, 0);
            }
        }
    }

    public float GetBaseStat(Stat stat)
    {
        return BaseStats[stat];
    }
    
    public float GetStat(Stat stat)
    {
        float statValue = BaseStats[stat];

        if (upgradesManager)
        {
            statValue = upgradesManager.ApplyModifiers(statValue, stat);
        }

        if (statusManager)
        {
            statValue = statusManager.ApplyModifiers(statValue, stat);
        }
        
        return statValue;
    }
}
