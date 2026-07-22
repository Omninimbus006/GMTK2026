using System;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    public readonly List<Status> Statuses = new List<Status>();
    
    private void Update()
    {
        for (int i = 0; i < Statuses.Count; i++)
        {
            Status status = null;
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
}
