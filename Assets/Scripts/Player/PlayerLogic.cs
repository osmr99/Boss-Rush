using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public static class StaticInputManager
{
    public static PlayerInputActions input { get; private set; } = new PlayerInputActions();
}

public class PlayerLogic : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float gravity;
    [SerializeField] GameObject sword;
    [SerializeField] GameObject projectile;
    [SerializeField] Animator animator;

    CharacterController controller;
    Vector3 moveVelocity;
    bool grounded = false;

    bool canControl = true;
    bool aiming = false;

    float timeSinceLastMelee = 0;

    [SerializeField] LayerMask ground;

    bool usingMouse = false;

    public UnityEvent OnStartDash;
    public UnityEvent OnEndDash;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        StaticInputManager.input.Player.Jump.performed += Jump;
        StaticInputManager.input.Player.Melee.performed += Melee;
        StaticInputManager.input.Player.Dodge.performed += Dodge;
        StaticInputManager.input.Player.Aim.started += ctx => { aiming = true; usingMouse = ctx.control.parent.name == "Mouse" ? true : false; };
        StaticInputManager.input.Player.Aim.canceled += ctx => { aiming = false; usingMouse = ctx.control.parent.name == "Mouse" ? true : false; };
        StaticInputManager.input.Player.Shoot.performed += Shoot;

        StaticInputManager.input.Player.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastMelee += Time.deltaTime;

        bool prevGrounded = grounded;
        grounded = controller.isGrounded;

        if(prevGrounded != grounded && grounded)
        {
            // landed
        }

        if(grounded && moveVelocity.y < 0)
        {
            moveVelocity.y = 0;
        }

        if(canControl)
        {
            Vector2 moveInput = StaticInputManager.input.Player.Move.ReadValue<Vector2>();

            Vector3 adjustedInput = GetCameraRelativeInput(moveInput);

            if (adjustedInput != Vector3.zero && !aiming)
                transform.forward = adjustedInput.normalized;

            if (aiming)
            {
                moveVelocity = new Vector3(0, moveVelocity.y, 0);

                if(usingMouse)
                {
                    Vector2 mousePos = Mouse.current.position.value;

                    Ray mouseRay = Camera.main.ScreenPointToRay(mousePos);

                    if (Physics.Raycast(mouseRay, out RaycastHit hit, 1000, ground))
                    {
                        var aimDirection = (hit.point - transform.position).normalized;
                        aimDirection.y = 0;
                        transform.forward = aimDirection;
                    }
                }

                else
                {
                    if (adjustedInput != Vector3.zero)
                        transform.forward = adjustedInput.normalized;
                }

            }

            else
            {
                moveVelocity = new Vector3(adjustedInput.x * moveSpeed, moveVelocity.y, adjustedInput.z * moveSpeed);
            }
        }

        moveVelocity.y += gravity * Time.deltaTime;

        controller.Move((moveVelocity) * Time.deltaTime);

        animator.SetFloat("speed", controller.velocity.magnitude);
    }

    void Jump(InputAction.CallbackContext ctx)
    {
        if (!grounded)
            return;

        moveVelocity.y = jumpSpeed;
    }

    void Melee(InputAction.CallbackContext ctx)
    {
        if (!canControl)
            return;

        if (!grounded)
            return;

        if (timeSinceLastMelee < 0.25f)
            return;

        if (aiming)
            return;

        StartCoroutine(HandleMelee());
    }

    IEnumerator HandleMelee()
    {
        timeSinceLastMelee = 0;
        sword.SetActive(true);
        sword.GetComponent<Animator>().SetTrigger("swing");
        yield return new WaitForSeconds(0.25f);
        sword.SetActive(false);
    }

    void Dodge(InputAction.CallbackContext ctx)
    {
        if (!canControl)
            return;

        StartCoroutine(HandleDash());
    }

    IEnumerator HandleDash()
    {
        OnStartDash?.Invoke();
        canControl = false;
        moveVelocity = transform.forward * 20;
        yield return new WaitForSeconds(0.25f);
        moveVelocity = Vector3.zero;
        canControl = true;
        OnEndDash?.Invoke();
    }

    public void Shoot(InputAction.CallbackContext ctx)
    {
        if (!canControl)
            return;

        if (!aiming)
            return;

        GameObject p = Instantiate(projectile, transform.position, Quaternion.identity);
        p.transform.forward = transform.forward;
    }

    Vector3 GetCameraRelativeInput(Vector2 input)
    {
        Transform cam = Camera.main.transform;

        Vector3 camRight = cam.right;
        camRight.y = 0;
        camRight.Normalize();
        Vector3 camForward = cam.forward;
        camForward.y = 0;
        camForward.Normalize();

        return input.x * camRight + input.y * camForward;
    }

    public void ApplyKnockback(Damage damage)
    {
        HandleKnockback(damage.direction * damage.knockbackForce);
    }

    IEnumerator HandleKnockback(Vector3 knockback)
    {
        moveVelocity = knockback;
        canControl = false;
        yield return new WaitForSeconds(0.5f);
        canControl = true;
        moveVelocity = Vector3.zero;
    }

    public void Death()
    {
        canControl = false;
        GameManager.instance.RestartLevel();
    }
}
