using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using TetraCreations.Attributes;

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

    private void Awake()
    {
        Instance = this;
        TouchInputManager.OnSwipeGesture += SwipeHandler;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SwipeHandler(Player player, SwipeDirection dir)
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
