using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tankController : MonoBehaviour
{
    public const float mSpd = 100f;
    public const float rSpd = 180f;
    public const float tSpd = 90f;
    public const float maxLaunchForce = 100f;
    Rigidbody rb;
    Transform turret;
   
    //Controller
    protected Vector2 inputV;
    protected Vector2 inputT;
    protected float launchRatio;


    //Shooting
    public Transform firePos;
    public Rigidbody shellPre;

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        turret = transform.GetChild(0).GetChild(3);
    }

    public virtual void Update()
    {
    }
    public virtual void FixedUpdate()
    {
        move();
        turn();
        turnTurret();
    }


    public virtual void Shoot()
    {
        
    }
    public void move()
    {
        if (inputV.y != 0)
        {
            rb.velocity = transform.forward * mSpd * Time.fixedDeltaTime;
        }
    }
    public void turn()
    {
        Quaternion angle = Quaternion.Euler(0, inputV.x * Time.deltaTime * rSpd, 0);
        rb.MoveRotation(rb.rotation * angle);
    }
    public void turnTurret()
    {
        float rX = inputT.y * -1 * Time.deltaTime * tSpd + turret.eulerAngles.x;
        float rY = inputT.x * Time.deltaTime * tSpd + turret.eulerAngles.y;
        if (rX > 180)
        {
            rX = Mathf.Clamp(rX, 290, 361);
        }
        if (rX < 180)
        {
            rX = Mathf.Clamp(rX, -1, 20);
        }

        Quaternion xRotation = Quaternion.Euler(rX, 0, 0);
        Quaternion yRotation = Quaternion.Euler(0, rY, 0);

        // Combine rotations in the correct order (Y, X)
        Quaternion newRotation = Quaternion.Euler(0,0,0) * yRotation * xRotation;


        turret.rotation = newRotation;
    }
}

