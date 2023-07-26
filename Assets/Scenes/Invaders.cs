using UnityEngine;
using UnityEngine.UI;

public class Invaders : MonoBehaviour
{
    Canvas uiCanvas;
    Text uiScore;
    Light dirlight;
    
	static int score = 0;
	static int lives = 2;

	GameObject player;
	GameObject[,] invaders = new GameObject[10, 5];
	GameObject[] bullets = new GameObject[4];		// Bullet 0 is player's, rest are invader's
	GameObject[] playerLives = new GameObject[2];

	bool invadersMovingLeft = true;
	bool invadersMovingDown = false;
	int numInvadersDead = 0;
	bool gameOver = false;

	void Start()
	{
        //Light& Camera Set Up
        gameObject.GetComponent<Camera>().backgroundColor = Color.black;
		dirlight = new GameObject("Light").AddComponent<Light>();
		dirlight.type = LightType.Directional;  //dirlight.color = Color.green;


        //Create Player:
        player = GameObject.CreatePrimitive(PrimitiveType.Cube);
		player.transform.position = new Vector3(-7, -4, 0);
		player.transform.localScale = new Vector3(0.75f, 0.5f, 0.5f);
		player.layer = 1;

        //Create Invaders:
        for (int i=0; i<invaders.GetLength(0); i++)
			for(int j=0; j<invaders.GetLength(1); j++)
			{
				invaders[i,j] = GameObject.CreatePrimitive(PrimitiveType.Cube);
				invaders[i,j].transform.position = new Vector3(i-5, (j*0.5f)+1, 0);
				invaders[i,j].transform.localScale = new Vector3((j==(invaders.GetLength(1)-1))?0.4f:0.6f, 0.4f, 0.5f);	// Top row are smaller and harder to hit
				invaders[i,j].layer = 2;
			}

        //Create Bricks:
        for (int b=0; b<4; b++)
			for(int i=0; i<5; i++)
				for(int j=0; j<4; j++)
				{
					GameObject baseBrick = GameObject.CreatePrimitive(PrimitiveType.Cube);
					baseBrick.transform.position = new Vector3((b*2.2f)+(i*0.2f)-3.6f, (j*0.2f)-3, 0);
					baseBrick.transform.localScale = new Vector3(0.2f, 0.2f, 0.5f);
					baseBrick.layer = 3;
				}

        //Create Bullets for player and invaders:
        for (int i=0; i<bullets.Length; i++)
		{
			bullets[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
			bullets[i].transform.localScale = new Vector3(0.1f, 0.5f, 0.5f);
			bullets[i].SetActive(false);
			bullets[i].layer = (i==0)?4:5;	// Bullet 0 is player bullet
		}
		//create player lives
		for(int i=0; (i<playerLives.Length)&&(i<lives); i++)
		{
			playerLives[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
			playerLives[i].transform.position = new Vector3(-6+(i*1f), -5, 0);
			playerLives[i].transform.localScale = new Vector3(0.75f, 0.5f, 0.5f);
		}

		//Ui Manager:
		uiCanvas = new GameObject("UI").AddComponent<Canvas>();
		uiCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
		uiScore = uiCanvas.gameObject.AddComponent<Text>();
		uiScore.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
		uiScore.fontSize = 50;
	}

	void Update()
	{
		if(!gameOver)
		{
            //UpdateInvadersMovement:
            bool moveLeftThisUpdate = invadersMovingLeft;
			bool moveDownThisUpdate = invadersMovingDown;
			invadersMovingDown = false;

            //UpdateNumbersInvadersAlive:
            int numInvadersAlive = (invaders.GetLength(0)*invaders.GetLength(1)) - numInvadersDead;

            //CalculateInvaderSpeed:
            float invaderSpeed = 0.3f+((numInvadersDead/10)*0.1f)+((numInvadersAlive<=3)?((4-numInvadersAlive)*1f):0.0f);

           
            for (int i=0; i<invaders.GetLength(0); i++)
				for(int j=0; j<invaders.GetLength(1); j++)

					if(invaders[i,j].activeSelf)
					{
                        //CalculateNewInvaderPosition:
                        Vector3 newPos = invaders[i,j].transform.position;
						if(moveDownThisUpdate)
							newPos.y -= 0.25f;
						if(newPos.y < -2f)
							gameOver = true;								// Invaders reached bases = game over
						newPos.x -= (moveLeftThisUpdate?invaderSpeed:-invaderSpeed) * Time.deltaTime;
						
						if( (invadersMovingLeft&&(newPos.x < -7.0f)) || ((!invadersMovingLeft)&&(newPos.x > 7.0f)) )
						{
							invadersMovingLeft = !invadersMovingLeft;
							invadersMovingDown = true;
						}
						invaders[i,j].transform.position = newPos;

						//InvadersShootBullets:
						if( !bullets[(i%(bullets.Length-1))+1].activeSelf && ((j == 0) || !invaders[i,j-1].activeSelf) && (Random.value < 0.01f) )
						{
							bullets[(i%(bullets.Length-1))+1].transform.position = invaders[i,j].transform.position - (Vector3.up*0.5f);
							bullets[(i%(bullets.Length-1))+1].SetActive(true);
						}
					}

            // UpdateBulletsMovement:
            for (int i=0; i<bullets.Length; i++)
				if(bullets[i].activeSelf)
				{
                    //MoveBullets:
                    float newBulletY = bullets[i].transform.position.y + ((i==0)?20f:-5f) * Time.deltaTime;
					bullets[i].transform.position = new Vector3(bullets[i].transform.position.x, newBulletY, bullets[i].transform.position.z);

                    //CheckBulletsCollisions
                  
                   Collider[] hits = Physics.OverlapBox(bullets[i].GetComponent<Collider>().bounds.center, bullets[i].GetComponent<Collider>().bounds.extents, bullets[i].transform.rotation, ((i==0)?(1<<2)+(1<<5):(1<<1)+(1<<4))+(1<<3));
                    
					//HandleBulletCollision:
                    if ((hits != null) && (hits.Length > 0))
					{
						bullets[i].SetActive(false);
						hits[0].gameObject.SetActive(false);

                        //HandleBulletCollision with invaders:
                        if (hits[0].gameObject.layer == 2)
						{
                            //Add Score & IncreaseNumbersInvadersDead:
                            score += 10;
							numInvadersDead++;
						}

                        //HandleBulletCollision with player:
                        else if (hits[0].gameObject.layer == 1)
						{
							if(--lives >= 0)
							{
								//Player takes damage & restarts its position:
								playerLives[lives].SetActive(false);
								player.SetActive(true);
								player.transform.position = new Vector3(-7, -4, 0);
							}
							else
								//Lives = 0 (Game over condition)
								gameOver = true;							// Player dead = game over
						}
					}
                    //IsBulletOutOfBoundaries:
                    if ((bullets[i].transform.position.y < -4) || (bullets[i].transform.position.y > 4))
						bullets[i].SetActive(false);
				}

            // UpdatePlayerMovement:

            float newPlayerX = player.transform.position.x + (Input.GetAxis("Horizontal") * 10f * Time.deltaTime);
			player.transform.position = new Vector3(Mathf.Clamp(newPlayerX, -7f, 7f), -4f, 0f);

            //PlayerShootBullets:
            if (Input.GetKeyDown(KeyCode.Space) && !bullets[0].activeSelf) //KeyCode.LeftShift
            {
				bullets[0].transform.position = player.transform.position;
				bullets[0].SetActive(true);									// Fire a player bullet
			}
			//Win condition & Reload Scene:
			if(numInvadersAlive == 0)
				UnityEngine.SceneManagement.SceneManager.LoadScene(0);		// Reload scene for a new attack wave
		}
		else
		{
			//Reset Score/Lives on Game Over & Reload Scene
			if(Input.GetKeyDown(KeyCode.Space))
			{
				score = 0;
				lives = 2;
				UnityEngine.SceneManagement.SceneManager.LoadScene(0);		// Reload scene and reset score & lives for a new game
			}
		}
		//Update UI:
		uiScore.text = string.Format("SCORE: {0:0000}         {1}", score, gameOver?"GAME OVER!\n   PRESS SPACE TO RESTART":"");
	}
}