using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    [SerializeField] private float timeDelay = 0;

    internal void setDelay(float delay) { timeDelay = delay; }

    private void Start()
    {
        Destroy(this.gameObject, timeDelay);
    }
}
