using UnityEngine;

public class UnitAnimTest : MonoBehaviour
{
    private Animator animator;
    private bool isDead = false;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>(); // UnitRoot �Ʒ� Animator ã��
    }

    void Update()
    {
        if (isDead) return;

        // 1. �̵� ���� (WASD �Է� ����)
        bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                        Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        animator.SetBool("isMoving", isMoving);

        // 2. ���� (���콺 ���� Ŭ��)
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }

        // 3. ��� (K Ű)
        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("Die");
            isDead = true;
        }
    }
}
