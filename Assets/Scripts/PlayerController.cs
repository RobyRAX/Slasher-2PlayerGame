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
    #region
    public static event Action<Player, Direction> OnAttack;
    public static event Action<Player> OnAttackReceived;
    public static event Action<Player> OnAttackBlocked;
    #endregion

    public Player player;
    public Animator anim;

    public GameObject stunEffect;
    [SerializeField] private bool isDefend;
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

    [Title("Particle")]
    public ParticleSystem UpDownFX;
    public ParticleSystem Right2LeftFX;
    public ParticleSystem Left2RightFX;
    public ParticleSystem DiagonalRightFX;
    public ParticleSystem DiagonalLeftFX;
    public ParticleSystem BlockFX;

    bool canAttack = true;
    bool stunned = false;
    public bool Stunned
    {
        get
        {
            return stunned;
        }
        set
        {
            stunned = value;
            stunEffect.SetActive(stunned);
        }
    }

    private void Awake()
    {
        TouchInputManager.OnSwipeGesture += CommenceAttack;
        TouchInputManager.OnHoldGesture += CommenceDefend;
        PlayerController.OnAttack += Attacked;
        PlayerController.OnAttackBlocked += Blocked;
        GameManager.OnGameOver += Lose;
        GameManager.OnGameStateChange += GameStateChangedHandler;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (GameManager.Instance.currentState != GameState.GameOn)
            return;

        if(IsDefend)
        {
            UpdateDefend();
            defendText.text = defendDirection.ToString();
        }
    }

    void GameStateChangedHandler(GameState gameState)
    {
        if(gameState == GameState.InitGame)
        {
            anim.Rebind();
            Stunned = false;
            canAttack = true;
            IsDefend = false;
        }
    }

    void CommenceAttack(Player attacker, Direction attackDir)
    {        
        if(attacker == this.player && !IsDefend && canAttack && !Stunned)
        {
            anim.SetTrigger("Attack");
            anim.SetFloat("Direction", Array.IndexOf(Enum.GetValues(typeof(Direction)), attackDir));

            canAttack = false;
        }
    }

    void CommenceDefend(Player self, bool isHold)
    {
        if(self == this.player && !Stunned)
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
                defendDirection = Direction.Right;
            }
            else if (TouchInputManager.Instance.line1Angle < 270 + angleDirectionThreshold &&
                TouchInputManager.Instance.line1Angle > 270 - angleDirectionThreshold)
            {
                defendDirection = Direction.Left;
            }
            else if(TouchInputManager.Instance.line1Angle < 45 + angleDirectionThreshold &&
                TouchInputManager.Instance.line1Angle > 45 - angleDirectionThreshold)
            {
                defendDirection = Direction.DiagonalLeft;
            }
            else if (TouchInputManager.Instance.line1Angle < 225 + angleDirectionThreshold &&
                TouchInputManager.Instance.line1Angle > 225 - angleDirectionThreshold)
            {
                defendDirection = Direction.DiagonalLeft;
            }
            else if (TouchInputManager.Instance.line1Angle < 135 + angleDirectionThreshold &&
                TouchInputManager.Instance.line1Angle > 135 - angleDirectionThreshold)
            {
                defendDirection = Direction.DiagonalRight;
            }
            else if (TouchInputManager.Instance.line1Angle < 315 + angleDirectionThreshold &&
                TouchInputManager.Instance.line1Angle > 315 - angleDirectionThreshold)
            {
                defendDirection = Direction.DiagonalRight;
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
                defendDirection = Direction.Right;
            }
            else if (TouchInputManager.Instance.line2Angle < 270 + angleDirectionThreshold &&
                TouchInputManager.Instance.line2Angle > 270 - angleDirectionThreshold)
            {
                defendDirection = Direction.Left;
            }
            else if (TouchInputManager.Instance.line2Angle < 45 + angleDirectionThreshold &&
                TouchInputManager.Instance.line2Angle > 45 - angleDirectionThreshold)
            {
                defendDirection = Direction.DiagonalLeft;
            }
            else if (TouchInputManager.Instance.line2Angle < 225 + angleDirectionThreshold &&
                TouchInputManager.Instance.line2Angle > 225 - angleDirectionThreshold)
            {
                defendDirection = Direction.DiagonalLeft;
            }
            else if (TouchInputManager.Instance.line2Angle < 135 + angleDirectionThreshold &&
                TouchInputManager.Instance.line2Angle > 135 - angleDirectionThreshold)
            {
                defendDirection = Direction.DiagonalRight;
            }
            else if (TouchInputManager.Instance.line2Angle < 315 + angleDirectionThreshold &&
                TouchInputManager.Instance.line2Angle > 315 - angleDirectionThreshold)
            {
                defendDirection = Direction.DiagonalRight;
            }
        }
    }

    void AttackVertical()
    {
        OnAttack(player, Direction.UpDown);
        UpDownFX.Play();
    }
    void AttackRightLeft()
    {
        OnAttack(player, Direction.Right);
        Right2LeftFX.Play();
    }
    void AttackLeftRight()
    {
        OnAttack(player, Direction.Left);
        Left2RightFX.Play();
    }
    void AttackDiagonalLeft()
    {
        OnAttack(player, Direction.DiagonalLeft);
        DiagonalLeftFX.Play();
    }
    void AttackDiagonalRight()
    {
        OnAttack(player, Direction.DiagonalRight);
        DiagonalRightFX.Play();
    }

    void ResetAttack()
    {
        canAttack = true;
    }

    void Attacked(Player attacker, Direction attackDir)
    {
        if(attacker != this.player)
        {
            if (isDefend)
            {
                if (attackDir == Direction.UpDown)
                {
                    if(defendDirection == Direction.UpDown)
                    {
                        OnAttackBlocked(attacker);
                        BlockFX.Play();
                    }
                    else
                    {
                        OnAttackReceived(this.player);
                        anim.SetTrigger("Hit");
                    }
                }
                else if(attackDir == Direction.Right)
                {
                    if(defendDirection == Direction.Right || defendDirection == Direction.Left)
                    {
                        OnAttackBlocked(attacker);
                        BlockFX.Play();
                    }
                    else
                    {
                        OnAttackReceived(this.player);
                        anim.SetTrigger("Hit");
                    }
                }
                else if (attackDir == Direction.Left)
                {
                    if (defendDirection == Direction.Right || defendDirection == Direction.Left)
                    {
                        OnAttackBlocked(attacker);
                        BlockFX.Play();
                    }
                    else
                    {
                        OnAttackReceived(this.player);
                        anim.SetTrigger("Hit");
                    }
                }
                else if(attackDir == Direction.DiagonalLeft)
                {
                    if (defendDirection == Direction.DiagonalLeft)
                    {
                        OnAttackBlocked(attacker);
                        BlockFX.Play();
                    }
                    else
                    {
                        OnAttackReceived(this.player);
                        anim.SetTrigger("Hit");
                    }
                }
                else if (attackDir == Direction.DiagonalRight)
                {
                    if (defendDirection == Direction.DiagonalRight)
                    {
                        OnAttackBlocked(attacker);
                        BlockFX.Play();
                    }
                    else
                    {
                        OnAttackReceived(this.player);
                        anim.SetTrigger("Hit");
                    }
                }                                  
            }
            else
            {
                OnAttackReceived(this.player);
                anim.SetTrigger("Hit");
            }
        }        
    }

    void Blocked(Player attacker)
    {
        if(attacker == this.player)
        {
            anim.SetTrigger("Blocked");
            StartCoroutine(BlockedCo());
        }
    }

    IEnumerator BlockedCo()
    {
        Stunned = true;
        yield return new WaitForSeconds(1.5f);
        Stunned = false;
    }

    void ResetStun()
    {
        Stunned = false;
    }

    void Lose(Player loser)
    {
        if(loser == this.player)
        {
            anim.SetTrigger("Lose");
        }
    }
}
