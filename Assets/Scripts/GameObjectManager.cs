using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager : MonoBehaviour, IGameElement
{
    public PrimitiveType primitiveType = PrimitiveType.Cube;

    public void SetGameObjectColor(GameObject gameObject, Color color)
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
        }
    }

    public GameObject CreateGameObject(PrimitiveType type) //   player = GameObject.CreatePrimitive(PrimitiveType.Cube);
    {
        GameObject gameObject = GameObject.CreatePrimitive(type);
        return gameObject;
    }

    public void SetTransformProperties(GameObject gameObject, Vector3 position, Vector3 localScale)
    {
        gameObject.transform.position = position;
        gameObject.transform.localScale = localScale;
    }


    public void SetGameObjectLocalScale(GameObject gameObject, Vector3 scale)
    {
        gameObject.transform.localScale = scale;
    }

    public void SetGameObjectPosition(GameObject gameObject, Vector3 position)
    {
        gameObject.transform.position = position;
    }



    public void SetGameObjectName(GameObject gameObject, string name)
    {
        gameObject.name = name;
    }


    public void AssignLayerToGameObject(GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
    }
}
