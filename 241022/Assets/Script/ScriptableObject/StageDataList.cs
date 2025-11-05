using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "StageDataList", menuName = "Game Data/StageDataList")]
public class StageDataList : ScriptableObject
{
    public StageData[] stageDatasList;

    private Dictionary<int, StageData> dataDic;

    // 처음 접근할 때 딕셔너리 생성
    public void BuildMap()
    {
        if (dataDic == null)
            dataDic = stageDatasList.ToDictionary(x => x.stageID, x => x);
    }

    public StageData Get(int id)
    {
        BuildMap();
        dataDic.TryGetValue(id, out var data);
        return data;
    }

}
