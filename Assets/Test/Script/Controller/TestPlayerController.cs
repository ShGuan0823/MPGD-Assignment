using UnityEngine;
using UnityEngine.InputSystem;


public class TestPlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;
    Rigidbody rigid;

    private Vector2 mRotation;
    private Vector2 mLook;
    private Vector2 mMove;
    private float forwardAmount;
    private float rotateAmount;
    private bool isPressRightBtn = false;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public void OnMoveByKeyBoard(InputAction.CallbackContext context)
    {
        mMove = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mLook = context.ReadValue<Vector2>();
    }

    public void OnTest(InputAction.CallbackContext context)
    {
        if (context.Equals(null))
        {
            isPressRightBtn = false;
            Debug.Log("false");
        }
        else
        {
            isPressRightBtn = true;
            Debug.Log("true");
        }
    }

    public void onTest2(InputAction.CallbackContext context)
    {
        isPressRightBtn = false;
    }

    public void Update()
    {
        Move();
    }

    private void FixedUpdate()
    {
        rigid.velocity = forwardAmount * transform.forward * moveSpeed;
        //rigid.MoveRotation(rigid.rotation * Quaternion.Euler(0, rotateAmount * rotateSpeed, 0));

    }

    private void Move()
    {
        // Update orientation first, then move. Otherwise move orientation will lag
        // behind by one frame.
        //Debug.Log(mLook);
        Look(mLook);
        MoveByKeyBoard(mMove);
    }

    private void Look(Vector2 rotate)
    {
        if (!isPressRightBtn)
            return;
        if (rotate.sqrMagnitude < 0.01)
            return;
        var scaledRotateSpeed = rotateSpeed * Time.deltaTime;
        mRotation.y += rotate.x * scaledRotateSpeed;
        //mRotation.x = Mathf.Clamp(mRotation.x - rotate.y * scaledRotateSpeed, -89, 89);
        //Debug.Log("Rotation: " + mRotation);
        transform.localEulerAngles = mRotation;
        //Debug.Log(Camera.current.name);
    }

    private void MoveByKeyBoard(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01)
            return;
        //var scaledMoveSpeed = moveSpeed * Time.deltaTime;
        //var move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);
        var localMove = transform.InverseTransformDirection(new Vector3(direction.x, 0, direction.y));
        forwardAmount = localMove.z;
        rotateAmount = Mathf.Atan2(localMove.x, localMove.y);
        //transform.position += move * scaledMoveSpeed;
    }

}
