using UnityEngine;

public class TitleScreenHandler : BaseMenuScreen {

	private CanvasGroup m_group;

	private void Start() {
		m_group = GetComponent<CanvasGroup>();
	}

	public override void SetAlpha(float alpha) {
		m_group.alpha = alpha;
	}

	public override void UpdateMenu(Vector2 p1Direction, bool p1Confirm, Vector2 p2Direction, bool p2Confirm) {
		// TODO
	}
}
