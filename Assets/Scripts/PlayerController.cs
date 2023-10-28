using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TetraCreations.Attributes;

public enum Player
{
    Player_1,
    Player_2,
}

public class PlayerController : MonoBehaviour
{
    public Player player;
    Animator anim;

    public bool isDefend;

    private void Awake()
    {
        TouchInputManager.OnSwipeGesture += CommenceAttack;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void CommenceAttack(Player player, SwipeDirection dir)
    {        
        if(player == this.player && !isDefend)
        {
            anim.SetTrigger("Attack");
            anim.SetFloat("Direction", Array.IndexOf(Enum.GetValues(typeof(SwipeDirection)), dir));
        }
    }
}
