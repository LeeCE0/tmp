using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GameManager>();
            return instance;
        }
    }
    [SerializeField] StageDataList allStageData;
    public int selectedStage = 1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        LoadStage(1);

    }
    public void GameStart()
    {
        UIManager.Instance.LoadUI(UIManager.eUIType.StageUI);
    }

    public void LoadStage(int stageIndex)
    {
        if (allStageData == null)
        {
            Debug.LogError("StageData is NULL");
            return;
        }

        selectedStage = stageIndex;
        SceneManager.LoadScene("BattleScene");

        StageData target = allStageData.Get(selectedStage);

        if (target != null)
            Debug.Log($"로드 완료: {target.stageID}");
        
        StageManager.Instance.SetStage(target);
    }

}
