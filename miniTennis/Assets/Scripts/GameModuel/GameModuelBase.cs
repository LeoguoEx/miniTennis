using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameModuelBase : MonoBehaviour
{
    protected abstract void Awake();

    protected abstract void Start();

    protected virtual void Update(){}

    protected virtual void FixedUpdate(){}

    protected virtual void LateUpdate(){}

    protected virtual void OnGUI(){}

    protected virtual void OnEnable(){}

    protected virtual void OnDisable(){}

    protected abstract void OnDestory();
}
