using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public float Hp;
    public float Mana;
    public float Speed;

    public float PlayTime;
    public int EnemyKilled;
}
