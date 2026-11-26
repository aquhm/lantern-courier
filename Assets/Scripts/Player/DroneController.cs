using UnityEngine;
using UnityEngine.InputSystem;

namespace LanternCourier.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class DroneController : MonoBehaviour
    {
        [SerializeField] InputActionReference move;
        [SerializeField] float speed = 7f;
        [SerializeField] float turnSpeed = 720f;
        [SerializeField] Vector2 areaHalfExtents = new(24f, 15f);

        Rigidbody body;
        Vector2 input;

        public Vector3 Facing { get; private set; } = Vector3.forward;

        void Awake()
        {
            body = GetComponent<Rigidbody>();
        }

        void OnEnable()
        {
            move.action.Enable();
        }

        void Update()
        {
            input = move.action.ReadValue<Vector2>();
        }

        void FixedUpdate()
        {
            var velocity = new Vector3(input.x, 0f, input.y) * speed;
            body.linearVelocity = velocity;

            if (velocity.sqrMagnitude > 0.01f)
            {
                Facing = velocity.normalized;
                var target = Quaternion.LookRotation(Facing, Vector3.up);
                body.MoveRotation(Quaternion.RotateTowards(body.rotation, target, turnSpeed * Time.fixedDeltaTime));
            }

            var position = body.position;
            if (Mathf.Abs(position.x) > areaHalfExtents.x || Mathf.Abs(position.z) > areaHalfExtents.y)
            {
                position.x = Mathf.Clamp(position.x, -areaHalfExtents.x, areaHalfExtents.x);
                position.z = Mathf.Clamp(position.z, -areaHalfExtents.y, areaHalfExtents.y);
                body.position = position;
            }
        }
    }
}
