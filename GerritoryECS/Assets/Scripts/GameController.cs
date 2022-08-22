using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

public class GameController : MonoBehaviour
{
	Systems m_Systems;

	// Start is called before the first frame update
	private void Start()
	{
		var contexts = Contexts.SharedInstance;
		m_Systems = new Feature("Systems").Add(new TutorialFeature(contexts));
		m_Systems.Initialize();
	}

	// Update is called once per frame
	private void Update()
	{
		m_Systems.Update();
		m_Systems.Execute();
	}

	private void LateUpdate()
	{
		m_Systems.LateUpdate();
		m_Systems.Cleanup();
	}
}
