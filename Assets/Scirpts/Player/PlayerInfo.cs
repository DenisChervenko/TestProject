using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInfo", menuName = "Player/PlayerInfo")]
public class PlayerInfo : ScriptableObject
{
    new public string name;
    public int health;
    
    public int pistolDamage;
    public int riffleDamage;
}
