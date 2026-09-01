using UnityEngine;

namespace GridNav
{
    public class PlayerController : MonoBehaviour
    {
        private bool _canMove = true;

        public float moveSpeed = 5f;
        public float rotateSpeed = 10f;
        public float gravity = -9.81f;
        public LayerMask collisionLayer;

        private CharacterController characterController;
        private Vector3 velocity;

        public bool CanMove { get => _canMove; set => _canMove = value; }

        void Start()
        {
            characterController = GetComponent<CharacterController>();
        }
        public void OnUpdate()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            // Input for movement
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // Calculate movement direction relative to the camera
            Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;
            if (moveDirection.magnitude >= 0.1f)
            {
                // Rotate character towards movement direction
                float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;

                // Move the character
                Vector3 move = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
                characterController.Move(move * moveSpeed * Time.deltaTime);
            }

            // Apply gravity
            if (!characterController.isGrounded)
            {
                velocity.y += gravity * Time.deltaTime;
            }
            else
            {
                velocity.y = -2f; // Small negative to keep grounded
            }

            characterController.Move(velocity * Time.deltaTime);
        }
    }
}