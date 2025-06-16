using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using static DataClass;
using static Unit.UnitDataContainer;

public class PlayerDataManager : MonoBehaviour
{
    private static PlayerDataManager instance;
    public static PlayerDataManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PlayerDataManager>();
            return instance;
        }
    }
    Dictionary<int, UnitData> myUnit = new Dictionary<int, UnitData>();


    public void Start()
    {
        myUnit.Clear();

        //테이블 모든 유닛 데이터 가져오기 : 임시, 내 유닛은 따로 설정하기
        foreach (var item in UnitTable_UnitDataTData)
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
