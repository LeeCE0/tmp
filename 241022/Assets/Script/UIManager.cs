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
    [SerializeField] Canvas mainCanvas;
    [SerializeField] Canvas popupCanvas;

    public void LoadUI()
    {

    }
}
