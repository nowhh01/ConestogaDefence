using Microsoft.Xna.Framework.Input;

namespace ConestogaDefence
{
    class Keypad
    {
        public static KeyboardState CurrentState { get; private set; }
        public static KeyboardState OldState { get; private set; }
        private Keypad() { }

        public static void Update()
        {
            OldState = CurrentState;
            CurrentState = Keyboard.GetState();
        }
    }
}
