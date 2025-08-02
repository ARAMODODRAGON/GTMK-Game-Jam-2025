using System;
using TMPro;
using UnityEngine;

public class BattleUI : MonoBehaviour {

	// references
	[SerializeField] private TextMeshProUGUI m_player1HP;
	[SerializeField] private TextMeshProUGUI m_player2HP;
	[SerializeField] private TextMeshProUGUI m_timer;
	[SerializeField] private RectTransform m_announcmentTransform;
	[SerializeField] private TextMeshProUGUI m_announcmentText;

	// components
	[SerializeField] private Timer m_endDelayTimer;

	// properties
	[SerializeField] private float m_endDelay;
	[SerializeField] private float m_transformSpeed;

	private void Awake() {
		EventBus.updatePlayerHealth.AddListener(UpdatePlayerHP);
		EventBus.updateGameTimer.AddListener(UpdateGameTimer);
		EventBus.announceWinner.AddListener(AnnounceWinner);
		m_endDelayTimer.onTimerFinished.AddListener(EventBus.gameEnd.Invoke);
		m_endDelayTimer.onTimerUpdated.AddListener(UpdateEndGame);
	}

	private void UpdatePlayerHP(int playerindex, int hpvalue) {
		if (playerindex == 1) {
			m_player1HP.text = "Player 1 -   " + hpvalue.ToString();
		} else {
			m_player2HP.text = hpvalue.ToString() + "   - Player 2";
		}
	}

	private void UpdateGameTimer(float time) {
		m_timer.text = $"{time:.}s";
	}

	private void AnnounceWinner(int playerindex) {
		m_endDelayTimer.StartTimer(m_endDelay);

		m_announcmentText.text = "PLAYER " + playerindex + "\nWON!";
		m_timer.text = "";
	}

	private void UpdateEndGame(float remainingTime) {
		Vector3 position = m_announcmentTransform.position;
		position.y = Util.MoveToward(position.y, 0.0f, m_transformSpeed * Time.deltaTime);
		m_announcmentTransform.position = position;
	}
}
