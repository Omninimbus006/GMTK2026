using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(StatsManager))]
public class DamageController : MonoBehaviour, IDamageable, IDestroyable
{
    private StatsManager statsManager;

    [SerializeField]
    protected DestroyOptions destroyOptions = DestroyOptions.None;

    public bool Invulnerable
    {
        get { return invulnerable; }
    }

    protected bool invulnerable;

    public float HP
    {
        get { return hp; }
    }

    private float hp;

    public float MaxHP
    {
        get { return maxHP; }
    }

    private float maxHP;

    [SerializeField]
    private GameObject destroyEffectPrefab;

    /// <inheritdoc />
    public event IDamageable.DamageEventHandler OnDamage;

    /// <inheritdoc />
    public event IDamageable.DamagedEventHandler OnDamaged;

    /// <inheritdoc />
    public event IDamageable.RepairEventHandler OnRepair;

    /// <inheritdoc />
    public event IDamageable.RepairedEventHandler OnRepaired;

    /// <inheritdoc />
    public event IDestroyable.DestroyEventHandler OnDestroy;

    /// <inheritdoc />
    public event IDestroyable.DestroyedEventHandler OnDestroyed;

    private void Start()
    {
        statsManager = GetComponent<StatsManager>();
        statsManager.OnStatModifierChanged += OnStatChanged;
        maxHP = statsManager.GetStat(Stat.Health);
    }

    private void OnStatChanged(StatModifierChangedEventArgs args)
    {
        if (args.Stat == Stat.Health)
        {
            float newHealth = statsManager.GetStat(Stat.Health);

            if (newHealth > maxHP)
            {
                hp += newHealth - maxHP;
                maxHP = newHealth;
            }
            else
            {
                maxHP = newHealth;
                if (hp > maxHP)
                {
                    hp = maxHP;
                }
            }
        }
    }

protected virtual void Internal_Damage(float amount, EffectSource source)
    {
        hp -= amount;

        if (HP <= 0)
        {
            Internal_Destroy(source);
        }
    }

    protected virtual void Internal_Destroy(EffectSource source)
    {
        if (destroyOptions.HasFlag(DestroyOptions.Effects))
        {
            PlayDestroyEffects();
        }
        if (destroyOptions.HasFlag(DestroyOptions.Destroy))
        {
            Object.Destroy(gameObject);
        }
        else if (destroyOptions.HasFlag(DestroyOptions.Disable))
        {
            gameObject.SetActive(false);
        }
    }

    protected virtual void Internal_Repair(float amount, EffectSource source)
    {
        hp = Mathf.Min(hp + amount, MaxHP);
    }

    /// <summary>
    /// Backup method for when you don't have an AttackValues object.
    /// </summary>
    /// <param name="amount">The amount of damage to deal</param>
    /// <param name="source">EffectSource of the attacker</param>
    /// <param name="direct">Should this damage bypass resistance</param>
    public void Damage(float amount, EffectSource source, bool direct = false)
    {
        DamageEventArgs e = new DamageEventArgs(amount, source);
        InvokeOnDamage(e); 
        if (e.Cancel) 
        {
            return;
        }

        Internal_Damage(amount, source);

        DamagedEventArgs damagedEventArgs = new DamagedEventArgs(amount, source);
        OnDamaged?.Invoke(damagedEventArgs);
    }

    /// <summary>
    /// Restores hitpoints to the entity, if possible. Does not apply to shields or armor.
    /// </summary>
    /// <param name="amount">How much HP to restore</param>
    /// <param name="source">The source of the repair</param>
    public void Repair(float amount, EffectSource source)
    {
        RepairEventArgs e = new RepairEventArgs(amount, source);
        InvokeOnRepair(e);
        if (e.Cancel)
        {
            return;
        }

        Internal_Repair(amount, source);

        RepairedEventArgs repairedEventArgs = new RepairedEventArgs(amount, source);
        OnRepaired?.Invoke(repairedEventArgs);
    }

    /// <summary>
    /// Destroy the object immediately, bypassing immunity.
    /// </summary>
    /// <param name="source">The source of the destruction</param>
    public void DestroyComponent(EffectSource source)
    {
        DestroyEventArgs e = new DestroyEventArgs(source);
        InvokeOnDestroy(e);
        if (e.Cancel)
        {
            return;
        }

        Internal_Destroy(source);

        DestroyedEventArgs destroyedEventArgs = new DestroyedEventArgs(source);
        OnDestroyed?.Invoke(destroyedEventArgs);
    }

    public virtual void SetInvulnerable(bool value)
    {
        invulnerable = value;
    }

    protected void InvokeOnDamage(DamageEventArgs e)
    {
        OnDamage?.Invoke(e);
    }

    protected void InvokeOnDestroy(DestroyEventArgs e)
    {
        OnDestroy?.Invoke(e);
    }

    protected void InvokeOnRepair(RepairEventArgs e)
    {
        OnRepair?.Invoke(e);
    }
    
    private void PlayDestroyEffects()
    {
        if (destroyEffectPrefab != null)
        {
            Instantiate(destroyEffectPrefab, transform.position, transform.rotation);
        }
    }
}
