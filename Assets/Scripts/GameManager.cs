using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using TetraCreations.Attributes;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI textSwipe_1;
    public TextMeshProUGUI textSwipe_2;

    public TextMeshProUGUI textTouchCount_1;
    public TextMeshProUGUI textTouchCount_2;

    public TextMeshProUGUI textLineAngle_1;
    public TextMeshProUGUI textLineAngle_2;

    public TextMeshProUGUI textDefend_1;
    public TextMeshProUGUI textDefend_2;

    public TextMeshProUGUI textAllTouchCount;

    public int step;
    [ReadOnly] public int currentStep;
    public float stepDistance;
    public float delay;

    public Transform playerParent;

    private void Awake()
    {
        Instance = this;
        TouchInputManager.OnSwipeGesture += SwipeHandler;
        PlayerController.OnAttackReceived += MovePlayerParent;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void MovePlayerParent(Player attackedPlayer)
    {
        if(attackedPlayer == Player.Player_1)
        {
            playerParent.DOMoveZ(playerParent.position.z + stepDistance, delay);
            currentStep++;
        }
        else if (attackedPlayer == Player.Player_2)
        {
            playerParent.DOMoveZ(playerParent.position.z - stepDistance, delay);
            currentStep--;
        }
    }

    void SwipeHandler(Player player, Direction dir)
    {
        if(player == Player.Player_1)
        {
            textSwipe_1.text = dir.ToString();
        }
        if (player == Player.Player_2)
        {
            textSwipe_2.text = dir.ToString();
        }
    }
}
