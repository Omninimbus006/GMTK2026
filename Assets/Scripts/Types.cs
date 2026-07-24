// Empty for now, this file is mostly for storing all our enums, structs, and static/instanceable classes in one place

using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Rendering;

public class Types
{
    
}

[Serializable]
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

[Serializable]
public enum ModifierType
{
    Flat,
    Multiply
}

[Serializable]
public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
}

[Flags]
public enum DestroyOptions
{
    None = 0,
    Effects = 1,
    Disable = 2,
    Destroy = 4,
}

public interface Collectible
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
    public ObservableList<Upgrade> Upgrades { get; }
    
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

    public delegate void UpgradeModifierChanged(StatModifierChangedEventArgs args);

    public event UpgradeModifierChanged OnUpgradeModifierChanged;
}

public class StatModifierChangedEventArgs : EventArgs
{
    public Stat Stat { get; }
    
    public StatModifierChangedEventArgs(Stat stat)
    {
        Stat = stat;
    }
}

// Interface to allow modular use of upgrades
public interface Upgrade : Collectible
{
    public List<StatModifier> Modifiers { get; }
    
    /// <summary>
    /// Only allow one instance of this upgrade per object.
    /// </summary>
    public bool IsSingleton { get; }
}

public interface Weapon : Collectible
{
    public WeaponControl WeaponControl { get; }
    
    public float BaseDamage { get; }
    
    public float BaseReloadTime { get; }

    public void OnPrimaryFire();
    
    public void OnSecondaryFire();

    public void OnReload();
}

public interface IDamageable
{
    public float HP { get; }
    
    public float MaxHP { get; }
    
    /// <summary>
    /// Damage the entity, if possible.
    /// This is a backup method for when you don't have an AttackValues object.
    /// </summary>
    /// <param name="amount">The amount of damage to deal</param>
    /// <param name="source">EffectSource of the attacker</param>
    /// <param name="direct">Should this damage bypass resistance</param>
    // ReSharper disable Unity.PerformanceAnalysis
    public void Damage(float amount, EffectSource source, bool direct = false);

    /// <summary>
    /// Restores hitpoints to the object, if possible. Does not affect shields or armor.
    /// </summary>
    /// <param name="amount">How many hitpoints to restore.</param>
    /// <param name="source">The source of the repair.</param>
    public void Repair(float amount, EffectSource source);
    
    public delegate void DamageEventHandler(DamageEventArgs args);
    public delegate void DamagedEventHandler(DamagedEventArgs args);
    public delegate void RepairEventHandler(RepairEventArgs args);
    public delegate void RepairedEventHandler(RepairedEventArgs args);
    
    public event DamageEventHandler OnDamage;
    public event DamagedEventHandler OnDamaged;
    public event RepairEventHandler OnRepair;
    public event RepairedEventHandler OnRepaired;
}

public class DamageEventArgs : CancelEventArgs
{
    public float Amount { get; set; }
    public EffectSource Source { get; }
    public DamageEventArgs(float amount, EffectSource source)
    {
        Amount = amount;
        Source = source;
    }
}

public class DestroyEventArgs : CancelEventArgs
{
    public EffectSource Source { get; }
    public DestroyEventArgs(EffectSource source)
    {
        Source = source;
    }
}

public class RepairEventArgs : CancelEventArgs
{
    public float Amount { get; set; }
    public EffectSource Source { get; }
    public RepairEventArgs(float amount, EffectSource source)
    {
        Amount = amount;
        Source = source;
    }
}

public class DamagedEventArgs : EventArgs
{
    public float Amount { get; }
    
    public EffectSource Source { get; }
    
    public DamagedEventArgs(float amount, EffectSource source)
    {
        this.Amount = amount;
        this.Source = source;
    }
}

public class DestroyedEventArgs : EventArgs
{
    public EffectSource Source { get; }

    public DestroyedEventArgs(EffectSource source)
    {
        this.Source = source;
    }
}

public enum SourceType
{
    Player,
    Entity,
    Environment,
    Special
}

public struct EffectSource
{
    /// <summary>
    /// What type of source is causing the effect.
    /// </summary>
    public SourceType sourceType;
    /// <summary>
    /// The object that is causing the effect.
    /// May be null if the source is not an object (e.g., environmental damage).
    /// </summary>
    public GameObject sourceObject;
}

public class RepairedEventArgs : EventArgs
{
    public float Amount { get; }

    public EffectSource Source { get; }

    public RepairedEventArgs(float amount, EffectSource source)
    {
        this.Amount = amount;
        this.Source = source;
    }
}

public interface IDestroyable
{
    /// <summary>
    /// Destroys the entity, applying any necessary effects or cleanup.
    /// </summary>
    /// <param name="source">The source of the destruction.</param>
    public void DestroyComponent(EffectSource source);
    
    public delegate void DestroyEventHandler(DestroyEventArgs args);
    public delegate void DestroyedEventHandler(DestroyedEventArgs args);
    
    public event DestroyEventHandler OnDestroy;
    public event DestroyedEventHandler OnDestroyed;
}

[Serializable]
public class StatModifier
{
    /// <summary>
    /// The amount to modify the stat by.
    /// </summary>
    [field:SerializeField]
    public float Modifier { get; set; }
    
    /// <summary>
    /// Which stat to modify.
    /// </summary>
    [field:SerializeField]
    public Stat Stat { get; set; }
    
    /// <summary>
    /// What operation (addition/subtraction, multiplication, division) to use.
    /// </summary>
    [field:SerializeField]
    public ModifierType ModifierType { get; set; }
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

[Serializable]
public enum ExpireStackAction
{
    Add,
    Remove,
    Expire
}

[Serializable]
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

public interface Effect
{
    public ObservableList<StatModifier> Modifiers { get; }
}

public class StatusEffect : Status, Effect
{
    public ObservableList<StatModifier> Modifiers { get; }
    
    public StatusEffect(string name, float duration, List<StatModifier> modifiers, bool disableWhenPaused = true) : base(name, duration, disableWhenPaused)
    {
        Modifiers = new ObservableList<StatModifier>(modifiers);
    }
}

public class StackableStatusEffect : StackableStatus, Effect
{
    public ObservableList<StatModifier> Modifiers { get; }
    
    public StackableStatusEffect(string name, float duration, int stacks, int maxStacks, List<StatModifier> modifiers, ExpireStackAction expireAction, OverflowStackAction overflowAction, bool disableWhenPaused = true) : base(name, duration, stacks, maxStacks, expireAction, overflowAction, disableWhenPaused)
    {
        Modifiers = new ObservableList<StatModifier>(modifiers);
    }
}

public interface IStatusEffectable
{
    /// <summary>
    /// The list of statuses on the object.
    /// </summary>
    public ObservableList<Status> Statuses { get; }

    /// <summary>
    /// Adds a status to the object.
    /// </summary>
    /// <param name="status">The status to add.</param>
    public void AddStatus(Status status);
    
    /// <summary>
    /// Tries to remove a status from the object.
    /// </summary>
    /// <param name="status">The status to remove.</param>
    /// <returns>True if the status was removed. False if the status was not found.</returns>
    public bool TryRemoveStatus(Status status);

    public delegate void StatusModifierChanged(StatModifierChangedEventArgs args);
    
    public event  StatusModifierChanged OnStatusModifierChanged;
}