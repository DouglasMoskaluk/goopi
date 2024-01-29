using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    [SerializeField] private float timeDelay = 0;

    private void Start()
    {
        Destroy(this.gameObject, timeDelay);
    }
}
