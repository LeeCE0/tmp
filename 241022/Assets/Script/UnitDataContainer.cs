using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unit
{
    public class UnitDataContainer : MonoBehaviour
    {
        private static UnitDataContainer instance;
        public static UnitDataContainer Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<UnitDataContainer>();
                return instance;
            }
        }
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
            public eUnitType UnitType { get; set; } 

            public GameObject UnitPrefabs { get; set; }
            public Sprite PortraitImage { get; set; }


            public UnitData(int key, float speed, string name, int atk, int def, int hp, int cost, float attackDistance, string objPath, string portPath)
            {
                UnitID = key;
                UnitSpeed = speed;
                ATK = atk;
                UnitName = name;
                DEF = def;
                HP = hp;
                Cost = cost;
                AttackDistance = attackDistance;
                UnitPrefabs = Resources.Load<GameObject>(objPath);
                PortraitImage = Resources.Load<Sprite>(portPath);
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
                AttackDistance = data.AttackDistance;
                UnitType = data.UnitType;
            }

            public void SetUnitType(eUnitType type)
            {
                UnitType = type;
            }
        }

        Dictionary<int, UnitData> allUnit = new Dictionary<int, UnitData>();

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject); 
            }
        }

        private void Start()
        {
            // 테이블 모든 유닛 데이터 가져오기
            foreach (var item in DataClass.UnitTable_UnitDataTData)
            {
                UnitData unitData = null;
                if (!allUnit.ContainsKey(item.Key))
                {
                    unitData = new UnitData (item.Key, item.Value.UnitSpeed, item.Value.UnitName, item.Value.ATK, item.Value.DEF, item.Value.HP, item.Value.Cost, item.Value.AttackDistance, item.Value.PrefabPath, item.Value.PortraitPath);
                    allUnit.Add(item.Key, unitData);
                }
            }
        }

       
    }


}
