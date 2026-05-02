using UnityEngine;
using static UnityEngine.AdaptivePerformance.Provider.AdaptivePerformanceSubsystemDescriptor;

public class CollisionDetect : MonoBehaviour
{
    [SerializeField] GameObject thePlayer;
    void OnTriggerEnter(Collider other)
    {
        //thePlayer.GetComponent(PlayerMovement).enabled = false;

    }
}
