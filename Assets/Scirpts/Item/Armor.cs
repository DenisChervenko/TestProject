using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Armor", menuName = "Items/Armor")]
public class Armor : Item
{
    public int protection;
    public string typeEquipment;
    public bool isEquiped;
}
