using System;
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
            Swordsman = 1,
            Barbarian,
            Magician,
            Bower,
            Knight,
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
            public float AttackDistance { get; set; }
            public byte UnitType { get; set; }

            public GameObject UnitPrefabs { get; set; } 


            public UnitData(int key, float speed, string name, int atk, int def, int hp, int cost, float attackDistance)
            {
                UnitID = key;
                UnitSpeed = speed;
                UnitName = name;
                ATK = atk;
                DEF = def;
                HP = hp;
                Cost = cost;
                AttackDistance = attackDistance;
                UnitType = (byte)UnitID;
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
                AttackDistance = data.AttackDistance;
                UnitType = (byte)data.UnitType;
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
                        (item.Key, item.Value.UnitSpeed, item.Value.UnitName, item.Value.ATK, item.Value.DEF, item.Value.HP, item.Value.Cost, item.Value.AttackDistance));
            }

            SpawnUnitManager.Instance.SetData();
        }

        public Dictionary<int, UnitData> GetAllUnit()
        {
            return myUnit;
        }
    }
}

