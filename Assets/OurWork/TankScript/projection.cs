using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projection : MonoBehaviour
{
    LineRenderer line;
    int numPoints = 50;
    float timeBetweenPts = 0.1f;
    public LayerMask collidableLayers;
    bool onLive;
    float launchF;
    float maxForce;
    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }
    public void setup(float f)
    {
        maxForce = f;
    }
    public void setOnLive(bool o)
    {
        onLive = o;
    }
    public void updateLaunchForce(float force)
    {
        launchF = force;
    }
    public void Update()
    {
        if (onLive)
        {
            line.positionCount = (int)numPoints;
            List<Vector3> pts = new List<Vector3>();
            Vector3 startPos = transform.position;
            Vector3 startVec = transform.forward * launchF * maxForce;
            for (float t = 0; t < numPoints; t += timeBetweenPts)
            {
                Vector3 newpt = startPos + t * startVec;
                newpt.y = startPos.y + startVec.y * t + Physics.gravity.y / 2f * t * t;
                pts.Add(newpt);
                //if (Physics.OverlapSphere(newpt, 2, collidableLayers).Length > 0)
                //{
                //    line.positionCount = pts.Count;
                //    break;
                //}
            }
            line.SetPositions(pts.ToArray());
        }
        else
        {
            line.positionCount = 0;
        }
    }
}
