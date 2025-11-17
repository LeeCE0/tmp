using System.Collections;
using UnityEngine;
using TMPro;

public class InGameBattleUI : MonoBehaviour
{
    [SerializeField] GameObject notEnoughCurrency;
    [SerializeField] TextMeshProUGUI resourceText;  // UI 텍스트

    public int curCurrency = 0;  // 현재 자원
    public int currencyPerSecond = 10;  // 초당 증가 자원량
    public float forSecond = 5f;

    Vector3 originalPosition;


    private void Start()
    {
        originalPosition = resourceText.rectTransform.localPosition;
        
    }

    public void OnEnable()
    {
        StageManager.Instance.OnChangeCurrency += UpdateResourceUI;
        StageManager.Instance.textShaking += StartShaking;
    }

    public void OnDisable()
    {
        if (StageManager.Instance == null)
            return; 

        StageManager.Instance.OnChangeCurrency -= UpdateResourceUI;
        StageManager.Instance.textShaking -= StartShaking;
    }

    void UpdateResourceUI(int value)
    {
        curCurrency = value;
        resourceText.text = curCurrency.ToString();
    }
     
    public void StartShaking()
    {
        StartCoroutine(ShakeText());
        StartCoroutine(WarningText());
    }

    private IEnumerator ShakeText()
    {
        float elapsedTime = 0f;
        resourceText.color = Color.red;
        while (elapsedTime < 0.4f)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-5f, 5f),
                Random.Range(-5f, 5f),
                0);

            resourceText.rectTransform.localPosition = originalPosition + randomOffset;
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        resourceText.rectTransform.localPosition = originalPosition;

        resourceText.color = Color.white;
    }

    private IEnumerator WarningText()
    {
        if (notEnoughCurrency.activeSelf)
            yield break;
        notEnoughCurrency.SetActive(true);

        yield return new WaitForSeconds(3f);

        notEnoughCurrency.SetActive(false);
    }
    public void SetGameEnd(bool isMine)
    {
        if (isMine)
            Debug.LogError("LOSE");
        else
            Debug.LogError("WIN");
    }
}
