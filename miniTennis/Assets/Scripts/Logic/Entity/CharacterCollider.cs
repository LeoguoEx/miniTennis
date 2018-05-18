using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollider : MonoBehaviour
{
    private bool m_ballEnter = false;
    public bool BallEnter
    {
        get { return m_ballEnter; }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        m_ballEnter = CheckBallEnter(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        m_ballEnter = CheckBallEnter(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        m_ballEnter = false;
    }

    private bool CheckBallEnter(Collider2D collider)
    {
        return collider != null && collider.gameObject.layer == LayerMask.NameToLayer("Ball");
    }
}
