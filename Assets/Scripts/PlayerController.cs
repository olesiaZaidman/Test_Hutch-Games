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
    int playerLayer = 1; 

    void CreatePlayer()
    {
        player = CreateGameObject(primitiveType); //or primitiveType
        SetTransformProperties(player, playerStartingPosition, playerLocalScale);
        SetGameObjectColor(player, playerColor);
        SetGameObjectName(player, "Player");
        AssignLayerToGameObject(player, playerLayer);
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
    void Start()
    {
        CreatePlayer();
        CreatePlayerLives();
    }

    void Update()
    {
        
    }
}
