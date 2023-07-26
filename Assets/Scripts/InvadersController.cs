using UnityEngine;


public class InvadersController : GameObjectManager
{

    //[Range(1f, 10f)]
    //public float invadersSpeedCoefficient = 3f;   
    const float INVADERS_SPEED_COEFFICIENT = 3f;
    const float MIN_INVADERS_SPEED = 0.3f;
    const float DEAD_INVADERS_SPEED_COEFFICIENT = 0.1f;
    const float BOSS_LEVEL_SPEED_COEFFICIENT = 1f;
    const int MAX_AMOUNT_INVADERS_BOSS_LEVEL = 3;

    const float LEFT_BOUNDARY = -7f;
    const float RIGHT_BOUNDARY = 7f;
    const float LOWEST_BOUNDARY = -2f;
    const float VERTICAL_MOVE_DISTANCE = 0.25f;

    const int MAX_INVADERS_HORIZ_COUNT = 10;
    const int MAX_INVADERS_VERT_COUNT = 5;

    GameObject[,] invaders = new GameObject[MAX_INVADERS_HORIZ_COUNT, MAX_INVADERS_VERT_COUNT];

    Vector3 invadersStartingPosition = new Vector3(-5, 1, 0);
    Color invadersColor = new Color(0.3f, 0.045f, 0.46f);

    int numInvadersDead = 0;
    static int numInvadersAlive;
    public static int NumInvadersAlive => numInvadersAlive;

    bool invadersMovingLeft = true;
    bool invadersMovingDown = false;

    float invadersMovementSpeed;

    private BulletsController bulletsController;
    GameObject[] bullets;

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

                Vector3 localScale = new Vector3((j == (invaders.GetLength(1) - 1)) ? topRowScale : otherRowsScale, topRowScale, invadersDepthScale);
                // Top row are smaller and harder to hit

                SetTransformProperties(invaders[i, j], position, localScale);
                AssignLayerToGameObject(invaders[i, j], GameManager.INVADERS_LAYER);
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
        numInvadersDead += i; 
    }


    float CalculateInvaderSpeed()
    {
        float extraSpeedWhenInvedersDie = ((numInvadersDead / MAX_INVADERS_HORIZ_COUNT) * DEAD_INVADERS_SPEED_COEFFICIENT);

        bool isBossLevel = (numInvadersAlive <= MAX_AMOUNT_INVADERS_BOSS_LEVEL);

        float extraSpeedInBossLevel = !isBossLevel ? 0 : (
            ((MAX_AMOUNT_INVADERS_BOSS_LEVEL + 1) - numInvadersAlive) * BOSS_LEVEL_SPEED_COEFFICIENT
        );

        return MIN_INVADERS_SPEED + extraSpeedWhenInvedersDie + extraSpeedInBossLevel;
    }

    void UpdateInvadersMovement()
    {
        UpdateNumbersInvadersAlive();

        invadersMovementSpeed = CalculateInvaderSpeed() * INVADERS_SPEED_COEFFICIENT;

        bool isMovingDownCurFrame = invadersMovingDown;

        invadersMovingDown = false;

        for (int i = 0; i < invaders.GetLength(0); i++)
        {
            for (int j = 0; j < invaders.GetLength(1); j++)
            {
                if (invaders[i, j].activeSelf)
                {
                    Vector3 newPos = CalculateNewInvaderPosition(invaders[i, j].transform.position, isMovingDownCurFrame, invadersMovementSpeed);
                    invaders[i, j].transform.position = newPos;
                    TryShootBullet(i, j);
                }
            }
        }

    }

    Vector3 CalculateNewInvaderPosition(Vector3 currentPosition, bool isMovingDownCurFrame, float invaderSpeed)
    {
        Vector3 newPos = currentPosition;

        float movementDirection = invadersMovingLeft ? -1f : 1f;
        newPos.x += movementDirection * invaderSpeed * Time.deltaTime;

        if (isMovingDownCurFrame)
        {
            newPos.y -= VERTICAL_MOVE_DISTANCE;
        }

        if (newPos.y < LOWEST_BOUNDARY)
        {
            GameManager.SetGameOver(true);
        }

        if (newPos.x < LEFT_BOUNDARY)
        {
            invadersMovingLeft = false;
            invadersMovingDown = true;

        }

        if (newPos.x > RIGHT_BOUNDARY)
        {
            invadersMovingLeft = true;
            invadersMovingDown = true;
        }

        return newPos;

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

    }
}