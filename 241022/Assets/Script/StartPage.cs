using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartPage : MonoBehaviour
{
    [SerializeField] private GameObject touchToStartText;
    [SerializeField] private float blinkInterval;

    private bool isStart = false;

    private void Start()
    {
        StartCoroutine(BlinkText());
    }

    private void Update()
    {
        if (isStart) return;

        if (Input.GetMouseButtonDown(0))
        {
            isStart = true;
            GoToNextPage();
        }
    }

    private IEnumerator BlinkText()
    {
        TextMeshProUGUI text = touchToStartText.GetComponent<TextMeshProUGUI>();
        float duration = blinkInterval; 
        float alpha = 0f;

        while (!isStart)
        {
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                alpha = Mathf.Lerp(0f, 1f, t / duration);
                SetAlpha(text, alpha);
                yield return null;
            }

            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                alpha = Mathf.Lerp(1f, 0f, t / duration);
                SetAlpha(text, alpha);
                yield return null;
            }
        }

        SetAlpha(text, 1f);
    }

    private void SetAlpha(TextMeshProUGUI text, float alpha)
    {
        Color c = text.color;
        c.a = alpha;
        text.color = c;
    }

    private void GoToNextPage()
    {
        GameManager.Instance.GameStart();
        gameObject.SetActive(false);
    }
}
