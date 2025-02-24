using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Items/Item", order = 1)]
public class ItemDef : ScriptableObject
{
    public string itemName;
    public int points;
    public bool isEvil;
}
