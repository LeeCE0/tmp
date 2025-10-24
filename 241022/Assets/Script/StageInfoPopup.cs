using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageInfoPopup : MonoBehaviour, LoopScrollDataSource
{
    public int stageNum;
    [SerializeField] TextMeshProUGUI stageTitleTxt;
    [SerializeField] LoopScrollRect mobScroll;
    [SerializeField] ButtonCustom startButton;

    Dictionary <int, UnitsData> mobList = new Dictionary<int, UnitsData>(); 


    public void OnClickStart()
    {

    }


    public void ProvideData(Transform trans, int index)
    {
        var slot = trans.GetComponent<UnitInfoSlot>(); 
        if (slot == null) return;

        slot.SetSlotData(mobList[index + 1]);
    }
}
