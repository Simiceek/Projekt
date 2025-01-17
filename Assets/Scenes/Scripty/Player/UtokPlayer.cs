using UnityEngine;

public class UtokPlayer : MonoBehaviour
{
    public int attackDamage = 20;       // Poškození útoku
    public float attackCooldown = 0.3f; // Délka blokování pohybu po útoku
    public float attackRange = 1f;     // Dosah útoku
    public float knockbackForce = 5f;  // Síla knockbacku
    public LayerMask enemyLayer;       // Vrstva nepøátel

    private Movement movementScript;    // Reference na Movement skript
    private Animator animator;          // Animator hráèe
    private bool canAttack = true;      // Kontrola, zda mùže hráè útoèit
    private bool isAttacking = false;   // Indikuje, zda právì probíhá útok

    void Start()
    {
        movementScript = GetComponent<Movement>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canAttack && !isAttacking) // Spuštìní útoku
        {
            Attack();
        }
    }

    void Attack()
    {
        isAttacking = true; // Oznaèení, že probíhá útok

        // Zastavení pohybu hráèe bìhem útoku
        if (movementScript != null)
        {
            movementScript.DisableMovement(attackCooldown);
        }

        // Nastavení cooldownu útoku
        canAttack = false;

        // Získání posledního smìru pohybu
        Vector2 lastMoveDirection = movementScript.GetLastMoveDirection();

        // Urèení animace útoku podle diagonální logiky
        string attackTrigger = DetermineAttackTrigger(lastMoveDirection);

        // Debugging (pro kontrolu)
        Debug.Log("Attack Trigger: " + attackTrigger + ", Last Direction: " + lastMoveDirection);

        // Spuštìní animace útoku
        animator.SetTrigger(attackTrigger);

        // Raycast pro detekci zásahù nepøátel
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, lastMoveDirection, attackRange, enemyLayer);

        foreach (var hit in hits)
        {
            if (hit.collider != null && hit.collider.TryGetComponent<UtokZivotyEnemy>(out UtokZivotyEnemy enemy))
            {
                // Aplikace poškození
                enemy.TakeDamage(attackDamage);

                // Aplikace knockbacku
                ApplyKnockback(enemy, lastMoveDirection);
            }
        }

        // Reset útoku po cooldownu
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    string DetermineAttackTrigger(Vector2 direction)
    {
        // Pokud je diagonální pohyb (obì složky != 0)
        if (direction.x != 0 && direction.y != 0)
        {
            // Preferujeme horizontální smìr
            return direction.x > 0 ? "AttackRight" : "AttackLeft";
        }

        // Pro pohyby pouze na jedné ose
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
        // Normalizovaný smìr knockbacku
        Vector2 knockbackDirection = attackDirection.normalized;

        // Použití knockbacku na nepøítele
        enemy.ApplyKnockback(knockbackDirection * knockbackForce);
    }

    void ResetAttack()
    {
        // Reset Triggerù
        animator.ResetTrigger("AttackRight");
        animator.ResetTrigger("AttackLeft");
        animator.ResetTrigger("AttackUp");
        animator.ResetTrigger("AttackDown");

        // Obnovení možnosti útoku
        isAttacking = false;
        canAttack = true;
    }

}
