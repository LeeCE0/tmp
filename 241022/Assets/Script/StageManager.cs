using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unit;
using UnityEngine;

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

    public int curStageNum = 0;
    public StageData curStageData = null;
    public int curWaveNum = 0;

    public int curUnitSpawn = 0; //현재 필드에 소환 되어있는 적 유닛 수
    public float minSpawnDelay = 0f; //웨이브 소환 타이머
    



    public int startValue = 0; //시작 자원
    public int curCurrency = 0;  // 현재 자원
    public int currencyPerSecond = 10;  // 초당 증가 자원량
    public float forSecond = 5f;


    public Action<int> OnChangeCurrency = null;
    public Action textShaking = null;

    void Start()
    {
        StartCoroutine(ResourceGain());
    }

    public void SetStage(StageData data)
    {
        curStageData = data;
        curStageNum = data.stageID;
        curWaveNum = 0;
    }

    #region Currency
    IEnumerator ResourceGain()
    {
        while (true)
        {
            curCurrency += currencyPerSecond;
            OnChangeCurrency?.Invoke(curCurrency);
            yield return new WaitForSeconds(forSecond);
        }
    }

    public void UseCurrency(int amount)
    {
        curCurrency -= amount;
        OnChangeCurrency(curCurrency);
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

    #region SpawnData

    public void WaveStart()
    {
        for (int i = 0; i < curStageData.waveData.Length; i++)
        {
            if(curUnitSpawn <= curStageData.triggerThreshold && Time.time >= curStageData.minInterval) //&& 최소 소환딜레이 추가)
            {
                //다음 웨이브 소환
                StartCoroutine(SpawnWave());
                minSpawnDelay = Time.time + curStageData.minInterval;
            }
        }
    }

    IEnumerator SpawnWave()
    {
        SpawnUnitManager.Instance.SpawnUnitFromPool();
        yield return new WaitForSeconds(forSecond);
    }


    #endregion
}
