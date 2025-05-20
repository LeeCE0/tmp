using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using static DataClass;


namespace Unit
{
    public class MyInfo : MonoBehaviour
    {
        private static MyInfo instance;
        public static MyInfo Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<MyInfo>();
                return instance;
            }
        }
        Dictionary<int, UnitData> myUnit = new Dictionary<int, UnitData>();

        public enum eUnitState
        {
            Idle,
            Walk,
            Attack,
            Dead,
        }

        public enum eUnitType
        {
            Magician,
            Bower,
            Knight,
            Swordsman,
            Barbarian,
        }


        public class UnitData
        {
            public int UnitID { get; set; }
            public float UnitSpeed { get; set; }
            public string UnitName { get; set; }
            public int ATK { get; set; }
            public int DEF { get; set; }
            public int HP { get; set; }
            public int Cost { get; set; }
            public byte UnitType { get; set; }
            public string AnimPath { get; set; }
            public RuntimeAnimatorController bodyController { get; set; }
            public RuntimeAnimatorController armController { get; set; }
            public RuntimeAnimatorController weaponController { get; set; }
            public RuntimeAnimatorController bulletController { get; set; }

            public GameObject UnitPrefabs { get; set; } 


            public UnitData(int key, float speed, string name, int atk, int def, int hp, string bodyPath, string armPath, string weaponPath = null, string bulletPath = null)
            {
                UnitID = key;
                UnitSpeed = speed;
                UnitName = name;
                ATK = atk;
                DEF = def;
                HP = hp;
                if (!string.IsNullOrWhiteSpace(bodyPath)) bodyController = Resources.Load<RuntimeAnimatorController>(bodyPath);
                if (!string.IsNullOrWhiteSpace(armPath)) armController = Resources.Load<RuntimeAnimatorController>(armPath);
                if (!string.IsNullOrWhiteSpace(weaponPath)) weaponController = Resources.Load<RuntimeAnimatorController>(weaponPath);
                if(!string.IsNullOrWhiteSpace(bulletPath)) bulletController = Resources.Load<RuntimeAnimatorController>(bulletPath);
            }
            public UnitData(UnitData data)
            {
                UnitID = data.UnitID;
                UnitSpeed = data.UnitSpeed;
                UnitName = data.UnitName; 
                ATK = data.ATK;
                DEF = data.DEF;
                HP = data.HP;
                Cost = data.Cost;
                UnitType = data.UnitType;
                bodyController = data.bodyController;
                armController = data.armController;
                weaponController = data.weaponController;
                bulletController = data.bulletController;
            }
        } 

        public void Start()
        {
            myUnit.Clear();

            //테이블 모든 유닛 데이터 가져오기 : 임시, 내 유닛은 따로 설정하기
            foreach(var item in UnitTable_UnitDataTData)
            {
                if (!myUnit.ContainsKey(item.Key))
                    myUnit.Add(item.Key, 
                        new UnitData
                        (item.Key, item.Value.UnitSpeed, item.Value.UnitName, item.Value.ATK, item.Value.DEF, item.Value.HP, item.Value.AnimPath_Body, item.Value.AnimPath_Arm, item.Value.AnimPath_Weapon, item.Value.AnimPath_Bullet));
            }

            SpawnUnitManager.Instance.SetData();
        }

        public Dictionary<int, UnitData> GetAllUnit()

        {
            return myUnit;
        }
    }
}

