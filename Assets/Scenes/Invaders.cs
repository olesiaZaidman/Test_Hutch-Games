using UnityEngine;
using UnityEngine.UI;

public class Invaders : MonoBehaviour
{
    GameObject player;
    Vector3 playerStartingPosition = new Vector3(-7, -4, 0);
    Vector3 playerLocalScale = new Vector3(0.75f, 0.5f, 0.5f);

    Vector3 playerLivesStartingPosition = new Vector3(7, 6, 0);
    Vector3 playerLivesLocalScale = new Vector3(0.75f, 0.5f, 0.5f);

    // playerLives[i].transform.position = new Vector3(7 + (i* 1f), 6, 0);

   Color playerColor = Color.blue;
   Color playerLivesColor = Color.red;

   private PrimitiveType primitiveType = PrimitiveType.Cube;


    GameObject[] bullets = new GameObject[4];       // Bullet 0 is player's, rest are invader's
    GameObject[] playerLives = new GameObject[2];
    GameObject[,] invaders = new GameObject[10, 5];
    GameObject[] baseBricks = new GameObject[4];

    bool invadersMovingLeft = true;
    bool invadersMovingDown = false;

    int numInvadersDead = 0;


    void SetGameObjectColor(GameObject gameObject, Color color)
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
        }
    }

    void CreatePlayer()
    {
        player = CreateGameObject(PrimitiveType.Cube); //or primitiveType
        SetTransformProperties(player, playerStartingPosition, playerLocalScale);
        SetGameObjectColor(player, playerColor);
        player.name = "Player";
        //   SetGameObjectPosition(player, playerStartingPosition);
        //  SetGameObjectLocalScale(player, playerLocalScale);
    }


    GameObject CreateGameObject(PrimitiveType type) //   player = GameObject.CreatePrimitive(PrimitiveType.Cube);
    {
        GameObject gameObject = GameObject.CreatePrimitive(type);
        return gameObject;
    }

    void SetTransformProperties(GameObject gameObject, Vector3 position, Vector3 localScale)
    {
        gameObject.transform.position = position;
        gameObject.transform.localScale = localScale;
    }


    void SetGameObjectLocalScale(GameObject gameObject, Vector3 scale)
    {
        gameObject.transform.localScale = scale;
    }

    void SetGameObjectPosition(GameObject gameObject, Vector3 position)
    {
        gameObject.transform.position = position;
    }

    void CreatePlayerLives()
    {
        playerLives = new GameObject[GameManager.MaxPlayerLives];

        for (int i = 0; i < GameManager.MaxPlayerLives; i++)
        {
            playerLives[i] = CreateGameObject(primitiveType);
            Vector3 position = playerLivesStartingPosition + new Vector3(i * 1f, 0, 0);

            SetTransformProperties(playerLives[i], position, playerLivesLocalScale);
            SetGameObjectColor(playerLives[i], playerLivesColor);
            playerLives[i].name = "PlayerLives "+(i+1);

        }

        //for (int i = 0; (i < playerLives.Length) && (i < GameManager.Lives); i++)
        //{
        //    playerLives[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //    SetGameObjectColor(playerLives[i], playerLivesColor);
        //    playerLives[i].transform.position = new Vector3(7 + (i * 1f), 6, 0);
        //    playerLives[i].transform.localScale = new Vector3(0.75f, 0.5f, 0.5f);
        //}
    }


    void CreateEnemies()
    {


    }

    void CreateBullets()
    {


    }


    void CreateBricks()
    {


    }


    void AssignLayerToGameObject(GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
    }




    void Start()
    {

        CreatePlayer();
        AssignLayerToGameObject(player, 1);


        for (int i = 0; (i < playerLives.Length) && (i < GameManager.Lives); i++)
        {
            playerLives[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            SetGameObjectColor(playerLives[i], Color.red);
            playerLives[i].transform.position = new Vector3(7 + (i * 1f), 6, 0);
            playerLives[i].transform.localScale = new Vector3(0.75f, 0.5f, 0.5f);
        }


        for (int i = 0; i < invaders.GetLength(0); i++)
            for (int j = 0; j < invaders.GetLength(1); j++)
            {
                invaders[i, j] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                invaders[i, j].transform.position = new Vector3(i - 5, (j * 0.5f) + 1, 0);
                invaders[i, j].transform.localScale = new Vector3((j == (invaders.GetLength(1) - 1)) ? 0.4f : 0.6f, 0.4f, 0.5f);    // Top row are smaller and harder to hit
                invaders[i, j].layer = 2;
            }

        for (int b = 0; b < baseBricks.Length; b++)

            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 4; j++)
                {
                    GameObject baseBrick = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    baseBrick.transform.position = new Vector3((b * 2.2f) + (i * 0.2f) - 3.6f, (j * 0.2f) - 3, 0);
                    baseBrick.transform.localScale = new Vector3(0.2f, 0.2f, 0.5f);
                    baseBrick.layer = 3;
                }

        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            SetGameObjectColor(bullets[i], Color.yellow);
            bullets[i].transform.localScale = new Vector3(0.1f, 0.5f, 0.5f);
            bullets[i].SetActive(false);
            bullets[i].layer = (i == 0) ? 4 : 5;    // Bullet 0 is player bullet
        }

        CreatePlayerLives();

    }

    void Update()
    {
        if (!GameManager.GameOver)
        {
            bool moveLeftThisUpdate = invadersMovingLeft;
            bool moveDownThisUpdate = invadersMovingDown;
            invadersMovingDown = false;
            int numInvadersAlive = (invaders.GetLength(0) * invaders.GetLength(1)) - numInvadersDead;
            float invaderSpeed = 0.3f + ((numInvadersDead / 10) * 0.1f) + ((numInvadersAlive <= 3) ? ((4 - numInvadersAlive) * 1f) : 0.0f);
            for (int i = 0; i < invaders.GetLength(0); i++)
                for (int j = 0; j < invaders.GetLength(1); j++)
                    if (invaders[i, j].activeSelf)
                    {
                        Vector3 newPos = invaders[i, j].transform.position;
                        if (moveDownThisUpdate)
                            newPos.y -= 0.25f;
                        if (newPos.y < -2f)
                            GameManager.SetGameOver(true);
                        // Invaders reached bases = game over
                        newPos.x -= (moveLeftThisUpdate ? invaderSpeed : -invaderSpeed) * Time.deltaTime;
                        if ((invadersMovingLeft && (newPos.x < -7.0f)) || ((!invadersMovingLeft) && (newPos.x > 7.0f)))
                        {
                            invadersMovingLeft = !invadersMovingLeft;
                            invadersMovingDown = true;
                        }
                        invaders[i, j].transform.position = newPos;
                        if (!bullets[(i % (bullets.Length - 1)) + 1].activeSelf && ((j == 0) || !invaders[i, j - 1].activeSelf) && (Random.value < 0.01f))
                        {
                            bullets[(i % (bullets.Length - 1)) + 1].transform.position = invaders[i, j].transform.position - (Vector3.up * 0.5f);
                            bullets[(i % (bullets.Length - 1)) + 1].SetActive(true);
                        }
                    }
            for (int i = 0; i < bullets.Length; i++)
                if (bullets[i].activeSelf)
                {
                    float newBulletY = bullets[i].transform.position.y + ((i == 0) ? 20f : -5f) * Time.deltaTime;
                    bullets[i].transform.position = new Vector3(bullets[i].transform.position.x, newBulletY, bullets[i].transform.position.z);
                    Collider[] hits = Physics.OverlapBox(bullets[i].GetComponent<Collider>().bounds.center, bullets[i].GetComponent<Collider>().bounds.extents, bullets[i].transform.rotation, ((i == 0) ? (1 << 2) + (1 << 5) : (1 << 1) + (1 << 4)) + (1 << 3));
                    if ((hits != null) && (hits.Length > 0))
                    {
                        bullets[i].SetActive(false);
                        hits[0].gameObject.SetActive(false);

                        if (hits[0].gameObject.layer == 2)
                        {
                            GameManager.AddPoints(GameManager.Score, 10);
                            numInvadersDead++;
                        }
                        else if (hits[0].gameObject.layer == 1)
                        {
                            GameManager.SubstractPoints(GameManager.Lives, 1); //--GameManager.Lives 

                            if (GameManager.Lives >= 0) //    if (--GameManager.Lives >= 0)
                            {
                                playerLives[GameManager.Lives].SetActive(false);
                                player.SetActive(true);
                                player.transform.position = new Vector3(-7, -4, 0);
                            }
                            else
                                GameManager.SetGameOver(true);                          // Player dead = game over
                        }
                    }
                    if ((bullets[i].transform.position.y < -4) || (bullets[i].transform.position.y > 4))
                        bullets[i].SetActive(false);
                }

            float newPlayerX = player.transform.position.x + (Input.GetAxis("Horizontal") * 10f * Time.deltaTime);
            player.transform.position = new Vector3(Mathf.Clamp(newPlayerX, -7f, 7f), -4f, 0f);

            if (Input.GetKeyDown(KeyCode.LeftShift) && !bullets[0].activeSelf)
            {
                bullets[0].transform.position = player.transform.position;
                bullets[0].SetActive(true);                                 // Fire a player bullet
            }

            if (numInvadersAlive == 0)
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);      // Reload scene for a new attack wave
        }

        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameManager.ResetLives();
                GameManager.ResetScore();

                UnityEngine.SceneManagement.SceneManager.LoadScene(0);      // Reload scene and reset score & lives for a new game
            }
        }
        UIManager.Instance.UpdateUITextScoreOnGameOver();
    }
}