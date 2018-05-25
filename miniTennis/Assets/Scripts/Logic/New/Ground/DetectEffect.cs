using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectEffect : MonoBehaviour 
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        GameEventModuel eveModuel = GameStart.GetInstance().EventModuel;
        if (eveModuel != null)
        {
            eveModuel.SendEvent(GameEventID.PLAYER_HIT_BALL, true, 0f);
        }
    }
}
