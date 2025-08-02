using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

	// input
	[SerializeField] private InputActionReference m_player1Direction;
	[SerializeField] private InputActionReference m_player1Confirm;
	[SerializeField] private InputActionReference m_player2Direction;
	[SerializeField] private InputActionReference m_player2Confirm;

	// references
	[SerializeField] private RawImage m_image;

	// menus
	[SerializeField] private List<BaseMenuScreen> m_menus;

	// variables
	private Stack<int> m_menuStack = new Stack<int>();
	private BaseMenuScreen m_currentMenu => (m_menus.Count > 0) ? m_menus[m_menuStack.Peek()] : null;
	private Vector2 m_imageSize;

	private void Start() {
		// sanity check
		if (m_menus.Count == 0) {
			Debug.LogError("There are no menus?"); 
			enabled = false;
			return;
		}

		// scrolling image
		//m_image.uvRect = new Rect(0.0f, 0.0f, Screen.width, Screen.height);
		m_imageSize = m_image.uvRect.max;

		// initialize menus
		for (int i = 0; i < m_menus.Count; i++) {
			// bind to OnMenuChange so that it knows which menu called it
			int _indexlocalcopy = i;
			m_menus[i].changeMenu.AddListener((int menuIndex) => OnMenuChange(menuIndex, _indexlocalcopy));
			// bind to OnExitMenu
			m_menus[i].exitMenu.AddListener(() => OnExitMenu(_indexlocalcopy));
			// make invisible
			m_menus[i].HideMenu();
		}

		// TODO: check if the game has already selected a level and load into a different menu from there

		// load first menu
		if (m_menus.Count > 0) {
			m_menuStack.Push(0);
			m_currentMenu.OpenMenu();
		}
	}

	// called when a menu wants to change to another menu
	private void OnMenuChange(int menuIndex, int callerMenu) {
		// confirm that the caller is the topmost menu
		if (m_menuStack.Peek() != callerMenu) return;

		// confirm we arent opening the same menu
		if (menuIndex == callerMenu) return;

		// validate index
		if (menuIndex >= m_menus.Count || menuIndex < 0) return;

		// now we change menus
		m_currentMenu.HideMenu();
		m_menuStack.Push(menuIndex);
		m_currentMenu.OpenMenu();
	}

	private void OnExitMenu(int callerMenu) {
		// confirm that the caller is the topmost menu
		if (m_menuStack.Peek() != callerMenu) return;

		// we cant exit if there are no other menus on the stack
		if (m_menuStack.Count == 1) return;

		// now we change menus
		m_currentMenu.HideMenu();
		m_menuStack.Pop();
		m_currentMenu.OpenMenu();
	}

	private void Update() {
		if (m_currentMenu) UpdateCurrentMenu();

		Rect uv = m_image.uvRect;
		uv.min += new Vector2(1.0f, 1.0f) * Time.deltaTime;
		uv.max = uv.min + m_imageSize;
		m_image.uvRect = uv;
	}

	private void UpdateCurrentMenu() {
		// get input
		Vector2 p1Direction = m_player1Direction.action.ReadValue<Vector2>();
		Vector2 p2Direction = m_player2Direction.action.ReadValue<Vector2>();
		bool p1Confirm = m_player1Confirm.action.WasPressedThisFrame();
		bool p2Confirm = m_player2Confirm.action.WasPressedThisFrame();

		// call to update current menu
		m_currentMenu.UpdateMenu(p1Direction, p1Confirm, p2Direction, p2Confirm);
	}
}
