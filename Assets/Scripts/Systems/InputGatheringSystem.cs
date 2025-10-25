using UnityEngine;
using Unity.Entities;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[UpdateInGroup(typeof(InitializationSystemGroup))]
partial class InputGatheringSystem : SystemBase
#if ENABLE_INPUT_SYSTEM
        ,
    InputActions.ICharacterControllerActions
#endif
    {
        EntityQuery m_CharacterControllerInputQuery;

#pragma warning disable 0649
        bool m_CharacterAction0Pressed;
        bool m_CharacterAction0Held;
        
        bool m_CharacterAction1Pressed;
        bool m_CharacterAction1Held;
        
        bool m_CharacterAction2Pressed;
        bool m_CharacterAction2Held;
        
        bool m_CharacterAction3Pressed;
        bool m_CharacterAction3Held;
        
        bool m_CharacterAction4Pressed;
        bool m_CharacterAction4Held;
#pragma warning restore 0649

    protected override void OnCreate()
    {
#if ENABLE_INPUT_SYSTEM
            m_InputActions = new InputActions();
            m_InputActions.CharacterController.SetCallbacks(this);
            //Debug.Log("InputSystem Found");
            OnPlayerSelect();
#endif

        m_CharacterControllerInputQuery = GetEntityQuery(typeof(CharacterControllerInput));
    }

#if ENABLE_INPUT_SYSTEM
        InputActions m_InputActions;

        public void OnPlayerSelect() => m_InputActions.Enable();
        public void OnPlayerDeselect() => m_InputActions.Disable();

        void InputActions.ICharacterControllerActions.OnAbility0(InputAction.CallbackContext context)
        {
            if (context.started) //Reset this on consumtion of input rather than every frame
                m_CharacterAction0Pressed = true;

            if (context.performed)
                m_CharacterAction0Held = true;

            if (context.canceled)
                m_CharacterAction0Held = false;
        }

        void InputActions.ICharacterControllerActions.OnAbility1(InputAction.CallbackContext context)
        {
            if (context.started)
                m_CharacterAction1Pressed = true;

            if (context.performed)
                m_CharacterAction1Held = true;

            if (context.canceled)
                m_CharacterAction1Held = false;
        }
        void InputActions.ICharacterControllerActions.OnAbility2(InputAction.CallbackContext context)
        {
            if (context.started)
                m_CharacterAction2Pressed = true;

            if (context.performed)
                m_CharacterAction2Held = true;

            if (context.canceled)
                m_CharacterAction2Held = false;
        }
        void InputActions.ICharacterControllerActions.OnAbility3(InputAction.CallbackContext context)
        {
            if (context.started)
                m_CharacterAction3Pressed = true;

            if (context.performed)
                m_CharacterAction3Held = true;

            if (context.canceled)
                m_CharacterAction3Held = false;
        }
        void InputActions.ICharacterControllerActions.OnAbility4(InputAction.CallbackContext context)
        {
            if (context.started)
                m_CharacterAction4Pressed = true;

            if (context.performed)
                m_CharacterAction4Held = true;

            if (context.canceled)
                m_CharacterAction4Held = false;
        }
#endif

    protected override void OnUpdate()
    {
        if(m_CharacterControllerInputQuery.CalculateEntityCount() == 0)
            EntityManager.CreateEntity(typeof(CharacterControllerInput));
        
        m_CharacterControllerInputQuery.SetSingleton(new CharacterControllerInput
        {
            Ability0Pressed = m_CharacterAction0Pressed,
            Ability0Held = m_CharacterAction0Held,
            
            Ability1Pressed = m_CharacterAction1Pressed,
            Ability1Held = m_CharacterAction1Held,
            
            Ability2Pressed = m_CharacterAction2Pressed,
            Ability2Held = m_CharacterAction2Held,
            
            Ability3Pressed = m_CharacterAction3Pressed,
            Ability3Held = m_CharacterAction3Held,
            
            Ability4Pressed = m_CharacterAction4Pressed,
            Ability4Held = m_CharacterAction4Held,
        });
        
        m_CharacterAction0Pressed = false;
        m_CharacterAction1Pressed = false;
        m_CharacterAction2Pressed = false;
        m_CharacterAction3Pressed = false;
        m_CharacterAction4Pressed = false;
    }
}
