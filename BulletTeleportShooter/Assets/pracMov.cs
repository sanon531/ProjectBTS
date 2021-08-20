using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MoreMountains.TopDownEngine
{
    public class pracMov : MonoBehaviour
    {
        private Vector3 _mousePosition;
        private Vector3 _direction;
        private Vector3 _currentAim;
        private Camera _mainCamera;
        private CharacterMovement _characterMovement;

        // Start is called before the first frame update
        void Start()
        {
            _mainCamera = Camera.main;

            _characterMovement = GetComponent<CharacterMovement>();

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (Input.GetMouseButton(0))
            {
                _mousePosition = Input.mousePosition;
                //_mousePosition.z = 10;

                _direction = _mainCamera.ScreenToWorldPoint(_mousePosition);
                //_direction.z = transform.position.z;

                _currentAim = _direction - transform.position;
                _currentAim.Normalize();

                Debug.Log(_currentAim);

                //InputManager.Instance.SetHengelMovement(new Vector2(_currentAim.x, _currentAim.y));
                _characterMovement.SetMovement(new Vector2(_currentAim.x, _currentAim.y));


            }


        }

    }
}
    