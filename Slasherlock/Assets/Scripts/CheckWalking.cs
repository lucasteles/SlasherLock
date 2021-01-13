using UnityEngine;
public class CheckWalking
{
    readonly Transform transform;

    Vector3 oldPosisition;
    public CheckWalking(Transform transform)
    {
        this.transform = transform;
        oldPosisition = transform.position;
    }

    public bool IsWaking() => oldPosisition != transform.position;

    public void Update() => oldPosisition = transform.position;
}