using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    #region AHMET OMAK
    [Header("Player State Values")]
    public List<float> playerSpeedValues;
    public List<float> swipeSensStates;
    #endregion

    [Header("Debug Values")]
    public float currentSwipeSens = 100f;
    public float currentForwardSpeed = 4f;
    public float leftLimit = -2f;
    public float rightLimit = 2f;
    public bool canDoMovement;
    public bool canMoveSideways;

    private float swipeSensitivity;

    private Vector3 targetPos;

    private bool lockLeft;
    private bool lockRight;

    private bool isGameStarted;

    private PlayerController player;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
    }

    private void Start()
    {
        TouchManager.instance.onTouchBegan += TouchBegan;
        TouchManager.instance.onTouchMoved += TouchMoved;
        TouchManager.instance.onTouchEnded += TouchEnded;
    }

    private void Update()
    {
        StartGame();
        Movement();
    }

    public void ChangePlayerSpeed(int playerStateIndex)
    {
        currentForwardSpeed = playerSpeedValues[playerStateIndex];
    }

    public void ChangePlayerSwipeSens(int swipeSensStateIndex)
    {
        currentSwipeSens = swipeSensStates[swipeSensStateIndex];
    }

    public void StartGame()
    {
        if (!isGameStarted && Input.GetMouseButtonDown(0))
        {
            isGameStarted = true;
            canDoMovement = true;
            canMoveSideways = true;
            player.SetAnimationToRunState();
        }
    }

    private void TouchBegan(TouchInput touch)
    {
        targetPos = transform.position;
    }

    private void TouchEnded(TouchInput touch)
    {
        targetPos = transform.position;
        swipeSensitivity = 0f;
    }

    private void TouchMoved(TouchInput touch)
    {
        if (!canMoveSideways)
        {
            targetPos = new Vector3(0f, targetPos.y, targetPos.z);
            return;
        }

        swipeSensitivity = Mathf.Abs(touch.DeltaScreenPosition.x);

        if (swipeSensitivity > currentSwipeSens)
        {
            swipeSensitivity = currentSwipeSens;
        }

        if (touch.DeltaScreenPosition.x > 0)
        {
            if (rightLimit < transform.position.x - (swipeSensitivity / 1000f))
            {
                targetPos = new Vector3(transform.position.x + (swipeSensitivity / 1000f), transform.position.y, transform.position.z);
            }
            else
            {
                targetPos = new Vector3(rightLimit, transform.position.y, transform.position.z);
            }

        }
        else if (touch.DeltaScreenPosition.x < 0)
        {
            if (leftLimit > transform.position.x + (swipeSensitivity / 1000f))
            {
                targetPos = new Vector3(transform.position.x + (((swipeSensitivity) / -1000f)), transform.position.y, transform.position.z);
            }
            else
            {
                targetPos = new Vector3(leftLimit, transform.position.y, transform.position.z);
            }

        }
        else
        {
            targetPos = transform.position;
        }
    }

    private void Movement()
    {
        if (!canDoMovement)
        {
            return;
        }

        if (isGameStarted)
        {
            if ((transform.position.x - targetPos.x < 0 && !lockRight) || (transform.position.x - targetPos.x > 0 && !lockLeft))
            {
                if (Time.timeScale >= 0.5f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * swipeSensitivity / 2f);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.unscaledDeltaTime * swipeSensitivity / 2f);
                }
            }

            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1f), Time.deltaTime * currentForwardSpeed);
        }
    }
}
