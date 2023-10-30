    using UnityEngine;
    using UnityEngine.InputSystem;
    using Cinemachine;

    public class PlayerCamInput : MonoBehaviour, AxisState.IInputAxisProvider
    {
        
        public InputAction lookValue;

    public float GetAxisValue(int axis)
        {
            switch (axis)
            {
                case 0: return lookValue.ReadValue<Vector2>().x;
                case 1: return lookValue.ReadValue<Vector2>().y;
            }

            return 0;
        }
    }