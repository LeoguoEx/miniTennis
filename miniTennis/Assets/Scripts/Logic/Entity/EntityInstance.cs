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
    
    private Vector2 m_hitBallRange = new Vector2(-30f, 30f);
    private Vector2 m_entityMoveRange = new Vector2(-0.05f, 0.05f);

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
    
    private void Switch(EEntityState state, Vector3 recordPos)
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
                        HitBall(recordPos);
                    }
                    break;
        }
    }

    private void HitBall(Vector3 recordPos)
    {
        GameEventModuel eventModuel = GameStart.GetInstance().EventModuel;
        Vector2 dir = CalculateHitBallDir(recordPos);
        eventModuel.SendEvent(GameEventID.ENTITY_HIT_BALL, true, 0f, EHitForceType.Middle, new Vector2(dir.x, dir.y));
    }

    private Vector3 CalculateHitBallDir(Vector3 recordPos)
    {
        float a = Mathf.Clamp(transform.position.x - recordPos.x, m_entityMoveRange.x, m_entityMoveRange.y);
        float b = m_entityMoveRange.y / m_hitBallRange.y;
        float range = a / b;
        Vector3 ballMoveDir = Quaternion.AngleAxis(range, Vector3.forward) * Vector3.up;
        ballMoveDir.z = 0f;
        return ballMoveDir.normalized;
    }
}