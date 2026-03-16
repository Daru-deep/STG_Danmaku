using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class meControler : MonoBehaviour
{
    public float normalSpeed = 5f;
    public float dashSpeed = 20f;

    [SerializeField] float parryTime = 0.5f;

    [SerializeField] private GameObject parryRingPrefab;

    [SerializeField] private GameManager gm;

    [SerializeField] private GameObject sp;
    
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
        if (mM == null) { Debug.LogError($"[{name}] meManager not found"); enabled = false; return; }
        meColor = sp.GetComponent<SpriteRenderer>();
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

        
        if(ctx.performed) 
        {

            gm.StartParticle(3,transform,2,0.5f);
            Debug.Log("PUSH_PARRY_EVENT!!");
            StartCoroutine(PushParyCount());
            
            
        }
        
    }
    
    IEnumerator PushParyCount()
    {
        mM.isParry = true;

        yield return new WaitForSeconds(parryTime);
        mM.isParry = false;

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
