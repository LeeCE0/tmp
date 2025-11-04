using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using Unit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class StageManager : MonoBehaviour
{
    //스테이지 담당하는 매니저
    private static StageManager instance;
    public static StageManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<StageManager>();
            return instance;
        }
    }

    public int startValue = 0; //시작 자원
    public int curCurrency = 0;  // 현재 자원
    public int currencyPerSecond = 10;  // 초당 증가 자원량
    public float forSecond = 5f;

    void Start()
    {
        StartCoroutine(ResourceGain());
    }

    #region Currency
    IEnumerator ResourceGain()
    {
        while (true)
        {
            curCurrency += currencyPerSecond;
            yield return new WaitForSeconds(forSecond);
        }
    }

    public void UseCurrency(int amount)
    {
        curCurrency -= amount;
    }

    public int GetCurrency()
    {
        return curCurrency;
    }

    public bool IsEnoughCurrency(int amount)
    {
        return GetCurrency() >= amount;
    }
    #endregion
}
