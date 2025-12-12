using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // -------------------------------------------------------------------------
    // Public Properties:
    // ------------------
    //   PlayerCamera
    //   CrouchHeight
    //   CrouchSpeed
    //   DefaultHeight
    //   Gravity
    //   JumpPower
    //   LookSpeed
    //   LookXLimit
    //   RunSpeed
    //   WalkSpeed
    // -------------------------------------------------------------------------

    #region .  Public Properties  .

    public Camera PlayerCamera;
    public float  CrouchHeight  = 1f;
    public float  CrouchSpeed   = 3f;
    public float  DefaultHeight = 2f;
    public float  Gravity       = 10f;
    public float  JumpPower     = 7f;
    public float  LookSpeed     = 2f;
    public float  LookXLimit    = 45f;
    public float  RunSpeed      = 12f;
    public float  WalkSpeed     = 6f;

    #endregion



    // -------------------------------------------------------------------------
    // Private Properties:
    // -------------------
    //   _characterController
    //   _canMove
    //   _moveDirection
    //   _rotationX
    // -------------------------------------------------------------------------

    #region .  Private Properties  .

    private CharacterController _characterController;
    private readonly bool       _canMove       = true;
    private Vector3             _moveDirection = Vector3.zero;
    private float               _rotationX     = 0;

    #endregion



    // -------------------------------------------------------------------------
    // Private Methods:
    // ----------------
    //   Start()
    //   Update()
    // -------------------------------------------------------------------------

    #region .  Start()  .
    // -------------------------------------------------------------------------
    //   Method.......:  Start()
    //   Description..:  
    //   Parameters...:  None
    //   Returns......:  Nothing
    // -------------------------------------------------------------------------
    private void Start()
    {
        this._characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;

    }   // Start
    #endregion


    #region .  Update()  .
    // -------------------------------------------------------------------------
    //   Method.......:  Update()
    //   Description..:  
    //   Parameters...:  None
    //   Returns......:  Nothing
    // -------------------------------------------------------------------------
    private void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right   = transform.TransformDirection(Vector3.right);

        bool  isRunning          = Input.GetKey(KeyCode.LeftShift);
        float currentSpeedX      = this._canMove ? (isRunning ? this.RunSpeed : this.WalkSpeed) * Input.GetAxis("Vertical")   : 0;
        float currentSpeedY      = this._canMove ? (isRunning ? this.RunSpeed : this.WalkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = this._moveDirection.y;

        this._moveDirection = (forward * currentSpeedX) + (right * currentSpeedY);

        if (Input.GetButton("Jump") && this._canMove && this._characterController.isGrounded)
        {
            this._moveDirection.y = this.JumpPower;
        }
        else
        {
            this._moveDirection.y = movementDirectionY;
        }

        if (!this._characterController.isGrounded)
        {
            this._moveDirection.y -= this.Gravity * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.R) && this._canMove)
        {
            this._characterController.height = this.CrouchHeight;
            this.WalkSpeed = this.CrouchSpeed;
            this.RunSpeed  = this.CrouchSpeed;

        }
        else
        {
            this._characterController.height = this.DefaultHeight;
            this.WalkSpeed = 6f;
            this.RunSpeed  = 12f;
        }

        this._characterController.Move(this._moveDirection * Time.deltaTime);

        if (this._canMove)
        {
            this._rotationX += -Input.GetAxis("Mouse Y") * this.LookSpeed;
            this._rotationX  = Mathf.Clamp(this._rotationX, -this.LookXLimit, this.LookXLimit);

            this.PlayerCamera.transform.localRotation = Quaternion.Euler(this._rotationX, 0, 0);
            this.transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * this.LookSpeed, 0);
        }

    }   // Update()
    #endregion


}   // class PlayerMovement()
