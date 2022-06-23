using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Color[] colors;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private GameObject tapSceenCanvas;
    [SerializeField] private Camera cam;

    [SerializeField] private GameObject leftObstacles;
    [SerializeField] private GameObject rightObstacles;
    [SerializeField] private AudioSource lose;


    public Animator anim;

    public float gravity = 1.5f;
    public float speed = 3f;
    public float jumpForce = 6f;
    public float max = 1;
    public float pmax = 5;
    public float pmax2 = 5;
    public bool game = false;
    private int indexBackground = 0;

    private void Awake()
    {
        PlayerPrefs.SetInt("score", 0);
        playerRb.gravityScale = 0;
        playerController.speed = 0;
        playerController.jumpForce = 0;
        leftObstacles.SetActive(false);
        rightObstacles.SetActive(false);
        anim = GetComponent<Animator>();
        lose.Play();

    }

    public void AddScore()
    {
        PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score", 0) + 1);
        scoreText.text = FormatNumber(PlayerPrefs.GetInt("score", 0));
    }

     
    private string FormatNumber(int e)
    {
        if (e < 10)
            return "0" + e;
        return e.ToString();
    }

    private void Update()
    {
        if (!game && Input.GetMouseButtonDown(0) && PlayerPrefs.GetInt("score", 0) == 0)
        {
            startGame();
            Debug.Log("Start");
            game = true;
        }else if(!game && Input.GetMouseButtonDown(0) && PlayerPrefs.GetInt("score", 0) < 0)
        {

            playerController.Die();
        }
    }

    public void startGame()
    {

        scoreText.color = colors[0];
        tapSceenCanvas.SetActive(false);
        playerRb.gravityScale = gravity;
        playerController.speed = speed;
        playerController.jumpForce = jumpForce;
        obstacles();
    }

    public void Lose()
    {
        Debug.Log("lose");
        game = false;
        playerController.jumpForce = 0;
        playerController.speed = 0;
        Destroy(player, 1f);
        playerController.Die();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void obstacles()
    {
        if(PlayerPrefs.GetInt("score",0) == pmax2)
        {
            indexBackground++;

            if (indexBackground > colors.Length-1)
                indexBackground = 0;
            max++;
            pmax2 += pmax;

            cam.backgroundColor = colors[indexBackground];
            scoreText.color = colors[indexBackground];
        }


        if (playerController.speed > 0)
        {
            rightObstacles.SetActive(true);
            leftObstacles.SetActive(false);

            for (int i = 0; i <  rightObstacles.transform.childCount; i++)
            {
                rightObstacles.transform.GetChild(i).gameObject.SetActive(false);
            }

            for (int i = 0; i < max; i++)
            {
                int numb = Random.Range(0, leftObstacles.transform.childCount);
                rightObstacles.transform.GetChild(numb).gameObject.SetActive(true);
            }

            anim.Play("rightOpen");
        }
        else
        {
            rightObstacles.SetActive(false);
            leftObstacles.SetActive(true);

            for (int i = 0; i < leftObstacles.transform.childCount; i++)
            {
                leftObstacles.transform.GetChild(i).gameObject.SetActive(false);
            }

            for (int i = 0; i < max; i++)
            {
                int numb = Random.Range(0, leftObstacles.transform.childCount);
                leftObstacles.transform.GetChild(numb).gameObject.SetActive(true);
            }
            anim.Play("leftOpen");

        }

    }
}
