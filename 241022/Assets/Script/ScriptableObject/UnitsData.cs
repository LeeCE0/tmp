using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "UnitsData", menuName = "Game Data/UnitsData")]
public class UnitsData : ScriptableObject
{
    public enum LockOnType
    {
        None,
    }

    public enum eUnitType
    {
        Swordsman = 1,
        Barbarian,
        Magician,
        Bower,
        Knight,
    }

    public float unitSpeed;
    public string unitName;
    public int atk;
    public int def;
    public int hp;
    public int cost;
    public float attackDistance;
    public LockOnType lockOnType;
    public string prefabPath;
    public string portraitPath;
}
