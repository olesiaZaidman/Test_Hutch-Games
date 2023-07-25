using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BricksController : GameObjectManager
{

    Vector3 bricksStartingPosition = new Vector3(-3.6f, -3, 0);
    Vector3 bricksLocalScale = new Vector3(0.2f, 0.2f, 0.5f);
    Color bricksColor = Color.green;
    static int maxbaseBricksAmount = 4;
    GameObject[] bricks = new GameObject[maxbaseBricksAmount];




    void CreateBricks()
    {
        float bricksHorizontalSpacing = 2.2f;
        float bricksVerticalSpacing = 0.2f;

        for (int b = 0; b < bricks.Length; b++)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    GameObject baseBrick = GameObject.CreatePrimitive(primitiveType);

                    // bricksStartingPosition
                    Vector3 position = bricksStartingPosition + new Vector3(b * bricksHorizontalSpacing + i * bricksVerticalSpacing, j * bricksVerticalSpacing, 0);
                    // baseBrick.transform.position = new Vector3((b * 2.2f) + (i * 0.2f) - 3.6f, (j * 0.2f) - 3, 0);

                    SetTransformProperties(baseBrick, position, bricksLocalScale);
                    AssignLayerToGameObject(baseBrick, GameManager.brickLayer);
                    SetGameObjectColor(baseBrick, bricksColor);
                    SetGameObjectName(baseBrick, "BaseBrick " + b + "," + i + "," + j);
                }
            }
        }
    }


    void Start()
    {
        CreateBricks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
