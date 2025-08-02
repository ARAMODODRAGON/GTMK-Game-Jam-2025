using System;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenHandler : BaseMenuScreen {

	// struct to represent a menu option and where it goes
	[Serializable]
	private struct MenuOption {
		public int nextMenuIndex; // assuming -1 means quit here
		public RectTransform transform;
	}

	// references to menu options
	[SerializeField] private List<MenuOption> m_menuOptions;

	// components
	private CanvasGroup m_group;

	// variables
	int m_optionIndex = 0;

	private void Awake() {
		m_group = GetComponent<CanvasGroup>();
		m_group.alpha = 0.0f;
	}

	public override void OpenMenu() {
		m_optionIndex = 0;
		m_group.alpha = 1.0f;
	}

	public override void HideMenu() {
		m_group.alpha = 0.0f;
	}

	public override void UpdateMenu(Vector2 p1Direction, bool p1Confirm, Vector2 p2Direction, bool p2Confirm) {
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
		UpdateOptions(upInput, downInput, combineConfirm);
	}

	private void UpdateOptions(bool upInput, bool downInput, bool confirm) {
		// store this value
		int lastOptionIndex = m_optionIndex;

		// move index
		if (upInput) m_optionIndex--;
		if (downInput) m_optionIndex++;

		// wrap
		if (m_optionIndex < 0) m_optionIndex += m_menuOptions.Count;
		if (m_optionIndex >= m_menuOptions.Count) m_optionIndex -= m_menuOptions.Count;

		// actually update the option
		if (lastOptionIndex != m_optionIndex)
			Debug.Log("selected: " + m_menuOptions[m_optionIndex]);

	}
}
