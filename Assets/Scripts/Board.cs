using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private int rows;
    [SerializeField] private int columns;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject ballReference;
    [SerializeField] private float padding = 0.1f;
    [SerializeField] private GameObject[] balls;


    public GameObject[,] allBalls;


   

    void Start()
    {
        allBalls = new GameObject[rows, columns];
        SetUp();
    }

   

    private void SetUp()
    {
        //grabbing the balls size for alignment and padding
        SpriteRenderer ballRenderer = ballReference.GetComponent<SpriteRenderer>();
        Vector2 ballSize = ballRenderer.bounds.size;
        Vector2 cellSize = new Vector2(ballSize.x + padding, ballSize.y + padding);

        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                Vector2 tempPosition = new Vector2(x * cellSize.x, y * cellSize.y);
                tempPosition += new Vector2(rows / 2, columns/ 2);
                
                int ballToUse = Random.Range(0, balls.Length);
                GameObject ball = Instantiate(balls[ballToUse], tempPosition, Quaternion.identity);
                ball.transform.parent = this.transform;
                ball.name = "(" + x + ", " + y + ")";

                allBalls[x, y] = ball;
            }

        }

    }
}
