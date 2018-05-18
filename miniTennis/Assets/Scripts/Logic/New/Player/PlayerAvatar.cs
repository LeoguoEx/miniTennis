using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    private void Start()
    {
        
    }

    public void Move(Vector2 position)
    {
        gameObject.transform.position = position;
    }

    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }
}
