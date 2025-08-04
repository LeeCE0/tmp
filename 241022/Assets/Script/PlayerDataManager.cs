using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using static Unit.UnitDataContainer;

public class PlayerDataManager : MonoBehaviour
{
    private static PlayerDataManager instance;
    public static PlayerDataManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PlayerDataManager>();
            return instance;
        }
    }
    Dictionary<int, UnitsData> myUnit = new Dictionary<int, UnitsData>();
    [SerializeField] UnitsDataList unitsDataList;
    Dictionary<int, UnitsData> allUnit = new Dictionary<int, UnitsData>();


    public void Start()
    {
        myUnit.Clear();

        unitsDataList.Init();
        // ���̺� ��� ���� ������ ��������
        foreach (var item in unitsDataList.GetAllUnitData())
        {
            if (!allUnit.ContainsKey(item.Key))
            {
                allUnit.Add(item.Key, item.Value);
            }
        }
        ObjectPoolManager.Instance.AddUnitPool(allUnit);
        SpawnUnitManager.Instance.SetData();        
    }

    public Dictionary<int, UnitsData> GetAllUnit()
    {
        return allUnit;
    }

    #region UnitUnlockData
    private Dictionary<int, int> unitLevels = new Dictionary<int, int>();

    private const string SaveKey = "UnitLevelData";

    // ���� �ر� ����
    public bool IsUnlocked(int unitID)
    {
        return unitLevels.ContainsKey(unitID);
    }

    // ���� �ر� (���� 1�� ���)
    public void UnlockUnit(int unitID)
    {
        if (!unitLevels.ContainsKey(unitID))
        {
            unitLevels[unitID] = 1;
            SaveUnitData();
        }
    }

    // ��ȭ
    public void UpgradeUnit(int unitID)
    {
        if (unitLevels.ContainsKey(unitID))
        {
            unitLevels[unitID]++;
            SaveUnitData();
        }
    }

    // ���� ��ȭ ����
    public int GetUnitLevel(int unitID)
    {
        return unitLevels.TryGetValue(unitID, out int level) ? level : 0;
    }

    // ��ü ������ ����
    public void SaveUnitData()
    {
        var saveList = new List<UnitLevelData>();
        foreach (var item in unitLevels)
        {
            saveList.Add(new UnitLevelData { Key = item.Key, Value = item.Value });
        }

        string json = JsonUtility.ToJson(new SaveWrapper { units = saveList });
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    // �ҷ�����
    public void LoadUnitData()
    {
        unitLevels.Clear();

        if (PlayerPrefs.HasKey(SaveKey))
        {
            string json = PlayerPrefs.GetString(SaveKey);
            var data = JsonUtility.FromJson<SaveWrapper>(json);
            if (data?.units != null)
            {
                foreach (var item in data.units)
                {
                    unitLevels[item.Key] = item.Value;
                }
            }
        }
    }

    [System.Serializable]
    private class SaveWrapper
    {
        public List<UnitLevelData> units;
    }

    [System.Serializable]
    private class UnitLevelData
    {
        public int Key;
        public int Value;

        public static implicit operator KeyValuePair<int, int>(UnitLevelData u)
            => new KeyValuePair<int, int>(u.Key, u.Value);

        public static implicit operator UnitLevelData(KeyValuePair<int, int> pair)
            => new UnitLevelData { Key = pair.Key, Value = pair.Value };
    }
    #endregion
}
