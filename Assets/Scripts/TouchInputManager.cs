using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TetraCreations.Attributes;

public enum SwipeDirection
{
    Up,
    Down,
    Right,
    Left,
    Diagonal,
}

public class TouchInputManager : MonoBehaviour
{
    #region
    public static event Action<Player, SwipeDirection> OnSwipeGesture;
    public static event Action<Player, bool> OnHoldGesture;
    #endregion

    private Vector2 player1TouchStartPos;
    private Vector2 player2TouchStartPos;
    private bool player1Swiped = false;
    private bool player2Swiped = false;
    private float player1SwipeAngle;
    private float player2SwipeAngle;

    private int player1FingerCount = 0;
    private int player2FingerCount = 0;
    private bool isPlayer1Holding = false;
    public bool IsPlayer1Holding
    {
        get { return isPlayer1Holding; }
        set
        {
            if (value == isPlayer1Holding)
                return;
            else
            {
                if (value == true)
                    OnHoldGesture(Player.Player_1, value);

                isPlayer1Holding = value;
                Debug.Log("CHANGED_1");
            }                
        }
    }
    private bool isPlayer2Holding = false;
    public bool IsPlayer2Holding
    {
        get { return isPlayer2Holding; }
        set
        {
            if (value == isPlayer2Holding)
                return;
            else
            {
                if (value == true)
                    OnHoldGesture(Player.Player_2, value);

                isPlayer2Holding = value;
                Debug.Log("CHANGED_2");
            }               
        }
    }

    public float minSwipeDistance = 50f; // Minimum distance to consider a swipe.
    [Range(0, 22.5f)] public float angleDirectionThreshold;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            player1FingerCount++;
        if(Input.GetKeyUp(KeyCode.LeftControl))
            player1FingerCount--;

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                // Check if the touch is within player 1's or player 2's area.
                bool isPlayer1Area = touch.position.y < Screen.height / 2;
                bool isPlayer2Area = !isPlayer1Area;

                if (isPlayer1Area)
                {
                    player1FingerCount++;
                }
                else if (isPlayer2Area)
                {
                    player2FingerCount++;
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                // Check if the touch was within player 1's or player 2's area.
                bool isPlayer1Area = touch.position.y < Screen.height / 2;
                bool isPlayer2Area = !isPlayer1Area;

                if (isPlayer1Area)
                {
                    player1FingerCount--;
                }
                else if (isPlayer2Area)
                {
                    player2FingerCount--;
                }
            }

            IsPlayer1Holding = player1FingerCount >= 2;
            IsPlayer2Holding = player2FingerCount >= 2;

            // Check for two-finger holds for each player.
            if (player1FingerCount >= 2)
            {
                // Player 1 is holding with two fingers in their area.
                Debug.Log("Player 1 is holding with two fingers.");
            }

