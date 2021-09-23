using System;
using Models;
using UniRx;
using UnityEngine;

namespace Controllers
{
    public class PigController : MonoBehaviour
    {
        private readonly int X = Animator.StringToHash("x");
        private readonly int Y = Animator.StringToHash("y");
        private readonly int IsWalking = Animator.StringToHash("IsWalking");
        [SerializeField] private Animator _pigMovementAnimator;
        [SerializeField] private Pig _pig;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private float _speed;
        [SerializeField] private float _minSpeed;
        private DefaultControls _defaultControls;

        public void DecreaseSpeed(float value)
        {
            if (_speed > _minSpeed + value)
            {
                _speed -= value;
            }
        }
        
        private void Awake()
        {
            Observable
                .EveryUpdate()
                .Where(_ => _defaultControls.Player.Enter.triggered)
                .Subscribe(_ => _pig.SetBomb());
            _rigidbody.freezeRotation = true;
            _defaultControls = new DefaultControls();
        }

        private void FixedUpdate()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                Vector2 moveInput = _defaultControls.Player.Move.ReadValue<Vector2>();

                _rigidbody.velocity = moveInput * _speed;
                if (moveInput != Vector2.zero)
                {
                    Animate(moveInput);
                }
            }

            if (Application.platform == RuntimePlatform.Android)
            {
                Vector2 moveInput = _defaultControls.Player.VJMove.ReadValue<Vector2>();
                _rigidbody.velocity = moveInput * _speed;
                if (moveInput != Vector2.zero)
                {
                    Animate(moveInput);
                }
            }
        }

        private void Animate(Vector2 direction)
        {
            _pigMovementAnimator.SetFloat(X, direction.x);
            _pigMovementAnimator.SetFloat(Y, direction.y);
            _pigMovementAnimator.SetBool(IsWalking,true);
        }

        private void OnEnable()
        {
            _defaultControls.Enable();
        }

        private void OnDisable()
        {
            _defaultControls.Disable();
        }
    }
}