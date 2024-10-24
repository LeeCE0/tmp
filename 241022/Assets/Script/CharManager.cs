using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharManager : MonoBehaviour
{
    public class CharInfo
    {
        public string name;
        public int charID;
        public int HP;
        public int charLV;
    }
    [SerializeField] GameObject tmpChar;

    private Dictionary<int, CharInfo> myChar = new Dictionary<int, CharInfo>();

    public void InitCharacter()
    {
        if (tmpChar == null)
            return;
       tmpChar.SetActive(true);
        myChar.Add(1, new CharInfo()
        {
            name = "토끼",
            charID = 1,
            HP = 10,
            charLV = 1,
        });
    }


}
