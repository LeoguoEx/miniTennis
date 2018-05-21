using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallData
{
    public Rect m_ballBoundArea;
    
    public BallData()
    {
        m_ballBoundArea = new Rect(-10f, -10f, 10f, 10f);
    }
}
