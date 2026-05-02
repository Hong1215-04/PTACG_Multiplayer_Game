using System.Threading;
using UnityEngine;
using static UnityEngine.AdaptivePerformance.Provider.AdaptivePerformanceSubsystemDescriptor;

public class CollisionDetect : MonoBehaviour
{
    [SerializeField] GameObject thePlayer;
    [SerializeField] GameObject playerAnim;
    void OnTriggerEnter(Collider other)
    {
        //thePlayer.GetComponent(PlayerMovement).enabled = false;
        playerAnim.GetComponent<Animator>().Play("Stumble Backwards");

    }
}
