using System.Collections.Generic;
using Unit;
using UnityEngine;

public class SpawnUnitController : MonoBehaviour
{
    [SerializeField] UnitSlot[] unitBtn;
    [SerializeField] UnitBase myUnit;
    [SerializeField] UnitBase enemyUnit;
    [SerializeField] GameObject MyUnitSpawnPoint;
    [SerializeField] GameObject EnemyUnitSpawnPoint;
    [SerializeField] public NexusInfo myNexus;
    [SerializeField] public NexusInfo enemyNexus;

    Dictionary<int, UnitsData> unitList = new Dictionary<int, UnitsData>();   // 뽑을 수 있는 유닛 리스트
    public List<UnitBase> myUnitList = new List<UnitBase>();   // 필드에 소환되어있는 유닛
    public List<UnitBase> enemyUnitList = new List<UnitBase>();  //필드에 소환되어있는 적 유닛


    public void Start()
    {
        SetData();
    }
    public void OnEnable()
    {
        StageManager.Instance.RegisterController(this);
    }

    public void SetData()
    {
        unitList.Clear();
        unitList = UnitDataContainer.Instance.GetAllUnitData();

        for (int i = 0; i < unitBtn.Length; i++)
        {
            unitBtn[i].SetData(i, SpawnUnit);
        }
    }
    public void SpawnUnit(int index)
    {
        if (StageManager.Instance.IsEnoughCurrency(unitList[index + 1].cost))
        {
            StageManager.Instance.UseCurrency(unitList[index + 1].cost);
            GameObject item = ObjectPoolManager.Instance.SpawnFromPool(ObjectPoolManager.ePoolingObj.MyUnit, MyUnitSpawnPoint, Quaternion.identity, MyUnitSpawnPoint);
            UnitBase newUnit = item.GetComponent<UnitBase>();
            if (newUnit == null)
            {
                Debug.LogError("no component : UnitInfo");
                return;
            }
            newUnit.isMyUnit = true;
            newUnit.SetSpawn(unitList[index + 1]);
            myUnitList.Add(newUnit);
            newUnit.gameObject.SetActive(true);
        }
        else
        {
            StageManager.Instance.textShaking();
        }
    }

    public void RemoveUnit(int UnitID)
    {
        var rmUnit = myUnitList.Find(x => x.GetUnitID() == UnitID);

    }

    public void SpawnUnitFromPool(int unitID)
    {
        GameObject unit = ObjectPoolManager.Instance.SpawnFromPool(ObjectPoolManager.ePoolingObj.Enemy, EnemyUnitSpawnPoint, Quaternion.identity, EnemyUnitSpawnPoint);
        UnitBase newUnit = unit.GetComponent<UnitBase>();
        unitList.TryGetValue(unitID, out var data);

        newUnit.isMyUnit = false;
        newUnit.SetSpawn(data);
        enemyUnitList.Add(newUnit);

        unit.transform.position = EnemyUnitSpawnPoint.transform.position;
        unit.transform.rotation = Quaternion.identity;

    }
}
