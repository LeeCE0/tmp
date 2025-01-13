using System.Collections.Generic;

public static class DataClass
{
    public class UnitTable_Sheet1
    {
        public float UnitSpeed { get; set; }
        public string UnitName { get; set; }
        public int ATK { get; set; }
        public int DEF { get; set; }
        public int HP { get; set; }
    }
    public static Dictionary<int, UnitTable_Sheet1> UnitTable_Sheet1Data = new Dictionary<int, UnitTable_Sheet1>();
    static DataClass()
    {
       UnitTable_Sheet1Data.Add(1, new UnitTable_Sheet1
        {
            UnitSpeed = 0.2f,
            UnitName = "A",
            ATK = 3,
            DEF = 2,
            HP = 10,
        });
       UnitTable_Sheet1Data.Add(2, new UnitTable_Sheet1
        {
            UnitSpeed = 2f,
            UnitName = "b",
            ATK = 1,
            DEF = 3,
            HP = 20,
        });
       UnitTable_Sheet1Data.Add(3, new UnitTable_Sheet1
        {
            UnitSpeed = 6f,
            UnitName = "c",
            ATK = 5,
            DEF = 1,
            HP = 8,
        });
    }

}
