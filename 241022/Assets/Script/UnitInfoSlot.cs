using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unit;
using UnityEngine;
using UnityEngine.UI;
using static Unit.UnitDataContainer;

public class UnitInfoSlot : MonoBehaviour
{

    [SerializeField] int ID = 0;
    [SerializeField] Image portrait;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] Image costImage;


    public void SetSlotData(UnitData data)
    {
        ID = data.UnitID;
        costText.text = data.Cost.ToString("N0");
    }
}
