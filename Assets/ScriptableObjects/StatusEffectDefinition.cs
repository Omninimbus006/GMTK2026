using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffectDefinition", menuName = "Scriptable Objects/StatusEffectDefinition")]
public class StatusEffectDefinition : ScriptableObject
{
    /// <summary>
    /// NON-UNIQUE name of the status.
    /// </summary>
    [field:SerializeField]
    public string Name { get; set; }
    
    /// <summary>
    /// Should the status no longer apply its effect when paused.
    /// </summary>
    [field:SerializeField]
    public bool DisableWhenPaused { get; set; }
    
    /// <summary>
    /// The duration of the status. Set to -1 to make it infinite.
    /// </summary>
    [field:SerializeField]
    public float Duration { get; set; }

    [field:SerializeField]
    private List<StatModifier> Modifiers { get; set; }

    public StatusEffect StatusEffect
    {
        get
        {
            return new StatusEffect(Name, Duration, Modifiers, DisableWhenPaused);
        }
    }
}
