using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : GameObjectManager
{
    GameObject player;
    GameObject[] playerLives = new GameObject[GameManager.MaxPlayerLives];

    Color playerColor = Color.blue;
    Color playerLivesColor = Color.red;

    Vector3 playerStartingPosition = new Vector3(-7, -4, 0);
    Vector3 playerLocalScale = new Vector3(0.75f, 0.5f, 0.5f);

    Vector3 playerLivesStartingPosition = new Vector3(7, 6, 0);
    Vector3 playerLivesLocalScale = new Vector3(0.75f, 0.5f, 0.5f);


    BulletsController bulletsController;

    float movementInput = Input.GetAxis("Horizontal");
    float movementSpeed = 10f;

    void Start()
    {
        CreatePlayer();
        CreatePlayerLives();
        bulletsController = FindObjectOfType<BulletsController>();  
    }

    public Vector3 GetPlayerPosition()
    {
        return player.transform.position;
    }

    public void TakeDamage()
    {
        playerLives[GameManager.Lives].SetActive(false);
        player.SetActive(true);
        player.transform.position = playerStartingPosition;
        Debug.Log("GameManager.Lives = "+ GameManager.Lives);

    }

    void Update()
    {
        if (!GameManager.GameOver)
        {
            UpdatePlayerMovement();

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                bulletsController.PlayerShootBullets();
            }

            if (InvadersController.NumInvadersAlive == 0)
            {
                GameManager.SetWinGameLevel(true);
            }
        }

    }

    void CreatePlayer()
    {
        player = CreateGameObject(primitiveType); //or primitiveType
        SetTransformProperties(player, playerStartingPosition, playerLocalScale);
        SetGameObjectColor(player, playerColor);
        SetGameObjectName(player, "Player");
        AssignLayerToGameObject(player, GameManager.playerLayer);
    }



    void CreatePlayerLives()
    {
        // playerLives = new GameObject[GameManager.MaxPlayerLives];

        for (int i = 0; i < playerLives.Length; i++)
        {
            playerLives[i] = CreateGameObject(primitiveType);
            Vector3 position = playerLivesStartingPosition + new Vector3(i * 1f, 0, 0);

            SetTransformProperties(playerLives[i], position, playerLivesLocalScale);
            SetGameObjectColor(playerLives[i], playerLivesColor);
            SetGameObjectName(playerLives[i], "PlayerLife " + (i + 1));

        }
    }


    //void UpdatePlayerMovement()
    //{

    //    float newPlayerX = player.transform.position.x + (Input.GetAxis("Horizontal") * 10f * Time.deltaTime);
    //    player.transform.position = new Vector3(Mathf.Clamp(newPlayerX, -7f, 7f), -4f, 0f);

    //}

    private void UpdatePlayerMovement()
    {
        float newPlayerX = CalculateNewPlayerX(movementInput, movementSpeed);

        float minXValue = -7f;
        float maxXValue = 7f;
        float clampedX = Mathf.Clamp(newPlayerX, minXValue, maxXValue);

        Vector3 newPosition = new Vector3(clampedX, -4f, 0f);

        MovePlayerToPosition(newPosition);
    }

    private float CalculateNewPlayerX(float movementInput, float movementSpeed)
    {
        return player.transform.position.x + (movementInput * movementSpeed * Time.deltaTime);
    }

    private void MovePlayerToPosition(Vector3 newPosition)
    {
        player.transform.position = newPosition;
    }
}
