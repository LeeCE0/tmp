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
    }
    public static Dictionary<int, UnitTable_UnitDataT> UnitTable_UnitDataTData = new Dictionary<int, UnitTable_UnitDataT>();
    static DataClass()
    {
       UnitTable_UnitDataTData.Add(1, new UnitTable_UnitDataT
        {
            UnitSpeed = 1f,
            UnitName = "A",
            ATK = 3,
            DEF = 2,
            HP = 10,
            Cost = 20,
            AttackDistance = 2f,
        });
       UnitTable_UnitDataTData.Add(2, new UnitTable_UnitDataT
        {
            UnitSpeed = 1.2f,
            UnitName = "b",
            ATK = 1,
            DEF = 3,
            HP = 20,
            Cost = 40,
            AttackDistance = 4f,
        });
       UnitTable_UnitDataTData.Add(3, new UnitTable_UnitDataT
        {
            UnitSpeed = 1.5f,
            UnitName = "c",
            ATK = 5,
            DEF = 1,
            HP = 8,
            Cost = 80,
            AttackDistance = 4f,
        });
    }

}
