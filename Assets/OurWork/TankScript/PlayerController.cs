using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : tankController
{
    const float launchSpd = 0.1f;
    public PlayerInput input;
    public bool shootPressed;
    public projection pro;


    [Header("SKILL UI")]
    public GameObject coolDownPan;
    public TextMeshProUGUI cTxt;
    



    public override void Awake()
    {
        base.Awake();
        input = new PlayerInput();
        input.Enable();
        input.Player.Shoot.performed += ctx => startLaunch();
        input.Player.Shoot.canceled += ctx => Shoot();
        input.Player.Skill.performed += ctx => skillPressed();
        launchRatio = 0;
        setShootPressed(false);
        pro.setup(maxLaunchForce);
    }

    public void setShootPressed(bool b)
    {
        shootPressed = b;
        pro.setOnLive(b);
    }
    public void startLaunch()
    {
        setShootPressed(true);
    }
    public override void Shoot()
    {
        Rigidbody r = Instantiate(shellPre, firePos.position, firePos.rotation);
        r.velocity = firePos.forward * maxLaunchForce * launchRatio;
        launchRatio = 0;
        setShootPressed(false);
    }
    public override void FixedUpdate()
    {
        if (shootPressed)
        {
            launchRatio = Mathf.Clamp(Time.fixedDeltaTime * launchSpd + launchRatio, 0, 1);
            pro.updateLaunchForce(launchRatio);

        }

        inputV = input.Player.Movement.ReadValue<Vector2>();
        inputV = new Vector2(Mathf.CeilToInt(inputV.x), Mathf.CeilToInt(inputV.y));

        base.FixedUpdate();
    }
    public override void turnTurret()
    {
        if (inputV.x == 0)
        {
            Vector3 mousePosition = getMouseWorldPos();
            // Calculate the direction from the object to the mouse position
            Vector3 directionToMouse = (mousePosition - turret.position);

            //make sure the forward position of mouse - turret is positive so that the turret only face forward
            //directionToMouse.z = Mathf.Abs(directionToMouse.z);
            directionToMouse = directionToMouse.normalized;
            // Calculate the rotation needed to look at the mouse position
            Quaternion targetRotation = Quaternion.LookRotation(directionToMouse, Vector3.up);
            // Rotate the object towards the mouse position gradually
            Quaternion current = turret.rotation;
            turret.rotation = Quaternion.Slerp(current, targetRotation, tSpd * Time.fixedDeltaTime);
        }
    }
    Vector3 getMouseWorldPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }

        return Vector3.zero;
    }

    public void skillPressed()
    {
        StartCoroutine(startSkillUse());
    }




    public IEnumerator startSkillUse()
    {
        anim.Play("upgrade");
        coolDownPan.gameObject.SetActive(true);
        status.dmgImmune = true;
        float maxTimer = 30;
        float duration = 5;
        float currentTimer = maxTimer;
        while (currentTimer > 0)
        {
            if (maxTimer - currentTimer > duration)
            {
                anim.Play("none");
                status.dmgImmune = false;
            }
            currentTimer -= 1;
            cTxt.text = currentTimer.ToString();
            float ratio = currentTimer / maxTimer;
            coolDownPan.transform.localScale = new Vector3(1, ratio, 1);
            yield return new WaitForSeconds(1);
        }
        coolDownPan.gameObject.SetActive(false);
    }
}
public class tankStatus
{
    public bool dmgImmune { get; set; }
    public tankStatus()
    {
        dmgImmune = false;
    }



}
