using UnityEngine;

public class HPbar : MonoBehaviour
{
    [SerializeField] SpriteRenderer hpImage;
    private float maxWidth;

    private void Awake()
    {
        maxWidth = hpImage.size.x;
    }

    public void UpdateBar(int curHP, int maxHP)
    {
        curHP = Mathf.Clamp(curHP, 0, maxHP);

        float ratio = (float)curHP / maxHP;

        hpImage.size = new Vector2(maxWidth * ratio, hpImage.size.y);
    }
}
