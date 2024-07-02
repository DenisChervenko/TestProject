using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy/BaseEnemy")]
public class EnemyInfo : ScriptableObject
{
    public Sprite icon;
    new public string name;

    public int health;
    public int damage;
}
