using UnityEngine;

public class ParticleFollow : MonoBehaviour
{
    public Rigidbody2D targetRigidbody;  // 目标物体的Rigidbody2D
    public ParticleSystem particleSystem; // 分离出来的粒子系统

    private void Update()
    {
        // 将粒子系统的transform位置与目标物体的位置同步
        if (targetRigidbody != null && particleSystem != null)
        {
            particleSystem.transform.position = targetRigidbody.transform.position;
        }
    }
}
