using Unit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using static Unit.MyInfo;
using Unity.VisualScripting;

public class SpawnUnitManager : MonoBehaviour
{
    private static SpawnUnitManager instance;
    public static SpawnUnitManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<SpawnUnitManager>();
            return instance;
        }
    }

    [SerializeField] UnitSlot[] unitBtn;
    [SerializeField] UnitInfo myUnit;
    [SerializeField] UnitInfo enemyUnit;
    [SerializeField] GameObject MyUnitSpawnPoint;
    [SerializeField] GameObject EnemyUnitSpawnPoint;
    [SerializeField] public NexusInfo myNexus;
    [SerializeField] public NexusInfo enemyNexus;

    Dictionary<int, MyInfo.UnitData> unitList = new Dictionary<int, MyInfo.UnitData>();   // 뽑을 수 있는 유닛 리스트
    public List<UnitInfo> myUnitList = new List<UnitInfo>();   // 필드에 소환되어있는 유닛
    public List<UnitInfo> enemyUnitList = new List<UnitInfo>();  //필드에 소환되어있는 적 유닛



    public void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    public void SetData()
    {
        unitList.Clear();
        unitList = MyInfo.Instance.GetAllUnit();

        for (int i = 0; i < unitBtn.Length; i++)
        {
            unitBtn[i].SetData(i, SpawnUnit);
        }
    }
    public void SpawnUnit(int index)
    {
        if (GameManager.Instance.IsEnoughCurrency(unitList[index + 1].Cost))
        {
            GameManager.Instance.UseCurrency(unitList[index + 1].Cost);
            GameObject item = ObjectPoolManager.Instance.GetObjPool("MyUnit", MyUnitSpawnPoint, Quaternion.identity);
            UnitInfo newUnit = item.GetComponent<UnitInfo>();

            newUnit.SetSpawn(unitList[index + 1]);
            myUnitList.Add(newUnit);
            newUnit.gameObject.SetActive(true);
        }
        else
        {
            GameManager.Instance.StartShaking();
        }
    }

    public void RemoveUnit(int UnitID)
    {
        var rmUnit = myUnitList.Find(x => x.GetUnitID() == UnitID);
    }

    public float spawnInterval = 10f; // 10초

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnUnitFromPool();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnUnitFromPool()
    {
        GameObject unit = ObjectPoolManager.Instance.GetObjPool("Enemy", EnemyUnitSpawnPoint, Quaternion.identity);
        UnitInfo newUnit = unit.GetComponent<UnitInfo>();

        newUnit.SetSpawn(GetRandomUnitData());
        enemyUnitList.Add(newUnit);

        unit.transform.position = EnemyUnitSpawnPoint.transform.position;
        unit.transform.rotation = Quaternion.identity;

    }

    public void DeadUnitToPool()
    {

    }


    private MyInfo.UnitData GetRandomUnitData()
    {
        return new MyInfo.UnitData(
            100,
            1,
            "",
            2,
            1,
            10,
            0,
            2);
    }


}
