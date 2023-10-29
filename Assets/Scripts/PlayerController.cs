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
    public static event Action<Player, SwipeDirection> OnAttack;

    public Player player;
    Animator anim;

    public bool isDefend;
    public HoldDirection defendDirection;
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
            if(TouchInputManager.Instance.line1Angle < 180 + angleDirectionThreshold &&
                TouchInputManager.Instance.line1Angle > 180 - angleDirectionThreshold)
            {
                defendDirection = HoldDirection.UpDown;
            }
            else if (TouchInputManager.Instance.line1Angle < 90 + angleDirectionThreshold &&
                TouchInputManager.Instance.line1Angle > 90 - angleDirectionThreshold)
            {
                defendDirection = HoldDirection.LeftRight;
            }


            defendText.text = defendDirection.ToString();
        }
    }

    void CommenceAttack(Player player, SwipeDirection dir)
    {        
        if(player == this.player && !isDefend)
        {
            anim.SetTrigger("Attack");
            anim.SetFloat("Direction", Array.IndexOf(Enum.GetValues(typeof(SwipeDirection)), dir));

            OnAttack(player, dir);
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

    void Attacked(Player player, SwipeDirection dir)
    {
        
    }
}
