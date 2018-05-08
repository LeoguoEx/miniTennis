using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameModuelBase : MonoBehaviour
{
    public abstract void Init();

    protected void Log(ELogType logType, string log)
    {
        GameStart.GetInstance().LogModuel.Log(logType, log);
    }
}
