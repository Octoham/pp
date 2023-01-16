using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldableItem : Item
{

    [Header("HoldableItem")]
    public Sprite sprite; // sprite when looking right
    public Vector2 center; // center of the sprite when looking right
    public Vector2 positionOffset; // offset from the center when looking right
}
