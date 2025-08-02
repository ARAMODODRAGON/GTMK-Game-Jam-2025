using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuController : MonoBehaviour {

	// input
	[SerializeField] private InputActionReference m_player1Direction;
	[SerializeField] private InputActionReference m_player1Confirm;
	[SerializeField] private InputActionReference m_player2Direction;
	[SerializeField] private InputActionReference m_player2Confirm;

	// menus
	[SerializeField] private List<BaseMenuScreen> m_menus;

	// variables
	private Stack<int> m_menuStack = new Stack<int>();
	private BaseMenuScreen m_currentMenu => (m_menus.Count > 0) ? m_menus[m_menuStack.Peek()] : null;

	private void Start() {
		// sanity check
		if (m_menus.Count == 0) {
			Debug.LogError("There are no menus?"); 
			enabled = false;
			return;
		}
		
		// initialize menus
		for (int i = 0; i < m_menus.Count; i++) {
			// bind to OnMenuChange so that it knows which menu called it
			m_menus[i].changeMenu.AddListener((int menuIndex) => OnMenuChange(menuIndex, i));
			// bind to OnExitMenu
			m_menus[i].exitMenu.AddListener(() => OnExitMenu(i));
			// make invisible
			m_menus[i].SetAlpha(0.0f);
		}

		// TODO: check if the game has already selected a level and load into a different menu from there

		// load first menu
		if (m_menus.Count > 0) {
			m_menuStack.Push(0);
			m_currentMenu.SetAlpha(1.0f);
		}
	}

	// called when a menu wants to change to another menu
	private void OnMenuChange(int menuIndex, int callerMenu) {
		// TODO
	}

	private void OnExitMenu(int callerMenu) {
		// TODO
	}

	private void Update() {
		if (m_currentMenu) UpdateCurrentMenu();
	}

	private void UpdateCurrentMenu() {

	}
}
