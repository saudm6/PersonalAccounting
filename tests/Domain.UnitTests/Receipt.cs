using NUnit.Framework;
using PersonalAccounting.Domain.Entities;

namespace PersonalAccounting.Domain.UnitTests;

[TestFixture]
public class ReceiptTests
{
    private Receipt _receipt;

    [SetUp]
    public void Setup()
    {
        _receipt = new Receipt
        {
            ReceiptName = "Groceries",
            ReceiptDate = DateOnly.FromDateTime(DateTime.Now)
        };
    }

    [Test]
    public void AddItem_Should_Add_Valid_Item_And_Update_Total()
    {
        // Arrange
        var item = new ReceiptItem
        {
            ItemName = "Milk",
            ItemPrice = 1.5m,
            ItemQuantity = 2,
            TotalPrice = 3.0m
        };

        // Act
        _receipt.AddItem(item);

        // Assert
        Assert.That(_receipt.ReceiptItems.Count, Is.EqualTo(1));
        Assert.That(_receipt.ReceiptTotal, Is.EqualTo(3.0m));
    }

    [Test]
    public void AddItem_Should_Throw_When_Item_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() => _receipt.AddItem(null!));
    }

    [Test]
    public void AddItem_Should_Throw_When_Quantity_Is_Zero_Or_Negative()
    {
        var item = new ReceiptItem { ItemPrice = 2.0m, ItemQuantity = 0 };
        Assert.Throws<ArgumentOutOfRangeException>(() => _receipt.AddItem(item));
    }

    [Test]
    public void AddItem_Should_Throw_When_Price_Is_Negative()
    {
        var item = new ReceiptItem { ItemPrice = -5.0m, ItemQuantity = 1 };
        Assert.Throws<ArgumentOutOfRangeException>(() => _receipt.AddItem(item));
    }

    [Test]
    public void AddItem_Should_Recalculate_Total_When_Multiple_Items_Added()
    {
        // Arrange
        var item1 = new ReceiptItem { ItemPrice = 2.0m, ItemQuantity = 2, TotalPrice = 4.0m };
        var item2 = new ReceiptItem { ItemPrice = 3.0m, ItemQuantity = 1, TotalPrice = 3.0m };

        // Act
        _receipt.AddItem(item1);
        _receipt.AddItem(item2);

        // Assert
        Assert.That(_receipt.ReceiptItems.Count, Is.EqualTo(2));
        Assert.That(_receipt.ReceiptTotal, Is.EqualTo(7.0m));
    }
}

[TestFixture]
public class ReceiptItemTests
{
    [Test]
    public void Create_Should_Return_Valid_ReceiptItem()
    {
        // Arrange
        var receipt = new Receipt();
        var itemName = "Bread";
        var itemPrice = 1.25m;
        var itemQuantity = 4;
        var description = "Whole grain";

        // Act
        var result = ReceiptItem.Create(itemName, itemPrice, itemQuantity, description, receipt);

        // Assert
        Assert.That(result.ItemName, Is.EqualTo(itemName));
        Assert.That(result.ItemPrice, Is.EqualTo(itemPrice));
        Assert.That(result.ItemQuantity, Is.EqualTo(itemQuantity));
        Assert.That(result.TotalPrice, Is.EqualTo(5.0m));
        Assert.That(result.Receipt, Is.EqualTo(receipt));
    }

    [Test]
    public void Create_Should_Compute_Correct_TotalPrice()
    {
        var receipt = new Receipt();
        var item = ReceiptItem.Create("Eggs", 2.5m, 6, "Fresh", receipt);

        Assert.That(item.TotalPrice, Is.EqualTo(15.0m));
    }

}

