using UnityEngine;

public interface IPlayerState
{
	void Init(Player player, PlayerStateMachine playerStateMachine);
	void Update();
	void HandleInput();
	void Remove();
}
