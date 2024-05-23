using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interfaces
{
    public interface ISubject<T>
    {
        // ���
        public void AddObserver(T observer);

        // ����
        public void RemoveObserver(T observer);

        // ����
        // private�� �����ϱ� ���� �ּ� ó��
        //public void NotifyObservers();
    }


    public interface IPlayerStateObserver
    {
        // ������Ʈ
        public void UpdatePlayerState();
    }
}
