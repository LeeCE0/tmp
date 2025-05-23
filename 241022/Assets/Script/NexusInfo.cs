using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NexusInfo : MonoBehaviour
{
    public int HP;
    public int Atk;

    public bool isMyNexus;

    public void Start()
    {
        //stageManager 데이터 셋팅하기
    }

    public void TakeDMG(int DMG)
    {
        HP -= DMG;
        if(HP <= 0)
            NexusEnd();
    }

    public void NexusEnd()
    {
        Debug.LogError(isMyNexus);
        //stageManager에 뭐가 터졌는지 넘기기
    }
}
