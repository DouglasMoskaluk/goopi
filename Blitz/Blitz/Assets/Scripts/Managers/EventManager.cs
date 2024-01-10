using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;


    // NOTICE because these events are private they must be initialized in awake, otherwise they produce a nulref error
    private UnityEvent<EventParams> onRoundStartEarly;
    private UnityEvent<EventParams> onRoundStart;
    private UnityEvent<EventParams> onRoundStartLate;

    private UnityEvent<EventParams> onRoundEndEarly;
    private UnityEvent<EventParams> onRoundEnd;
    private UnityEvent<EventParams> onRoundEndLate;

    private UnityEvent<EventParams> onPlayerDeathEarly;
    private UnityEvent<EventParams> onPlayerDeath;
    private UnityEvent<EventParams> onPlayerDeathLate;

    private void Awake()
    {
        if (instance == null) instance = this;

        onRoundEndEarly = new UnityEvent<EventParams>();
        onRoundEnd = new UnityEvent<EventParams>();
        onRoundEndLate = new UnityEvent<EventParams>();

        onRoundStartEarly = new UnityEvent<EventParams>();
        onRoundStart = new UnityEvent<EventParams>();
        onRoundStartLate = new UnityEvent<EventParams>();

        onPlayerDeathEarly = new UnityEvent<EventParams>();
        onPlayerDeath = new UnityEvent<EventParams>();
        onPlayerDeathLate = new UnityEvent<EventParams>();
    }

    /// <summary>
    /// adds an action to the specified event with the given priority
    /// </summary>
    /// <param name="eventType"> the type of event that is to be added to </param>
    /// <param name="eventPriority"> which priority of the event that is being added to, 0 -> early, 1 -> regular, 2 -> late </param>
    /// <param name="action"> the action that is being added, the function being called from the invoking of the event </param>
    public void addListener(Events eventType, UnityAction<EventParams> action, int eventPriority = 1)
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
            case Events.onPlayerDeath:
                switch (eventPriority)
                {
                    case 0:
                        onPlayerDeathEarly.AddListener(action);
                        break;
                    case 1:
                        onPlayerDeath.AddListener(action);
                        break;
                    case 2:
                        onPlayerDeathLate.AddListener(action);
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
    public void removeListener(Events eventType, UnityAction<EventParams> action, int eventPriority = 1)
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
                        onPlayerDeathEarly.RemoveListener(action);
                        break;
                    case 1:
                        onPlayerDeath.RemoveListener(action);
                        break;
                    case 2:
                        onPlayerDeathLate.RemoveListener(action);
                        break;
                }
                break;
        }
    }

    /// <summary>
    /// invokes the specified event
    /// </summary>
    /// <param name="eventType"> the event which is to be invoked </param>
    public void invokeEvent(Events eventType, EventParams param = new EventParams())
    {
        Debug.Log("Invoking event " + eventType);
        switch (eventType)
        {
            case Events.onRoundStart:
                onRoundStartEarly.Invoke(param);
                onRoundStart.Invoke(param);
                onRoundStartLate.Invoke(param);
                break;
            case Events.onRoundEnd:
                onRoundEndEarly.Invoke(param);
                onRoundEnd.Invoke(param);
                onRoundEndLate.Invoke(param);
                break;
            case Events.onPlayerDeath:
                onPlayerDeathEarly.Invoke(param);
                onPlayerDeath.Invoke(param);
                onPlayerDeathLate.Invoke(param);
                break;
        }
    }

}

/// <summary>
/// collection of all game events
/// </summary>
public enum Events
{
    onRoundStart, onRoundEnd, onPlayerDeath
}

public struct EventParams
{

}
