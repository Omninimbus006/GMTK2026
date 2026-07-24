using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class UpgradeList : MonoBehaviour
{
    public TMP_Text upgradeList;

    public void OpenMenu(List<Upgrade> upgrades)
    {
        List<string> upgradeStrings = new List<string>();

        for (int i = 0; i < upgrades.Count; i++)
        {
            string text = "";

            Upgrade upgradeData = upgrades[i];

            foreach (StatModifier mod in upgradeData.Modifiers)
            {
                // Add modifier type
                text += mod.Stat + " ";

                // Flat modifier
                if (mod.ModifierType == ModifierType.Flat)
                {
                    if (mod.Modifier > 0)
                    {
                        text += "+" + mod.Modifier;
                    }
                    else
                    {
                        text += mod.Modifier;
                    }
                }
                // Percentage modifier
                else
                {
                    if (mod.Modifier > 0)
                    {
                        text += "+" + (mod.Modifier * 100) + "%";
                    }
                    else
                    {
                        text += (mod.Modifier * 100) + "%";
                    }
                }

                // Separator between modifiers
                text += ", ";
            }

            // Remove the last ", " if there were modifiers
            if (upgradeData.Modifiers.Count > 0)
            {
                text = text.Remove(text.Length - 2);
            }

            // Add this upgrade to the list
            upgradeStrings.Add(text);
        }

        // Display all upgrades with a newline between each one
        upgradeList.text = string.Join("\n", upgradeStrings);
    }
}
