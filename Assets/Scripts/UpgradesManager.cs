using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class UpgradesManager : MonoBehaviour, IUpgradable
{
    public ObservableList<Upgrade> Upgrades { get; } = new ObservableList<Upgrade>();
    
    /// <inheritdoc />
    public event IUpgradable.UpgradeModifierChanged OnUpgradeModifierChanged;

    private void Start()
    {
        Upgrades.ItemAdded += OnUpgradesChanged;
        Upgrades.ItemRemoved += OnUpgradesChanged;
    }

    private void OnUpgradesChanged(ObservableList<Upgrade> upgrades, ListChangedEventArgs<Upgrade> args)
    {
        List<Stat> invokedStats = new List<Stat>();
        
        foreach (StatModifier mod in args.item.Modifiers)
        {
            if (!invokedStats.Contains(mod.Stat))
            {
                OnUpgradeModifierChanged?.Invoke(new StatModifierChangedEventArgs(mod.Stat));
                invokedStats.Add(mod.Stat);
            }
        }
    }
    
    /// <summary>
    /// Applies all upgrades for a given stat to the given number.
    /// </summary>
    /// <param name="input">The base stat to modify.</param>
    /// <param name="stat">What stat to use for loading modifiers.</param>
    /// <returns>The modified stat.</returns>
    public float ApplyModifiers(float input, Stat stat)
    {
        input += Upgrades.Sum(upgrade => upgrade.Modifiers.Where(modifier => modifier.Stat == stat && modifier.ModifierType == ModifierType.Flat).Sum(modifier => modifier.Modifier));
        
        input *= 1 + Upgrades.Sum(upgrade => upgrade.Modifiers.Where(modifier => modifier.Stat == stat && modifier.ModifierType == ModifierType.Flat).Sum(modifier => modifier.Modifier));

        return input;
    }

    /// <summary>
    /// Gets the total modifier of the specified type on the specified stat.
    /// </summary>
    /// <param name="stat">Which stat to query.</param>
    /// <param name="modifierType">Which modifier type to return.</param>
    /// <returns>The requested modifier.</returns>
    public float GetModifier(Stat stat, ModifierType modifierType)
    {
        return Upgrades.Sum(upgrade => upgrade.Modifiers.Where(modifier => modifier.Stat == stat &&  modifier.ModifierType == modifierType).Sum(modifier => modifier.Modifier));
    }

    /// <summary>
    /// Tries to add an upgrade.
    /// </summary>
    /// <param name="upgrade">The upgrade to add.</param>
    /// <returns>True if the upgrade was added. False if the upgrade was singleton and an instance was already present.</returns>
    public bool TryAddUpgrade(Upgrade upgrade)
    {
        if (upgrade.IsSingleton)
        {
            if (Upgrades.Contains(upgrade))
            {
                Debug.LogWarning("Tried to add second instance of singleton upgrade!");
                return false;
            }
        }
        else
        {
            Upgrades.Add(upgrade);
        }
        return true;
    }

    /// <summary>
    /// Tries to remove an upgrade.
    /// </summary>
    /// <param name="upgrade">The upgrade to remove.</param>
    /// <returns>True if the upgrade was removed. False if the upgrade was not found.</returns>
    public bool TryRemoveUpgrade(Upgrade upgrade)
    {
        return Upgrades.Remove(upgrade);
    }
}