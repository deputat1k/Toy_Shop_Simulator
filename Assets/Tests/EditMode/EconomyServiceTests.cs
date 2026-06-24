using NUnit.Framework;
using ToyShop.Gameplay.Economy;

namespace ToyShop.Tests.EditMode
{
    public class EconomyServiceTests
    {
        private EconomyService _economy;

        [SetUp]
        public void SetUp()
        {
            _economy = new EconomyService();
        }

        [Test]
        public void InitialBalance_Is100()
        {
            Assert.AreEqual(100, _economy.CurrentBalance);
        }

        [Test]
        public void TrySpend_WithEnoughMoney_DeductsAndReturnsTrue()
        {
            bool result = _economy.TrySpend(40);

            Assert.IsTrue(result);
            Assert.AreEqual(60, _economy.CurrentBalance);
        }

        [Test]
        public void TrySpend_WithNotEnoughMoney_ReturnsFalseAndBalanceUnchanged()
        {
            bool result = _economy.TrySpend(1000);

            Assert.IsFalse(result);
            Assert.AreEqual(100, _economy.CurrentBalance);
        }

        [Test]
        public void TrySpend_WithNegativeOrZeroAmount_ReturnsFalse()
        {
            Assert.IsFalse(_economy.TrySpend(0));
            Assert.IsFalse(_economy.TrySpend(-10));
            Assert.AreEqual(100, _economy.CurrentBalance);
        }

        [Test]
        public void Add_IncreasesBalance()
        {
            _economy.Add(50);
            Assert.AreEqual(150, _economy.CurrentBalance);
        }

        [Test]
        public void Add_WithZeroOrNegative_DoesNothing()
        {
            _economy.Add(0);
            _economy.Add(-20);
            Assert.AreEqual(100, _economy.CurrentBalance);
        }

        [Test]
        public void SetBalance_OverwritesCurrentBalance()
        {
            _economy.SetBalance(500);
            Assert.AreEqual(500, _economy.CurrentBalance);
        }

        [Test]
        public void SetBalance_WithNegativeValue_IsIgnored()
        {
            _economy.SetBalance(-50);
            Assert.AreEqual(100, _economy.CurrentBalance);
        }

        [Test]
        public void OnBalanceChanged_FiresWithNewBalance_OnSuccessfulSpend()
        {
            int? receivedBalance = null;
            _economy.OnBalanceChanged += b => receivedBalance = b;

            _economy.TrySpend(30);

            Assert.AreEqual(70, receivedBalance);
        }

        [Test]
        public void OnBalanceChanged_DoesNotFire_OnFailedSpend()
        {
            bool fired = false;
            _economy.OnBalanceChanged += _ => fired = true;

            _economy.TrySpend(99999);

            Assert.IsFalse(fired);
        }
    }
}