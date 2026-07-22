using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradesManager : MonoBehaviour, IUpgradable
{
    public List<Upgrade> Upgrades { get; private set; }

    /// <summary>
    /// Applies all upgrades for a given stat to the given number.
    /// </summary>
    /// <param name="input">The base stat to modify.</param>
    /// <param name="stat">What stat to use for loading modifiers.</param>
    /// <returns>The modified stat.</returns>
    public float ApplyModifiers(float input, Stat stat)
    {
        input += Upgrades.Where(upgrade => upgrade.Stat == stat && upgrade.ModifierType == ModifierType.Flat).Sum(upgrade => upgrade.Modifier);

        input = Upgrades.Where(upgrade => upgrade.Stat == stat && upgrade.ModifierType == ModifierType.Multiply).Aggregate(input, (current, upgrade) => current * upgrade.Modifier);

        return Upgrades.Where(upgrade => upgrade.Stat == stat && upgrade.ModifierType == ModifierType.Divide).Aggregate(input, (current, upgrade) => current / upgrade.Modifier);
    }

    /// <summary>
    /// Gets the total modifier of the specified type on the specified stat.
    /// </summary>
    /// <param name="stat">Which stat to query.</param>
    /// <param name="modifierType">Which modifier type to return.</param>
    /// <returns>The requested modifier.</returns>
    public float GetModifier(Stat stat, ModifierType modifierType)
    {
        return Upgrades.Where(upgrade => upgrade.Stat == stat && upgrade.ModifierType == modifierType).Sum(upgrade => upgrade.Modifier);
    }

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

    public bool TryRemoveUpgrade(Upgrade upgrade)
    {
        return Upgrades.Remove(upgrade);
    }
}