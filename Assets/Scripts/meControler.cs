using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class meControler : MonoBehaviour
{
    public float normalSpeed = 5f;
    public float dashSpeed = 20f;

    Vector2 move;
    float moveSpeed;

    bool isDashing;
    bool dashOnCooldown;
    bool shiftHeld;

    Coroutine dashCo;
    SpriteRenderer meColor;
    meManager mM ;

    void Awake()
    {
        mM = GetComponent<meManager>();
        meColor = GetComponent<SpriteRenderer>();
        moveSpeed = normalSpeed;
    }

    public void OnMoveEvent(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<Vector2>();
        if (move.sqrMagnitude > 1f) move = move.normalized;

        if (ctx.canceled) move = Vector2.zero; 
    }

    public void OnDashEvent(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) shiftHeld = true;
        if (ctx.canceled)  shiftHeld = false;
    }

    public void OnParryEvent(InputAction.CallbackContext ctx)
    {
        if(isDashing&&ctx.performed) mM.isParry = true;
        
    }

    void Update()
    {
        if (shiftHeld && move.sqrMagnitude > 0f && !isDashing && !dashOnCooldown && dashCo == null)
            dashCo = StartCoroutine(DashRoutine());

        MovePosition(move);
    }

    void MovePosition(Vector2 input)
    {
        if (isDashing)
        {
            moveSpeed = dashSpeed;
            meColor.color = Color.red;
        }
        else
        {
            moveSpeed = normalSpeed;
            meColor.color = Color.white;
        }

        transform.Translate(input * moveSpeed * Time.deltaTime, Space.World);
    }

    IEnumerator DashRoutine()
    {
        isDashing = true;
        yield return new WaitForSeconds(0.1f);
        isDashing = false;

        dashOnCooldown = true;
        yield return new WaitForSeconds(0.2f);
        dashOnCooldown = false;

        dashCo = null;
    }
}
