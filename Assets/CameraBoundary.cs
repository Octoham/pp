using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoundary : MonoBehaviour
{
    public static Box boundary;
    public Box box;
    // Start is called before the first frame update
    void Start()
    {
        if (boundary == null)
        {
            boundary = box;
        }
    }

}

[System.Serializable]
public class Box
{
    public Vector2 topLeft;
    public Vector2 topRight;
    public Vector2 bottomLeft;
    public Vector2 bottomRight;

    public Box(Vector2 topLeft, Vector2 topRight, Vector2 bottomLeft, Vector2 bottomRight)
    {
        this.topLeft = topLeft;
        this.topRight = topRight;
        this.bottomLeft = bottomLeft;
        this.bottomLeft = bottomLeft;
    }

    public Box(Vector2 center, float height, float width)
    {
        topLeft = new Vector2(center.x - width / 2, center.y + height / 2);
        topRight = new Vector2(center.x + width / 2, center.y + height / 2);
        bottomLeft = new Vector2(center.x - width / 2, center.y - height / 2);
        bottomRight = new Vector2(center.x + width / 2, center.y - height / 2);
    }

}
