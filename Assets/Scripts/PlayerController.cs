using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TetraCreations.Attributes;
using TMPro;

public enum Player
{
    Player_1,
    Player_2,
}

public class PlayerController : MonoBehaviour
{
    public static event Action<Player, Direction> OnAttack;
    public static event Action<Player> OnAttackReceived;
    public static event Action<Player> OnAttackBlocked;

    public Player player;
    Animator anim;

    public bool isDefend;
    public Direction defendDirection;
    [Range(0, 22.5f)] public float angleDirectionThreshold;
    public TextMeshProUGUI defendText;

    private void Awake()
    {
        TouchInputManager.OnSwipeGesture += CommenceAttack;
        TouchInputManager.OnHoldGesture += CommenceDefend;
        PlayerController.OnAttack += Attacked;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(isDefend)
        {
            UpdateDefend();
            defendText.text = defendDirection.ToString();
        }
    }

    void CommenceAttack(Player player, Direction attackDir)
    {        
        if(player == this.player && !isDefend)
        {
            anim.SetTrigger("Attack");
            anim.SetFloat("Direction", Array.IndexOf(Enum.GetValues(typeof(Direction)), attackDir));

            OnAttack(player, attackDir);
        }
    }

    void CommenceDefend(Player player, bool isHold)
    {
        if(player == this.player)
        {
            isDefend = isHold;            
        }
        if(this.player == Player.Player_1)
        {
            GameManager.Instance.textDefend_1.text = isDefend ? "Defend_On" : "Defend_Off";
        }
        else if (this.player == Player.Player_2)
        {
            GameManager.Instance.textDefend_2.text = isDefend ? "Defend_On" : "Defend_Off";
        }
    }

    void UpdateDefend()
    {
        if(player == Player.Player_1)
        {
            if (TouchInputManager.Instance.line1Angle <= 180 + angleDirectionThreshold &&
                TouchInputManager.Instance.line1Angle >= 180 - angleDirectionThreshold)
            {
                defendDirection = Direction.UpDown;
            }
            else if (TouchInputManager.Instance.line1Angle <= 0 + angleDirectionThreshold ||
                TouchInputManager.Instance.line1Angle >= 360 - angleDirectionThreshold)
            {
                defendDirection = Direction.UpDown;
            }
            else if (TouchInputManager.Instance.line1Angle < 90 + angleDirectionThreshold &&
                TouchInputManager.Instance.line1Angle > 90 - angleDirectionThreshold)
            {
                defendDirection = Direction.LeftRight;
            }
            else if (TouchInputManager.Instance.line1Angle < 270 + angleDirectionThreshold &&
                TouchInputManager.Instance.line1Angle > 270 - angleDirectionThreshold)
            {
                defendDirection = Direction.LeftRight;
            }
            else if(TouchInputManager.Instance.line1Angle < 45 + angleDirectionThreshold &&
                TouchInputManager.Instance.line1Angle > 45 - angleDirectionThreshold)
            {
                defendDirection = Direction.DiagonalRight;
            }
            else if (TouchInputManager.Instance.line1Angle < 225 + angleDirectionThreshold &&
                TouchInputManager.Instance.line1Angle > 225 - angleDirectionThreshold)
            {
                defendDirection = Direction.DiagonalRight;
            }
            else if (TouchInputManager.Instance.line1Angle < 135 + angleDirectionThreshold &&
                TouchInputManager.Instance.line1Angle > 135 - angleDirectionThreshold)
            {
                defendDirection = Direction.DiagonalLeft;
            }
            else if (TouchInputManager.Instance.line1Angle < 315 + angleDirectionThreshold &&
                TouchInputManager.Instance.line1Angle > 315 - angleDirectionThreshold)
            {
                defendDirection = Direction.DiagonalLeft;
            }
        }
        else if (player == Player.Player_2)
        {
            if (TouchInputManager.Instance.line2Angle <= 180 + angleDirectionThreshold &&
                TouchInputManager.Instance.line2Angle >= 180 - angleDirectionThreshold)
            {
                defendDirection = Direction.UpDown;
            }
            else if (TouchInputManager.Instance.line2Angle <= 0 + angleDirectionThreshold ||
                TouchInputManager.Instance.line2Angle >= 360 - angleDirectionThreshold)
            {
                defendDirection = Direction.UpDown;
            }
            else if (TouchInputManager.Instance.line2Angle < 90 + angleDirectionThreshold &&
                TouchInputManager.Instance.line2Angle > 90 - angleDirectionThreshold)
            {
                defendDirection = Direction.LeftRight;
            }
            else if (TouchInputManager.Instance.line2Angle < 270 + angleDirectionThreshold &&
                TouchInputManager.Instance.line2Angle > 270 - angleDirectionThreshold)
            {
                defendDirection = Direction.LeftRight;
            }
            else if (TouchInputManager.Instance.line2Angle < 45 + angleDirectionThreshold &&
                TouchInputManager.Instance.line2Angle > 45 - angleDirectionThreshold)
            {
                defendDirection = Direction.DiagonalRight;
            }
            else if (TouchInputManager.Instance.line2Angle < 225 + angleDirectionThreshold &&
                TouchInputManager.Instance.line2Angle > 225 - angleDirectionThreshold)
            {
                defendDirection = Direction.DiagonalRight;
            }
            else if (TouchInputManager.Instance.line2Angle < 135 + angleDirectionThreshold &&
                TouchInputManager.Instance.line2Angle > 135 - angleDirectionThreshold)
            {
                defendDirection = Direction.DiagonalLeft;
            }
            else if (TouchInputManager.Instance.line2Angle < 315 + angleDirectionThreshold &&
                TouchInputManager.Instance.line2Angle > 315 - angleDirectionThreshold)
            {
                defendDirection = Direction.DiagonalLeft;
            }
        }
    }

    void Attacked(Player player, Direction attackDir)
    {
        if(isDefend)
        {
            if(attackDir == defendDirection)
            {
                OnAttackBlocked(this.player);
            }
        }
        else
        {
            OnAttackReceived(this.player);
        }
    }
}
