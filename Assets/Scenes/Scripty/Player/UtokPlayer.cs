using UnityEngine;

public class UtokPlayer : MonoBehaviour
{
    public int attackDamage = 20;       // Po�kozen� �toku
    public float attackCooldown = 0.3f; // D�lka blokov�n� pohybu po �toku
    public float attackRange = 1f;     // Dosah �toku
    public float knockbackForce = 5f;  // S�la knockbacku
    public LayerMask enemyLayer;       // Vrstva nep��tel

    private Movement movementScript;    // Reference na Movement skript
    private Animator animator;          // Animator hr��e
    private bool canAttack = true;      // Kontrola, zda m��e hr�� �to�it
    private bool isAttacking = false;   // Indikuje, zda pr�v� prob�h� �tok

    void Start()
    {
        movementScript = GetComponent<Movement>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canAttack && !isAttacking) // Spu�t�n� �toku
        {
            Attack();
        }
    }

    void Attack()
    {
        isAttacking = true; // Ozna�en�, �e prob�h� �tok

        // Zastaven� pohybu hr��e b�hem �toku
        if (movementScript != null)
        {
            movementScript.DisableMovement(attackCooldown);
        }

        // Nastaven� cooldownu �toku
        canAttack = false;

        // Z�sk�n� posledn�ho sm�ru pohybu
        Vector2 lastMoveDirection = movementScript.GetLastMoveDirection();

        // Ur�en� animace �toku podle diagon�ln� logiky
        string attackTrigger = DetermineAttackTrigger(lastMoveDirection);

        // Debugging (pro kontrolu)
        Debug.Log("Attack Trigger: " + attackTrigger + ", Last Direction: " + lastMoveDirection);

        // Spu�t�n� animace �toku
        animator.SetTrigger(attackTrigger);

        // Raycast pro detekci z�sah� nep��tel
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, lastMoveDirection, attackRange, enemyLayer);

        foreach (var hit in hits)
        {
            if (hit.collider != null && hit.collider.TryGetComponent<UtokZivotyEnemy>(out UtokZivotyEnemy enemy))
            {
                // Aplikace po�kozen�
                enemy.TakeDamage(attackDamage);

                // Aplikace knockbacku
                ApplyKnockback(enemy, lastMoveDirection);
            }
        }

        // Reset �toku po cooldownu
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    string DetermineAttackTrigger(Vector2 direction)
    {
        // Pokud je diagon�ln� pohyb (ob� slo�ky != 0)
        if (direction.x != 0 && direction.y != 0)
        {
            // Preferujeme horizont�ln� sm�r
            return direction.x > 0 ? "AttackRight" : "AttackLeft";
        }

        // Pro pohyby pouze na jedn� ose
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return direction.x > 0 ? "AttackRight" : "AttackLeft";
        }
        else
        {
            return direction.y > 0 ? "AttackUp" : "AttackDown";
        }
    }

    void ApplyKnockback(UtokZivotyEnemy enemy, Vector2 attackDirection)
    {
        // Normalizovan� sm�r knockbacku
        Vector2 knockbackDirection = attackDirection.normalized;

        // Pou�it� knockbacku na nep��tele
        enemy.ApplyKnockback(knockbackDirection * knockbackForce);
    }

    void ResetAttack()
    {
        // Reset Trigger�
        animator.ResetTrigger("AttackRight");
        animator.ResetTrigger("AttackLeft");
        animator.ResetTrigger("AttackUp");
        animator.ResetTrigger("AttackDown");

        // Obnoven� mo�nosti �toku
        isAttacking = false;
        canAttack = true;
    }

}
