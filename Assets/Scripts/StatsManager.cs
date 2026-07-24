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
    
    public delegate void StatModifierChanged(StatModifierChangedEventArgs args);
    
    public event StatModifierChanged OnStatModifierChanged;

    private void Start()
    {
        upgradesManager = GetComponent<UpgradesManager>();
        statusManager = GetComponent<StatusManager>();

        if (upgradesManager != null)
        {
            upgradesManager.OnUpgradeModifierChanged += OnModifierChanged;
        }

        if (statusManager != null)
        {
            statusManager.OnStatusModifierChanged += OnModifierChanged;
        }
        
        // Ensure we have no NullReferenceExceptions 
        foreach (Stat stat in Enum.GetValues(typeof(Stat)))
        {
            if (!BaseStats.ContainsKey(stat))
            {
                BaseStats.Add(stat, 0);
            }
        }
    }

    private void OnModifierChanged(StatModifierChangedEventArgs args)
    {
        OnStatModifierChanged?.Invoke(args);
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
