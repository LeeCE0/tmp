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

    public class UnitData
    {
        Color color = Color.black;
        int needResource = 0;

        public void SetData(int cost, Color colr)
        {
            color = colr;
            needResource = cost;
        }

        public Color GetData() { return color;  }

        public int GetNeedResource() { return needResource; }
    }

    [SerializeField] UnitSlot[] unitBtn;
    [SerializeField] UnitInfo myUnit;
    [SerializeField] UnitInfo enemyUnit;
    [SerializeField] Transform MyUnitSpawnPoint;
    [SerializeField] Transform EnemyUnitSpawnPoint;

    Dictionary<int, UnitData> unitList = new Dictionary<int, UnitData>();
    List<UnitInfo> myUnitList = new List<UnitInfo>();


    public void Start()
    {
        unitList.Clear();

        for (int i = 0; i < unitBtn.Length; i++)
        {
            unitList.Add(i, new UnitData());
            unitBtn[i].SetData(i, SpawnUnit);
        }
        unitList[0].SetData(50, Color.white);
        unitList[1].SetData(100, Color.red);
        unitList[2].SetData(150, Color.cyan);
        unitList[3].SetData(200, Color.magenta);
    }

    public void SpawnUnit(int index)
    {
        int cost = unitList[index].GetNeedResource();
        if (GameManager.Instance.IsEnoughCurrency(cost))
        {
            GameManager.Instance.UseCurrency(cost);
            UnitInfo newUnit = Instantiate(myUnit, MyUnitSpawnPoint.position, Quaternion.identity, MyUnitSpawnPoint);
            myUnitList.Add(newUnit);
            newUnit.SetColor(unitList[index].GetData());
            newUnit.gameObject.SetActive(true);
        }
        else
        {
            GameManager.Instance.StartShaking();
        }
    }



}
