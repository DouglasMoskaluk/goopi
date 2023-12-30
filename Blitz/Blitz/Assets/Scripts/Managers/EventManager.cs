using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;


    // NOTICE because these events are private they must be initialized in awake, otherwise they produce a nulref error
    private UnityEvent onRoundStartEarly;
    private UnityEvent onRoundStart;
    private UnityEvent onRoundStartLate;

    private UnityEvent onRoundEndEarly;
    private UnityEvent onRoundEnd;
    private UnityEvent onRoundEndLate;


    private void Awake()
    {
        if (instance == null) instance = this;

        onRoundEndEarly = new UnityEvent();
        onRoundEnd = new UnityEvent();
        onRoundEndLate = new UnityEvent();

        onRoundStartEarly = new UnityEvent();
        onRoundStart = new UnityEvent();
        onRoundStartLate = new UnityEvent();
    }

    /// <summary>
    /// adds an action to the specified event with the given priority
    /// </summary>
    /// <param name="eventType"> the type of event that is to be added to </param>
    /// <param name="eventPriority"> which priority of the event that is being added to, 0 -> early, 1 -> regular, 2 -> late </param>
    /// <param name="action"> the action that is being added, the function being called from the invoking of the event </param>
    public void addListener(Events eventType, UnityAction action, int eventPriority = 1)
    {
        switch(eventType)
        {
            case Events.onRoundStart:
                switch (eventPriority)
                {
                    case 0:
                        onRoundStartEarly.AddListener(action);
                        break;
                    case 1:
                        onRoundStart.AddListener(action);
                        break;
                    case 2:
                        onRoundStartLate.AddListener(action);
                        break;
                }
                break;
            case Events.onRoundEnd:
                switch (eventPriority)
                {
                    case 0:
                        onRoundEndEarly.AddListener(action);
                        break;
                    case 1:
                        onRoundEnd.AddListener(action);
                        break;
                    case 2:
                        onRoundEndLate.AddListener(action);
                        break;
                }
                break;
        }
    }

    /// <summary>
    /// removes an action of the specified event with the given priority
    /// </summary>
    /// <param name="eventType"> the type of event that is to be removed from </param>
    /// <param name="eventPriority">  which priority of the event that is being removed, 0 -> early, 1 -> regular, 2 -> late  </param>
    /// <param name="action"> the action that is being removed, the function being removed from the invoking of the event </param>
    public void removeListener(Events eventType, UnityAction action, int eventPriority = 1)
    {
        switch (eventType)
        {
            case Events.onRoundStart:
                switch (eventPriority)
                {
                    case 0:
                        onRoundStartEarly.RemoveListener(action);
                        break;
                    case 1:
                        onRoundStart.RemoveListener(action);
                        break;
                    case 2:
                        onRoundStartLate.RemoveListener(action);
                        break;
                }
                break;
            case Events.onRoundEnd:
                switch (eventPriority)
                {
                    case 0:
                        onRoundEndEarly.RemoveListener(action);
                        break;
                    case 1:
                        onRoundEnd.RemoveListener(action);
                        break;
                    case 2:
                        onRoundEndLate.RemoveListener(action);
                        break;
                }
                break;
        }
    }

    /// <summary>
    /// invokes the specified event
    /// </summary>
    /// <param name="eventType"> the event which is to be invoked </param>
    public void invokeEvent(Events eventType)
    {
        Debug.Log("Invoking event " + eventType);
        switch (eventType)
        {
            case Events.onRoundStart:
                onRoundStartEarly.Invoke();
                onRoundStart.Invoke();
                onRoundStartLate.Invoke();
                break;
            case Events.onRoundEnd:
                onRoundEndEarly.Invoke();
                onRoundEnd.Invoke();
                onRoundEndLate.Invoke();
                break;
        }
    }

}

/// <summary>
/// collection of all game events
/// </summary>
public enum Events
{
    onRoundStart, onRoundEnd
}
