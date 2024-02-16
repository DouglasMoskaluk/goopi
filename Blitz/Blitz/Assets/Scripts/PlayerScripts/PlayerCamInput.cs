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

        private float ogX;
        private float ogY;

        private float newX;
        private float newY;

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
                    case 0:
                        ogX = lookValue.ReadValue<Vector2>().x;
                        newX = Mathf.Pow(ogX * aimAssistSlowdown * charSelect, 2);

                        if (ogX < 0)
                        {
                            newX *= -1;
                        }

                       return newX;
                    case 1:
                        ogY = lookValue.ReadValue<Vector2>().y;
                        newY = Mathf.Pow(ogY * aimAssistSlowdown * charSelect, 2);

                        if (ogY < 0)
                        {
                            newY *= -1;
                        }

                    return newY;
                }

                return 0;
            }
        }