using UnityEngine;

public enum EEntityState
{
    Idle = 1,
    Prepare = 2,
    Hit = 3,
}

public class EntityInstance : MonoBehaviour
{
    protected CharacterAnim m_characterAnim;
    protected CharacterCollider m_characterCollider;
    protected CharacterControl m_characterControl;
    
    [SerializeField]
    private GameObject m_target;

    [SerializeField] 
    private GameObject m_forward;

    void Start()
    {
        m_characterAnim = new CharacterAnim(gameObject);
        m_characterCollider = gameObject.AddComponent<CharacterCollider>();
        m_characterControl= gameObject.GetComponentInChildren<CharacterControl>();
        m_characterControl.SwitchState = Switch;
        m_characterControl.SetMoveEntity(gameObject);

        if (m_characterAnim != null)
        {
            m_characterAnim.PlayAnim(EEntityState.Idle);
        }
            
    }

    void Update()
    {
//        if (m_target != null && m_forward != null)
//        {
//            Vector3 direction = m_target.transform.position - transform.position;
//            direction = direction.normalized;
//            Vector3 up = (m_forward.transform.position - transform.position).normalized;
//            float angle = Vector3.Dot(direction, up);
//            angle = Mathf.Abs(1 - angle);
//            if (angle >= 0.01)
//            {
//                gameObject.transform.RotateAround(gameObject.transform.position, Vector3.forward, -10);
//            }
//            else if(angle <= -0.01)
//            {
//                gameObject.transform.RotateAround(gameObject.transform.position, Vector3.forward, 10);
//            }
//        }
    }

    public void SetTarget(GameObject target)
    {
        m_target = target;
    }
    
    private void Switch(EEntityState state)
    {
        GameStart.GetInstance().LogModuel.Log(ELogType.Normal, "Switch State : " + state.ToString());
        //播放对应动画
        if (m_characterAnim != null)
        {
            m_characterAnim.PlayAnim(state);
        }
        
        switch (state)
        {
                case EEntityState.Hit:
                    if (m_characterCollider != null && m_characterCollider.BallEnter)
                    {
                        HitBall();
                    }
                    break;
        }
    }

    private void HitBall()
    {
        GameEventModuel eventModuel = GameStart.GetInstance().EventModuel;
        eventModuel.SendEvent(GameEventID.ENTITY_HIT_BALL,true, 0f, EHitForceType.Middle, Vector2.up);
    }
}