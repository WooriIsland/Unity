using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 옵저버를 관리하는 클래스입니다.
public class PlayerStateSubject : ISubject<IPlayerStateObserver>
{
    // 옵저버

    private List<IPlayerStateObserver> _observer = new List<IPlayerStateObserver>();
    //public List<IPlayerStateObserver> Observers { get {  return _observer; } }


    // 옵저버 추가
    public void AddObserver(IPlayerStateObserver observer)
    {
        _observer.Add(observer);
    }

    // 옵저버 삭제
    public void RemoveObserver(IPlayerStateObserver observer)
    {
        _observer.Remove(observer);
    }    

    // 옵저버 실행
    private void NotifyObservers(string nickName, bool isOnline, bool isChangeCharacter)
    {
        foreach (IPlayerStateObserver observer in _observer)
        {
            observer.UpdatePlayerState(nickName, isOnline, isChangeCharacter);
        }
    }

    public void UpdatePlayerState(string nickName, bool isOnline, bool isChangeCharacter = false)
    {
        NotifyObservers(nickName, isOnline, isChangeCharacter);
    }

}
