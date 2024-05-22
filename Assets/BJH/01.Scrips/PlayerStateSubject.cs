using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 옵저버 패턴의 Subject 클래스
public class PlayerStateSubject : MonoBehaviour
{
    private List<IObserverPlayerState> observers = new List<IObserverPlayerState>();

    public void AddObserver(IObserverPlayerState observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IObserverPlayerState observer)
    {
        observers.Remove(observer);
    }
    public void ChangePlayerState(string name, bool isOnline)
    {
        NotifyObservers(name, isOnline);
    }

    public void NotifyObservers(string name, bool isOnline)
    {
        foreach(var observer in observers)
        {
            observer.ChangePlayerState(name, isOnline);
        }
    }

}
