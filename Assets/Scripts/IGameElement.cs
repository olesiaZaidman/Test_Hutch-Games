
using UnityEngine;

public interface IGameElement
{
    /*It is unnecessary interface created for demonstration purposes :o */
    void SetGameObjectColor(GameObject gameObject, Color color);
    GameObject CreateGameObject(PrimitiveType type);
    void SetTransformProperties(GameObject gameObject, Vector3 position, Vector3 localScale);
    void SetGameObjectLocalScale(GameObject gameObject, Vector3 scale);
    void SetGameObjectPosition(GameObject gameObject, Vector3 position);
    void SetGameObjectName(GameObject gameObject, string name);
    void AssignLayerToGameObject(GameObject gameObject, int layer);
}
