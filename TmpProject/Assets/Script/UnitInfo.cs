using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInfo : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    [SerializeField] SpriteRenderer image;

    void Update()
    {
        // 오른쪽으로 이동
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }

    public void SetColor(Color colr)
    {
        image.color = colr;
    }
}
