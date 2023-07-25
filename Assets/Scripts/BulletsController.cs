using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsController : GameObjectManager
{

    PlayerController playerController;

    Color bulletsColor = Color.yellow;
    static int maxBulletsAmount = 4;

    GameObject[] bullets = new GameObject[maxBulletsAmount];       // Bullet 0 is player's, rest are invader's
    Vector3 bulletLocalScale = new Vector3(0.1f, 0.5f, 0.5f);

    InvadersController invadersController;

    float playerBulletSpeed = 20f;
    float enemyBulletSpeed = -5f;

    float bulletSpeed;
    void CreateBullets()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = CreateGameObject(primitiveType);
            SetGameObjectLocalScale(bullets[i], bulletLocalScale);

            bullets[i].SetActive(false);
            AssignLayerToGameObject(bullets[i], (i == 0) ? GameManager.playerBulletLayer : GameManager.invadersBulletLayer); // Bullet 0 is player bullet (layer #4 is PlayerBullet)
            SetGameObjectColor(bullets[i], bulletsColor);
            SetGameObjectName(bullets[i], "Bullet " + (i + 1));
        }

    }

    void Start()
    {
        invadersController = FindObjectOfType<InvadersController>();
        playerController = FindObjectOfType<PlayerController>();
        CreateBullets();
    }


    void Update()
    {
        if (!GameManager.GameOver)
        {
            UpdateBulletsMovement();
        }

    }
   public void PlayerShootBullets()
    {
        //PlayerController playerController = FindObjectOfType<PlayerController>();

        if (!bullets[0].activeSelf)
        {
            bullets[0].transform.position = playerController.GetPlayerPosition();
            bullets[0].SetActive(true);                                 // Fire a player bullet}
        }
    }
  
    void UpdateBulletsMovement()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            if (bullets[i].activeSelf)
            {
                bulletSpeed = (i == 0) ? playerBulletSpeed : enemyBulletSpeed;

                float newBulletY = bullets[i].transform.position.y + (bulletSpeed * Time.deltaTime);
                MoveBulletVertically(bullets[i], newBulletY);

                Collider[] hits = CheckCollisions(bullets[i]);

                if (hits != null && hits.Length > 0)
                {
                    HandleBulletCollision(bullets[i], hits[0].gameObject);
                }

                if (IsBulletOutOfBoundaries(bullets[i]))
                {
                    bullets[i].SetActive(false);
                }
            }
        }
        void MoveBulletVertically(GameObject bullet, float newY)
        {
            bullet.transform.position = new Vector3(bullet.transform.position.x, newY, bullet.transform.position.z);
        }

        Collider[] CheckCollisions(GameObject bullet)
        {
            Vector3 center = bullet.GetComponent<Collider>().bounds.center;
            Vector3 halfExtents = bullet.GetComponent<Collider>().bounds.extents;
            Quaternion orientation = bullet.transform.rotation;
            int layerMask = (bullet.layer == 0) ? GameManager.invadersLayer + GameManager.invadersBulletLayer : GameManager.playerLayer + GameManager.playerBulletLayer;

            //  int layerMask = (bullet.layer == 0) ? (1 << 2) + (1 << 5) : (1 << 1) + (1 << 4); //bitwise operations 
            // (1 << 2) + (1 << 5) corresponds  to layers 2 and 5 (GameManager.invadersLayer and GameManager.invadersBulletLayer)
            //(1 << 1) + (1 << 4) corresponds  to layers 1 and 4 (GameManager.playerLayer and GameManager.playerBulletLayer )

            return Physics.OverlapBox(center, halfExtents, orientation, layerMask);

        }

        void HandleBulletCollision(GameObject bullet, GameObject target)
        {
            bullet.SetActive(false);
            target.SetActive(false);

            if (target.layer == GameManager.invadersLayer)
            {
                GameManager.AddPoints(GameManager.Score, GameManager.VictoryPoints);
                invadersController.IncreaseNumbersInvadersDead(1);
            }
            else if (target.layer == GameManager.playerLayer)
            {
                GameManager.SubstractPoints(GameManager.Lives, GameManager.DamageAmount);//--GameManager.Lives 
                if (GameManager.Lives >= 0)
                {
                    playerController.TakeDamage();
                }
                else
                {
                    GameManager.SetGameOver(true);
                }
            }
        }

        bool IsBulletOutOfBoundaries(GameObject bullet)
        {
            float boundary = 4f;
            return bullet.transform.position.y < -boundary || bullet.transform.position.y > boundary;
        }




        //for (int i = 0; i < bullets.Length; i++)

        //    if (bullets[i].activeSelf)
        //    {
        //        float newBulletY = bullets[i].transform.position.y + ((i == 0) ? 20f : -5f) * Time.deltaTime;
        //        bullets[i].transform.position = new Vector3(bullets[i].transform.position.x, newBulletY, bullets[i].transform.position.z);
        //        Collider[] hits = Physics.OverlapBox(bullets[i].GetComponent<Collider>().bounds.center, bullets[i].GetComponent<Collider>().bounds.extents, bullets[i].transform.rotation, ((i == 0) ? (1 << 2) + (1 << 5) : (1 << 1) + (1 << 4)) + (1 << 3));

        //        if ((hits != null) && (hits.Length > 0))
        //        {
        //            bullets[i].SetActive(false);
        //            hits[0].gameObject.SetActive(false);

        //            if (hits[0].gameObject.layer == GameManager.invadersLayer)
        //            {
        //                GameManager.AddPoints(GameManager.Score, GameManager.VictoryPoints);
        //                invadersController.IncreaseNumbersInvadersDead(1);
        //            }
        //            else if (hits[0].gameObject.layer == GameManager.playerLayer)
        //            {
        //                GameManager.SubstractPoints(GameManager.Lives, GameManager.DamageAmount); //--GameManager.Lives 

        //                if (GameManager.Lives >= 0) //    if (--GameManager.Lives >= 0)
        //                {
        //                    playerController.TakeDamage();
        //                }
        //                else
        //                    GameManager.SetGameOver(true);                          // Player dead = game over
        //            }
        //        }
        //        if ((bullets[i].transform.position.y < -4) || (bullets[i].transform.position.y > 4))
        //            bullets[i].SetActive(false);
        //    }


    }


}
