using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsController : GameObjectManager
{
    Color bulletsColor = Color.yellow;
    static int maxBulletsAmount = 4;

    GameObject[] bullets = new GameObject[maxBulletsAmount];       // Bullet 0 is player's, rest are invader's
    Vector3 bulletLocalScale = new Vector3(0.1f, 0.5f, 0.5f);
    int playerBulletLayer = 4;
    int invadersBulletLayer = 5;
    void CreateBullets()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = CreateGameObject(primitiveType);
            SetGameObjectLocalScale(bullets[i], bulletLocalScale);

            bullets[i].SetActive(false);
            AssignLayerToGameObject(bullets[i], (i == 0) ? playerBulletLayer : invadersBulletLayer); // Bullet 0 is player bullet (layer #4 is PlayerBullet)
            SetGameObjectColor(bullets[i], bulletsColor);
            SetGameObjectName(bullets[i], "Bullet " + (i + 1));
        }

    }


    void Start()
    {
        CreateBullets();
    }


    void Update()
    {
        
    }
}
