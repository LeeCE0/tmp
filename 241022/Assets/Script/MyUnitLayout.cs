using System.Collections;
using System.Collections.Generic;
using Unit;
using UnityEngine;
using UnityEngine.UI;

public class MyUnitLayout : MonoBehaviour, LoopScrollDataSource
{
    [SerializeField] LoopScrollRect scrollRect;
    [SerializeField] UnitInfoSlot unitSlot;
    Dictionary<int, UnitsData> unitList = new Dictionary<int, UnitsData>();

    public void Start()
    {
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
