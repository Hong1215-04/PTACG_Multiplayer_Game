using UnityEngine;

public class OpenHiddenPlaces : MonoBehaviour
{
    [SerializeField] Animator HiddenDoor;
    [SerializeField] MeshRenderer ItemVisual;
    [SerializeField] Collider col;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HiddenDoor.SetBool("Open", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        col.enabled = false;
        ItemVisual.enabled = false;
        HiddenDoor.SetBool("Open", true);
    }
}
