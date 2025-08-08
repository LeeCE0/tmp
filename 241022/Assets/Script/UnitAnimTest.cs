using UnityEngine;

public class UnitAnimTest : MonoBehaviour
{
    private Animator animator;
    private bool isDead = false;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>(); // UnitRoot 아래 Animator 찾기
    }

    void Update()
    {
        if (isDead) return;

        // 1. 이동 판정 (WASD 입력 감지)
        bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                        Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        animator.SetBool("isMoving", isMoving);

        // 2. 공격 (마우스 왼쪽 클릭)
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }

        // 3. 사망 (K 키)
        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("Die");
            isDead = true;
        }
    }
}
