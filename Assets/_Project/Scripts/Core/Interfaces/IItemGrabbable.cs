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

        // Instead of a simple dot, we pass the entire owner of the hand
        void Grab(IItemHolder holder);
        void Drop();
        void Throw(Vector3 appliedForce);
    }
}