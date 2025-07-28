using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "UnitsDataList", menuName = "Game Data/UnitsData List")]
public class UnitsDataList : ScriptableObject
{
    public List<UnitsData> units;
}
