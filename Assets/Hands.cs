using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Hands : MonoBehaviour
{

    public Gun gun;

    // private vars
    private Vector2 aimVector = Vector2.zero; // current aim progress
    private float currentAngle = 0; // current recoil angle
    private Quaternion rotationQuaternion = Quaternion.Euler(0f, 0f, 0f); // rotation for weapon recoil
    private bool hasShot = false;
    private bool isAiming = false;
    private float aimProgress = 0f;
    private float nextTimeToFire = 0f;
    private int burstRound = 0;
    private float nextTimeToBurstRound = 0f;

    public void Start()
    {
        // player = GetComponentInParent<Transform>().GetComponentInParent<Transform>().GetComponentInParent<Player>();
        // fpscamera = GetComponentInParent<Transform>().GetComponentInParent<Camera>();
        // camFOV = fpscamera.fieldOfView;
        // sens = player.playerData.sens;
    }

    public void OnEnable()
    {
        if (Input.GetButton("Fire2"))
        {
            isAiming = true;
        }
        else
        {
            isAiming = false;
        }
    }

    public void OnDisable()
    {
        aimProgress = 0;
        currentAngle = 0f;
        burstRound = 0;
    }

    // Update is called once per frame
    public void Update()
    {
        if (GetComponentInParent<Player>().IsOwner)
        {

            Vector2 aimDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.parent.position);
            Vector2 centerPosition = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.parent.parent.position);
            // do proper weapon position
            GetComponent<SpriteRenderer>().sprite = gun.sprite;





            // Checking if the player stopped pressing the trigger for Semi-Auto, Burst and Shotguns
            if (hasShot && !Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
            {
                hasShot = false;
            }


            if (/*!player.inMenu*/ true)
            {
                // Actually Shooting Properly
                if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
                {
                    if (gun.fireMode == Gun.FireModes.FullAuto)
                    {
                        nextTimeToFire = Time.time + 1 / gun.fireRate;
                        Shoot();
                    }
                    else if (gun.fireMode == Gun.FireModes.SemiAuto || gun.fireMode == Gun.FireModes.Shotgun)
                    {
                        if (!hasShot)
                        {
                            nextTimeToFire = Time.time + 1 / gun.fireRate;
                            if (gun.fireMode == Gun.FireModes.SemiAuto)
                                Shoot();
                            else
                                for (int i = 0; i < gun.burstRounds; i++)
                                    Shoot();
                            hasShot = true;
                        }
                    }
                    else if (gun.fireMode == Gun.FireModes.Burst)
                    {
                        if (!hasShot && burstRound == 0)
                        {
                            nextTimeToFire = Time.time + 1 / gun.fireRate;
                            Debug.Log("Shot Number: " + (burstRound + 1).ToString() + " Time.time: " + Time.time.ToString());
                            Shoot();
                            nextTimeToBurstRound = Time.time + (gun.timeBetweenBurstRounds / 1000);
                            burstRound++;
                            hasShot = true;
                        }
                    }
                }

                // Aiming
                if (Input.GetButtonDown("Fire2"))
                {
                    isAiming = true;
                }
                else if (Input.GetButtonUp("Fire2"))
                {
                    isAiming = false;
                }
            }
            if (isAiming && aimProgress < 1)
            {
                aimProgress += Time.deltaTime / gun.aimingTime;
                if (aimProgress > 1)
                {
                    aimProgress = 1;
                }
            }
            else if (!isAiming && aimProgress > 0)
            {
                aimProgress -= Time.deltaTime / gun.aimingTime;
                if (aimProgress < 0)
                {
                    aimProgress = 0;
                }
            }
            aimVector = (gun.aimPosition * aimProgress);

            // Weapon Recoil
            currentAngle -= gun.recoilReduction * Time.deltaTime * ((currentAngle / gun.maxRecoilAngle) <= 0.15f ? 0.15f : (currentAngle / gun.maxRecoilAngle));
            if (currentAngle < 0) currentAngle = 0;
            if (currentAngle > gun.maxRecoilAngle) currentAngle = gun.maxRecoilAngle;
            rotationQuaternion = Quaternion.Euler(0f, 0f, aimDirection.x < 0 ? -currentAngle : currentAngle);

            // Do Burst Rounds
            if (gun.fireMode == Gun.FireModes.Burst && burstRound != 0 && nextTimeToBurstRound <= Time.time)
            {
                Debug.Log("Shot Number: " + (burstRound + 1).ToString() + " Time.time: " + Time.time.ToString());
                Shoot();
                if (burstRound + 1 >= gun.burstRounds)
                {
                    burstRound = 0;
                }
                else
                {
                    burstRound++; nextTimeToBurstRound = Time.time + (gun.timeBetweenBurstRounds / 1000);
                }
            }

            // Other
            transform.localPosition = centerPosition.x < 0 ? flipX(aimVector + gun.positionOffset) : aimVector + gun.positionOffset;
            transform.localRotation = rotationQuaternion;
            transform.parent.localPosition = centerPosition.x < 0 ? flipX(gun.center) : gun.center;
            transform.parent.localRotation = Quaternion.Euler(0f, 0f, getAngle(aimDirection, centerPosition));
            GetComponent<SpriteRenderer>().flipX = aimDirection.x < 0;
        }
    }

    void Shoot()
    {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.parent.position, this.transform.position.x < GetComponentInParent<Player>().transform.position.x ? -this.transform.right : this.transform.right, gun.range, gun.layerMask);
        if (hit.collider)
        {

            Target target = hit.transform.GetComponent<Target>();
            if (target != null /*&& target != player*/)
            {
                target.TakeDamage(gun.damage);
                Debug.Log(target.gameObject);
            }

            if (hit.rigidbody != null /*&& target != player*/)
            {
                hit.rigidbody.AddForce(this.transform.forward * gun.impactForce);
            }
        }
        currentAngle += gun.recoilAngle * (1 + ((gun.recoilMultiplier - 1) * (currentAngle / gun.maxRecoilAngle)));
    }

    private Vector3 flipX(Vector3 vector3)
    {
        vector3.x = -vector3.x;
        return vector3;
    }

    private float getAngle(Vector2 aimDirection, Vector2 centerPosition)
    {
        float angle = Mathf.Atan(aimDirection.y/aimDirection.x) * Mathf.Rad2Deg;
        if ((centerPosition.x < 0 && aimDirection.x >= 0) || (centerPosition.x >= 0 && aimDirection.x < 0))
        {
            angle += 180;
            if(angle > 180)
            {
                angle -= 360;
            }
        }
        angle = Mathf.Clamp(angle, -90f, 90f);
        return angle;
    }
}
