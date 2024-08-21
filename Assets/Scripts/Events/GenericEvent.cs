using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericEvent<T> : ScriptableObject
{
    public UnityAction<T> OnEventRaised;

    public void RaiseEvent(T arg)
    {
        OnEventRaised?.Invoke(arg);
    }
}

public class GenericEvent : ScriptableObject
{
    public UnityAction OnEventRaised;

    public void RaiseEvent() 
    {
        OnEventRaised?.Invoke();
    }
}