            if (player2FingerCount >= 2)
            {
                // Player 2 is holding with two fingers in their area.
                Debug.Log("Player 2 is holding with two fingers.");
            }
        }

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                // Store the initial touch position for each player.
                if (touch.position.y < Screen.height / 2)
                {
                    player1TouchStartPos = touch.position;
                    player1Swiped = false;
                }
                else
                {
                    player2TouchStartPos = touch.position;
                    player2Swiped = false;
                }
            }
            else if (touch.phase == TouchPhase.Moved && !player1Swiped && !player2Swiped)
            {
                if (touch.position.y < Screen.height / 2)
                {
                    // This touch belongs to player 1 (bottom half).
                    Vector2 swipeDelta = touch.position - player1TouchStartPos;

                    if (swipeDelta.magnitude >= minSwipeDistance)
                    {
                        // Calculate the angle in radians.
                        float angleRadians = Mathf.Atan2(swipeDelta.y, swipeDelta.x);

                        // Convert the angle from radians to degrees.
                        player1SwipeAngle = (angleRadians * Mathf.Rad2Deg + 360) % 360;

                        // Handle the swipe for player 1.
                        player1Swiped = true;
                        Debug.Log(player1SwipeAngle);

                        if (player1SwipeAngle <= 90 + angleDirectionThreshold && player1SwipeAngle >= 90 - angleDirectionThreshold)
                        {
                            OnSwipeGesture(Player.Player_1, SwipeDirection.Up); // Up
                        }
                        else if (player1SwipeAngle <= 180 + angleDirectionThreshold && player1SwipeAngle >= 180 - angleDirectionThreshold)
                        {
                            OnSwipeGesture(Player.Player_1, SwipeDirection.Left); // Left
                        }
                        else if (player1SwipeAngle <= 270 + angleDirectionThreshold && player1SwipeAngle >= 270 - angleDirectionThreshold)
                        {
                            OnSwipeGesture(Player.Player_1, SwipeDirection.Down); // Down
                        }
                        else if (player1SwipeAngle <= 0 + angleDirectionThreshold || player1SwipeAngle >= 360 - angleDirectionThreshold)
                        {
                            OnSwipeGesture(Player.Player_1, SwipeDirection.Right); // Right
                        }
                        else
                            OnSwipeGesture(Player.Player_1, SwipeDirection.Diagonal);
                    }
                }
                else
                {
                    // This touch belongs to player 2 (top half).
                    Vector2 swipeDelta = touch.position - player2TouchStartPos;

                    if (swipeDelta.magnitude >= minSwipeDistance)
                    {
                        // Calculate the angle in radians.
                        float angleRadians = Mathf.Atan2(swipeDelta.y, swipeDelta.x);

                        // Convert the angle from radians to degrees.
                        player2SwipeAngle = (angleRadians * Mathf.Rad2Deg + 360) % 360;

                        // Handle the swipe for player 2.
                        player2Swiped = true;
                        Debug.Log(player1SwipeAngle);

                        if (player2SwipeAngle <= 90 + angleDirectionThreshold && player2SwipeAngle >= 90 - angleDirectionThreshold)
                        {
                            OnSwipeGesture(Player.Player_2, SwipeDirection.Up); // Up
                        }
                        else if (player2SwipeAngle <= 180 + angleDirectionThreshold && player2SwipeAngle >= 180 - angleDirectionThreshold)
                        {
                            OnSwipeGesture(Player.Player_2, SwipeDirection.Left); // Left
                        }
                        else if (player2SwipeAngle <= 270 + angleDirectionThreshold && player2SwipeAngle >= 270 - angleDirectionThreshold)
                        {
                            OnSwipeGesture(Player.Player_2, SwipeDirection.Down); // Down
                        }
                        else if (player2SwipeAngle <= 0 + angleDirectionThreshold || player2SwipeAngle >= 360 - angleDirectionThreshold)
                        {
                            OnSwipeGesture(Player.Player_2, SwipeDirection.Right); // Right
                        }
                        else
                            OnSwipeGesture(Player.Player_2, SwipeDirection.Diagonal);
                    }
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                // Reset the touch start positions when the touch ends, but only if a swipe was detected.
                if (player1Swiped)
                {
                    player1Swiped = false;
                    player1TouchStartPos = Vector2.zero;
                }
                if (player2Swiped)
                {
                    player2Swiped = false;
                    player2TouchStartPos = Vector2.zero;
                }
            }
        }
    }

    /*public static TouchInputManager Instance;
    public static event Action<SwipeDirection, Player> OnSwipe;

    private SwipeGestureRecognizer swipeGesture_Player_1;
    private SwipeGestureRecognizer swipeGesture_Player_2;

    public bool player1_Swipe;
    public bool player2_Swipe;

    [SerializeField, Range(0, 22.5f)] float angleDirectionThreshold;

    private void InitTouchInput()
    {
        swipeGesture_Player_1 = new SwipeGestureRecognizer();
        swipeGesture_Player_1.Direction = SwipeGestureRecognizerDirection.Any;
        swipeGesture_Player_1.StateUpdated += SwipeGestureCallback;
        swipeGesture_Player_1.DirectionThreshold = 1.0f; // allow a swipe, regardless of slope
        FingersScript.Instance.AddGesture(swipeGesture_Player_1);

        swipeGesture_Player_2 = new SwipeGestureRecognizer();
        swipeGesture_Player_2.Direction = SwipeGestureRecognizerDirection.Any;
        swipeGesture_Player_2.StateUpdated += SwipeGestureCallback;
        swipeGesture_Player_2.DirectionThreshold = 1.0f; // allow a swipe, regardless of slope
        FingersScript.Instance.AddGesture(swipeGesture_Player_2);
    }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitTouchInput();
    }

    private void SwipeGestureCallback(GestureRecognizer gesture)
    {
        if(gesture.State == GestureRecognizerState.Possible)
        {
            Vector2 startPoint = new Vector2(gesture.StartFocusX, gesture.StartFocusY);           
            if(startPoint.y < Screen.height/2 && startPoint.y > 0)
            {
                SwipeHandler(gesture, Player.Player_1);
                player1_Swipe = true;
            }
            else if(startPoint.y > Screen.height / 2 && startPoint.y < Screen.height)
            {
                SwipeHandler(gesture, Player.Player_2);
                player2_Swipe = true;
            }
        }

        if (gesture.State == GestureRecognizerState.Ended)
        {
            Vector2 endPoint = new Vector2(gesture.FocusX, gesture.FocusY);
            if (endPoint.y < Screen.height / 2 && endPoint.y > 0)
            {
                SwipeHandler(gesture, Player.Player_1);
                player1_Swipe = false;
            }
            else if (endPoint.y > Screen.height / 2 && endPoint.y < Screen.height)
            {
                SwipeHandler(gesture, Player.Player_2);
                player2_Swipe = false;
            }
        }
    }


    void SwipeHandler(GestureRecognizer gesture, Player player)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            Vector2 velocityVector = new Vector2(gesture.VelocityX, gesture.VelocityY);
            float angle = Mathf.Atan2(velocityVector.y, velocityVector.x) * Mathf.Rad2Deg;
            if (angle < 0)
            {
                angle += 360;
            }

            Debug.Log(angle);

            if (angle <= 90 + angleDirectionThreshold && angle >= 90 - angleDirectionThreshold)
            {
                OnSwipe(SwipeDirection.Up, player); // Up
            }
            else if (angle <= 180 + angleDirectionThreshold && angle >= 180 - angleDirectionThreshold)
            {
                OnSwipe(SwipeDirection.Left, player); // Left
            }
            else if (angle <= 270 + angleDirectionThreshold && angle >= 270 - angleDirectionThreshold)
            {
                OnSwipe(SwipeDirection.Down, player); // Down
            }
            else if (angle <= 0 + angleDirectionThreshold || angle >= 360 - angleDirectionThreshold)
            {
                OnSwipe(SwipeDirection.Right, player); // Right
            }
            else
                OnSwipe(SwipeDirection.Diagonal, player);

            Debug.DrawLine(new Vector3(gesture.StartFocusX, gesture.StartFocusY, 0), new Vector3(gesture.FocusX, gesture.FocusY, 0));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
