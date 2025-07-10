using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageSlot : MonoBehaviour
{
    [SerializeField] int stageNum;
    [SerializeField] string stageName;

    [SerializeField] GameObject conqueredIcon;
    [SerializeField] GameObject unConqueredIcon;

    [SerializeField] ButtonCustom stageButton;


    public void Awake()
    {
        stageButton.OnClickAddListener(OpenInfoPopup);
    }
    public void SetStageInfo(int num, bool isClear)
    {
        stageNum = num;
        conqueredIcon.SetActive(isClear);
        unConqueredIcon.SetActive(!isClear);
    }

    public void OpenInfoPopup()
    {

    }

}
