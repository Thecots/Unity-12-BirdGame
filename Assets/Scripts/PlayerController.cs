using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform player;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource wallSound;



    private Vector3 direction;
    public float speed = 3f;
    public float jumpForce = 5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) && gameManager.game)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpSound.Play();
        }

        SpriteAnimation();

        transform.position += new Vector3(1, 0, 0) * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "right")
        {
            player.localScale = new Vector3(-1, 1, 1);
            speed = -speed;
            gameManager.AddScore();
            gameManager.obstacles();
            wallSound.Play();
        }
        else if(collision.gameObject.tag == "left")
        {
            player.localScale = new Vector3(1, 1, 1);
            speed = -speed;
            gameManager.AddScore();
            gameManager.obstacles();
            wallSound.Play();

        }

        if (collision.gameObject.tag == "triangle")
        {
            gameManager.Lose();
        }
    }

    private void SpriteAnimation()
    {
        if (rb.velocity.y < 1 && gameManager.game)
            sr.sprite = sprites[1];
        else
            sr.sprite = sprites[0];    
    }

    public void Die()
    {
        sr.sprite = sprites[2];

    }

}
