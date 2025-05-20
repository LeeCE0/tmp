using Unit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

    Dictionary<int, MyInfo.UnitData> unitList = new Dictionary<int, MyInfo.UnitData>();   // 뽑을 수 있는 유닛 리스트
    List<UnitInfo> myUnitList = new List<UnitInfo>();   // 필드에 소환되어있는 유닛
    List<UnitInfo> enemyUnitList = new List<UnitInfo>();  //필드에 소환되어있는 적 유닛


    public void Start()
    {
       
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
            GameObject item = ObjectPoolManager.Instance.GetObjPool("Unit", MyUnitSpawnPoint, Quaternion.identity);
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


}
