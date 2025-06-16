using System.Collections;
using System.Collections.Generic;
using Unit;
using UnityEngine;
using UnityEngine.UI;
using static DataClass;
using static Unit.UnitDataContainer;

public class MyUnitLayout : MonoBehaviour, LoopScrollDataSource
{
    [SerializeField] LoopScrollRect scrollRect;
    [SerializeField] UnitInfoSlot unitSlot;
    Dictionary<int, UnitData> unitList = new Dictionary<int, UnitData>();

    public void Start()
    {
        foreach (var item in UnitTable_UnitDataTData)
        {
            if (!unitList.ContainsKey(item.Key))
                unitList.Add(item.Key,
                    new UnitData
                    (item.Key, item.Value.UnitSpeed, item.Value.UnitName, item.Value.ATK, item.Value.DEF, item.Value.HP, item.Value.Cost, item.Value.AttackDistance));
        }

        scrollRect.totalCount = unitList.Count;
        scrollRect.Initialize(this);
        scrollRect.RefillCells(); 
    }

    public void ActiveUI()
    {
        scrollRect.totalCount = unitList.Count;
        scrollRect.RefillCells();
    }

    public void RefreshUI()
    {
        scrollRect.totalCount = unitList.Count;
        scrollRect.RefreshCells();
    }

    public  void ProvideData(Transform trans, int index)
    {
        var slot = trans.GetComponent<UnitInfoSlot>();

        if (slot == null) return;

        slot.SetSlotData(unitList[index + 1]);
    }
}
