using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMediumRange
{
    public void StartAiming();
    public void StopAiming();
    public void RangeAttackTarget(Transform target);

}
