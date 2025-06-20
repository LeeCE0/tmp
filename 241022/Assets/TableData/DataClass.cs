using System.Collections.Generic;

public static class DataClass
{
    public class UnitTable_UnitDataT
    {
        public float UnitSpeed { get; set; }
        public string UnitName { get; set; }
        public int ATK { get; set; }
        public int DEF { get; set; }
        public int HP { get; set; }
        public int Cost { get; set; }
        public float AttackDistance { get; set; }
        public LockOnType eLockOnType { get; set; }
        public string PrefabPath { get; set; }
        public string PortraitPath { get; set; }
    }
    public static Dictionary<int, UnitTable_UnitDataT> UnitTable_UnitDataTData = new Dictionary<int, UnitTable_UnitDataT>();
    static DataClass()
    {
       UnitTable_UnitDataTData.Add(1, new UnitTable_UnitDataT
        {
            UnitSpeed = 1.3f,
            UnitName = "전사",
            ATK = 3,
            DEF = 5,
            HP = 30,
            Cost = 20,
            AttackDistance = 2f,
    eLockOnType = (LockOnType)System.Enum.Parse(typeof(LockOnType), "None"),
            PrefabPath = "Sword",
            PortraitPath = "Portraits/Swordsman",
        });
       UnitTable_UnitDataTData.Add(2, new UnitTable_UnitDataT
        {
            UnitSpeed = 1f,
            UnitName = "마법사",
            ATK = 6,
            DEF = 1,
            HP = 15,
            Cost = 60,
            AttackDistance = 4f,
    eLockOnType = (LockOnType)System.Enum.Parse(typeof(LockOnType), "None"),
            PrefabPath = "Magician",
            PortraitPath = "Portraits/Magician",
        });
       UnitTable_UnitDataTData.Add(3, new UnitTable_UnitDataT
        {
            UnitSpeed = 1.1f,
            UnitName = "궁수",
            ATK = 4,
            DEF = 2,
            HP = 15,
            Cost = 50,
            AttackDistance = 6f,
    eLockOnType = (LockOnType)System.Enum.Parse(typeof(LockOnType), "None"),
            PrefabPath = "Bower",
            PortraitPath = "Portraits/Bower",
        });
       UnitTable_UnitDataTData.Add(4, new UnitTable_UnitDataT
        {
            UnitSpeed = 1.5f,
            UnitName = "버서커",
            ATK = 5,
            DEF = 6,
            HP = 25,
            Cost = 30,
            AttackDistance = 2f,
    eLockOnType = (LockOnType)System.Enum.Parse(typeof(LockOnType), "None"),
            PrefabPath = "Barbarian",
            PortraitPath = "Portraits/Barbarian",
        });
    }

}
public enum LockOnType
{
    None,
}

