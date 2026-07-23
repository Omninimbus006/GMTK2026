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
        string[] upgrade = new string[];
        string text = "";

        for(int i = 0; i < upgrades.Count; i++)
        {
            
            Upgrade upgradeData = upgrades[i];

            foreach (StatModifier mod in upgradeData.Modifiers)
            {
                //Add modifier type to string to be printed
                text += mod.Stat + " ";

                //If modifier is flat, print +/- amount
                if(mod.ModifierType == ModifierType.Flat)
                {
                    //Check if modifier is positive or negative to print + or -
                    if(mod.Modifier > 0)
                    {
                        text += "+" + mod.Modifier;
                    }
                    else
                    {
                        text += mod.Modifier;
                    }
                }
                //If modifier type is multiplication, print + or - %
                else
                {
                    //Check if modifier is positive or negative to print + or -
                    if(mod.Modifier > 0)
                    {
                        text += "+" + mod.Modifier*100 + "%";
                    }
                    else
                    {
                        text += "-" + mod.Modifier*100 + "%";
                    }
                }

                //Add comma space for next modifier
                text += ", ";
                
            }

            //remove last comma and space and add newline
            text = text.Remove(text.Length - 2);
            text += "\n";
            //add [i] upgrade to list
            upgrade[i] = text;
        }
        
        upgradeList.text = upgrade;
    }
}