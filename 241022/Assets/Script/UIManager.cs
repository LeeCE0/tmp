using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<UIManager>();
            return instance;
        }
    }
    public enum CavasType
    {
        Main,
        Popup,
    }

    public enum eUIType
    {
        StartUI,
        UnitSelectUI,
        StageUI,
    }


    [SerializeField] Canvas mainCanvas;
    [SerializeField] Canvas popupCanvas;

    [SerializeField] StartPage startPage;
    [SerializeField] UnitSelectUI unitSelectUI;
    [SerializeField] GameObject StageUI;

    public void LoadUI(eUIType uiType)
    {
        switch(uiType)
        {
            case eUIType.StartUI:
                startPage.gameObject.SetActive(true);
                break;
            case eUIType.UnitSelectUI:
                break;
            case eUIType.StageUI:
                break;
        }
    }
}
