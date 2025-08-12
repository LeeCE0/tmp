using UnityEditor.Animations;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "UnitsData", menuName = "Game Data/UnitsData")]
public class UnitsData : ScriptableObject
{
    public enum eLockOnType
    {
        None,
    }

    public enum eUnitType
    {
        SwordMaster_mine = 1,
        Barbarian_mine,
        Magician_mine,
        Archer_mine,
        Knight_mine,
    }

    public int unitID;
    public float unitSpeed;
    public string unitName;
    public int atk;
    public int def;
    public int hp;
    public int cost;
    public float attackDistance;
    public eLockOnType lockOnType;
    public Sprite spriteImg;
    public Sprite portraitImg;
    public AnimatorController animCtrl;
    public eUnitType unitType; 
}
