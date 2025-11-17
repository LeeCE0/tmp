using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

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
    public WaveData waveDatas = null;
    public int[] spawnUnitList = null;  //스폰 되어야 할 유닛 리스트

    public int curUnitSpawn = 0; //현재 필드에 소환 되어있는 적 유닛 수
    public float minSpawnDelay = 0f; //웨이브 소환 타이머

    public int startValue = 0; //시작 자원
    public int curCurrency = 0;  // 현재 자원
    public int currencyPerSecond = 10;  // 초당 증가 자원량
    public float forSecond = 5f;

    public Action<int> OnChangeCurrency = null;
    public Action textShaking = null;

    public SpawnUnitController spawner = null;


    public Action spawEnemy = null;

    void Start()
    {
    }

    public void SetStage(StageData data)
    {
        curStageData = data;
        curStageNum = data.stageID;
        curWaveNum = 0;
        GameStart();
    }

    public void RegisterController(SpawnUnitController control)
    {
        spawner = control;
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
        if (GetCurrency() < amount)
            textShaking?.Invoke();
        return GetCurrency() >= amount;
    }
    #endregion

    #region SpawnData

    public void GameStart()
    {
        StartCoroutine(ResourceGain());
        NextWaveReady();
    }


    public void NextWaveReady()
    {
        //웨이브 시작 : 최소 유닛수, 최소 인터벌 충족시
        if (curUnitSpawn > curStageData.triggerThreshold)
            return;
        if (Time.time < minSpawnDelay)
            return;
        if (curWaveNum >= curStageData.waveData.Length)
            return;

        waveDatas = curStageData.waveData[curWaveNum];
        StartCoroutine(SpawnWave(waveDatas));
        minSpawnDelay = Time.time + curStageData.minInterval;
        curWaveNum++;
    }


    IEnumerator SpawnWave(WaveData wave)
    {
        if (spawner == null)
        {
            Debug.LogError("Controller is missing");
            yield return null;
        }

        //웨이브 내에서 유닛들 스폰 중
        for(int i = 0; i < wave.unitIDList.Length; i++)
        {
            float spawnDelay = UnityEngine.Random.Range(wave.spawnDelayMin, wave.spawnDelayMax);
            yield return new WaitForSeconds(spawnDelay);

            spawner.SpawnUnitFromPool(wave.unitIDList[i]);
        }
    }


    #endregion
}
