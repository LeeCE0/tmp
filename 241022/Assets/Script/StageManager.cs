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

    // 테스트용 더미 데이터
}
