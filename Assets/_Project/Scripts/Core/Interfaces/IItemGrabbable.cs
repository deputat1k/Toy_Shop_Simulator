using System;
using UnityEngine;

namespace ToyShop.Core.Interfaces
{
   
    public interface IItemGrabbable
    {
     
        event Action OnGrabbed;
        event Action OnDropped;
        event Action OnThrown;

        bool IsHeld { get; }

        
        IItemHolder CurrentHolder { get; }

        // Замість простої точки, передаємо всього власника рук
        void Grab(IItemHolder holder);
        void Drop();
        void Throw(Vector3 appliedForce);
    }
}