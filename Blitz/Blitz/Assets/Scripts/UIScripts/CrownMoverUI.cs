using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CrownMoverUI : MonoBehaviour
{
    // Start is called before the first frame update

    private Vector3 endPoint;

    private Vector3 startPoint;

    private RectTransform rect;

    private Vector3 endScale = new Vector3(0.065f,0.065f,0.065f);

    void Start()
    {
        rect = GetComponent<RectTransform>();
        startPoint = GetComponent<RectTransform>().position;
        StartCoroutine(flyToPoint());
        //StartCoroutine(destoySelf());
    }

    public void InitializeEndPoint(RectTransform setPoint, int wins)
    {
        endPoint = new Vector3(setPoint.position.x, (setPoint.position.y - (wins * 100)), setPoint.position.z);
    }

    IEnumerator flyToPoint()
    {
        float timer = 0f;

        while(timer < 0.5f)
        {
            timer += Time.unscaledDeltaTime;

            float ratio = timer / 0.5f;

            Vector3 newPoint = Vector3.Lerp(startPoint, endPoint, ratio);
            Vector3 newScale = Vector3.Lerp(new Vector3(0.6f,0.6f,0.6f), endScale, ratio);
            //new Vector3(0.503999949f, 0.503999949f, 0.503999949f)

            rect.position = newPoint;
            rect.localScale = newScale;

            yield return null;
        }

        rect.position = endPoint;
        rect.localScale = endScale;

    }

    //IEnumerator destoySelf()
    //{
    //    yield return new WaitForSecondsRealtime(2f);
    //    Destroy(this.gameObject);
    //}
}
