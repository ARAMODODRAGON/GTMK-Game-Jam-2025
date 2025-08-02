using System;
using TMPro;
using UnityEngine;

public class BattleUI : MonoBehaviour {

	// references
	[SerializeField] private TextMeshProUGUI m_player1HP;
	[SerializeField] private TextMeshProUGUI m_player2HP;
	[SerializeField] private TextMeshProUGUI m_timer;

	private void Awake() {
		EventBus.updatePlayerHealth.AddListener(UpdatePlayerHP);
		EventBus.updateGameTimer.AddListener(UpdateGameTimer);
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
}
