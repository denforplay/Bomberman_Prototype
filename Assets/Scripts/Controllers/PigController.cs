using System;
using Models;
using UniRx;
using UnityEngine;

namespace Controllers
{
    public class PigController : MonoBehaviour
    {
        [SerializeField] private Pig _pig;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private float _speed;
        private DefaultControls _defaultControls;
        private void Awake()
        {
            Observable.EveryUpdate()
                .Where(_ => _defaultControls.Player.Enter.triggered)
                .Subscribe(_ => _pig.SetBomb());
            _rigidbody.freezeRotation = true;
            _defaultControls = new DefaultControls();
        }

        private void FixedUpdate()
        {
            Vector2 moveInput = _defaultControls.Player.Move.ReadValue<Vector2>();
            _rigidbody.velocity = moveInput * _speed;

            if (Application.platform == RuntimePlatform.Android)
            {
                moveInput = _defaultControls.Player.VJMove.ReadValue<Vector2>();
                _rigidbody.velocity = moveInput * _speed;
            }
            
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