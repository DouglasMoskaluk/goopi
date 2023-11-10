//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/Scripts/Playerinput/PlayerInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""Character"",
            ""id"": ""baa86f04-0f29-4eb4-8c35-a75bf23b32ea"",
            ""actions"": [
                {
                    ""name"": ""Motion"",
                    ""type"": ""Value"",
                    ""id"": ""884a5c8e-d690-4094-bb5f-584e22c32d02"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""aa7e9096-dbcd-48fe-806c-f82af554e095"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""8d7ca68a-e7c0-4aa7-9851-88b9b5620b1a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""71b7a199-bcfa-4d38-8e54-5f1eb6e7fe8c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SprintToggle"",
                    ""type"": ""Button"",
                    ""id"": ""4e9ab0b1-05ee-4a8a-8784-4a695ac807d0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""DropGrenade"",
                    ""type"": ""Button"",
                    ""id"": ""22567ee2-f9fa-4abf-8a3b-eea10e5f6585"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ThrowGrenade"",
                    ""type"": ""Button"",
                    ""id"": ""ac7f02b4-011f-461c-b2d3-6e406e82ac42"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""2d58c7f2-73d6-4648-8ccf-fc826fda8a92"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ChangeSensitivity"",
                    ""type"": ""Value"",
                    ""id"": ""f388ebc2-72c0-4796-8da8-d84fef89eade"",
                    ""expectedControlType"": ""Dpad"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Slide"",
                    ""type"": ""Button"",
                    ""id"": ""92645691-e1c2-4a10-84be-7dbd400a3dae"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""PCVector"",
                    ""id"": ""2b1a0a3c-0975-4e29-8d1b-6cbc939ca3b3"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Motion"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""3d9016d9-dd06-4e7a-8c71-ca9021d79ce3"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Motion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ff46f986-2bd9-47b4-8b2e-fca538b839ad"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Motion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""c0776e64-f35a-4ec3-a7a2-5e9fde7b8684"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Motion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""8e139c96-0e51-4c80-96fc-ff140eeb6579"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Motion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""aa8e0991-361b-4a55-b470-dd68bb77e86d"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Motion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""GamepadVector"",
                    ""id"": ""7cbcb8e1-376a-450f-95f5-21f657675dee"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""eb51f12d-7ad4-4066-b92f-bfb6bd550666"",
                    ""path"": ""<Mouse>/delta/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""4db46733-e915-47eb-bc2a-f4f8f8638718"",
                    ""path"": ""<Mouse>/delta/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""39637794-16ad-474c-bec7-b804e965d1b6"",
                    ""path"": ""<Mouse>/delta/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""dd411103-07c6-4c1f-b71d-175136bc8e45"",
                    ""path"": ""<Mouse>/delta/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""73938e23-a18f-4a71-9e43-e327fa31aea1"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""KeyboardVector"",
                    ""id"": ""0aac36f2-e1c8-4722-9e07-6c5e368c9408"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""fe31e487-fc7a-49d2-95b2-18d4164fda5c"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""d8fe4d89-2241-46d1-b21e-227623312b25"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""76c71e33-c92b-4eac-9ed7-1a00420d8d87"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""135cf21d-20ef-4269-8685-66199cfa0342"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""3ba65d11-0589-4a6a-85ca-a984d364507d"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""13e51682-cc5a-45b8-ba0b-a7fc280dd139"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""080f8d72-4fa0-4ab7-ae0a-74bcb1d3a795"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1715bea0-7545-4d04-858b-6c935e849d10"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""80d24866-693a-4898-ab42-cc50636d8475"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SprintToggle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7a0d5a7d-775a-4d2a-aafd-2b845c430f80"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SprintToggle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""67987730-c34d-4177-9588-9527f46fb09d"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DropGrenade"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5813e7a8-bdc6-4b34-8512-acd6e3c9fcb1"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DropGrenade"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8f4be120-2dc1-4995-98c9-43381c263fc2"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ThrowGrenade"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""75db29b3-4549-4b4a-8498-bae33a6796b1"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ThrowGrenade"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""21b8efec-49d9-4945-8a4c-d8e408128922"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""909bb623-4e90-4a07-83b5-cbe54572490f"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""DPadAxis"",
                    ""id"": ""4eb0ec95-e206-4f42-b7e3-1060a800b9ec"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeSensitivity"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Up"",
                    ""id"": ""90f4215e-25b2-4b0c-8563-171114c64513"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeSensitivity"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Down"",
                    ""id"": ""b9753d5f-07d3-4229-b799-da5d1f4dc3b6"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeSensitivity"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left"",
                    ""id"": ""8a9c3087-5532-43b1-880a-3e8583bb6656"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeSensitivity"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Right"",
                    ""id"": ""44a9e88c-3bd2-4868-bb42-3e40688764e8"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeSensitivity"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""dc27d51a-db21-4a68-b451-3a5b6dd587be"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Slide"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Character
        m_Character = asset.FindActionMap("Character", throwIfNotFound: true);
        m_Character_Motion = m_Character.FindAction("Motion", throwIfNotFound: true);
        m_Character_Look = m_Character.FindAction("Look", throwIfNotFound: true);
        m_Character_Jump = m_Character.FindAction("Jump", throwIfNotFound: true);
        m_Character_Reload = m_Character.FindAction("Reload", throwIfNotFound: true);
        m_Character_SprintToggle = m_Character.FindAction("SprintToggle", throwIfNotFound: true);
        m_Character_DropGrenade = m_Character.FindAction("DropGrenade", throwIfNotFound: true);
        m_Character_ThrowGrenade = m_Character.FindAction("ThrowGrenade", throwIfNotFound: true);
        m_Character_Shoot = m_Character.FindAction("Shoot", throwIfNotFound: true);
        m_Character_ChangeSensitivity = m_Character.FindAction("ChangeSensitivity", throwIfNotFound: true);
        m_Character_Slide = m_Character.FindAction("Slide", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Character
    private readonly InputActionMap m_Character;
    private List<ICharacterActions> m_CharacterActionsCallbackInterfaces = new List<ICharacterActions>();
    private readonly InputAction m_Character_Motion;
    private readonly InputAction m_Character_Look;
    private readonly InputAction m_Character_Jump;
    private readonly InputAction m_Character_Reload;
    private readonly InputAction m_Character_SprintToggle;
    private readonly InputAction m_Character_DropGrenade;
    private readonly InputAction m_Character_ThrowGrenade;
    private readonly InputAction m_Character_Shoot;
    private readonly InputAction m_Character_ChangeSensitivity;
    private readonly InputAction m_Character_Slide;
    public struct CharacterActions
    {
        private @PlayerInputActions m_Wrapper;
        public CharacterActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Motion => m_Wrapper.m_Character_Motion;
        public InputAction @Look => m_Wrapper.m_Character_Look;
        public InputAction @Jump => m_Wrapper.m_Character_Jump;
        public InputAction @Reload => m_Wrapper.m_Character_Reload;
        public InputAction @SprintToggle => m_Wrapper.m_Character_SprintToggle;
        public InputAction @DropGrenade => m_Wrapper.m_Character_DropGrenade;
        public InputAction @ThrowGrenade => m_Wrapper.m_Character_ThrowGrenade;
        public InputAction @Shoot => m_Wrapper.m_Character_Shoot;
        public InputAction @ChangeSensitivity => m_Wrapper.m_Character_ChangeSensitivity;
        public InputAction @Slide => m_Wrapper.m_Character_Slide;
        public InputActionMap Get() { return m_Wrapper.m_Character; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CharacterActions set) { return set.Get(); }
        public void AddCallbacks(ICharacterActions instance)
        {
            if (instance == null || m_Wrapper.m_CharacterActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_CharacterActionsCallbackInterfaces.Add(instance);
            @Motion.started += instance.OnMotion;
            @Motion.performed += instance.OnMotion;
            @Motion.canceled += instance.OnMotion;
            @Look.started += instance.OnLook;
            @Look.performed += instance.OnLook;
            @Look.canceled += instance.OnLook;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Reload.started += instance.OnReload;
            @Reload.performed += instance.OnReload;
            @Reload.canceled += instance.OnReload;
            @SprintToggle.started += instance.OnSprintToggle;
            @SprintToggle.performed += instance.OnSprintToggle;
            @SprintToggle.canceled += instance.OnSprintToggle;
            @DropGrenade.started += instance.OnDropGrenade;
            @DropGrenade.performed += instance.OnDropGrenade;
            @DropGrenade.canceled += instance.OnDropGrenade;
            @ThrowGrenade.started += instance.OnThrowGrenade;
            @ThrowGrenade.performed += instance.OnThrowGrenade;
            @ThrowGrenade.canceled += instance.OnThrowGrenade;
            @Shoot.started += instance.OnShoot;
            @Shoot.performed += instance.OnShoot;
            @Shoot.canceled += instance.OnShoot;
            @ChangeSensitivity.started += instance.OnChangeSensitivity;
            @ChangeSensitivity.performed += instance.OnChangeSensitivity;
            @ChangeSensitivity.canceled += instance.OnChangeSensitivity;
            @Slide.started += instance.OnSlide;
            @Slide.performed += instance.OnSlide;
            @Slide.canceled += instance.OnSlide;
        }

        private void UnregisterCallbacks(ICharacterActions instance)
        {
            @Motion.started -= instance.OnMotion;
            @Motion.performed -= instance.OnMotion;
            @Motion.canceled -= instance.OnMotion;
            @Look.started -= instance.OnLook;
            @Look.performed -= instance.OnLook;
            @Look.canceled -= instance.OnLook;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Reload.started -= instance.OnReload;
            @Reload.performed -= instance.OnReload;
            @Reload.canceled -= instance.OnReload;
            @SprintToggle.started -= instance.OnSprintToggle;
            @SprintToggle.performed -= instance.OnSprintToggle;
            @SprintToggle.canceled -= instance.OnSprintToggle;
            @DropGrenade.started -= instance.OnDropGrenade;
            @DropGrenade.performed -= instance.OnDropGrenade;
            @DropGrenade.canceled -= instance.OnDropGrenade;
            @ThrowGrenade.started -= instance.OnThrowGrenade;
            @ThrowGrenade.performed -= instance.OnThrowGrenade;
            @ThrowGrenade.canceled -= instance.OnThrowGrenade;
            @Shoot.started -= instance.OnShoot;
            @Shoot.performed -= instance.OnShoot;
            @Shoot.canceled -= instance.OnShoot;
            @ChangeSensitivity.started -= instance.OnChangeSensitivity;
            @ChangeSensitivity.performed -= instance.OnChangeSensitivity;
            @ChangeSensitivity.canceled -= instance.OnChangeSensitivity;
            @Slide.started -= instance.OnSlide;
            @Slide.performed -= instance.OnSlide;
            @Slide.canceled -= instance.OnSlide;
        }

        public void RemoveCallbacks(ICharacterActions instance)
        {
            if (m_Wrapper.m_CharacterActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ICharacterActions instance)
        {
            foreach (var item in m_Wrapper.m_CharacterActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_CharacterActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public CharacterActions @Character => new CharacterActions(this);
    public interface ICharacterActions
    {
        void OnMotion(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnReload(InputAction.CallbackContext context);
        void OnSprintToggle(InputAction.CallbackContext context);
        void OnDropGrenade(InputAction.CallbackContext context);
        void OnThrowGrenade(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnChangeSensitivity(InputAction.CallbackContext context);
        void OnSlide(InputAction.CallbackContext context);
    }
}
