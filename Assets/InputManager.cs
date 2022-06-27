using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public UnityEvent<Vector3> inputMoveEvent;
    public UnityEvent<Vector3> inputMouseEvent;
    public UnityEvent inputShiftDownEvent;
    public UnityEvent inputShiftUpEvent;
    public UnityEvent inputSpaceEvent;
    public UnityEvent inputMouseLeftDownEvent;
    public UnityEvent inputMouseLeftStayEvent;
    public UnityEvent inputMouseLeftUpEvent;
    public UnityEvent inputMouseRightDownEvent;
    public UnityEvent inputMouseRightStayEvent;
    public UnityEvent inputMouseRightUpEvent;
    public UnityEvent inputCEvent;
    public UnityEvent inputREvent;
    public UnityEvent inputTEvent;
    public UnityEvent inputEEvent;
    public UnityEvent inputVEvent;
    public UnityEvent inputEscEvent;
    public UnityEvent inputQEvent;
    
    bool isRun;
    Animator animator;
    public float lerpedAnimation;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        RecieveMouse();
        RecieveMouseClick();
        RecieveMove();
        RecieveShift();
        RecieveSpaceBar();
        RecieveC();
        RecieveR();
        RecieveT();
        RecieveE();
        RecieveV();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print("esc");
            inputEscEvent?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Q))
            inputQEvent?.Invoke();
    }

    public void RecieveMove()
    {
        Vector3 unNormalizedDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        animator.SetFloat("forward", Mathf.Lerp(unNormalizedDir.z * (isRun ? 2 : 1), animator.GetFloat("forward"),lerpedAnimation));
        animator.SetFloat("right", Mathf.Lerp(unNormalizedDir.x, animator.GetFloat("right"), lerpedAnimation));
        inputMoveEvent?.Invoke(unNormalizedDir);
    }
    public void RecieveMouse()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        Vector3 dir = new Vector3(-mouseY, mouseX, 0);
        inputMouseEvent?.Invoke(dir);
    }

    public void RecieveMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
            inputMouseLeftDownEvent?.Invoke();
        if (Input.GetMouseButton(0))
            inputMouseLeftStayEvent?.Invoke();
        if (Input.GetMouseButtonUp(0))
            inputMouseLeftUpEvent?.Invoke();
        if (Input.GetMouseButtonDown(1))
            inputMouseRightDownEvent?.Invoke();
        if (Input.GetMouseButton(1))
            inputMouseRightStayEvent?.Invoke();
        if (Input.GetMouseButtonUp(1))
            inputMouseRightUpEvent?.Invoke();

    }

    public void RecieveShift()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRun = true;
            inputShiftDownEvent?.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {

            isRun = false;
            inputShiftUpEvent?.Invoke();
        }
    }

    public void RecieveC()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            inputCEvent?.Invoke();
        }
    }

    public void RecieveR()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            inputREvent?.Invoke();
        }
    }

    public void RecieveT()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            inputTEvent?.Invoke();
        }
    }

    public void RecieveE()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            inputEEvent?.Invoke();
        }
    }

    public void RecieveV()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            inputVEvent?.Invoke();
        }
    }

    public void RecieveSpaceBar()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            inputSpaceEvent?.Invoke();
    }
}
