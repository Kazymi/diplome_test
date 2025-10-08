using StateMachine;
using UnityEngine;

public class PlayerWalkState : State
{
    private readonly CharacterParamSystem _characterParamSystem;
    private readonly PlayerAnimatorController _playerAnimatorController;
    private readonly Transform _player;
    private readonly Joystick _joystick;
    private readonly float _scale;

    public PlayerWalkState(CharacterParamSystem characterParamSystem, PlayerAnimatorController playerAnimatorController,
        Transform player, Joystick joystick)
    {
        _characterParamSystem = characterParamSystem;
        _playerAnimatorController = playerAnimatorController;
        _player = player;
        _joystick = joystick;
        _scale = player.localScale.x;
    }

    public override void OnStateEnter()
    {
        _playerAnimatorController.SetBool(PlayerAnimationType.Walk, true);
    }

    public override void OnStateExit()
    {
        _playerAnimatorController.SetBool(PlayerAnimationType.Walk, false);
    }

    public override void Tick()
    {
        var dir = _joystick.Direction;
        Vector3 movement = new Vector3(dir.x, dir.y, 0f) * _characterParamSystem.Speed * Time.deltaTime;
        _player.position += movement;
        if (dir.x > 0.01f)
        {
            _player.localScale = new Vector3(_scale, _scale, _scale);
        }
        else if (dir.x < -0.01f)
        {
            _player.localScale = new Vector3(-_scale, _scale, _scale);
        }
    }
}