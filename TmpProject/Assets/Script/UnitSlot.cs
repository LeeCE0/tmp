using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSlot : MonoBehaviour
{
    [SerializeField] ButtonCustom slotBtn;

    public int slotIndex;
    Action<int> spawnAct;

    public void Awake()
    {
        slotBtn.OnClickAddListener(OnClickSlot);
        spawnAct = null;
    }

    public void SetData(int index, Action<int> callback)
    {
        slotIndex = index;
        spawnAct = callback;
    }


    public void OnClickSlot()
    {
        spawnAct?.Invoke(slotIndex);
    }

}
