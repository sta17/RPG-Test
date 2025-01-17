using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{

    [Header("Misc")]
    [SerializeField] private PCControls PCControls;
    [SerializeField] private CharacterController CharacterController;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform CameraRotationPointTransform;
    [SerializeField] private Transform ModelTransform;

    [SerializeField] private Vector2 currentMovementInput;
    [SerializeField] private Vector3 currentMovement;
    [SerializeField] private bool isMovementPressed;

    [SerializeField] private bool isUpPressed;
    [SerializeField] private bool iDownPressed;
    [SerializeField] private bool isLeftPressed;
    [SerializeField] private bool isRightPressed;

    [Header("Vertical Notion - Zooming")]
    [SerializeField] private float stepSize = 2f;
    [SerializeField] private float zoomDampening = 7.5f;
    [SerializeField] private float minHeight = 5f;
    [SerializeField] private float maxHeight = 50f;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float zoomHeight;
    [SerializeField] public float smoothTime = 0.3F;

    [Header("Rotation")]
    [SerializeField] private float maxRotationSpeed = 1f;

    private void Awake()
    {
        PCControls = new PCControls();
        CharacterController = GetComponent<CharacterController>();

        PCControls.CharacterControls.Movement.started += OnMovementInput;
        PCControls.CharacterControls.Movement.canceled += OnMovementInput;
        PCControls.CharacterControls.Movement.performed += OnMovementInput;

        PCControls.CharacterControls.Rotate.started += OnRotateInput;
        PCControls.CharacterControls.Rotate.performed += OnRotateInput;
        PCControls.CharacterControls.Rotate.canceled += OnRotateInput;

        PCControls.CharacterControls.ZoomCamera.started += OnZoomInput;
        PCControls.CharacterControls.ZoomCamera.performed += OnZoomInput;
        PCControls.CharacterControls.ZoomCamera.canceled += OnZoomInput;
    }


    private void Start()
    {
        Vector3 startpos = Vector3.zero;
        startpos.x = (float)-0.4;
        startpos.y = 5;
        startpos.z = -7;
        cameraTransform.position = startpos;

        Quaternion startrot = Quaternion.Euler(45, 0, 0);
        cameraTransform.rotation = startrot;
    }
    void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x * 4.0f;
        currentMovement.z = currentMovementInput.y * 4.0f;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    void OnKeyPressed() {

        isUpPressed = PCControls.CharacterControls.Up.WasPerformedThisFrame();
        iDownPressed = PCControls.CharacterControls.Down.WasPerformedThisFrame();
        isLeftPressed = PCControls.CharacterControls.Left.WasPerformedThisFrame();
        isRightPressed = PCControls.CharacterControls.Right.WasPerformedThisFrame();
        
        if (isUpPressed && isLeftPressed) {
            ModelTransform.rotation = Quaternion.Euler(0f, -45, 0f);

        }
        else if (isUpPressed && isRightPressed)
        {
            ModelTransform.rotation = Quaternion.Euler(0f, 45, 0f);

        }
        else if (iDownPressed && isLeftPressed)
        {
            ModelTransform.rotation = Quaternion.Euler(0f, -135, 0f);

        }
        else if (iDownPressed && isRightPressed)
        {
            ModelTransform.rotation = Quaternion.Euler(0f, 135, 0f);

        }
        else if (isUpPressed)
        {
            ModelTransform.rotation = Quaternion.Euler(0f, 0, 0f);

        }
        else if (iDownPressed)
        {
            ModelTransform.rotation = Quaternion.Euler(0f, 180, 0f);

        }
        else if (isRightPressed) {
            ModelTransform.rotation = Quaternion.Euler(0f, 90, 0f);

        } else if (isLeftPressed) {
            ModelTransform.rotation = Quaternion.Euler(0f, -90, 0f);

        }
    }

    private void OnRotateInput(InputAction.CallbackContext inputValue)
    {
        if (!Mouse.current.middleButton.isPressed)
        {
            return;
        }
        float value = inputValue.ReadValue<Vector2>().x;
        CameraRotationPointTransform.rotation = Quaternion.Euler(0f, value * maxRotationSpeed + CameraRotationPointTransform.rotation.eulerAngles.y, 0f);

    }

    void OnZoomInput(InputAction.CallbackContext inputValue)
    {
        float value = -inputValue.ReadValue<Vector2>().y / 100f;

        if (Mathf.Abs(value) > 0.1f)
        {
            zoomHeight = cameraTransform.localPosition.y + value * stepSize;
            if (zoomHeight < minHeight)
            {
                zoomHeight = minHeight;
            }
            else if (zoomHeight > maxHeight)
            {
                zoomHeight = maxHeight;
            }
        }
    }

    private void UpdateCameraPosition()
    {
        Vector3 zoomTarget = new(cameraTransform.localPosition.x, zoomHeight, cameraTransform.localPosition.z);
        zoomTarget -= zoomSpeed * (zoomHeight - cameraTransform.localPosition.y) * Vector3.forward;

        Vector3 velocity = Vector3.zero;
        cameraTransform.localPosition = Vector3.SmoothDamp(cameraTransform.localPosition, zoomTarget,ref velocity, zoomDampening);
        cameraTransform.LookAt(CameraRotationPointTransform);
    }

    // Update is called once per frame
    void Update()
    {
        CharacterController.Move(currentMovement * Time.deltaTime);
    }

    private void LateUpdate()
    {
        UpdateCameraPosition();
        OnKeyPressed();
    }

    void OnEnable()
    {
        PCControls.CharacterControls.Enable();
    }

     void OnDisable()
    {
        PCControls.CharacterControls.Disable();
    }
}
