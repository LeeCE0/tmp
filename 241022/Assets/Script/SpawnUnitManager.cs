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

    Dictionary<int, MyInfo.UnitData> unitList = new Dictionary<int, MyInfo.UnitData>();
    List<UnitInfo> myUnitList = new List<UnitInfo>();
    List<UnitInfo> enemyUnitList = new List<UnitInfo>();


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
        if (GameManager.Instance.IsEnoughCurrency(unitList[index+ 1].Cost))
        {
            GameManager.Instance.UseCurrency(unitList[index + 1].Cost);
            UnitInfo newUnit = Instantiate(myUnit, MyUnitSpawnPoint.position, Quaternion.identity, MyUnitSpawnPoint);
            myUnitList.Add(newUnit);
            newUnit.SetData(unitList[index+1]);
            newUnit.gameObject.SetActive(true);
        }
        else
        {
            GameManager.Instance.StartShaking();
        }
    }



}
