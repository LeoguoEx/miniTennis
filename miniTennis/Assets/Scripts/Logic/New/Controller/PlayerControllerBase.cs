using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerControllerBase : MonoBehaviour
{
    protected Player m_player;

    public abstract void InitController(Player player);

    public abstract void DestroyController();
}
