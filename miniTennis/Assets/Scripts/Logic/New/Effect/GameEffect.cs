using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEffectType
{
    Shield = 0,
    BananaBall = 1,
    //DisapearAndApear = 2,
    MaxType = 2,
}

public class GameEffect
{
    private Dictionary<byte, EffectBase> m_effectDic;
    
    private GameEffectInstance m_effectInstance;
    private GameEffectData m_effectData;

    private bool m_effecting;
    private float m_createNextTime;
    private float m_value;

    private EEffectType m_preEffectType;

    public EEffectType PreEffectType
    {
        get { return m_preEffectType; }
    }
    
    public GameEffect()
    {
        m_effectDic = new Dictionary<byte, EffectBase>
        {
            {(byte)EEffectType.Shield, new EffectShield(5f)},
            {(byte)EEffectType.BananaBall, new EffectBananaBall(10f)}
        };
        
        m_effectData = new GameEffectData();
        
        GameResModuel resModuel = GameStart.GetInstance().ResModuel;
        GameObject prefab = resModuel.LoadResources<GameObject>(EResourceType.Effect, "Warning");

        prefab = CommonFunc.Instantiate(prefab);
        m_effectInstance = prefab.AddComponent<GameEffectInstance>();
        m_effectInstance.gameObject.SetActive(false);
        
        m_effecting = false;
        m_createNextTime = Time.time + m_effectData.GetRandomTime();
        
        m_preEffectType = EEffectType.MaxType;
    }

    public T GetEffectData<T>(EEffectType effectType)
        where T : EffectBase
    {
        m_preEffectType = effectType;
        EffectBase effect = null;
        m_effectDic.TryGetValue((byte) effectType, out effect);
        return effect as T;
    }

    public void SetEffectEnd()
    {
        m_effecting = false;
        m_createNextTime = Time.time + m_effectData.GetRandomTime();
    }

    public void CreateEffect()
    {
        m_effecting = true;
        if (m_effectInstance != null)
        {
            float x = Random.Range(m_effectData.m_createEffectRect.x, m_effectData.m_createEffectRect.y);
            float y = Random.Range(m_effectData.m_createEffectRect.width, m_effectData.m_createEffectRect.height);
            m_effectInstance.gameObject.SetActive(true);
            m_effectInstance.transform.position = new Vector3(x, y, 0f);
            m_effectInstance.PlayTweenScale();
            m_value = Random.Range(0, 1) > 0.5f ? 1 : -1;
        }
    }

    public void Update()
    {
        if (!m_effecting && Time.time > m_createNextTime)
        {
            CreateEffect();
        }

        if (m_effectInstance != null && m_effectInstance.gameObject.activeSelf)
        {
            Vector3 position = m_effectInstance.gameObject.transform.position;
            position += new Vector3(Time.deltaTime * m_effectData.m_moveSpeed * m_value, 0f);
            if (position.x > m_effectData.m_createEffectRect.y)
            {
                m_value = -1;
            }
            else if (position.x < m_effectData.m_createEffectRect.x)
            {
                m_value = 1;
            }
            m_effectInstance.transform.position = position;
        }
    }

    public void Destory()
    {
        if (m_effectInstance != null)
        {
            GameObject.Destroy(m_effectInstance.gameObject);
        }

        m_effectInstance = null;
    }
}
