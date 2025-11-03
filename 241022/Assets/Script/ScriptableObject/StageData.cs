using UnityEngine;
using static StageDataList;


[System.Serializable]
[CreateAssetMenu(fileName = "StageData", menuName = "Game Data/StageData")]
public class StageData : ScriptableObject
{
    public int stageID;
    public string name;


    public int allSpawnCount; // 총 스폰 될 적 개체 수
    public int triggerThreshold; // 다음 웨이브가 소환되는 최소 개체 수
    public float minInterval; // 다음 웨이브 소환에 필요한 최소 딜레이 (무한스폰 방지)
    public int maxConcurrent;
    public WaveData[] waveData;

}

[System.Serializable]
public class WaveData
{
    public int waveNum;   //웨이브 번호
    public int[] unitIDList;  //웨이브 당 스폰 될 유닛 리스트
    public int spawnDelayMin;  //각 유닛당 스폰 딜레이
    public int spawnDelayMax;
}