
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PersonalAccounting.Domain.Entities;

namespace PersonalAccounting.Domain.UnitTests
{
    [TestFixture]
    public class Receipt_InvalidValues_Tests
    {
        private Receipt _receipt;

        [SetUp]
        public void Setup()
        {
            _receipt = new Receipt
            {
                ReceiptName = "Invalids",
                ReceiptDate = DateOnly.FromDateTime(DateTime.Now)
            };
        }

        [Test]
        public void AddItem_Throws_When_Item_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => _receipt.AddItem(null!));
        }

        [Test]
        public void AddItem_Throws_When_Quantity_Is_Zero()
        {
            var item = new ReceiptItem
            {
                ItemPrice = 10m,
                ItemQuantity = 0,
                TotalPrice = 0m
            };

            Assert.Throws<ArgumentOutOfRangeException>(() => _receipt.AddItem(item));
        }

        [Test]
        public void AddItem_Throws_When_Quantity_Is_Negative()
        {
            var item = new ReceiptItem
            {
                ItemPrice = 10m,
                ItemQuantity = -1,
                TotalPrice = -10m
            };

            Assert.Throws<ArgumentOutOfRangeException>(() => _receipt.AddItem(item));
        }

        [Test]
        public void AddItem_Throws_When_Price_Is_Negative()
        {
            var item = new ReceiptItem
            {
                ItemPrice = -5m,
                ItemQuantity = 2,
                TotalPrice = -10m
            };

            Assert.Throws<ArgumentOutOfRangeException>(() => _receipt.AddItem(item));
        }

        [Test]
        public void AddItem_Allows_ZeroPrice_As_Valid_But_Positive_Quantity()
        {
            var item = new ReceiptItem
            {
                ItemPrice = 0m,
                ItemQuantity = 3,
                TotalPrice = 0m
            };

            _receipt.AddItem(item);

            Assert.That(_receipt.ReceiptItems.Count, Is.EqualTo(1));
            Assert.That(_receipt.ReceiptTotal, Is.EqualTo(0m));
        }

        // --- “Wrong value” cases (mismatched or negative TotalPrice passed in) ---

        [Test]
        public void AddItem_CurrentBehavior_Uses_Item_TotalPrice_As_Is_Even_If_Mismatched()
        {
            // NOTE: Code validates price>=0 and qty>0, but does NOT recompute or validate TotalPrice.
            // This test documents current behavior (potential bug): ReceiptTotal sums the provided TotalPrice.
            var item = new ReceiptItem
            {
                ItemPrice = 10m,
                ItemQuantity = 2,   // Should imply 20m
                TotalPrice = 123m   // WRONG on purpose
            };

            _receipt.AddItem(item);

            Assert.That(_receipt.ReceiptTotal, Is.EqualTo(123m),
                "Current behavior: ReceiptTotal relies on each item's TotalPrice as provided.");
        }

        [Test]
        public void AddItem_CurrentBehavior_Allows_Negative_TotalPrice_If_PriceQty_Valid()
        {
            // price >= 0 and qty > 0 pass validation; negative TotalPrice is not checked in AddItem.
            var item = new ReceiptItem
            {
                ItemPrice = 1m,
                ItemQuantity = 1,
                TotalPrice = -50m // WRONG on purpose
            };

            _receipt.AddItem(item);

            Assert.That(_receipt.ReceiptTotal, Is.EqualTo(-50m),
                "Current behavior allows negative ReceiptTotal due to trusting item.TotalPrice.");
        }
    }

    [TestFixture]
    public class ReceiptMultipleItemsTests
    {
        private Receipt _receipt;

        [SetUp]
        public void Setup()
        {
            _receipt = new Receipt
            {
                ReceiptName = "Mega Purchase",
                ReceiptDate = DateOnly.FromDateTime(DateTime.Now)
            };
        }

        [TestCase(1.5, 1, 1.5)]
        [TestCase(2.0, 3, 6.0)]
        [TestCase(10.0, 5, 50.0)]
        [TestCase(0.99, 10, 9.9)]
        [TestCase(15.5, 2, 31.0)]
        [TestCase(100, 1, 100)]
        [TestCase(7.25, 4, 29.0)]
        [TestCase(0.5, 20, 10.0)]
        [TestCase(999.99, 2, 1999.98)]
        [TestCase(3.33, 3, 9.99)]
        public void AddItem_Should_Correctly_Add_And_Recalculate_Total(decimal price, int quantity, decimal expectedTotal)
        {
            // Arrange
            var item = new ReceiptItem
            {
                ItemPrice = price,
                ItemQuantity = quantity,
                TotalPrice = price * quantity
            };

            // Act
            _receipt.AddItem(item);

            // Assert
            Assert.That(_receipt.ReceiptItems.Count, Is.GreaterThan(0));
            Assert.That(_receipt.ReceiptTotal, Is.EqualTo(_receipt.ReceiptItems.Sum(i => i.TotalPrice)));
            Assert.That(_receipt.ReceiptTotal, Is.GreaterThan(0));
        }

        [Test]
        public void AddItem_Should_Handle_Multiple_Diverse_Items()
        {
            // Arrange
            var items = new List<ReceiptItem>
            {
                new() { ItemPrice = 1.25m, ItemQuantity = 4, TotalPrice = 5.0m },
                new() { ItemPrice = 2.5m,  ItemQuantity = 2, TotalPrice = 5.0m },
                new() { ItemPrice = 10.0m, ItemQuantity = 1, TotalPrice = 10.0m },
                new() { ItemPrice = 0.99m, ItemQuantity = 10, TotalPrice = 9.9m },
                new() { ItemPrice = 50.0m, ItemQuantity = 3, TotalPrice = 150.0m }
            };

            decimal expectedTotal = items.Sum(i => i.TotalPrice);

            // Act
            foreach (var item in items)
                _receipt.AddItem(item);

            // Assert
            Assert.That(_receipt.ReceiptItems.Count, Is.EqualTo(items.Count));
            Assert.That(_receipt.ReceiptTotal, Is.EqualTo(expectedTotal));
        }

        [Test]
        public void AddItem_Should_Accumulate_Total_After_Each_Addition()
        {
            // Arrange
            var items = new List<(decimal price, int qty)>
            {
                (5.0m, 2), (10.0m, 1), (2.5m, 3), (20m, 2)
            };

            decimal runningTotal = 0m;

            // Act + Assert Step-by-Step
            foreach (var (price, qty) in items)
            {
                var item = new ReceiptItem
                {
                    ItemPrice = price,
                    ItemQuantity = qty,
                    TotalPrice = price * qty
                };

                _receipt.AddItem(item);
                runningTotal += price * qty;

                Assert.That(_receipt.ReceiptTotal, Is.EqualTo(runningTotal),
                    $"Failed after adding item {price} × {qty}");
            }
        }

        [Test]
        public void AddItem_Should_Handle_High_Price_And_High_Quantity_Without_Overflow()
        {
            // Arrange
            var expensiveItem = new ReceiptItem
            {
                ItemPrice = 1_000_000m,
                ItemQuantity = 1_000,
                TotalPrice = 1_000_000m * 1_000
            };

            // Act
            _receipt.AddItem(expensiveItem);

            // Assert
            Assert.That(_receipt.ReceiptTotal, Is.EqualTo(1_000_000_000m));
        }

        [Test]
        public void AddItem_Should_Work_For_Decimal_Fractional_Prices()
        {
            // Arrange
            var item1 = new ReceiptItem { ItemPrice = 0.333m, ItemQuantity = 3, TotalPrice = 0.999m };
            var item2 = new ReceiptItem { ItemPrice = 0.666m, ItemQuantity = 3, TotalPrice = 1.998m };

            // Act
            _receipt.AddItem(item1);
            _receipt.AddItem(item2);

            // Assert (rounded total)
            Assert.That(Math.Round(_receipt.ReceiptTotal, 3), Is.EqualTo(2.997m));
        }
    }
}
