using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<T>
{
    void OnEnter(T c);
    void OnExecute(T c);
    void OnExit(T c);
}
