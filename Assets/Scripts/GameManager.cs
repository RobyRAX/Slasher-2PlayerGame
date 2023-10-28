using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using TetraCreations.Attributes;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI text_1;
    public TextMeshProUGUI text_2;

    public int step;
    [ReadOnly] public int currentStep;

    private void Awake()
    {
        //TouchInputManager.OnSwipe += SwipeHandler;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SwipeHandler(SwipeDirection dir, Player player)
    {
        
    }
}
