
using UnityEngine;

public class BulletsController : GameObjectManager
{
    const float PLAYER_BULLET_SPEED = 20f;
    const float ENEMY_BULLET_SPEED = -5f;

    float bulletSpeed;

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
            AssignLayerToGameObject(bullets[i], (i == 0) ? GameManager.PLAYER_BULLET_LAYER : GameManager.INVADERS_BULLET_LAYER); // Bullet 0 is player bullet (layer #4 is PlayerBullet)
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
                bulletSpeed = (i == 0) ? PLAYER_BULLET_SPEED : ENEMY_BULLET_SPEED;
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
            int layerMask = ((bullet.layer == GameManager.PLAYER_BULLET_LAYER) ?
                (1 << GameManager.INVADERS_LAYER) +  (1 << GameManager.INVADERS_BULLET_LAYER) :
                (1 << GameManager.PLAYER_LAYER) + (1 << GameManager.PLAYER_BULLET_LAYER))
                + (1 << GameManager.BRICK_LAYER); //bitwise operations 

            return Physics.OverlapBox(center, halfExtents, orientation, layerMask);

        }



        void HandleBulletCollision(GameObject bullet, GameObject target)
        {
            bullet.SetActive(false);
         //   Debug.Log("HandleBulletCollision: "+ target.layer);
            target.SetActive(false);

            if (target.layer == GameManager.INVADERS_LAYER)
            {
                GameManager.IncreaseScore(GameManager.VICTORY_POINTS);
                UIManager.Instance.UpdateUITextScore();
                invadersController.IncreaseNumbersInvadersDead(GameManager.ENEMIES_DAMAGE_AMOUNT);
            }
            else if (target.layer == GameManager.PLAYER_LAYER)
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
