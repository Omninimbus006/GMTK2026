using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleUpgradeDefinition", menuName = "Scriptable Objects/SimpleUpgradeDefinition")]
public class SimpleUpgradeDefinition : ScriptableObject, Upgrade
{
    [field: SerializeField]
    public List<StatModifier> Modifiers { get; set; } = new List<StatModifier>();
    
    /// <summary>
    /// NON-UNIQUE name of the upgrade to display to the player.
    /// </summary>
    [field: SerializeField]
    public string Name { get; set; }

    /// <inheritdoc />
    [field: SerializeField]
    public bool IsSingleton { get; set; }

    /// <inheritdoc />
    [field: SerializeField]
    public Rarity Rarity { get; set; }
}
