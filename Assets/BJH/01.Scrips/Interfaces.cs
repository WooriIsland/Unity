using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interfaces
{
    public interface ISubject<T>
    {
        // 등록
        public void AddObserver(T observer);

        // 해지
        public void RemoveObserver(T observer);

        // 실행
        // private로 선언하기 위해 주석 처리
        //public void NotifyObservers();
    }


    public interface IPlayerStateObserver
    {
        // 업데이트
        public void UpdatePlayerState();
    }
}
