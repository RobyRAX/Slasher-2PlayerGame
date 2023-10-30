using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TetraCreations.Attributes;
using RotaryHeart.Lib.SerializableDictionary;

public enum Direction
{
    UpDown,
    Right,
    Left,   
    DiagonalLeft,
    DiagonalRight,
}

public class TouchInputManager : MonoBehaviour
{
    #region
    public static event Action<Player, Direction> OnSwipeGesture;
    public static event Action<Player, bool> OnHoldGesture;

    public static TouchInputManager Instance; 
    #endregion

    private Vector2 player1TouchStartPos;
    private Vector2 player2TouchStartPos;
    private bool player1Swiped = false;
    private bool player2Swiped = false;
    private float player1SwipeAngle;
    private float player2SwipeAngle;

    private Dictionary<int, Touch> player1Touches = new Dictionary<int, Touch>();
    private Dictionary<int, Touch> player2Touches = new Dictionary<int, Touch>();
    private Dictionary<int, Touch> activeTouches = new Dictionary<int, Touch>();   

    private int player1TouchCount;
    public int Player1TouchCount
    {
        get { return player1TouchCount; }
        set
        {
            if (value > 2 || value < 0)
                return;
            else
            {
                player1TouchCount = value;
                GameManager.Instance.textTouchCount_1.text = player1TouchCount.ToString();
            }
        }
    }
    private int player2TouchCount;
    public int Player2TouchCount
    {
        get { return player2TouchCount; }
        set
        {
            if (value > 2 || value < 0)
                return;
            else
            {
                player2TouchCount = value;
                GameManager.Instance.textTouchCount_2.text = player2TouchCount.ToString();
            }
        }
    }

    private Vector3[] touchPositions = new Vector3[2];

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
                OnHoldGesture(Player.Player_1, value);

                isPlayer1Holding = value;
                //Debug.Log("CHANGED_1");
                player1FingerLine.enabled = value;
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
                OnHoldGesture(Player.Player_2, value);

