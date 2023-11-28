using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tankController : MonoBehaviour
{
    protected float mSpd = 300f;
    protected float rSpd = 150f;

    public const float tSpd = 5f;
    public const float maxLaunchForce = 500f;
    Rigidbody rb;
    public Transform turret;
   
    //Controller
    protected Vector2 inputV;
    protected Vector2 turretDirToMousePos;
    protected float launchRatio;


    //Shooting
    public Transform firePos;
    public Rigidbody shellPre;
    public Animator anim;
    public tankStatus status;
    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        turret = transform.GetChild(0).GetChild(3);
        anim = GetComponent<Animator>();
        status = new tankStatus();

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
            rb.velocity = transform.forward * mSpd * Time.fixedDeltaTime * inputV.y;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }
    public void turn()
    {
        if (inputV.x != 0)
        {
            Quaternion angle = Quaternion.Euler(0, inputV.x * Time.deltaTime * rSpd, 0);
            rb.MoveRotation(rb.rotation * angle);
        }
        else rb.angularVelocity = Vector3.zero;
    }


    public virtual void turnTurret()
    {

    }
}

