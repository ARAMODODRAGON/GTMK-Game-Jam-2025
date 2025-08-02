using UnityEngine;
using UnityEngine.Events;

public static class EventBus {

	// called to update the UI
	// args (int playerindex, int health)
	public static UnityEvent<int, int> updatePlayerHealth = new UnityEvent<int, int>();

	// called when the game ends
	public static UnityEvent gameEnd = new UnityEvent();

	// called to update the timer in the UI
	public static UnityEvent<float> updateGameTimer = new UnityEvent<float>();

}