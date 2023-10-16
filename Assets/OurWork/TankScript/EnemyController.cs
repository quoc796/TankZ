using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : tankController
{
    const float range = 5;
    const float pathFindTimeInterval = 0.5f;
    const float rGapMin = 30;
    const float dGapMin = 1;

    public const float coolDown = 2f;
    Vector3 nextWaypoint;



    Timer shootCoolDown;
    NavMeshAgent agent;
    Transform target;
    bool inRange;
    public override void Awake()
    {
        rSpd = 90;
        base.Awake();
        agent = GetComponent<NavMeshAgent>();

        //InvokeRepeating("calculatePath", 0, pathFindTimeInterval);



        target = GameObject.Find("Player").transform;
    }
    public override void Update()
    {
        if (shootCoolDown != null) shootCoolDown.Update();
    }
    public override void Shoot()
    {
        if (shootCoolDown == null)
        {
            shootCoolDown = new Timer(coolDown, () =>
            {
                shootCoolDown = null;
            });
            Rigidbody r = Instantiate(shellPre, firePos.position, firePos.rotation);
            Debug.Log("SHOOT");
            //later
            r.velocity = firePos.forward * maxLaunchForce;
        }
    }
    public void inAtkRange()
    {
        float mag = Vector3.Magnitude(transform.position - target.position);
        if (!inRange)
        {
            inRange = mag < range * 0.7f;
        }
        else
        {
            inRange = mag < range * 1.2f;
        }
    }

    public void enmeyInput()
    {
        inAtkRange();
        if (inRange)
            Shoot();
    }
    public void calculatePath()
    {
        if (inRange)
        {
            inputV = Vector2.zero;
            return;
        }
        Vector3 targetpos = target.transform.position;

        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, targetpos, NavMesh.AllAreas, path);

        // Check if the calculated path is valid and contains at least one corner
        if (path.status == NavMeshPathStatus.PathComplete && path.corners.Length > 1)
        {
            nextWaypoint = path.corners[1];
            Vector3 direction = (nextWaypoint - transform.position).normalized;

            float angle = Vector3.Angle(transform.forward, direction);


            //angle > min => rotate
            if (Mathf.Abs(angle) > rGapMin)
            {
                Vector3 crossProduct = Vector3.Cross(transform.forward, direction);
                inputV.x = crossProduct.y > 0 ? 1 : -1;
                inputV.y = 0;
            }
            //move
            else
            {
                //transform.rotation = Quaternion.LookRotation(direction);
                inputV.y = 1;
                inputV.x = 0;
            }
        }
    }
    public void WaitForNewPath()
    {

        if (inRange)
        {
            inputV = Vector2.zero;
        }
        //rotating
        else if (inputV.x != 0)
        {
            Vector3 direction = (nextWaypoint - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, direction);
            Debug.Log(Mathf.Abs(angle));
            if (Mathf.Abs(angle) < rGapMin)
            {
                Debug.Log("stop rotate");
                inputV.x = 0;
            }
        }
        //moving forward
        else if(inputV.y != 0)
        {
            
            if (Vector3.Distance(transform.position, nextWaypoint) < dGapMin)
            {
                Debug.Log("stop move");
                inputV.y = 0;
            }
        }
    }

    public override void FixedUpdate()
    {
        calculatePath();
        enmeyInput();
        //WaitForNewPath();
        base.FixedUpdate();
        //if in range => shoot();
    }
}
