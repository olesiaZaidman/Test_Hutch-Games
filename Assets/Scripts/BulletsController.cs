using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class BulletsController : GameObjectManager
{
    float bulletSpeed;

    [Tooltip("The bullet bulletSpeed shoot by the player")]
    public static float PlayerBulletSpeed { get; private set; } = 20f;
    [Tooltip("The bullet bulletSpeed shoot by the enemies")]
    public static float EnemyBulletSpeed { get; private set; } = -5f;

    Color bulletsColor = Color.yellow;
    const int MAX_BULLETS_COUNT = 4;

    GameObject[] bullets = new GameObject[MAX_BULLETS_COUNT];       // Bullet 0 is player's, rest are invader's
    public GameObject[] Bullets => bullets;

    Vector3 bulletLocalScale = new Vector3(0.1f, 0.5f, 0.5f);

    PlayerController playerController;
    InvadersController invadersController;


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
        if (!bullets[0].activeSelf)
        {
            Vector3 offset = new Vector3(0,1,0);
            bullets[0].transform.position = playerController.GetPlayerPosition()+ offset;
            bullets[0].SetActive(true);                                 // Fire a player bullet
        }
    }
  
    void UpdateBulletsMovement()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            if (bullets[i].activeSelf)
            {
                bulletSpeed = (i == 0) ? PlayerBulletSpeed : EnemyBulletSpeed;
              //  Debug.Log(bullets[i].name + "(bullets[" + i+"] "+ "bulletSpeed: "+ bulletSpeed);
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
            int layerMask = ((bullet.layer == GameManager.playerBulletLayer) ?
                (1 << GameManager.invadersLayer) +  (1 << GameManager.invadersBulletLayer) :
                (1 << GameManager.playerLayer) + (1 << GameManager.playerBulletLayer))
                + (1 << GameManager.brickLayer); //bitwise operations 

            return Physics.OverlapBox(center, halfExtents, orientation, layerMask);

        }



        void HandleBulletCollision(GameObject bullet, GameObject target)
        {
            bullet.SetActive(false);
         //   Debug.Log("HandleBulletCollision: "+ target.layer);
            target.SetActive(false);

            if (target.layer == GameManager.invadersLayer)
            {
                GameManager.IncreaseScore(GameManager.VICTORY_POINTS);
                invadersController.IncreaseNumbersInvadersDead(GameManager.ENEMIES_DAMAGE_AMOUNT);
            }
            else if (target.layer == GameManager.playerLayer)
            {
                playerController.TakeDamage();
            }
        }

        bool IsBulletOutOfBoundaries(GameObject bullet)
        {
            float boundary = 4f;
            return bullet.transform.position.y < -boundary || bullet.transform.position.y > boundary;
        }
    }


}
