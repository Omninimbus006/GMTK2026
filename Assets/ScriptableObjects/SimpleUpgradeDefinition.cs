using UnityEngine;

[CreateAssetMenu(fileName = "SimpleUpgradeDefinition", menuName = "Scriptable Objects/SimpleUpgradeDefinition")]
public class SimpleUpgradeDefinition : ScriptableObject, Upgrade
{
    /// <summary>
    /// NON-UNIQUE name of the upgrade to display to the player.
    /// </summary>
    [field: SerializeField]
    public string Name { get; set; }
    
    /// <inheritdoc/>
    [field: SerializeField]
    public float Modifier { get; set; }
    
    /// <inheritdoc/>
    [field: SerializeField]
    public Stat Stat { get; set; }
    
    /// <inheritdoc/>
    [field: SerializeField]
    public ModifierType ModifierType { get; set; }

    /// <inheritdoc />
    [field: SerializeField]
    public bool IsSingleton { get; set; }

    /// <inheritdoc />
    [field: SerializeField]
    public Rarity Rarity { get; set; }
}
