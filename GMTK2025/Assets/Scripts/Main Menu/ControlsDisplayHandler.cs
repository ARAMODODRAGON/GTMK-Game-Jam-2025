using UnityEngine;

public class ControlsDisplayHandler : BaseMenuScreen {

	private CanvasGroup m_group;

	private void Awake() {
		m_group = GetComponent<CanvasGroup>();
		m_group.alpha = 0.0f;
	}

	public override void HideMenu() {
		m_group.alpha = 0.0f;
	}

	public override void OpenMenu() {
		m_group.alpha = 1.0f;
	}

	public override void UpdateMenu(Vector2 p1Direction, bool p1Confirm, Vector2 p2Direction, bool p2Confirm) {
		if (p1Confirm || p2Confirm) {
			exitMenu.Invoke();
		}
	}
}
