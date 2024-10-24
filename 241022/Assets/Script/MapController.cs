using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MapController : MonoBehaviour
{
    public float scrollSpeed = 100f; // 배경 이동 속도
    public float backgroundHeight;
    [SerializeField] GameObject[] mapTile;

    void Start()
    {
        RectTransform rect = mapTile[0].transform as RectTransform;
        backgroundHeight = rect.sizeDelta.y;
    }
    void Update()
    {
        foreach (GameObject background in mapTile)
        {
            background.transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);
        }

        foreach (GameObject background in mapTile)
        {
            if (background.transform.position.y < -backgroundHeight)
            {
                background.transform.Translate(0, backgroundHeight * mapTile.Length + 1, 0);
            }
        }
    }
}
