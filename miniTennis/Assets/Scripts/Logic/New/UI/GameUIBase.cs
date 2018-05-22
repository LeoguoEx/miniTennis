using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameUIBase : MonoBehaviour
{

    void Start()
    {
        OnInit();
        RegisterEvent();
    }

    void Destory()
    {
        OnDispatch();
        UnRegisterEvent();
    }
    
    public abstract void OnInit();

    public abstract void OnDispatch();

    public virtual void RegisterEvent(){}
    
    public virtual void UnRegisterEvent(){}
}
