using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CrownMoverUI : MonoBehaviour
{
    // Start is called before the first frame update

    private RectTransform endPoint;

    private Vector3 startPoint;

    private RectTransform rect;

    private Vector3 endScale = new Vector3(0.14f,0.14f,0.14f);

    void Start()
    {
        rect = GetComponent<RectTransform>();
        startPoint = GetComponent<RectTransform>().anchoredPosition;
        StartCoroutine(flyToPoint());
        StartCoroutine(destoySelf());
    }

    public void InitializeEndPoint(RectTransform setPoint)
    {
        endPoint = setPoint;
    }

    IEnumerator flyToPoint()
    {
        float timer = 0f;

        while(timer < 0.5f)
        {
            timer += Time.unscaledDeltaTime;

            float ratio = timer / 0.5f;

            Vector3 newPoint = Vector3.Lerp(startPoint, endPoint.anchoredPosition, ratio);
            Vector3 newScale = Vector3.Lerp(Vector3.one, endScale, ratio);
            //new Vector3(0.503999949f, 0.503999949f, 0.503999949f)

            rect.anchoredPosition = newPoint;
            rect.localScale = newScale;

            yield return null;
        }
    }

    IEnumerator destoySelf()
    {
        yield return new WaitForSecondsRealtime(6f);
        Destroy(this.gameObject);
    }
}
