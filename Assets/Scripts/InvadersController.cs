using UnityEngine;
using UnityEngine.UI;

public class InvadersController : GameObjectManager
{

    Vector3 invadersStartingPosition = new Vector3(-5, 1, 0);
    Color invadersColor = new Color(0.3f, 0.045f, 0.46f);

    static int maxInvadersHorizontalAmount = 10;
    static int maxInvadersVerticalAmount = 5;

    GameObject[,] invaders = new GameObject[maxInvadersHorizontalAmount, maxInvadersVerticalAmount];


    bool invadersMovingLeft = true;
    bool invadersMovingDown = false;

    int numInvadersDead = 0;
    static int numInvadersAlive;
    float invadersMovementSpeed;

    private BulletsController bulletsController;
    GameObject[] bullets;

    public static int NumInvadersAlive
    {
        get { return numInvadersAlive; }
        //   private set { numInvadersAlive = value; }
    }

    void Start()
    {
        CreateInvaders();
        bulletsController = FindObjectOfType<BulletsController>();
        if (bulletsController != null)
        {
            bullets = bulletsController.Bullets;
        }
        else
        {
            Debug.LogError("BulletsController not found in the scene.");
        }

    }

    void Update()
    {
        if (!GameManager.GameOver)
        {
            UpdateInvadersMovement();
        }
    }

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
                AssignLayerToGameObject(invaders[i, j], GameManager.invadersLayer);
                SetGameObjectColor(invaders[i, j], invadersColor);
                SetGameObjectName(invaders[i, j], "Invader " + i + "," + j);
            }
        }
    }

    void UpdateNumbersInvadersAlive()
    {
        numInvadersAlive = (invaders.GetLength(0) * invaders.GetLength(1)) - numInvadersDead;
    }

    public void IncreaseNumbersInvadersDead(int i)
    {
        numInvadersDead += i; //        numInvadersDead++;
    }


    float CalculateInvaderSpeed()
    {
        return 0.3f + ((numInvadersDead / 10) * 0.1f) + ((numInvadersAlive <= 3) ? ((4 - numInvadersAlive) * 1f) : 0.0f);
    }

    void UpdateInvadersMovement()
    {
        bool moveLeftThisUpdate = invadersMovingLeft;
        bool moveDownThisUpdate = invadersMovingDown;

        invadersMovingDown = false;

        UpdateNumbersInvadersAlive();

        invadersMovementSpeed = CalculateInvaderSpeed();


        for (int i = 0; i < invaders.GetLength(0); i++)
        {
            for (int j = 0; j < invaders.GetLength(1); j++)
            {

                if (invaders[i, j].activeSelf)
                {
                    Vector3 newPos = CalculateNewInvaderPosition(invaders[i, j].transform.position, moveLeftThisUpdate, moveDownThisUpdate, invadersMovementSpeed);
                    invaders[i, j].transform.position = newPos;

                    TryShootBullet(i, j);
                }
            }
        }
    }


    void TryShootBullet(int i, int j)
    {
        int bulletIndex = (i % (bullets.Length - 1)) + 1;

        GameObject currentInvader = invaders[i, j];
        GameObject invaderAbove = (j - 1 >= 0) ? invaders[i, j - 1] : null; // Check if invaderAbove exists


        //  GameObject invaderAbove = invaders[i, j - 1];
        //we check if there is an invaderAbove in the row above to determine
        //if a new bullet can be shot from the current invaderAbove.

        bool canShoot = !bullets[bulletIndex].activeSelf &&
                        ((j == 0) || !invaderAbove.activeSelf) && /*&&
                                               (invaderAbove == null || 
                                               !invaderAbove.activeSelf)???*/
                        (Random.value < 0.01f);


        if (canShoot)
        {
            Vector3 bulletPosition = currentInvader.transform.position - (Vector3.up * 0.5f);

            bullets[bulletIndex].transform.position = bulletPosition;
            bullets[bulletIndex].SetActive(true);
        }

        /*
         	if( !bullets[(i%(bullets.Length-1))+1].activeSelf && ((j == 0) || !invaders[i,j-1].activeSelf) && (Random.value < 0.01f) )
	{
	bullets[(i%(bullets.Length-1))+1].transform.position = invaders[i,j].transform.position - (Vector3.up*0.5f);
	bullets[(i%(bullets.Length-1))+1].SetActive(true);
	}
        */

    }

    Vector3 CalculateNewInvaderPosition(Vector3 currentPosition, bool moveLeft, bool moveDown, float invaderSpeed)
    {
        Vector3 newPos = currentPosition;

        if (moveDown)
        {
            newPos.y -= 0.25f;
        }

        if (newPos.y < -2f)
        {
            GameManager.SetGameOver(true);
        }

        float movementDirection = moveLeft ? -1f : 1f;
        newPos.x -= movementDirection * invaderSpeed * Time.deltaTime;

        if ((moveLeft && (newPos.x < -7.0f)) || (!moveLeft && (newPos.x > 7.0f)))
        {
            invadersMovingLeft = !moveLeft;
            invadersMovingDown = true;
        }

        return newPos;


        /*
                    Vector3 newPos = invaders[i, j].transform.position;
                    if (moveDownThisUpdate)
                        newPos.y -= 0.25f;
                    if (newPos.y < -2f)
                        GameManager.SetGameOver(true);
                    // InvadersController reached bases = game over
                    newPos.x -= (moveLeftThisUpdate ? invadersMovementSpeed : -invadersMovementSpeed) * Time.deltaTime;
                    if ((invadersMovingLeft && (newPos.x < -7.0f)) || ((!invadersMovingLeft) && (newPos.x > 7.0f)))
                    {
                        invadersMovingLeft = !invadersMovingLeft;
                        invadersMovingDown = true;
                    }
                   */
    }



}