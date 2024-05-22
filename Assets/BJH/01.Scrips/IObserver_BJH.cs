using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserverPlayerState
{
    void ChangePlayerState(string nickName, bool isOnline);
}
