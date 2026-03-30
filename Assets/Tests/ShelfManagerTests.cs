using NUnit.Framework;
using ToyShop.Gameplay.Environment;
using ToyShop.Tests.Fakes;
using ToyShop.Core.Interfaces;
using System.Linq;

namespace ToyShop.Tests
{
    [TestFixture]
    public class ShelfManagerTests
    {
        private ShelfManager _sut; 

        [SetUp]
        public void Setup()
        {
            
            _sut = new ShelfManager();
        }

        private FakeShelfSlot[] CreateFakeSlots(int count, bool isOccupied = false)
        {
            var slots = new FakeShelfSlot[count];
            for (int i = 0; i < count; i++)
            {
                slots[i] = new FakeShelfSlot();
                if (isOccupied) slots[i].ForceOccupy();
            }
            return slots;
        }

        #region Scenario 1: Single Item Interactions

        [Test]
        public void ProcessInteraction_ValidPlaceableItem_OccupiesFirstEmptySlot()
        {
            // Arrange
            var slots = CreateFakeSlots(1);
            _sut.Initialize(slots);
            var fakeItem = new FakePlaceableItem();
            var fakeHolder = new FakeItemHolder { HeldItem = fakeItem };

            // Act
            _sut.ProcessInteraction(fakeHolder);

            // Assert
            Assert.IsTrue(slots[0].IsOccupied);
        }

        [Test]
        public void ProcessInteraction_ValidPlaceableItem_DropsItemFromHand()
        {
            // Arrange
            var slots = CreateFakeSlots(1);
            _sut.Initialize(slots);
            var fakeItem = new FakePlaceableItem();
            var fakeHolder = new FakeItemHolder { HeldItem = fakeItem };

            // Act
            _sut.ProcessInteraction(fakeHolder);

            // Assert
            Assert.IsTrue(fakeItem.WasDropped);
        }

        [Test]
        public void ProcessInteraction_NonPlaceableItem_KeepsItemInHand()
        {
            // Arrange
            var slots = CreateFakeSlots(1);
            _sut.Initialize(slots);
            var fakeItem = new FakeNonPlaceableItem();
            var fakeHolder = new FakeItemHolder { HeldItem = fakeItem };

            // Act
            _sut.ProcessInteraction(fakeHolder);

            // Assert
            Assert.IsFalse(slots[0].IsOccupied);
            Assert.AreEqual(fakeItem, fakeHolder.HeldItem);
        }

        #endregion

        #region Scenario 2: Container Interactions

        [Test]
        public void ProcessInteraction_ValidContainer_ExtractsItem()
        {
            // Arrange
            var slots = CreateFakeSlots(1);
            _sut.Initialize(slots);
            var extractableItem = new FakePlaceableItem();
            var fakeContainer = new FakeContainer(true, extractableItem);
            var fakeHolder = new FakeItemHolder { HeldItem = fakeContainer };

            // Act
            _sut.ProcessInteraction(fakeHolder);

            // Assert
            Assert.IsTrue(fakeContainer.WasExtracted);
        }

        [Test]
        public void ProcessInteraction_ValidContainer_OccupiesSlotWithExtractedItem()
        {
            // Arrange
            var slots = CreateFakeSlots(1);
            _sut.Initialize(slots);
            var extractableItem = new FakePlaceableItem();
            var fakeContainer = new FakeContainer(true, extractableItem);
            var fakeHolder = new FakeItemHolder { HeldItem = fakeContainer };

            // Act
            _sut.ProcessInteraction(fakeHolder);

            // Assert
            Assert.IsTrue(slots[0].IsOccupied);
            Assert.AreEqual(extractableItem, slots[0].OccupiedBy);
        }

        [Test]
        public void ProcessInteraction_ContainerWithUnplaceableItem_KeepsSlotEmpty()
        {
            // Arrange
            var slots = CreateFakeSlots(1);
            _sut.Initialize(slots);
            var unplaceableItem = new FakeNonPlaceableItem();
            var fakeContainer = new FakeContainer(true, unplaceableItem);
            var fakeHolder = new FakeItemHolder { HeldItem = fakeContainer };

            // Act
            _sut.ProcessInteraction(fakeHolder);

            // Assert
            Assert.IsFalse(slots[0].IsOccupied);
        }

        #endregion

        #region Scenario 3: Empty Container Interactions

        [Test]
        public void ProcessInteraction_EmptyContainer_TriggersOnEmptyContainerProvidedEvent()
        {
            // Arrange
            var slots = CreateFakeSlots(1);
            _sut.Initialize(slots);
            var fakeContainer = new FakeContainer(false);
            var fakeHolder = new FakeItemHolder { HeldItem = fakeContainer };
            bool eventTriggered = false;
            _sut.OnEmptyContainerProvided += () => eventTriggered = true;

            // Act
            _sut.ProcessInteraction(fakeHolder);

            // Assert
            Assert.IsTrue(eventTriggered);
        }

