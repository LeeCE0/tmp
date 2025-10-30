using UnityEngine;
using System.Collections.Generic;


//모든 유닛데이터
[CreateAssetMenu(fileName = "UnitsDataList", menuName = "Game Data/UnitsData List")]
public class UnitsDataList : ScriptableObject
{
    [SerializeField]
    private List<UnitsData> units;

    private Dictionary<int, UnitsData> unitDict;

    public void Init()
    {
        unitDict = new Dictionary<int, UnitsData>();

        foreach (var unit in units) 
        {
            if (!unitDict.ContainsKey(unit.unitID))
                unitDict[unit.unitID] = unit;
        }
    }

    public Dictionary<int, UnitsData> GetAllUnitData()
    {
        return unitDict;
    }

    public Dictionary<int, UnitsData> GetMineUnit()
    {
            Dictionary<int, UnitsData> result = new Dictionary<int, UnitsData>();

            foreach (var unit in units)
            {
                if(unit.unitID < 100)
                    result.Add(unit.unitID, unit);
            }

        return result;
    }

    public Dictionary<int, UnitsData> GetEnemyUnit()
    {
        Dictionary<int, UnitsData> result = new Dictionary<int, UnitsData>();

        foreach (var unit in units)
        {
            if (unit.unitID >= 100)
                result.Add(unit.unitID, unit);
        }

        return result;
    }
}


