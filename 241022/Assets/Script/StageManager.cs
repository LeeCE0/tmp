using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    #region Currency

    [SerializeField] GameObject notEnoughCurrency;
    [SerializeField] TextMeshProUGUI resourceText;  // UI 텍스트

    public int curCurrency = 100;  // 시작 자원
    public int currencyPerSecond = 10;  // 초당 증가 자원량
    public float forSecond = 5f;

    Vector3 originalPosition;

    void Start()
    {
        resourceText.text = curCurrency.ToString();
        StartCoroutine(ResourceGain());

        originalPosition = resourceText.rectTransform.localPosition;
    }

    IEnumerator ResourceGain()
    {
        while (true)
        {
            curCurrency += currencyPerSecond;
            UpdateResourceUI();
            yield return new WaitForSeconds(forSecond);
        }
    }

    public void UseCurrency(int amount)
    {
        curCurrency -= amount;
        UpdateResourceUI();
    }

    public int GetCurrency()
    {
        return curCurrency;
    }

    public bool IsEnoughCurrency(int amount)
    {
        return GetCurrency() >= amount;
    }

    void UpdateResourceUI()
    {
        resourceText.text = curCurrency.ToString();
    }
    #endregion

    public void StartShaking()
    {
        StartCoroutine(ShakeText());
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

    public void SetGameEnd(bool isMine)
    {
        if (isMine)
            Debug.LogError("LOSE");
        else
            Debug.LogError("WIN");
    }
}
