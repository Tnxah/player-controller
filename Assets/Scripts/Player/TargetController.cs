using UnityEngine;

public class TargetController : PlayerComponentBase
{
    [SerializeField] float searchRadius = 15f;
    private Transform currentTarget;
    [SerializeField] LayerMask targetMask;

    private void ToggleLock()
    {
        if (currentTarget)
        {
            ClearTarget();
            return;
        }

        Transform nearest = FindNearestTarget();
        if (nearest)
        {
            currentTarget = nearest;
            print(currentTarget.name);
            EventBus.Publish(new LockOnTargetEvent { target = currentTarget});
        }
    }

    private Transform FindNearestTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, searchRadius, targetMask);
        float best = float.MaxValue;
        Transform pick = null;
        foreach (var h in hits)
        {
            float d = (h.transform.position - transform.position).sqrMagnitude;
            if (d < best)
            {
                best = d;
                pick = h.transform;
            }
        }
        return pick;
    }

    private void ClearTarget()
    {
        currentTarget = null;
        EventBus.Publish(new LockOnTargetEvent { target = currentTarget });
    }

    protected override void SubscribeToInputActions()
    {
        inputActions.Player.LockTarget.performed += _ => ToggleLock();
    }

    protected override void UnsubscribeFromInputActions()
    {
        inputActions.Player.LockTarget.performed -= _ => ToggleLock();
    }
}

public struct LockOnTargetEvent
{
    public Transform target;
}
