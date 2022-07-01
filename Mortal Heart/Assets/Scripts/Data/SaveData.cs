using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "SaveData", menuName = "ScriptableObjects/SaveData")]
public class SaveData : ScriptableObject
{
    public bool isNewSaveData = true;
    public float baseMaxHealth = 100f;
    public float baseSpeed = 4f;
}
