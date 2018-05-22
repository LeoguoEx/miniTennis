using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIModuel : GameModuelBase
{
	private GameObject m_uiParent;
	private Transform m_parent;

	private Dictionary<string, GameUIBase> m_uiDic;
	
	public override void Init()
	{
		m_uiDic = new Dictionary<string, GameUIBase>();
		
		GameResModuel resModuel = GameStart.GetInstance().ResModuel;
		GameObject ui = resModuel.LoadResources<GameObject>(EResourceType.UI, "UIParent");
		m_uiParent = CommonFunc.Instantiate(ui);
		m_parent = CommonFunc.GetChild(m_uiParent, "Canvas").transform;
	}

	public T LoadResUI<T>(string name)
		where T : GameUIBase
	{
		GameResModuel resModuel = GameStart.GetInstance().ResModuel;
		GameObject ui = resModuel.LoadResources<GameObject>(EResourceType.UI, name);
		ui = CommonFunc.Instantiate(ui);
		ui.name = name;
		ui.transform.parent = m_parent;
		ui.transform.localPosition = Vector3.zero;
		ui.transform.localScale = Vector3.one;
		T script = ui.AddComponent<T>();
		m_uiDic.Add(name, script);
		return script;
	}

	public void UnLoadResUI(GameObject go)
	{
		GameResModuel resModuel = GameStart.GetInstance().ResModuel;
		GameObject.Destroy(go);
		m_uiDic.Remove(go.name);
		resModuel.ResourcesUnLoad();
	}
}
