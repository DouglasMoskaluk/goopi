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

    private UnityEvent<EventParams> onPlayerRespawnEarly;
    private UnityEvent<EventParams> onPlayerRespawn;
    private UnityEvent<EventParams> onPlayerRespawnLate;

    public UnityEvent<EventParams> onGameStartEarly;
    public UnityEvent<EventParams> onGameStart;
    public UnityEvent<EventParams> onGameStartLate;

    public UnityEvent<EventParams> onGameEndEarly;
    public UnityEvent<EventParams> onGameEnd;
    public UnityEvent<EventParams> onGameEndLate;

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

        onPlayerRespawnEarly = new UnityEvent<EventParams>();
        onPlayerRespawn = new UnityEvent<EventParams>();
        onPlayerRespawnLate = new UnityEvent<EventParams>();

        onGameEndEarly = new UnityEvent<EventParams>();
        onGameEnd = new UnityEvent<EventParams>();
        onGameEndLate = new UnityEvent<EventParams>();
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
            case Events.onPlayerRespawn:
                switch (eventPriority)
                {
                    case 0:
                        onPlayerRespawnEarly.AddListener(action);
                        break;
                    case 1:
                        onPlayerRespawn.AddListener(action);
                        break;
                    case 2:
                        onPlayerRespawnLate.AddListener(action);
                        break;
                }
                break;
            case Events.onGameEnd:
                switch (eventPriority)
                {
                    case 0:
                        onGameEndEarly.AddListener(action);
                        break;
                    case 1:
                        onGameEnd.AddListener(action);
                        break;
                    case 2:
                        onGameEndLate.AddListener(action);
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
            case Events.onPlayerDeath:
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
            case Events.onPlayerRespawn:
                switch (eventPriority)
                {
                    case 0:
                        onPlayerRespawnEarly.RemoveListener(action);
                        break;
                    case 1:
                        onPlayerRespawn.RemoveListener(action);
                        break;
                    case 2:
                        onPlayerRespawnLate.RemoveListener(action);
                        break;
                }
                break;
            case Events.onGameEnd:
                switch (eventPriority)
                {
                    case 0:
                        onGameEndEarly.RemoveListener(action);
                        break;
                    case 1:
                        onGameEnd.RemoveListener(action);
                        break;
                    case 2:
                        onGameEndLate.RemoveListener(action);
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
        //Debug.Log("Invoking event " + eventType);
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
            case Events.onPlayerRespawn:
                onPlayerRespawnEarly.Invoke(param);
                onPlayerRespawn.Invoke(param);
                onPlayerRespawnLate.Invoke(param);
                break;
            case Events.onGameEnd:
                onGameEndEarly.Invoke(param);
                onGameEnd.Invoke(param);
                onGameEndLate.Invoke(param);
                break;
        }
    }

}

/// <summary>
/// collection of all game events
/// </summary>
public enum Events
{
    onRoundStart, onRoundEnd, onPlayerDeath, onPlayerRespawn, onGameEnd
}

public struct EventParams
{
    internal int killer;
    internal int killed;

    internal EventParams(int Killed, int Killer)
    {
        killed = Killed;
        killer = Killer;
    }
}
