using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : tankController
{
    const float range = 30;
    const float rGapMin = 30;
    public const float coolDown = 4f;
    Timer shootCoolDown;
    NavMeshAgent agent;
    Transform target;
    bool inRange;
    bool isStop;
    float castCoolDown;
    float castCoolDownMax;
    public override void Awake()
    {
        rSpd = 90;
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("Player").transform;
        isStop = false;
        castCoolDownMax = Random.Range(10, 50);
        castCoolDown = castCoolDownMax;
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

            //later
            r.velocity = firePos.forward * maxLaunchForce * 0.25f;
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

    //public void enemyControl()
    //{
    //    if (inRange)
    //    {
    //        inputV = Vector2.zero;
    //        return;
    //    }
    //    Vector3 targetpos = target.transform.position;

    //    NavMeshPath path = new NavMeshPath();
    //    NavMesh.CalculatePath(transform.position, targetpos, NavMesh.AllAreas, path);

    //    // Check if the calculated path is valid and contains at least one corner
    //    if (path.status == NavMeshPathStatus.PathComplete && path.corners.Length > 1)
    //    {
    //        nextWaypoint = path.corners[1];
    //        Vector3 direction = (nextWaypoint - transform.position).normalized;

    //        float angle = Vector3.Angle(transform.forward, direction);


    //        //angle > min => rotate
    //        if (angle > rGapMin)
    //        {
    //            Vector3 crossProduct = Vector3.Cross(transform.forward, direction);
    //            inputV.x = crossProduct.y > 0 ? 1 : -1;
    //            inputV.y = 0;
    //        }
    //        //move
    //        else
    //        {
    //            //transform.rotation = Quaternion.LookRotation(direction);
    //            inputV.y = 1;
    //            inputV.x = 0;
    //        }
    //    }
    //}
    public override void turnTurret()
    {

        Vector3 dirToTarget = (target.position - turret.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(dirToTarget, Vector3.up);
        Quaternion current = turret.rotation;
        turret.rotation = Quaternion.Slerp(current, targetRotation, tSpd * Time.fixedDeltaTime);
        float angleToTarget = Vector3.Angle(turret.forward, dirToTarget);
        ; // Adjust this value according to your requirements
        // Check if the turret is approximately facing the target within the threshold angle
        if (angleToTarget <= rGapMin)
        {
            inAtkRange();
            if (inRange && castCoolDown < 0)
            {
                castCoolDown = castCoolDownMax;
                Vector3 boxCenter = transform.position;
                Vector3 boxHalfSize = new Vector3(5, 5, 5); // half size of the box
                Vector3 boxDirection = (target.position - transform.position).normalized;

                // Declare a RaycastHit variable to store information about the hit
                RaycastHit hit;

                // Perform the BoxCast. If it hits something, the 'hit' variable will contain information about the hit.
                if (Physics.BoxCast(boxCenter, boxHalfSize, boxDirection, out hit, Quaternion.identity))
                {
                    if (hit.collider != null)
                    {
                        if (hit.collider.tag == "PLAYER")
                        {
                            Shoot();
                            isStop = true;
                        }
                        else isStop = false;
                    }
                }






            }
            else
            {
                isStop = false;
            }
        }
    }
    public override void FixedUpdate()
    {
        turnTurret();
        castCoolDown -= 1;
        if (isStop)
        {
            agent.SetDestination(transform.position);
        }
        else
        {
            agent.SetDestination(target.position);
        }
        

    }
}
