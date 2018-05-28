using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameStartUI : GameUIBase, IPointerDownHandler, IPointerUpHandler
{
    private Animator m_animator;
    private Button m_button;
    private Button m_contestButton;
    private Button m_bombButton;
    private GameObject m_mode;
    
    public override void OnInit()
    {
        m_animator = gameObject.GetComponent<Animator>();
        m_button = CommonFunc.GetChild(gameObject, "ContinueButton").GetComponent<Button>();
        m_button.onClick.AddListener(OnContinueButtonClick);

        m_mode = CommonFunc.GetChild(gameObject, "Mode");
        m_contestButton = CommonFunc.GetChild(gameObject, "ContestButton").GetComponent<Button>();
        m_bombButton = CommonFunc.GetChild(gameObject, "BombButton").GetComponent<Button>();
        m_contestButton.onClick.AddListener(OnEnterContestState);
        m_bombButton.onClick.AddListener(OnEnterBombState);
        
        m_mode.SetActive(false);
        
        StartCoroutine(PlayButtonTweenAnim());
    }

    public override void OnDispatch()
    {
        
    }

    private void OnContinueButtonClick()
    {
        m_mode.SetActive(true);
        m_button.gameObject.SetActive(false);

        GameAudioModuel moduel = GameStart.GetInstance().AudioModuel;
        moduel.PlayAudio("click_01");
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
    
    void IPointerDownHandler.OnPointerDown (PointerEventData data)
    {
        OnButtonPress (true);
    }

    void IPointerUpHandler.OnPointerUp (PointerEventData data)
    {
        OnButtonPress (false);
    }

    private void OnButtonPress(bool press)
    {
        Vector3 targetScale = press ? m_button.transform.localScale * 0.9f: m_button.transform.localScale;
        LeanTween.scale (m_button.gameObject, targetScale, 0.15f).setIgnoreTimeScale(true);
    }

    private void OnEnterContestState()
    {
        GameEventModuel eveModuel = GameStart.GetInstance().EventModuel;
        eveModuel.SendEvent(GameEventID.SWITCH_GAME_STATE, true, 0f, EGameStateType.GameContestState);

        GameAudioModuel moduel = GameStart.GetInstance().AudioModuel;
        moduel.PlayAudio("click_01");
    }

    private void OnEnterBombState()
    {
        GameEventModuel eveModuel = GameStart.GetInstance().EventModuel;
        eveModuel.SendEvent(GameEventID.SWITCH_GAME_STATE, true, 0f, EGameStateType.BombState);

        GameAudioModuel moduel = GameStart.GetInstance().AudioModuel;
        moduel.PlayAudio("click_01");
    }
}
