    using UnityEngine;
    using UnityEngine.InputSystem;
    using Cinemachine;

    public class PlayerCamInput : MonoBehaviour, AxisState.IInputAxisProvider
    {
        
        public InputAction lookValue;

        [HideInInspector]
        public float aimAssistSlowdown = 1;   

    public float GetAxisValue(int axis)
        {
            switch (axis)
            {
                case 0: return lookValue.ReadValue<Vector2>().x * aimAssistSlowdown;
                case 1: return lookValue.ReadValue<Vector2>().y * aimAssistSlowdown;
            }

            return 0;
        }
    }