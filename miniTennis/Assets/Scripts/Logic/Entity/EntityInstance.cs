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
    protected CharacterCollider m_batCollider;
    protected CharacterControl m_characterControl;
    
    [SerializeField]
    private GameObject m_target;

    [SerializeField] 
    private GameObject m_forward;
    
    private Vector2 m_hitBallRange = new Vector2(-30f, 30f);
    private Vector2 m_entityMoveRange = new Vector2(-0.05f, 0.05f);
    private Vector4 m_moveArea;

    private Vector3 m_startPosition;

    void Start()
    {
        m_characterAnim = new CharacterAnim(gameObject);
        m_characterCollider = gameObject.GetComponent<CharacterCollider>();
        GameObject bat = CommonFunc.GetChild(gameObject, "Bat");
        m_batCollider = CommonFunc.AddSingleComponent<CharacterCollider>(bat);
        m_characterControl= gameObject.GetComponentInChildren<CharacterControl>();
        if (m_characterControl != null)
        {
            m_characterControl.SwitchState = Switch;
            m_characterControl.MoveEntityAction = SetMovePosition;
            m_characterControl.SetMoveEntity(gameObject);
        }

        if (m_characterAnim != null)
        {
            m_characterAnim.PlayAnim(EEntityState.Idle);
        }

        m_startPosition = transform.position;
    }

    public void ResetStartPosition()
    {
        gameObject.transform.position = m_startPosition;
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

    public Vector2 GetForwardDir()
    {
        if (m_forward != null)
        {
            return (m_forward.transform.position - transform.position);
        }
        return Vector2.zero;
    }

    public void SetTarget(GameObject target)
    {
        m_target = target;
    }
    
    public void Switch(EEntityState state, Vector3 recordPos)
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
                    if ((m_characterCollider != null && m_characterCollider.BallEnter) || (m_batCollider != null && m_batCollider.BallEnter))
                    {
                        HitBall(recordPos);
                    }
                    break;
        }
    }

    private void HitBall(Vector3 recordPos)
    {
        GameEventModuel eventModuel = GameStart.GetInstance().EventModuel;
        Vector2 dir = CalculateHitBallDir(recordPos);
        if (m_characterControl != null)
        {
            eventModuel.SendEvent(GameEventID.ENTITY_HIT_BALL, true, 0f, EHitForceType.Middle, new Vector2(dir.x, dir.y));
        }
        else
        {
            EHitForceType type = (EHitForceType)Random.Range((int) EHitForceType.Soft, (int) EHitForceType.High);
            eventModuel.SendEvent(GameEventID.AI_HIT_BALL, true, 0f, type, new Vector2(dir.x, dir.y));
        }
    }

    private Vector3 CalculateHitBallDir(Vector3 recordPos)
    {
        float a = Mathf.Clamp(transform.position.x - recordPos.x, m_entityMoveRange.x, m_entityMoveRange.y);
        float b = m_entityMoveRange.y / m_hitBallRange.y;
        float range = a / b;
        Vector3 ballMoveDir = Quaternion.AngleAxis(range, Vector3.forward) * Vector3.up * GetEntityMoveDir();
        ballMoveDir.z = 0f;
        return ballMoveDir.normalized;
    }

    private float GetEntityMoveDir()
    {
        if (m_characterControl != null)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }

    public void SetMovePosition(Vector3 movePosition)
    {
        gameObject.transform.position = CheckOutofRange(movePosition);
    }

    public void SetMoveRange(Vector4 moveRange)
    {
        m_moveArea = moveRange;
    }

    private Vector3 CheckOutofRange(Vector3 point)
    {
        point.x = Mathf.Clamp(point.x, m_moveArea.x, m_moveArea.z);
        point.y = Mathf.Clamp(point.y, m_moveArea.w, m_moveArea.y);
        return point;
    }
}