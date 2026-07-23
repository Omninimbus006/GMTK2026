using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusManager : MonoBehaviour, IStatusEffectable
{
    public List<Status> Statuses { get; } = new List<Status>();
    
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
