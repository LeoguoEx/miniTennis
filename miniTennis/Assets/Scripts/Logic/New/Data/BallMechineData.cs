using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMechineData
{
    public Vector3 m_mechinePosition;
    public Vector3 m_mechineRotation;

    public string m_servieBallAnim;

    public BallMechineData()
    {
        m_mechinePosition = new Vector3(0f, 8.34f, 0f);
        m_mechineRotation = new Vector3(0f, 0f, 180f);
        m_servieBallAnim = "FireBallMechine";
    }

    public IEnumerator FirstFireBall()
    {
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator SecondFireBall()
    {
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator ThirdFireBall()
    {
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator FourthFireBall()
    {
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator FifthFireBall()
    {
        yield return new WaitForEndOfFrame();
    }
}
