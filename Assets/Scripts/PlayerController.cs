using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTutorial.Manager;

namespace UnityTutorial.PlayerControl
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Animation & Camera Settings")]
        [SerializeField] private float AnimBlendSpeed = 8.9f;
        [SerializeField] private Transform CameraRoot;
        [SerializeField] private Transform Camera;
        [SerializeField] private float UpperLimit = -40f;
        [SerializeField] private float BottomLimit = 70f;
        [SerializeField] private float MouseSensitivity = 21.9f;

        [Header("Follow Maya Settings")]
        [SerializeField] private Transform maya; 
        [SerializeField] private float DP = 2.5f;

        [Header("Corridor Settings")]
        public Transform[] corridors;
        private Transform _currentCorridor;

        private Rigidbody _playerRigidbody;
        private InputManager _inputManager;
        private Animator _animator;
        private bool _hasAnimator;
        private int _xVelHash;
        private int _yVelHash;
        private float _xRotation;

        private const float _walkSpeed = 2f;
        private const float _runSpeed = 6f;
        private Vector2 _currentVelocity;

        private void Start()
        {
            _hasAnimator = TryGetComponent<Animator>(out _animator);
            _playerRigidbody = GetComponent<Rigidbody>();
            _inputManager = GetComponent<InputManager>();

            _xVelHash = Animator.StringToHash("X_Velocity");
            _yVelHash = Animator.StringToHash("Y_Velocity");

            Cursor.lockState = CursorLockMode.Locked;

            // Initialize starting corridor
            if (corridors != null && corridors.Length > 0)
                _currentCorridor = corridors[0];
        }

        private void Update()
        {
            UpdateCurrentCorridor();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void LateUpdate()
        {
            CamMovements();
            BatasiCorridor(); // Constrain position after movement and camera rotations
        }

        private void Move()
        {
            if(!_hasAnimator) return;

            float targetSpeed = _inputManager.Run ? _runSpeed : _walkSpeed;

            // Logika Jarak ke Maya
            if (maya != null)
            {
                float distance = Vector3.Distance(transform.position, maya.position);
                if (distance > DP)
                {
                    targetSpeed *= 0.2f; 
                }
            }
            
            if(_inputManager.Move == Vector2.zero) targetSpeed = 0f;

            _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, _inputManager.Move.x * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);
            _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, _inputManager.Move.y * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);

            // Using linearVelocity (Unity 2023+) or velocity (Older versions)
            // Note: If you are on an older Unity version, change .linearVelocity to .velocity
            var xVelDifference = _currentVelocity.x - _playerRigidbody.linearVelocity.x;
            var zVelDifference = _currentVelocity.y - _playerRigidbody.linearVelocity.z;

            _playerRigidbody.AddForce(transform.TransformVector(new Vector3(xVelDifference, 0, zVelDifference)), ForceMode.VelocityChange);
            
            _animator.SetFloat(_xVelHash, _currentVelocity.x);
            _animator.SetFloat(_yVelHash, _currentVelocity.y);
        }

        private void CamMovements()
        {
            if(!_hasAnimator) return;

            var Mouse_X = _inputManager.Look.x;
            var Mouse_Y = _inputManager.Look.y;
            Camera.position = CameraRoot.position;

            _xRotation -= Mouse_Y * MouseSensitivity * Time.deltaTime;
            _xRotation = Mathf.Clamp(_xRotation, UpperLimit, BottomLimit);

            Camera.localRotation = Quaternion.Euler(_xRotation, 0, 0);
            transform.Rotate(Vector3.up, Mouse_X * MouseSensitivity * Time.deltaTime);
        }

        private void UpdateCurrentCorridor()
        {
            if (corridors == null) return;

            Vector3 pos = transform.position;

            foreach (Transform corridor in corridors)
            {
                if (corridor == null) continue;

                float halfWidth = corridor.localScale.x / 2f;
                float halfLength = corridor.localScale.z / 2f;

                float minX = corridor.position.x - halfWidth;
                float maxX = corridor.position.x + halfWidth;

                float minZ = corridor.position.z - halfLength;
                float maxZ = corridor.position.z + halfLength;

                if (pos.x >= minX && pos.x <= maxX &&
                    pos.z >= minZ && pos.z <= maxZ)
                {
                    _currentCorridor = corridor;
                    return;
                }
            }
        }

        private void BatasiCorridor()
        {
            if (_currentCorridor == null) return;

            Vector3 pos = transform.position;

            float halfWidth = _currentCorridor.localScale.x / 2f;
            float halfLength = _currentCorridor.localScale.z / 2f;

            float minX = _currentCorridor.position.x - halfWidth;
            float maxX = _currentCorridor.position.x + halfWidth;

            float minZ = _currentCorridor.position.z - halfLength;
            float maxZ = _currentCorridor.position.z + halfLength;

            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.z = Mathf.Clamp(pos.z, minZ, maxZ);

            transform.position = pos;
        }
    }
}