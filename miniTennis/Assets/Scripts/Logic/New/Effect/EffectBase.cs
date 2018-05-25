using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectBase
{
    public EEffectType m_effectType;
    protected ESide m_side;
    public float m_duringTime;
    
    public EffectBase(EEffectType effectType, float time)
    {
        m_effectType = effectType;
        m_duringTime = time;
    }

    public virtual void SetSide(ESide side)
    {
        m_side = side;
    }
}

public class EffectShield : EffectBase
{
    
    public EffectShield(float duringtTime)
        : base(EEffectType.Shield, duringtTime)
    {
    }
}

public class EffectBananaBall : EffectBase
{
    public EffectBananaBall(float duringtTime)
        : base(EEffectType.BananaBall, duringtTime)
    {
        
    }
}

//public class EffectDisappearAndApear : EffectBase
//{
//    public EffectDisappearAndApear(float duringtTime) 
//        : base(EEffectType.DisapearAndApear, duringtTime)
//    {
//        
//    }
//}
