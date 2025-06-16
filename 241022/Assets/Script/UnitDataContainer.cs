using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    public class UnitDataContainer : MonoBehaviour
    {
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

    }


}
