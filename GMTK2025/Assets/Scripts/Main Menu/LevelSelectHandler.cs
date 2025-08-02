using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectHandler : BaseMenuScreen {

	
	// components
	private CanvasGroup m_group;

	// animation
	[SerializeField] private float m_scaleIncrease;
	[SerializeField] private float m_scaleSpeed;
	[SerializeField] private float m_scrollDelay;
	
	// struct to represent a menu option and where it goes
	[Serializable]
	private struct MenuOption {
		public int loadLevelIndex; // -1 means return here
		public RectTransform transform;
	}

	// references to menu options
	[SerializeField] private List<MenuOption> m_menuOptions;

	// variables
	private int m_optionIndex = 0;
	private float m_scrollDelayTimer = 0.0f;

	private void Awake() {
		m_group = GetComponent<CanvasGroup>();
		m_group.alpha = 0.0f;
	}
	
	public override void OpenMenu() {
		m_group.alpha = 1.0f;
	}

	public override void HideMenu() {
		m_group.alpha = 0.0f;
	}

	public override void UpdateMenu(Vector2 p1Direction, bool p1Confirm, Vector2 p2Direction, bool p2Confirm) {
		// update timer
		if (m_scrollDelayTimer > 0.0f) m_scrollDelayTimer -= Time.deltaTime;

		// sanity check
		if (m_menuOptions.Count == 0) return;

		// combine inputs
		Vector2 combineDirection = p1Direction + p2Direction;
		bool combineConfirm = p1Confirm || p2Confirm;

		// convert direction (use [very_small] so that we can make sure that
		// if [y] is very small but not 0 it does not move up or down)
		bool upInput = combineDirection.y > Util.very_small;
		bool downInput = combineDirection.y < -Util.very_small;

		// update options
		UpdateOptions(upInput, downInput);

		// confirm option
		if (combineConfirm) {
			AudioManager.instance.PlaySound(AudioManager.instance.uiScroll, transform.position, 1.0f, 0.0f, false);
			// this is to quit
			if (m_menuOptions[m_optionIndex].loadLevelIndex == -1) {
				exitMenu.Invoke();
				return;
			}

			// load level
			SceneManager.LoadScene(m_menuOptions[m_optionIndex].loadLevelIndex);
		}
	}
	
	private void UpdateOptions(bool upInput, bool downInput) {
		if (m_scrollDelayTimer > 0.0f) return;

		// store this value
		int lastOptionIndex = m_optionIndex;

		// move index
		if (upInput) m_optionIndex--;
		if (downInput) m_optionIndex++;

		if (lastOptionIndex != m_optionIndex) {
			// reset timer
			m_scrollDelayTimer = m_scrollDelay;
			AudioManager.instance.PlaySound(AudioManager.instance.uiScroll, transform.position, 1.0f, 0.0f, false);
			// wrap
			if (m_optionIndex < 0) m_optionIndex += m_menuOptions.Count;
			if (m_optionIndex >= m_menuOptions.Count) m_optionIndex -= m_menuOptions.Count;

			// print
			//Debug.Log("selected: " + m_menuOptions[m_optionIndex].transform.name);

		}
	}

	private void LateUpdate() {
		for (int i = 0; i < m_menuOptions.Count; i++) {
			float targetScale = 1.0f;
			if (i == m_optionIndex) targetScale = m_scaleIncrease;

			float scale = m_menuOptions[i].transform.localScale.x;
			scale = Util.MoveToward(scale, targetScale, m_scaleSpeed * Time.deltaTime);
			m_menuOptions[i].transform.localScale = new Vector3(scale, scale, scale);
		}
	}
}
