// Empty for now, this file is mostly for storing all our enums, structs, and static/instanceable classes in one place

using System;
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


public class Status
{
    /// <summary>
    /// NON-UNIQUE name of the status.
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Whether or not the status is active (not paused).
    /// </summary>
    public bool Active { get; protected set; }
    
    /// <summary>
    /// Should the status no longer apply its effect when paused.
    /// </summary>
    public bool DisableWhenPaused { get; }
    
    /// <summary>
    /// The duration of the status. Set to -1 to make it infinite.
    /// </summary>
    public float Duration { get; }
    
    /// <summary>
    /// Counts down until effect expires.
    /// </summary>
    public float Timer { get; protected set; }

    /// <summary>
    /// Has the effect expired. Cannot be unset once set.
    /// </summary>
    public bool Expired { get; protected set; }
    
    /// <summary>
    /// Event that fires when the status expires.
    /// </summary>
    public event Action OnExpired;

    /// <summary>
    /// Updates the status effect.
    /// </summary>
    public virtual void Tick(float deltaTime)
    {
        if (Active)
        {
            Timer -= deltaTime;

            if (Timer <= 0)
            {
                Expired = true;
                OnExpired?.Invoke();
            }
        }
    }
    
    /// <summary>
    /// Add time to the timer.
    /// </summary>
    /// <param name="time">How much time to add in seconds.</param>
    public virtual void AddTime(float time)
    {
        Timer += time;
    }

    /// <summary>
    /// Remove time from the timer.
    /// </summary>
    /// <param name="time">How much time to remove in seconds.</param>
    public virtual void RemoveTime(float time)
    {
        Timer -= time;

        if (Timer <= 0)
        {
            Expire();
        }
    }

    public virtual void Pause()
    {
        Active = false;
    }

    public virtual void Resume()
    {
        Active = true;
    }

    public virtual void Expire()
    {
        Expired = true;
        OnExpired?.Invoke();
    }

    public Status(string name, float duration, bool disableWhenPaused = true)
    {
        Name = name;
        Active = true;
        DisableWhenPaused = disableWhenPaused;
        Duration = duration;
        Timer = duration;
        Expired = false;
    }
}

public enum ExpireStackAction
{
    Add,
    Remove,
    Expire
}

public enum OverflowStackAction
{
    Add,
    Remove,
    Nothing
}

public class StackableStatus : Status
{
    public int Stacks { get; private set; }
    
    public int MaxStacks { get; }
    
    public ExpireStackAction ExpireAction { get; }
    
    public OverflowStackAction OverflowAction { get; }
    
    public StackableStatus(string name, float duration, int stacks, int maxStacks, ExpireStackAction expireAction, OverflowStackAction overflowAction, bool disableWhenPaused = true) : base(name, duration, disableWhenPaused)
    {
        Stacks = stacks;
        MaxStacks = maxStacks;
        ExpireAction = expireAction;
        OverflowAction = overflowAction;
    }

    public override void Tick(float deltaTime)
    {
        if (Active)
        {
            Timer -= deltaTime;

            if (Timer <= 0)
            {
                switch (ExpireAction)
                {
                    case ExpireStackAction.Add:
                        if (Stacks < MaxStacks)
                        {
                            Stacks++;
                        }
                        Timer = Duration;
                        break;
                    
                    case ExpireStackAction.Remove:
                        Stacks--;
                        if (Stacks <= 0)
                        {
                            Expire();
                            return;
                        }
                        Timer = Duration;
                        break;
                    
                    default:
                    case ExpireStackAction.Expire:
                        Expired = true;
                        Expire();
                        break;
                }
            }
        }
    }

    public override void AddTime(float time)
    {
        Timer += time;

        if (Timer >= Duration)
        {
            switch (OverflowAction)
            {
                case OverflowStackAction.Add:
                    Stacks++;
                    Timer = Duration;
                    break;
                case OverflowStackAction.Remove:
                    Stacks--;
                    Timer = Duration;
                    break;
                default:
                case OverflowStackAction.Nothing:
                    break;
            }
        }
    }

    public override void RemoveTime(float time)
    {
        Timer -= time;

        if (Timer <= Duration)
        {
            switch (ExpireAction)
            {
                case ExpireStackAction.Add:
                    Stacks++;
                    Timer = Duration;
                    break;
                    
                case ExpireStackAction.Remove:
                    Stacks--;
                    if (Stacks <= 0)
                    {
                        Expire();
                        return;
                    }
                    Timer = Duration;
                    break;
                    
                default:
                case ExpireStackAction.Expire:
                    Expired = true;
                    Expire();
                    break;
            }
        }
    }

    public void AddStacks(int stacks)
    {
        Stacks += stacks;
        if (Stacks >= MaxStacks)
        {
            Stacks = MaxStacks;
        }
    }

    public void RemoveStacks(int stacks)
    {
        Stacks -= stacks;
        if (Stacks <= 0)
        {
            Expire();
        }
    }
}

public class StatusEffect : Status
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
    
    public StatusEffect(string name, float duration, float modifier, Stat stat, ModifierType modifierType, bool disableWhenPaused = true) : base(name, duration, disableWhenPaused)
    {
        Modifier = modifier;
        Stat = stat;
        ModifierType = modifierType;
    }
}

public class StackableStatusEffect : StackableStatus
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
    
    public StackableStatusEffect(string name, float duration, int stacks, int maxStacks, float modifier, Stat stat, ModifierType modifierType, ExpireStackAction expireAction, OverflowStackAction overflowAction, bool disableWhenPaused = true) : base(name, duration, stacks, maxStacks, expireAction, overflowAction, disableWhenPaused)
    {
        Modifier = modifier;
        Stat = stat;
        ModifierType = modifierType;
    }
}