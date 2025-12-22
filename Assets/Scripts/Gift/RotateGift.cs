using UnityEngine;

namespace Gift
{
    public class RotateGift : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float rotationAmountX = 0.5f;
        [SerializeField] private float rotationAmountY = 0.5f;
        [SerializeField] private float rotationAmountZ = 0.5f;

        private Vector3 _rotationDirection;
        
        private void Start()
        {
            _rotationDirection = new Vector3(rotationAmountX, rotationAmountY, rotationAmountZ);
        }
        
        // Update is called once per frame
        private void FixedUpdate()
        {
            transform.Rotate(_rotationDirection, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}