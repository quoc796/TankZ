using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private Action action;
    private float timer;

    public Timer(float timer, Action action)
    {
        this.action = action;
        this.timer = timer;
    }
    public void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            action();
        }
    }
}
