using NUnit.Framework;
using ToyShop.Data;
using ToyShop.Gameplay.Cart;
using UnityEngine;

namespace ToyShop.Tests.EditMode
{
    public class CartServiceTests
    {
        private CartService _cart;
        private ToyData _toyA;
        private ToyData _toyB;

        [SetUp]
        public void SetUp()
        {
            _cart = new CartService();
            _toyA = CreateToy("toy_a", "Teddy Bear", 20);
            _toyB = CreateToy("toy_b", "Robot", 35);
        }

        private static ToyData CreateToy(string id, string name, int price)
        {
            var toy = ScriptableObject.CreateInstance<ToyData>();
            toy.Id = id;
            toy.DisplayName = name;
            toy.PurchasePrice = price;
            return toy;
        }

        [Test]
        public void AddItem_NewToy_AddsWithQuantityOne()
        {
            _cart.AddItem(_toyA);

            Assert.IsTrue(_cart.HasItem("toy_a"));
            Assert.AreEqual(1, _cart.GetItem("toy_a").Quantity);
        }

        [Test]
        public void AddItem_SameToyTwice_IncreasesQuantityInsteadOfDuplicating()
        {
            _cart.AddItem(_toyA);
            _cart.AddItem(_toyA);

            Assert.AreEqual(2, _cart.GetItem("toy_a").Quantity);
            Assert.AreEqual(1, _cart.Items.Count);
        }

        [Test]
        public void TotalPrice_SumsLineTotalsAcrossDifferentToys()
        {
            _cart.AddItem(_toyA); // 20
            _cart.AddItem(_toyB); // 35
            _cart.AddItem(_toyB); // +35

            Assert.AreEqual(90, _cart.TotalPrice);
        }

        [Test]
        public void TotalItems_CountsQuantitiesNotUniqueToys()
        {
            _cart.AddItem(_toyA);
            _cart.AddItem(_toyA);
            _cart.AddItem(_toyB);

            Assert.AreEqual(3, _cart.TotalItems);
        }

        [Test]
        public void ChangeQuantity_Increase_UpdatesQuantityAndTotal()
        {
            _cart.AddItem(_toyA);
            _cart.ChangeQuantity("toy_a", +2);

            Assert.AreEqual(3, _cart.GetItem("toy_a").Quantity);
            Assert.AreEqual(60, _cart.TotalPrice);
        }

        [Test]
        public void ChangeQuantity_ReachingZero_RemovesItemFromCart()
        {
            _cart.AddItem(_toyA);
            _cart.ChangeQuantity("toy_a", -1);

            Assert.IsFalse(_cart.HasItem("toy_a"));
            Assert.AreEqual(0, _cart.TotalItems);
        }

        [Test]
        public void RemoveItem_RemovesRegardlessOfQuantity()
        {
            _cart.AddItem(_toyA);
            _cart.ChangeQuantity("toy_a", +5);
            _cart.RemoveItem("toy_a");

            Assert.IsFalse(_cart.HasItem("toy_a"));
        }

        [Test]
        public void Clear_EmptiesCartCompletely()
        {
            _cart.AddItem(_toyA);
            _cart.AddItem(_toyB);
            _cart.Clear();

            Assert.AreEqual(0, _cart.Items.Count);
            Assert.AreEqual(0, _cart.TotalPrice);
        }

        [Test]
        public void GetItem_ForToyNotInCart_ReturnsNull()
        {
            Assert.IsNull(_cart.GetItem("nonexistent"));
        }

        [Test]
        public void OnCartChanged_FiresExactlyOnce_PerAddItem()
        {
            int callCount = 0;
            _cart.OnCartChanged += () => callCount++;

            _cart.AddItem(_toyA);

            Assert.AreEqual(1, callCount);
        }

        [Test]
        public void AddItem_WithNullToy_DoesNothingAndDoesNotThrow()
        {
            Assert.DoesNotThrow(() => _cart.AddItem(null));
            Assert.AreEqual(0, _cart.Items.Count);
        }
    }
}