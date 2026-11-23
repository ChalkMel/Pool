using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputListener : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Camera mainCamera;
    
        private InputSystem_Actions _inputSystemActions;
        private Vector2 _mousePosition;

        private void Awake()
        {
            _inputSystemActions = new InputSystem_Actions();
        }

        private void OnEnable()
        {
            _inputSystemActions.Player.Attack.performed += OnAttackPerformed;
        
            _inputSystemActions.Enable();
        }

        private void OnDisable()
        {
            _inputSystemActions.Player.Attack.performed -= OnAttackPerformed;
        
            _inputSystemActions.Disable();
        }

        private void Update()
        {
            _mousePosition = Mouse.current.position.ReadValue();
            Vector2 moveDirection = _inputSystemActions.Player.Move.ReadValue<Vector2>();
            playerController.Move(moveDirection);
        }
    
        private void OnAttackPerformed(InputAction.CallbackContext context)
        {
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(
                new Vector3(_mousePosition.x, _mousePosition.y, 0)
            );
            mouseWorldPos.z = 0;
        
            Vector2 shootDirection = (mouseWorldPos - playerController.FirePoint.position).normalized;
        
            playerController.Shoot(shootDirection);
        }
    }
}