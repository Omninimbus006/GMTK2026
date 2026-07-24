using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class StatusManager : MonoBehaviour, IStatusEffectable
{
    public ObservableList<Status> Statuses { get; } = new ObservableList<Status>();
    
    /// <inheritdoc />
    public event IStatusEffectable.StatusModifierChanged OnStatusModifierChanged;

    private void OnStatusesChanged(ObservableList<Status> statuses, ListChangedEventArgs<Status> args)
    {
        List<Stat> invokedStats = new List<Stat>();
        
        if (args.item is Effect effect)
        {
            foreach (StatModifier mod in effect.Modifiers)
            {
                if (!invokedStats.Contains(mod.Stat))
                {
                    OnStatusModifierChanged?.Invoke(new StatModifierChangedEventArgs(mod.Stat));
                    invokedStats.Add(mod.Stat);
                }
            }
        }
    }

    private void Start()
    {
        Statuses.ItemAdded += OnStatusesChanged;
        Statuses.ItemRemoved += OnStatusesChanged;
    }
    
    private void Update()
    {
        for (int i = 0; i < Statuses.Count; i++)
        {
            Status status;
            try
            {
                status = Statuses[i];
            }
            catch (IndexOutOfRangeException)
            {
                break;
            }
            
            status.Tick(Time.deltaTime);
            if (status.Expired)
            {
                Statuses.Remove(status);
                i--;
            }
        }
    }

    /// <summary>
    /// Adds a status to the object.
    /// </summary>
    /// <param name="status">The status to add.</param>
    public void AddStatus(Status status)
    {
        Statuses.Add(status);
    }

    /// <summary>
    /// Tries to remove a status from this object.
    /// </summary>
    /// <param name="status">The status to add.</param>
    /// <returns>True if the status was removed. False if the status was not found.</returns>
    public bool TryRemoveStatus(Status status)
    {
        return Statuses.Remove(status);
    }

    public float ApplyModifiers(float input, Stat stat)
    {
        input += Statuses.Where(status => status is Effect).Sum(status => ((Effect)status).Modifiers.Where(modifier => modifier.Stat == stat && modifier.ModifierType == ModifierType.Flat).Sum(modifier => modifier.Modifier));
        
        input *= 1 + Statuses.Where(status => status is Effect).Sum(status => ((Effect)status).Modifiers.Where(modifier => modifier.Stat == stat && modifier.ModifierType == ModifierType.Multiply).Sum(modifier => modifier.Modifier));
        
        return input;
    }

    public float GetModifiers(Stat stat, ModifierType modifierType)
    {
        return Statuses.Where(status => status is Effect).Sum(status => ((Effect)status).Modifiers.Where(modifier => modifier.Stat == stat && modifierType == modifier.ModifierType).Sum(modifier => modifier.Modifier));
    }
}
