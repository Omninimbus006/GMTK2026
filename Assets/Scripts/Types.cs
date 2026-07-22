// Empty for now, this file is mostly for storing all our enums, structs, and static/instanceable classes in one place

using System.Collections.Generic;
using UnityEngine;

public class Types
{
    
}

public enum Stat
{
    Health,
    Regen,
    Speed,
    Jumps,
    DamageReduction,
    MagSize,
    ReloadSpeed,
    Luck,
    CritChance,
    CritMultiplier,
    BaseDamage,
    WeaponDamage,
    OffensiveAbilityScaling,
    DefensiveAbilityScaling,
    UtilityAbilityScaling,
}

public enum ModifierType
{
    Flat,
    Multiply,
    Divide
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
}

public interface ICollectible
{
    /// <summary>
    /// The rarity of the upgrade, determines chance to drop. 
    /// </summary>
    public Rarity Rarity { get; }
}

public interface IUpgradable
{
    /// <summary>
    /// List of all upgrades on the object.
    /// </summary>
    public List<Upgrade> Upgrades { get; }
    
    /// <summary>
    /// Tries to add an upgrade to the object.
    /// </summary>
    /// <param name="upgrade">The upgrade to add.</param>
    /// <returns>True if the upgrade was added. False if the upgrade was singleton and an instance was already present.</returns>
    public bool TryAddUpgrade(Upgrade upgrade);
    
    /// <summary>
    /// Tries to remove an upgrade from the object.
    /// </summary>
    /// <param name="upgrade">The upgrade to remove.</param>
    /// <returns>True if the upgrade was removed. False if the upgrade was not found.</returns>
    public bool TryRemoveUpgrade(Upgrade upgrade);
}

// Interface to allow modular use of upgrades
public interface Upgrade : ICollectible
{
    /// <summary>
    /// The amount to modify the stat by.
    /// </summary>
    public float Modifier { get; }
    
    /// <summary>
    /// Which stat to modify.
    /// </summary>
    public Stat Stat { get; }
    
    /// <summary>
    /// What operation (addition/subtraction, multiplication, division) to use.
    /// </summary>
    public ModifierType ModifierType { get; }
    
    /// <summary>
    /// Only allow one instance of this upgrade per object.
    /// </summary>
    public bool IsSingleton { get; }
}