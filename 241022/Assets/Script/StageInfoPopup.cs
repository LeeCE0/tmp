using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using static Unit.UnitDataContainer;

public class StageInfoPopup : MonoBehaviour, LoopScrollDataSource
{
    public int stageNum;
    [SerializeField] TextMeshProUGUI stageTitleTxt;
    [SerializeField] LoopScrollRect mobScroll;
    [SerializeField] ButtonCustom startButton;

    Dictionary <int, UnitData> mobList = new Dictionary<int, UnitData>(); 


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
