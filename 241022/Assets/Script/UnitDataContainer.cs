using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

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

            public bool IsMyUnit { get; set; }

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

            public void SetTeam(bool isMine)
            {
                IsMyUnit = isMine;
            }
        }
        [SerializeField] UnitsDataList unitsDataList;
        Dictionary<int, UnitsData> allUnit = new Dictionary<int, UnitsData>();

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
            unitsDataList.Init();
            // 모든 유닛 데이터 가져오기
            foreach (var item in unitsDataList.GetAllUnitData())
            {
                if (!allUnit.ContainsKey(item.Key))
                {
                    allUnit.Add(item.Key, item.Value);
                }
            }
        }

        public Dictionary<int, UnitsData> GetAllUnitData()
        {
            return allUnit;
        }
}

    }