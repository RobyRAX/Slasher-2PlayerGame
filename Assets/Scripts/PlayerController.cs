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

    [ReadOnly] private bool isDefend;
    public bool IsDefend
    {
        get { return isDefend; }
        set 
        {
            if (value == isDefend)
                return;
            else
            {
                isDefend = value;
                anim.SetBool("IsDefend", isDefend);
            }
        }
    }
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
        if(IsDefend)
        {
            UpdateDefend();
            defendText.text = defendDirection.ToString();
        }
    }

    void CommenceAttack(Player attacker, Direction attackDir)
    {        
        if(attacker == this.player && !IsDefend)
        {
            anim.SetTrigger("Attack");
            anim.SetFloat("Direction", Array.IndexOf(Enum.GetValues(typeof(Direction)), attackDir));            
        }
    }

    void CommenceDefend(Player defender, bool isHold)
    {
        if(defender == this.player)
        {
            IsDefend = isHold;            
        }
        if(this.player == Player.Player_1)
        {
            GameManager.Instance.textDefend_1.text = IsDefend ? "Defend_On" : "Defend_Off";
        }
        else if (this.player == Player.Player_2)
        {
            GameManager.Instance.textDefend_2.text = IsDefend ? "Defend_On" : "Defend_Off";
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

    void AttackVertical()
    {
        OnAttack(player, Direction.UpDown);
    }
    void AttackHorizontal()
    {
        OnAttack(player, Direction.LeftRight);
    }
    void AttackDiagonalLeft()
    {
        OnAttack(player, Direction.DiagonalLeft);
    }
    void AttackDiagonalRight()
    {
        OnAttack(player, Direction.DiagonalRight);
    }

    void Attacked(Player victim, Direction attackDir)
    {
        if(victim == this.player)
        {
            if (IsDefend)
            {
                if (attackDir == defendDirection)
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
}