        [Test]
        public void ProcessInteraction_EmptyContainer_PreservesAvailableSlots()
        {
            // Arrange
            var slots = CreateFakeSlots(1);
            _sut.Initialize(slots);
            var fakeContainer = new FakeContainer(false);
            var fakeHolder = new FakeItemHolder { HeldItem = fakeContainer };

            // Act
            _sut.ProcessInteraction(fakeHolder);

            // Assert
            Assert.IsFalse(slots[0].IsOccupied);
        }

        [Test]
        public void ProcessInteraction_EmptyContainer_DoesNotAttemptExtraction()
        {
            // Arrange
            var slots = CreateFakeSlots(1);
            _sut.Initialize(slots);
            var fakeContainer = new FakeContainer(false);
            var fakeHolder = new FakeItemHolder { HeldItem = fakeContainer };

            // Act
            _sut.ProcessInteraction(fakeHolder);

            // Assert
            Assert.IsFalse(fakeContainer.WasExtracted);
        }

        #endregion

        #region Scenario 4: Full Shelf (Boundary Conditions)

        [Test]
        public void ProcessInteraction_WhenShelfIsFull_TriggersOnShelfFullEvent()
        {
            // Arrange
            var slots = CreateFakeSlots(2, isOccupied: true);
            _sut.Initialize(slots);
            var fakeItem = new FakePlaceableItem();
            var fakeHolder = new FakeItemHolder { HeldItem = fakeItem };
            bool eventTriggered = false;
            _sut.OnShelfFull += () => eventTriggered = true;

            // Act
            _sut.ProcessInteraction(fakeHolder);

            // Assert
            Assert.IsTrue(eventTriggered);
        }

        [Test]
        public void ProcessInteraction_WhenShelfIsFullWithContainer_DoesNotExtractItem()
        {
            // Arrange
            var slots = CreateFakeSlots(1, isOccupied: true);
            _sut.Initialize(slots);
            var extractableItem = new FakePlaceableItem();
            var fakeContainer = new FakeContainer(true, extractableItem);
            var fakeHolder = new FakeItemHolder { HeldItem = fakeContainer };

            // Act
            _sut.ProcessInteraction(fakeHolder);

            // Assert
            Assert.IsFalse(fakeContainer.WasExtracted);
        }

        [Test]
        public void ProcessInteraction_WhenShelfIsFullWithSingleItem_KeepsItemInHand()
        {
            // Arrange
            var slots = CreateFakeSlots(1, isOccupied: true);
            _sut.Initialize(slots);
            var fakeItem = new FakePlaceableItem();
            var fakeHolder = new FakeItemHolder { HeldItem = fakeItem };

            // Act
            _sut.ProcessInteraction(fakeHolder);

            // Assert
            Assert.IsFalse(fakeItem.WasDropped);
        }

        #endregion

        #region Scenario 5: State Validation

        [Test]
        public void HasEmptySlot_WithAvailableSlots_ReturnsTrue()
        {
            // Arrange
            var slots = CreateFakeSlots(3);
            slots[0].ForceOccupy();
            _sut.Initialize(slots);

            // Act
            bool result = _sut.HasEmptySlot;

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void HasEmptySlot_WithAllSlotsOccupied_ReturnsFalse()
        {
            // Arrange
            var slots = CreateFakeSlots(3, isOccupied: true);
            _sut.Initialize(slots);

            // Act
            bool result = _sut.HasEmptySlot;

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void ProcessInteraction_NullHeldItem_MaintainsCurrentShelfState()
        {
            // Arrange
            var slots = CreateFakeSlots(1);
            _sut.Initialize(slots);
            var fakeHolder = new FakeItemHolder { HeldItem = null };

            // Act
            _sut.ProcessInteraction(fakeHolder);

            // Assert
            Assert.IsFalse(slots[0].IsOccupied);
        }

        [Test]
        public void ProcessInteraction_ContainerProviderReturnsFalse_MaintainsCurrentShelfState()
        {
            // Arrange
            var slots = CreateFakeSlots(1);
            _sut.Initialize(slots);
            var fakeContainer = new FakeContainer(true) { ProviderShouldFail = true };
            var fakeHolder = new FakeItemHolder { HeldItem = fakeContainer };

            // Act
            _sut.ProcessInteraction(fakeHolder);

            // Assert
            Assert.IsFalse(slots[0].IsOccupied);
        }

        #endregion
    }
}