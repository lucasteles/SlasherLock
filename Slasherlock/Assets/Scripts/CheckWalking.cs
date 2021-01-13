using UnityEngine;
public class CheckWalking
{
    readonly Transform transform;

    Vector3 oldPosisition;
    
    float maxRestTime = 0.1f;
    float restTime = 0;
    
    public CheckWalking(Transform transform)
    {
        this.transform = transform;
        oldPosisition = transform.position;
    }

    public bool IsWaking() => oldPosisition != transform.position;

    public bool IsStopped()
    {
        var isStopped = restTime >= maxRestTime;

        if (isStopped) restTime = 0;
        
        return isStopped;
    }

    public void Update(float time)
    {
        if (IsWaking())
            restTime = 0;
        else
            restTime += Time.deltaTime;

        
        oldPosisition = transform.position;
    }
}