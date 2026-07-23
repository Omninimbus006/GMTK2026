using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class UpgradeMenu : MonoBehaviour
{

    public TMP_Text upgrade1;
	public TMP_Text upgrade2;
    public TMP_Text upgrade3;
	public TMP_Text upgrade4;

    private List<Upgrade> storedUpgrades;
    private Action<Upgrade> callback;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenMenu(List<Upgrade> upgrades, Action<Upgrade> callback)
    {
        storedUpgrades = upgrades;
        this.callback = callback;


        string[] upgrade = new string[4];

        for(int i = 0; i < 4; i++)
        {
            string text = "";

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

            //remove last comma and space
            text = text.Remove(text.Length - 2);
            //add [i] upgrade to list
            upgrade[i] = text;
        }
        //Take right upgrade and print to correct button
        upgrade1.text = upgrade[0];
        upgrade2.text = upgrade[1];
        upgrade3.text = upgrade[2];
        upgrade4.text = upgrade[3];
    }

    public void upgradeButtonPressed(int upgrade)
    {
        callback?.Invoke(storedUpgrades[upgrade]);
    }
}
