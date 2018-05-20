using UnityEngine;

public class BallMechine
{
    private BallMechineInstance m_instance;
    private BallMechineData m_data;

    public BallMechine(BallMechineData data)
    {
        m_data = data;

        GameResModuel resModuel = GameStart.GetInstance().ResModuel;
        GameObject mechine = resModuel.LoadResources<GameObject>(EResourceType.BallMechine, "BallMechine");
        mechine = CommonFunc.Instantiate(mechine);
        m_instance = mechine.AddComponent<BallMechineInstance>();
        mechine.transform.position = data.m_mechinePosition;
        mechine.transform.rotation = Quaternion.Euler(data.m_mechineRotation);
    }

    public void ServieBall()
    {
        if (m_instance != null)
        {
            m_instance.PlayAnim(m_data.m_servieBallAnim);
        }
    }
}
