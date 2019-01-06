
using Services.DependencyInjection;
using UnityEngine;

namespace LegendsForTheDead.Game
{

    public class CheckForEventManagerCanvas : BaseMonoObject
    {
        
        protected override void Awake()
        {
            var found = FindObjectOfType<UnityEngine.EventSystems.EventSystem>();
            if (found == null)
            {
                gameObject.AddComponent<UnityEngine.EventSystems.EventSystem>();
                gameObject.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }

            // Always Call base Method at last!
            // This ensures, that this object will fall off if error was thrown
            base.Awake();
        }

        public override void ClearLocalContext()
        {
            // Always Call base Method at last!
            // This ensures, that this object will fall off if error was thrown
            base.ClearLocalContext();
        }


    }
}
