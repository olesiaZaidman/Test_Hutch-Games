
using UnityEngine;

public class BricksController : GameObjectManager
{
    const float BRICKS_HORIZONTAL_SPACING = 2.2f;
    const float BRICKS_VERTICAL_SPACING = 0.2f;

    const int MAX_BRICKS_COUNT = 4;
    GameObject[] bricks = new GameObject[MAX_BRICKS_COUNT];

    Color bricksColor = Color.green;

    Vector3 bricksStartingPosition = new Vector3(-3.6f, -3, 0);
    Vector3 bricksLocalScale = new Vector3(0.2f, 0.2f, 0.5f);

    void CreateBricks()
    {
        for (int b = 0; b < bricks.Length; b++)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    GameObject baseBrick = GameObject.CreatePrimitive(primitiveType);
                    Vector3 position = bricksStartingPosition + new Vector3(b * BRICKS_HORIZONTAL_SPACING + i * BRICKS_VERTICAL_SPACING, j * BRICKS_VERTICAL_SPACING, 0);
                    SetTransformProperties(baseBrick, position, bricksLocalScale);
                    AssignLayerToGameObject(baseBrick, GameManager.BRICK_LAYER);
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

}
