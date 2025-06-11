using System.Collections;
using System.Collections.Generic;
using Unit;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoSlot : MonoBehaviour
{

    [SerializeField] int ID = 0;


    public void SetSlotData(MyInfo.UnitData data)
    {
        ID = data.UnitID;
    }
}
