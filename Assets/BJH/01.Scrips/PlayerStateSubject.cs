using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �������� �����ϴ� Ŭ�����Դϴ�.
public class PlayerStateSubject : ISubject<IPlayerStateObserver>
{
    // ������

    private List<IPlayerStateObserver> _observer = new List<IPlayerStateObserver>();
    //public List<IPlayerStateObserver> Observers { get {  return _observer; } }


    // ������ �߰�
    public void AddObserver(IPlayerStateObserver observer)
    {
        _observer.Add(observer);
    }

    // ������ ����
    public void RemoveObserver(IPlayerStateObserver observer)
    {
        _observer.Remove(observer);
    }    

    // ������ ����
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
