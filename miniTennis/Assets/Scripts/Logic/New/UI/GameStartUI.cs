using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartUI : GameUIBase
{
    private Animator m_animator;
    private Button m_button;
    
    public override void OnInit()
    {
        m_animator = gameObject.GetComponent<Animator>();
        m_button = CommonFunc.GetChild(gameObject, "ContinueButton").GetComponent<Button>();
        m_button.onClick.AddListener(OnContinueButtonClick);
        
        StartCoroutine(PlayButtonTweenAnim());
    }

    public override void OnDispatch()
    {
        
    }

    private void OnContinueButtonClick()
    {
        GameEventModuel eveModuel = GameStart.GetInstance().EventModuel;
        eveModuel.SendEvent(GameEventID.SWITCH_GAME_STATE, true, 0f, EGameStateType.GameContestState);
    }

    private IEnumerator PlayButtonTweenAnim()
    {
        yield return new WaitForSeconds(0.5f);
        
        m_animator.Play("StartAnim");
        
        yield return new WaitForSeconds(1.08f);

//        LTDescr descr = LeanTween.size(m_button.gameObject.GetComponent<RectTransform>(), new Vector2(99f, 99f), 0.5f);
//        descr = descr.setFrom(new Vector3(90f, 90f));
//        descr = descr.setLoopPingPong();
//        descr.setRepeat(1000);
    }
}
