using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-100)]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private TankControls controls;

    public event System.Action<Vector2> OnLook;
    public event System.Action<float> OnElevate;
    public event System.Action<bool> OnFireMitra;   
    public event System.Action OnFireCannon;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        controls = new TankControls();
    }

    private void OnEnable()
    {
        if (controls == null) return;
        controls.Enable();

        controls.Tank.Look.performed += HandleLookPerformed;
        controls.Tank.Look.canceled += HandleLookCanceled;

        controls.Tank.Elevate.performed += HandleElevatePerformed;
        controls.Tank.Elevate.canceled += HandleElevateCanceled;

        controls.Tank.FireMitra.performed += HandleFireMitraPerformed;
        controls.Tank.FireMitra.canceled += HandleFireMitraCanceled;

        controls.Tank.FireCannon.performed += HandleFireCannonPerformed;
    }

    private void OnDisable()
    {
        if (controls == null) return;

        controls.Tank.Look.performed -= HandleLookPerformed;
        controls.Tank.Look.canceled -= HandleLookCanceled;

        controls.Tank.Elevate.performed -= HandleElevatePerformed;
        controls.Tank.Elevate.canceled -= HandleElevateCanceled;

        controls.Tank.FireMitra.performed -= HandleFireMitraPerformed;
        controls.Tank.FireMitra.canceled -= HandleFireMitraCanceled;

        controls.Tank.FireCannon.performed -= HandleFireCannonPerformed;

        controls.Disable();
    }

    private void HandleLookPerformed(InputAction.CallbackContext ctx) => OnLook?.Invoke(ctx.ReadValue<Vector2>());
    private void HandleLookCanceled(InputAction.CallbackContext ctx) => OnLook?.Invoke(Vector2.zero);

    private void HandleElevatePerformed(InputAction.CallbackContext ctx) => OnElevate?.Invoke(ctx.ReadValue<float>());
    private void HandleElevateCanceled(InputAction.CallbackContext ctx) => OnElevate?.Invoke(0f);

    private void HandleFireMitraPerformed(InputAction.CallbackContext ctx) => OnFireMitra?.Invoke(true);
    private void HandleFireMitraCanceled(InputAction.CallbackContext ctx) => OnFireMitra?.Invoke(false);

    private void HandleFireCannonPerformed(InputAction.CallbackContext ctx) => OnFireCannon?.Invoke();
}