    using UnityEngine;
    using UnityEngine.InputSystem;
    using Cinemachine;

    public class PlayerCamInput : MonoBehaviour, AxisState.IInputAxisProvider
    {
        
        public InputAction lookValue;

        [HideInInspector]
        public float aimAssistSlowdown = 1;

        [HideInInspector]
        public float charSelect = 1;

        private CinemachineFreeLook cam;

        private void Start()
        {
            cam = GetComponent<CinemachineFreeLook>();
            cam.m_XAxis.Value = 90;
            //cam.m_YAxis.Value = 0.44f;
            charSelect = 0;
        }

        public float GetAxisValue(int axis)
            {
                switch (axis)
                {
                    case 0: return lookValue.ReadValue<Vector2>().x * aimAssistSlowdown * charSelect;
                    case 1: return lookValue.ReadValue<Vector2>().y * charSelect;
                }

                return 0;
            }
        }