                isPlayer2Holding = value;
                player2FingerLine.enabled = value;
            }               
        }
    }

    public float minSwipeDistance = 50f; // Minimum distance to consider a swipe.
    [Range(0, 22.5f)] public float angleDirectionThreshold;

    public LineRenderer player1FingerLine; // Reference to the line renderer for player 1.
    public LineRenderer player2FingerLine; // Reference to the line renderer for player 2.
    Camera mainCamera;

    public float line1Angle;
    public float line2Angle;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Ensure the line renderers are initialized properly.
        player1FingerLine.enabled = false;
        player2FingerLine.enabled = false;

        mainCamera = Camera.main;
    }

    void Update()
    {
        if (GameManager.Instance.currentState != GameState.GameOn)
            return;

        GameManager.Instance.textAllTouchCount.text = Input.touches.Length.ToString();

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                // Check if the touch is within player 1's or player 2's area.
                bool isPlayer1Area = touch.position.y < Screen.height / 2;
                bool isPlayer2Area = !isPlayer1Area;

                if (isPlayer1Area)
                {
                    player1Touches[touch.fingerId] = touch;
                    Vector3 linePosition = mainCamera.ScreenToWorldPoint(new Vector3(player1Touches[touch.fingerId].position.x,
                                                                                player1Touches[touch.fingerId].position.y,
                                                                                mainCamera.nearClipPlane + 0.2f));
                    player1FingerLine.SetPosition(touch.fingerId, linePosition);

                    player1TouchStartPos = touch.position;
                    player1Swiped = false;
                }
                else if (isPlayer2Area)
                {
                    player2Touches[touch.fingerId] = touch;
                    Vector3 linePosition = mainCamera.ScreenToWorldPoint(new Vector3(player2Touches[touch.fingerId].position.x,
                                                                                player2Touches[touch.fingerId].position.y,
                                                                                mainCamera.nearClipPlane + 0.2f));
                    player2FingerLine.SetPosition(touch.fingerId, linePosition);

                    player2TouchStartPos = touch.position;
                    player2Swiped = false;
                }
            }

            if(touch.phase == TouchPhase.Moved)
            {
                if(player1Touches.ContainsKey(touch.fingerId))
                {
                    #region SET_LINE
                    player1Touches[touch.fingerId] = touch;
                    Vector3 linePosition = mainCamera.ScreenToWorldPoint(new Vector3(player1Touches[touch.fingerId].position.x,
                                                                                player1Touches[touch.fingerId].position.y,
                                                                                mainCamera.nearClipPlane + 0.2f));
                    player1FingerLine.SetPosition(touch.fingerId, linePosition);
                    #endregion

                    if (touch.position.y < Screen.height / 2 && !player1Swiped)
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
                            //Debug.Log(player1SwipeAngle);

                            if (player1SwipeAngle <= 90 + angleDirectionThreshold &&
                                player1SwipeAngle >= 90 - angleDirectionThreshold)
                            {
                                OnSwipeGesture(Player.Player_1, Direction.UpDown); // Up
                            }
                            else if (player1SwipeAngle <= 270 + angleDirectionThreshold &&
                                player1SwipeAngle >= 270 - angleDirectionThreshold)
                            {
                                OnSwipeGesture(Player.Player_1, Direction.UpDown); // Down
                            }
                            else if (player1SwipeAngle <= 180 + angleDirectionThreshold &&
                                player1SwipeAngle >= 180 - angleDirectionThreshold)
                            {
                                OnSwipeGesture(Player.Player_1, Direction.Right); // Left
                            }
                            else if (player1SwipeAngle <= 0 + angleDirectionThreshold ||
                                player1SwipeAngle >= 360 - angleDirectionThreshold)
                            {
                                OnSwipeGesture(Player.Player_1, Direction.Left); // Right
                            }
                            else if (player1SwipeAngle <= 45 + angleDirectionThreshold &&
                                player1SwipeAngle >= 45 - angleDirectionThreshold)
                            {
                                OnSwipeGesture(Player.Player_1, Direction.DiagonalRight); //Diagonal - Right
                            }
                            else if (player1SwipeAngle <= 225 + angleDirectionThreshold &&
                                player1SwipeAngle >= 225 - angleDirectionThreshold)
                            {
                                OnSwipeGesture(Player.Player_1, Direction.DiagonalRight); //Diagonal - Right
                            }
                            else if (player1SwipeAngle <= 135 + angleDirectionThreshold &&
                                player1SwipeAngle >= 135 - angleDirectionThreshold)
                            {
                                OnSwipeGesture(Player.Player_1, Direction.DiagonalLeft); //Diagonal - Left
                            }
                            else if (player1SwipeAngle <= 315 + angleDirectionThreshold &&
                                player1SwipeAngle >= 315 - angleDirectionThreshold)
                            {
                                OnSwipeGesture(Player.Player_1, Direction.DiagonalLeft); //Diagonal - Left
                            }
                        }
                    }
                }
                else
                {
                    
                }
                if (player2Touches.ContainsKey(touch.fingerId))
                {
                    #region SET_LINE
                    player2Touches[touch.fingerId] = touch;
                    Vector3 linePosition = mainCamera.ScreenToWorldPoint(new Vector3(player2Touches[touch.fingerId].position.x,
                                                                                player2Touches[touch.fingerId].position.y,
                                                                                mainCamera.nearClipPlane + 0.2f));
                    player2FingerLine.SetPosition(touch.fingerId, linePosition);
                    #endregion

                    if (touch.position.y > Screen.height / 2 && !player2Swiped)
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
                            //Debug.Log(player1SwipeAngle);

                            if (player2SwipeAngle <= 90 + angleDirectionThreshold &&
                                player2SwipeAngle >= 90 - angleDirectionThreshold)
                            {
                                OnSwipeGesture(Player.Player_2, Direction.UpDown); // Up
                            }
                            else if (player2SwipeAngle <= 270 + angleDirectionThreshold &&
                                player2SwipeAngle >= 270 - angleDirectionThreshold)
                            {
                                OnSwipeGesture(Player.Player_2, Direction.UpDown); // Down
                            }
                            else if (player2SwipeAngle <= 180 + angleDirectionThreshold &&
                                player2SwipeAngle >= 180 - angleDirectionThreshold)
                            {
                                OnSwipeGesture(Player.Player_2, Direction.Left); // Left
                            }
                            else if (player2SwipeAngle <= 0 + angleDirectionThreshold ||
                                player2SwipeAngle >= 360 - angleDirectionThreshold)
                            {
                                OnSwipeGesture(Player.Player_2, Direction.Right); // Right
                            }
                            else if (player2SwipeAngle <= 45 + angleDirectionThreshold &&
                                player2SwipeAngle >= 45 - angleDirectionThreshold)
                            {
                                OnSwipeGesture(Player.Player_2, Direction.DiagonalRight); //Diagonal - Right
                            }
                            else if (player2SwipeAngle <= 225 + angleDirectionThreshold &&
                                player2SwipeAngle >= 225 - angleDirectionThreshold)
                            {
                                OnSwipeGesture(Player.Player_2, Direction.DiagonalRight); //Diagonal - Right
                            }
                            else if (player2SwipeAngle <= 135 + angleDirectionThreshold &&
                                player2SwipeAngle >= 135 - angleDirectionThreshold)
                            {
                                OnSwipeGesture(Player.Player_2, Direction.DiagonalLeft); //Diagonal - Left
                            }
                            else if (player2SwipeAngle <= 315 + angleDirectionThreshold &&
                                player2SwipeAngle >= 315 - angleDirectionThreshold)
                            {
                                OnSwipeGesture(Player.Player_2, Direction.DiagonalLeft); //Diagonal - Left
                            }
                        }
                    }
                }
                else
                {
                    
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                try
                {
                    player1Touches.Remove(touch.fingerId);
                    player2Touches.Remove(touch.fingerId);

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
                catch(Exception e)
                {
                    Debug.Log(e);
                }                                
            }          
        }
        /*Player1TouchCount = player1Touches.Count;
        Player2TouchCount = player2Touches.Count;*/

        IsPlayer1Holding = player1Touches.Count >= 2;
        IsPlayer2Holding = player2Touches.Count >= 2;

        GameManager.Instance.textTouchCount_1.text = player1Touches.Count.ToString();
        GameManager.Instance.textTouchCount_2.text = player2Touches.Count.ToString();

        Vector3 line1Direction = player1FingerLine.GetPosition(1) - player1FingerLine.GetPosition(0);
        line1Angle = Mathf.Atan2(line1Direction.z, line1Direction.x) * Mathf.Rad2Deg;
        // Ensure the angle is between 0 and 360 degrees
        if (line1Angle < 0)
        {
            line1Angle += 360;
        }
        //GameManager.Instance.textLineAngle_1.text = line1Angle.ToString();

        Vector3 line2Direction = player2FingerLine.GetPosition(1) - player2FingerLine.GetPosition(0);
        line2Angle = Mathf.Atan2(line2Direction.z, line2Direction.x) * Mathf.Rad2Deg;
        // Ensure the angle is between 0 and 360 degrees
        if (line2Angle < 0)
        {
            line2Angle += 360;
        }       
        //GameManager.Instance.textLineAngle_2.text = line2Angle.ToString();

        //SWIPE
        /*foreach (Touch touch in Input.touches)
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
                        //Debug.Log(player1SwipeAngle);

                        if (player1SwipeAngle <= 90 + angleDirectionThreshold && 
                            player1SwipeAngle >= 90 - angleDirectionThreshold)
                        {
                            OnSwipeGesture(Player.Player_1, Direction.UpDown); // Up
                        }
                        else if (player1SwipeAngle <= 270 + angleDirectionThreshold &&
                            player1SwipeAngle >= 270 - angleDirectionThreshold)
                        {
                            OnSwipeGesture(Player.Player_1, Direction.UpDown); // Down
                        }
                        else if (player1SwipeAngle <= 180 + angleDirectionThreshold && 
                            player1SwipeAngle >= 180 - angleDirectionThreshold)
                        {
                            OnSwipeGesture(Player.Player_1, Direction.Right); // Left
                        }
                        else if (player1SwipeAngle <= 0 + angleDirectionThreshold ||
                            player1SwipeAngle >= 360 - angleDirectionThreshold)
                        {
                            OnSwipeGesture(Player.Player_1, Direction.Left); // Right
                        }                        
                        else if(player1SwipeAngle <= 45 + angleDirectionThreshold && 
                            player1SwipeAngle >= 45 - angleDirectionThreshold)
                        {
                            OnSwipeGesture(Player.Player_1, Direction.DiagonalRight); //Diagonal - Right
                        }                           
                        else if (player1SwipeAngle <= 225 + angleDirectionThreshold && 
                            player1SwipeAngle >= 225 - angleDirectionThreshold)
                        {
                            OnSwipeGesture(Player.Player_1, Direction.DiagonalRight); //Diagonal - Right
                        }
                        else if (player1SwipeAngle <= 135 + angleDirectionThreshold &&
                            player1SwipeAngle >= 135 - angleDirectionThreshold)
                        {
                            OnSwipeGesture(Player.Player_1, Direction.DiagonalLeft); //Diagonal - Left
                        }
                        else if (player1SwipeAngle <= 315 + angleDirectionThreshold &&
                            player1SwipeAngle >= 315 - angleDirectionThreshold)
                        {
                            OnSwipeGesture(Player.Player_1, Direction.DiagonalLeft); //Diagonal - Left
                        }
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
                        //Debug.Log(player1SwipeAngle);

                        if (player2SwipeAngle <= 90 + angleDirectionThreshold &&
                            player2SwipeAngle >= 90 - angleDirectionThreshold)
                        {
                            OnSwipeGesture(Player.Player_2, Direction.UpDown); // Up
                        }
                        else if (player2SwipeAngle <= 270 + angleDirectionThreshold &&
                            player2SwipeAngle >= 270 - angleDirectionThreshold)
                        {
                            OnSwipeGesture(Player.Player_2, Direction.UpDown); // Down
                        }
                        else if (player2SwipeAngle <= 180 + angleDirectionThreshold &&
                            player2SwipeAngle >= 180 - angleDirectionThreshold)
                        {
                            OnSwipeGesture(Player.Player_2, Direction.Left); // Left
                        }
                        else if (player2SwipeAngle <= 0 + angleDirectionThreshold ||
                            player2SwipeAngle >= 360 - angleDirectionThreshold)
                        {
                            OnSwipeGesture(Player.Player_2, Direction.Right); // Right
                        }
                        else if (player2SwipeAngle <= 45 + angleDirectionThreshold &&
                            player2SwipeAngle >= 45 - angleDirectionThreshold)
                        {
                            OnSwipeGesture(Player.Player_2, Direction.DiagonalRight); //Diagonal - Right
                        }
                        else if (player2SwipeAngle <= 225 + angleDirectionThreshold &&
                            player2SwipeAngle >= 225 - angleDirectionThreshold)
                        {
                            OnSwipeGesture(Player.Player_2, Direction.DiagonalRight); //Diagonal - Right
                        }
                        else if (player2SwipeAngle <= 135 + angleDirectionThreshold &&
                            player2SwipeAngle >= 135 - angleDirectionThreshold)
                        {
                            OnSwipeGesture(Player.Player_2, Direction.DiagonalLeft); //Diagonal - Left
                        }
                        else if (player2SwipeAngle <= 315 + angleDirectionThreshold &&
                            player2SwipeAngle >= 315 - angleDirectionThreshold)
                        {
                            OnSwipeGesture(Player.Player_2, Direction.DiagonalLeft); //Diagonal - Left
                        }
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
        }*/
    }
}
