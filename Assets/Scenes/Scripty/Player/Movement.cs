using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveVelocity;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 lastMoveDirection = Vector2.down; // Výchozí smìr pohybu (napø. dolù)
    public Vector2 fieldMin; // Minimální hranice pole (napø. -5, -5)
    public Vector2 fieldMax; // Maximální hranice pole (napø. 5, 5)


    private bool canMove = true; // Indikuje, zda hráè mùže hýbat
    private bool isMoving = false; // Indikuje, zda hráè aktuálnì pohybuje

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float clampedX = Mathf.Clamp(transform.position.x, fieldMin.x, fieldMax.x);
        float clampedY = Mathf.Clamp(transform.position.y, fieldMin.y, fieldMax.y);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);

        if (!canMove) // Pokud hráè nemùže hýbat, zastav pohyb
        {
            moveVelocity = Vector2.zero;
            isMoving = false;
            animator.SetBool("IsMoving", false);
            return;
        }

        // Reset hodnoty pohybu
        float moveInputX = 0f;
        float moveInputY = 0f;

        // Kontrola držených kláves pro osu X
        if (Input.GetKey(KeyCode.A))
        {
            moveInputX = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveInputX = 1f;
        }

        // Kontrola držených kláves pro osu Y
        if (Input.GetKey(KeyCode.W))
        {
            moveInputY = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveInputY = -1f;
        }

        // Zaznamenání posledního smìru pohybu
        if (moveInputX != 0 || moveInputY != 0)
        {
            lastMoveDirection = new Vector2(moveInputX, moveInputY).normalized;
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        // Vytvoøení vektoru pohybu
        Vector2 moveInput = new Vector2(moveInputX, moveInputY).normalized;
        moveVelocity = moveInput * speed;

        // Pøednost osy X pøi diagonálním pohybu
        int x = moveInputX != 0 ? (int)Mathf.Sign(moveInputX) : 0;
        int y = moveInputY != 0 && moveInputX == 0 ? (int)Mathf.Sign(moveInputY) : 0;

        // Aktualizace Animatoru
        animator.SetInteger("Horizontal", x);
        animator.SetInteger("Vertical", y);
        animator.SetBool("IsMoving", isMoving);

        // Nastavení flipX pro zmìnu smìru na ose X
        if (spriteRenderer != null && moveInputX != 0)
        {
            spriteRenderer.flipX = moveInputX < 0;
        }
    }

    void FixedUpdate()
    {
        // Aplikace pohybu
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

    public void DisableMovement(float duration)
    {
        canMove = false;
        Invoke(nameof(EnableMovement), duration); // Obnoví pohyb po dobì "duration"
    }

    private void EnableMovement()
    {
        canMove = true;
    }

    // Vrací poslední smìr pohybu
    public Vector2 GetLastMoveDirection()
    {
        return lastMoveDirection;
    }
}
