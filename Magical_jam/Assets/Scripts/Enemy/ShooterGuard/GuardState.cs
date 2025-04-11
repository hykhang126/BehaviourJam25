using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GuardState : State
{
    protected  ShootingGuard ctx;
    protected GuardState(ShootingGuard ctx)
    {
        this.ctx = ctx;
    }

}
