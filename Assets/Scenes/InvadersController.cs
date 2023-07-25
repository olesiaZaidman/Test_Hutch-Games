using UnityEngine;
using UnityEngine.UI;

public class InvadersController : GameObjectManager
{

    Vector3 invadersStartingPosition = new Vector3(-5, 1, 0);
    Color invadersColor = new Color(0.3f, 0.045f, 0.46f);

    static int maxInvadersAmount = 10;
    static int maxInvaderHeight = 5;

    GameObject[,] invaders = new GameObject[maxInvadersAmount, maxInvaderHeight];


    bool invadersMovingLeft = true;
    bool invadersMovingDown = false;

    int numInvadersDead = 0;
    int invadersLayer = 2;


    void CreateInvaders()
    {
        float invadersHorizontalSpacing = 1.0f;
        float invadersVerticalSpacing = 0.5f;
        float topRowScale = 0.4f;
        float otherRowsScale = 0.6f;
        float invadersDepthScale = 0.5f;

        for (int i = 0; i < invaders.GetLength(0); i++)
        {
            for (int j = 0; j < invaders.GetLength(1); j++)
            {
                invaders[i, j] = CreateGameObject(primitiveType);

                Vector3 position = invadersStartingPosition + new Vector3(i * invadersHorizontalSpacing, j * invadersVerticalSpacing, 0);
                //  Vector3 position = new Vector3(i - 5, (j * 0.5f) + 1, 0);

                Vector3 localScale = new Vector3((j == (invaders.GetLength(1) - 1)) ? topRowScale : otherRowsScale, topRowScale, invadersDepthScale);
             //   Vector3 localScale = new Vector3((j == (invaders.GetLength(1) - 1)) ? 0.4f : 0.6f, 0.4f, 0.5f); 
                // Top row are smaller and harder to hit

                SetTransformProperties(invaders[i, j], position, localScale);
                AssignLayerToGameObject(invaders[i, j], invadersLayer);
                SetGameObjectColor(invaders[i, j], invadersColor);
                SetGameObjectName(invaders[i, j], "Invader " + i + "," + j);
            }
        }
    }

    void Start()
    {
        CreateInvaders();
    }

    void Update()
    {
    //    if (!GameManager.GameOver)
    //    {
    //        bool moveLeftThisUpdate = invadersMovingLeft;
    //        bool moveDownThisUpdate = invadersMovingDown;
    //        invadersMovingDown = false;
    //        int numInvadersAlive = (invaders.GetLength(0) * invaders.GetLength(1)) - numInvadersDead;
    //        float invaderSpeed = 0.3f + ((numInvadersDead / 10) * 0.1f) + ((numInvadersAlive <= 3) ? ((4 - numInvadersAlive) * 1f) : 0.0f);
    //        for (int i = 0; i < invaders.GetLength(0); i++)
    //            for (int j = 0; j < invaders.GetLength(1); j++)
    //                if (invaders[i, j].activeSelf)
    //                {
    //                    Vector3 newPos = invaders[i, j].transform.position;
    //                    if (moveDownThisUpdate)
    //                        newPos.y -= 0.25f;
    //                    if (newPos.y < -2f)
    //                        GameManager.SetGameOver(true);
    //                    // InvadersController reached bases = game over
    //                    newPos.x -= (moveLeftThisUpdate ? invaderSpeed : -invaderSpeed) * Time.deltaTime;
    //                    if ((invadersMovingLeft && (newPos.x < -7.0f)) || ((!invadersMovingLeft) && (newPos.x > 7.0f)))
    //                    {
    //                        invadersMovingLeft = !invadersMovingLeft;
    //                        invadersMovingDown = true;
    //                    }
    //                    invaders[i, j].transform.position = newPos;
    //                    if (!bullets[(i % (bullets.Length - 1)) + 1].activeSelf && ((j == 0) || !invaders[i, j - 1].activeSelf) && (Random.value < 0.01f))
    //                    {
    //                        bullets[(i % (bullets.Length - 1)) + 1].transform.position = invaders[i, j].transform.position - (Vector3.up * 0.5f);
    //                        bullets[(i % (bullets.Length - 1)) + 1].SetActive(true);
    //                    }
    //                }
    //        for (int i = 0; i < bullets.Length; i++)
    //            if (bullets[i].activeSelf)
    //            {
    //                float newBulletY = bullets[i].transform.position.y + ((i == 0) ? 20f : -5f) * Time.deltaTime;
    //                bullets[i].transform.position = new Vector3(bullets[i].transform.position.x, newBulletY, bullets[i].transform.position.z);
    //                Collider[] hits = Physics.OverlapBox(bullets[i].GetComponent<Collider>().bounds.center, bullets[i].GetComponent<Collider>().bounds.extents, bullets[i].transform.rotation, ((i == 0) ? (1 << 2) + (1 << 5) : (1 << 1) + (1 << 4)) + (1 << 3));
    //                if ((hits != null) && (hits.Length > 0))
    //                {
    //                    bullets[i].SetActive(false);
    //                    hits[0].gameObject.SetActive(false);

    //                    if (hits[0].gameObject.layer == 2)
    //                    {
    //                        GameManager.AddPoints(GameManager.Score, 10);
    //                        numInvadersDead++;
    //                    }
    //                    else if (hits[0].gameObject.layer == 1)
    //                    {
    //                        GameManager.SubstractPoints(GameManager.Lives, 1); //--GameManager.Lives 

    //                        if (GameManager.Lives >= 0) //    if (--GameManager.Lives >= 0)
    //                        {
    //                            playerLives[GameManager.Lives].SetActive(false);
    //                            player.SetActive(true);
    //                            player.transform.position = new Vector3(-7, -4, 0);
    //                        }
    //                        else
    //                            GameManager.SetGameOver(true);                          // Player dead = game over
    //                    }
    //                }
    //                if ((bullets[i].transform.position.y < -4) || (bullets[i].transform.position.y > 4))
    //                    bullets[i].SetActive(false);
    //            }

    //        float newPlayerX = player.transform.position.x + (Input.GetAxis("Horizontal") * 10f * Time.deltaTime);
    //        player.transform.position = new Vector3(Mathf.Clamp(newPlayerX, -7f, 7f), -4f, 0f);

    //        if (Input.GetKeyDown(KeyCode.LeftShift) && !bullets[0].activeSelf)
    //        {
    //            bullets[0].transform.position = player.transform.position;
    //            bullets[0].SetActive(true);                                 // Fire a player bullet
    //        }

    //        if (numInvadersAlive == 0)
    //            UnityEngine.SceneManagement.SceneManager.LoadScene(0);      // Reload scene for a new attack wave
    //    }

    //    else
    //    {
    //        if (Input.GetKeyDown(KeyCode.Space))
    //        {
    //            GameManager.ResetLives();
    //            GameManager.ResetScore();

    //            UnityEngine.SceneManagement.SceneManager.LoadScene(0);      // Reload scene and reset score & lives for a new game
    //        }
    //    }
    //    UIManager.Instance.UpdateUITextScoreOnGameOver();
    }
}