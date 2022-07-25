using UnityEngine;
using System;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "AllEnemyData", menuName = "ScriptableObjects/AllEnemyData")]
public class AllEnemyData : ScriptableObject
{
    [TableList]
    public EnemyData[] EnemyList;
}

[Serializable]
public class EnemyData
{
    public BaseEnemyController enemy;
    public int diffValue;
    public int floor;
}