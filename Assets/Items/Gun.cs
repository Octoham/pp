using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Items/Gun", order = 2)]
public class Gun : HoldableItem
{
    public enum FireModes { FullAuto, Burst, SemiAuto, Shotgun };

    [Header("Gun")]
    [Header("Shooting")]
    public LayerMask layerMask;
    public FireModes fireMode;
    public float damage = 10;
    public float range = 100;
    public float fireRate = 15;
    [Tooltip("Number of burst rounds or shotgun pellets")]
    public float burstRounds = 3;
    [Tooltip("Time in milliseconds between burst rounds")]
    public float timeBetweenBurstRounds = 50;
    public float impactForce = 30;
    [Header("Aiming")]
    public float zoomAmount = 1;
    public Vector2 aimPosition;
    [Tooltip("Time in seconds to aim fully")]
    public float aimingTime = 0.2f;
    [Header("Weapon Recoil")]
    [Tooltip("The angle to add to the cursor per shot")]
    public float recoilAngle;
    [Tooltip("Recoil multiplier when at max recoil (when aiming?)")]
    public float recoilMultiplier = 1;
    [Tooltip("Angle Removed per second when recoil at max")]
    public float recoilReduction = 0;
    public float maxRecoilAngle;
    [Header("Shooting Animation")]
    public Vector2 shootingAnim;
    public float shootingAnimReduction = 1;
}
