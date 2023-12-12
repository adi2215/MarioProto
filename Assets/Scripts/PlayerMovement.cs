using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    private Camera k_camera;
    private CapsuleCollider2D _cap;

    public Vector2 vel;
    private Vector2 position;

    private float inputAxis;

    public float moveSpeed = 8f;
    public float jumpHeight = 4f;
    public float jumpTime = 1f;
    public float acceleration = 1f;

    protected float jumpForce => (2f * jumpHeight) / (jumpTime / 2f);
    protected float heroGravity => (-2f * jumpHeight) / Mathf.Pow((jumpTime / 2f), 2);

    public bool jumping {get; private set; }
    public bool grounded {get; private set; }
    public bool ceiled {get; private set; }
    public bool running => Mathf.Abs(vel.x) > 0f || Mathf.Abs(inputAxis) > 0f;
    private bool _cachedQueryStartInColliders;
    private bool FacingRight = true;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _cap = GetComponent<CapsuleCollider2D>();
        k_camera = Camera.main;

        _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
    }

    public void MakeDamage()
    {
        rb.AddForce(Vector2.up * jumpForce);
        GetComponent<Collider2D>().isTrigger = true;
        enabled = false;
    }

    private void Update()
    {
        Physics2D.queriesStartInColliders = false;

        grounded = rb.Raycast(Vector2.down, _cap);
        ceiled = rb.Raycast(Vector2.up, _cap);

        if (ceiled)
        {
            vel.y = Mathf.Min(0, vel.y);
        }

        if (grounded)
        {
            vel.y = Mathf.Max(vel.y, 0f);
            GroundedMov();
        }

        Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;

        HorizontalMov();

        ApplyingGravity();
    }

    private void GroundedMov()
    {
        jumping = vel.y > 0f;

        if (Input.GetButtonDown("Jump"))
        {
            vel.y = jumpForce;
            jumping = true;
        }
    }

    private void ApplyingGravity()
    {
        bool falling = vel.y < 0f || !Input.GetButton("Jump");
        float multiplier = falling ? 2f : 1f;

        vel.y += heroGravity * multiplier * Time.deltaTime;
        vel.y = Mathf.Max(vel.y, heroGravity / 1.5f);
    }

    private void HorizontalMov()
    {
        inputAxis = Input.GetAxis("Horizontal");
        vel.x = Mathf.MoveTowards(vel.x, inputAxis * moveSpeed, acceleration * moveSpeed * Time.deltaTime);

        if (rb.Raycast(Vector2.right * vel.x, _cap))
        {
            vel.x = 0f;
        }

        if (FacingRight && inputAxis < 0)
            Flip();

        else if (!FacingRight && inputAxis > 0)
            Flip();
    }

    private void FixedUpdate()
    {
        HeroPosition();
    }

    private void HeroPosition()
    {
        position = rb.position;
        position += vel * Time.fixedDeltaTime;

        CameraZone();
        rb.MovePosition(position);
    }

    private void CameraZone()
    {
        Vector2 leftEdge = k_camera.ScreenToWorldPoint(Vector2.zero);
        Vector2 rightEdge = k_camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        position.x = Mathf.Clamp(position.x, leftEdge.x + 0.6f, rightEdge.x - 0.6f);
    }

    private void Flip()
    {
        FacingRight = !FacingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (transform.DotTest(collision.transform, Vector2.down))
            {
                vel.y = jumpForce / 2f;
                jumping = true;
            }
            else if (transform.DotTest(collision.transform, Vector2.left))
            {
                vel.y = jumpForce;
                GetComponent<Collider2D>().isTrigger = true;
                Invoke("NewScene", 2f);
            }
            else if (transform.DotTest(collision.transform, Vector2.right))
            {
                vel.y = jumpForce;
                GetComponent<Collider2D>().isTrigger = true;
                Invoke("NewScene", 2f);
            }

        }
    }

    private void NewScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
