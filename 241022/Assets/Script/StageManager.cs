using System.Collections;
using System.Collections.Generic;
using Unit;
using UnityEngine;
using UnityEngine.Pool;

public class StageManager : MonoBehaviour
{
    //적 스폰과 스테이지를 담당하는 매니저
    private static StageManager instance;
    public static StageManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<StageManager>();
            return instance;
        }
    }

    public void SetGameEnd(bool isMine)
    {
        if (isMine)
            Debug.LogError("LOSE");
        else
            Debug.LogError("WIN");
    }
}
