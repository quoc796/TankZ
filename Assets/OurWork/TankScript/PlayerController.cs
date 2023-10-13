using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : tankController
{
    const float launchSpd = 0.1f;
    public PlayerInput input;
    public bool shootPressed;
    public projection pro;
    public override void Awake()
    {
        base.Awake();
        input = new PlayerInput();
        input.Enable();
        input.Player.Shoot.performed += ctx => startLaunch();
        input.Player.Shoot.canceled += ctx => Shoot();
        launchRatio = 0;
        setShootPressed(false);
        pro.setup(maxLaunchForce);
    }

    public void setShootPressed(bool b)
    {
        shootPressed = b ;
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
        inputT = input.Player.Turretmovement.ReadValue<Vector2>();
        inputT = new Vector2(Mathf.CeilToInt(inputT.x), Mathf.CeilToInt(inputT.y));
        base.FixedUpdate();
    }
}
