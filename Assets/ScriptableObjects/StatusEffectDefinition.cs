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
    /// Whether or not the status is active (not paused).
    /// </summary>
    [field:SerializeField]
    public bool Active { get; set; }
    
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

    public StatusEffect StatusEffect
    {
        get
        {
            return new StatusEffect(Name, Duration, Modifier, Stat, ModifierType, DisableWhenPaused);
        }
    }
}
