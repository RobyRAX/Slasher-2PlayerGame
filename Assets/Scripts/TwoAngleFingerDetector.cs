using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoFingerAngleDetector : MonoBehaviour
{
    private Vector2 player1TouchStartPos = Vector2.zero;
    private Vector2 player2TouchStartPos = Vector2.zero;
    private float player1Angle = 0;
    private float player2Angle = 0;

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                // Check if the touch is within player 1's or player 2's area.
                bool isPlayer1Area = touch.position.y < Screen.height / 2;
                bool isPlayer2Area = !isPlayer1Area;

                if (isPlayer1Area)
                {
                    player1TouchStartPos = touch.position;
                }
                else if (isPlayer2Area)
                {
                    player2TouchStartPos = touch.position;
                }
            }

            if (touch.phase == TouchPhase.Moved)
            {
                // Calculate angle between two fingers.
                float angle = Mathf.Atan2(touch.position.y - player1TouchStartPos.y, touch.position.x - player1TouchStartPos.x);

                if (angle < 0)
                {
                    angle += 2 * Mathf.PI;
                }

                // Check if the touch is within player 1's or player 2's area.
                bool isPlayer1Area = touch.position.y < Screen.height / 2;
                bool isPlayer2Area = !isPlayer1Area;

                if (isPlayer1Area)
                {
                    player1Angle = angle;
                }
                else if (isPlayer2Area)
                {
                    player2Angle = angle;
                }
            }
        }
    }

    // Access the player 1's angle.
    public float GetPlayer1Angle()
    {
        return player1Angle;
    }

    // Access the player 2's angle.
    public float GetPlayer2Angle()
    {
        return player2Angle;
    }
}

