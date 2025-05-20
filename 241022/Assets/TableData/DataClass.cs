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
        public string AnimPath_Body { get; set; }
        public string AnimPath_Arm { get; set; }
        public string AnimPath_Weapon { get; set; }
        public string AnimPath_Bullet { get; set; }
        public string Sprite_Body { get; set; }
        public string Sprite_Arm { get; set; }
        public string Sprite_Weapon { get; set; }
        public string Sprite_Bullet { get; set; }
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
            AnimPath_Body = "Animations/Boomer",
            AnimPath_Arm = "Boomer/Animation/bomb_hold_side-overlay",
            AnimPath_Weapon = "",
            AnimPath_Bullet = "Boomer/Animation/bomb",
            Sprite_Body = "Boomer/Sprite/",
            Sprite_Arm = "",
            Sprite_Weapon = "",
            Sprite_Bullet = "",
        });
       UnitTable_UnitDataTData.Add(2, new UnitTable_UnitDataT
        {
            UnitSpeed = 1.2f,
            UnitName = "b",
            ATK = 1,
            DEF = 3,
            HP = 20,
            AnimPath_Body = "Animations/Hammer",
            AnimPath_Arm = "Hammer/Animation/hammer_attack_side-overlay",
            AnimPath_Weapon = "",
            AnimPath_Bullet = "",
            Sprite_Body = "",
            Sprite_Arm = "",
            Sprite_Weapon = "",
            Sprite_Bullet = "",
        });
       UnitTable_UnitDataTData.Add(3, new UnitTable_UnitDataT
        {
            UnitSpeed = 1.5f,
            UnitName = "c",
            ATK = 5,
            DEF = 1,
            HP = 8,
            AnimPath_Body = "",
            AnimPath_Arm = "",
            AnimPath_Weapon = "",
            AnimPath_Bullet = "",
            Sprite_Body = "",
            Sprite_Arm = "",
            Sprite_Weapon = "",
            Sprite_Bullet = "",
        });
    }

}
