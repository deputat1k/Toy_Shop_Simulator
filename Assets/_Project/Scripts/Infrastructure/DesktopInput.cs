using System;
using ToyShop.Core.Interfaces;
using UnityEngine;
using Zenject;

namespace ToyShop.Infrastructure
{

    public class DesktopInput : IInputProvider, ITickable
    {
        [Inject] private SignalBus _signalBus;

        public event Action OnTabletTogglePressed;

        public Vector2 GetMovementDirection()
        {
            Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (direction.sqrMagnitude > 1f) direction.Normalize();
            return direction;
        }

        public bool IsInteractActionTriggered() => Input.GetKeyDown(KeyCode.E);

        public bool IsThrowActionTriggered() => Input.GetMouseButtonDown(0);

        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                OnTabletTogglePressed?.Invoke();
                _signalBus.Fire<InputTabletToggleSignal>(); 
            }
        }
    }
}