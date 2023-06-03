using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Player
{
    public static class PlayerInputHelper
    {
        public static PlayerInput InitializePlayer(this PlayerInput input, string controlScheme)
        {
            var instance = PlayerInput.Instantiate(input.gameObject, controlScheme: controlScheme,
                pairWithDevices: new InputDevice[] { Keyboard.current });
            
            Object.Destroy(input.gameObject);
            
            return instance;
        }
    }
}