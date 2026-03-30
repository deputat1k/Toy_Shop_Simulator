using System;
using UnityEngine;
using ToyShop.Core.Interfaces;

namespace ToyShop.Tests.Fakes
{
    public class FakeContainer : IItemGrabbable, IItemContainer, IContainerProvider
    {
        public event Action OnGrabbed;
        public event Action OnDropped;
        public event Action OnThrown;
        public event Action OnItemExtracted;
        public event Action OnContainerEmpty;

        public bool IsHeld { get; set; }
        public IItemHolder CurrentHolder { get; set; }

        public bool CanExtract { get; set; }
        public IItemGrabbable ItemToExtract { get; set; }
        public bool WasExtracted { get; private set; }
        public bool ProviderShouldFail { get; set; } = false;

        public FakeContainer(bool canExtract, IItemGrabbable itemToExtract = null)
        {
            CanExtract = canExtract;
            ItemToExtract = itemToExtract;
        }

        public bool TryGetContainer(out IItemContainer container)
        {
            if (ProviderShouldFail)
            {
                container = null;
                return false;
            }
            container = this;
            return true;
        }

        public bool TryExtract(out IItemGrabbable extractedItem)
        {
            if (!CanExtract)
            {
                extractedItem = null;
                return false;
            }
            WasExtracted = true;
            extractedItem = ItemToExtract;
            return true;
        }

        public void Drop() { }
        public void Grab(IItemHolder holder) { }
        public void Throw(Vector3 appliedForce) { }
    }
}