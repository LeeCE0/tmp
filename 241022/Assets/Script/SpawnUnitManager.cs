using Unit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] Transform MyUnitSpawnPoint;
    [SerializeField] Transform EnemyUnitSpawnPoint;

    Dictionary<int, MyInfo.UnitData> unitList = new Dictionary<int, MyInfo.UnitData>();   // 뽑을 수 있는 유닛 리스트
    Dictionary<int, UnitInfo> myUnitList = new Dictionary<int, UnitInfo>();   // 필드에 소환되어있는 유닛
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
            UnitInfo newUnit = Instantiate(myUnit, MyUnitSpawnPoint.position, Quaternion.identity, MyUnitSpawnPoint);

            if (myUnitList.ContainsKey(newUnit.GetUnitID()))
            {

            }



            newUnit.SetSpawn(unitList[index + 1]);
            newUnit.gameObject.SetActive(true);
        }
        else
        {
            GameManager.Instance.StartShaking();
        }
    }


    public void RemoveUnit()
    {

    }


}
