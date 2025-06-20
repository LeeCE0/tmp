using Unit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unit.UnitDataContainer;

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

    Dictionary<int, UnitData> unitList = new Dictionary<int, UnitData>();   // 뽑을 수 있는 유닛 리스트
    public List<UnitInfo> myUnitList = new List<UnitInfo>();   // 필드에 소환되어있는 유닛
    public List<UnitInfo> enemyUnitList = new List<UnitInfo>();  //필드에 소환되어있는 적 유닛



    public void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    public void SetData()
    {
        unitList.Clear();
        unitList = PlayerDataManager.Instance.GetAllUnit();

        for (int i = 0; i < unitBtn.Length; i++)
        {
            unitBtn[i].SetData(i, SpawnUnit);
        }
    }
    public void SpawnUnit(int index)
    {
        if (StageManager.Instance.IsEnoughCurrency(unitList[index + 1].Cost))
        {
            StageManager.Instance.UseCurrency(unitList[index+ 1].Cost);
            GameObject item = ObjectPoolManager.Instance.GetObjPool(unitList[index + 1].UnitType.ToString(), MyUnitSpawnPoint, Quaternion.identity);
            UnitInfo newUnit = item.GetComponent<UnitInfo>();
            if (newUnit == null)
            {
                Debug.LogError("no component : UnitInfo");
                return;
            } 

            newUnit.SetSpawn(unitList[index + 1]);
            myUnitList.Add(newUnit);
            newUnit.gameObject.SetActive(true);
        }
        else
        {
            StageManager.Instance.StartShaking();
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


    private UnitData GetRandomUnitData()
    {
        return new UnitData(
            100,
            1,
            "",
            2,
            1,
            10,
            0,
            2, null);
    }


}
