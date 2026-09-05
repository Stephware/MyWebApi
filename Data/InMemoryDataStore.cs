using System.Collections.Concurrent;
using MyWebApi.Models;

namespace MyWebApi.Data;

public class InMemoryDataStore
{
    public ConcurrentDictionary<int, Product> Products { get; } = new();
    public ConcurrentDictionary<int, Category> Categories { get; } = new();

    private int _nextProductId;
    private int _nextCategoryId;
    private readonly object _seedLock = new();
    private bool _seeded;

    public int GetNextProductId() => Interlocked.Increment(ref _nextProductId);
    public int GetNextCategoryId() => Interlocked.Increment(ref _nextCategoryId);

    public void Seed()
    {
        lock (_seedLock)
        {
            if (_seeded)
            {
                return;
            }

            var categories = new[]
            {
                new Category { Id = GetNextCategoryId(), Name = "Electronics", Description = "Devices and gadgets" },
                new Category { Id = GetNextCategoryId(), Name = "Office Supplies", Description = "Everyday office items" },
                new Category { Id = GetNextCategoryId(), Name = "Industrial", Description = "Tools and industrial equipment" },
                new Category { Id = GetNextCategoryId(), Name = "Groceries", Description = "Consumable goods" }
            };

            foreach (var category in categories)
            {
                Categories[category.Id] = category;
            }

            var sampleProducts = new List<(string Name, string Desc, decimal Price, int Stock, int CatIdx, string[] Tags)>
            {
                ("Wireless Mouse", "Ergonomic 2.4GHz wireless mouse", 19.99m, 150, 0, new[] { "wireless", "accessory" }),
                ("Mechanical Keyboard", "RGB backlit mechanical keyboard", 79.99m, 60, 0, new[] { "wireless", "gaming" }),
                ("27-inch Monitor", "1440p IPS monitor", 249.99m, 25, 0, new[] { "display" }),
                ("USB-C Hub", "7-in-1 USB-C hub", 34.50m, 0, 0, new[] { "accessory" }),
                ("Laptop Stand", "Aluminum adjustable laptop stand", 29.95m, 80, 0, Array.Empty<string>()),
                ("A4 Copy Paper (Ream)", "500 sheets, 80gsm", 4.99m, 500, 1, new[] { "paper" }),
                ("Stapler", "Heavy-duty desktop stapler", 8.25m, 120, 1, Array.Empty<string>()),
                ("Ballpoint Pens (Box of 12)", "Blue ink, medium tip", 3.60m, 300, 1, new[] { "writing" }),
                ("Sticky Notes (Pack)", "3x3 inch, assorted colors", 5.10m, 210, 1, Array.Empty<string>()),
                ("Office Chair", "Ergonomic mesh-back chair", 145.00m, 15, 1, new[] { "furniture" }),
                ("Cordless Drill", "18V lithium-ion cordless drill", 89.99m, 40, 2, new[] { "power-tool" }),
                ("Safety Helmet", "ANSI-rated hard hat", 14.75m, 200, 2, new[] { "safety" }),
                ("Work Gloves (Pair)", "Cut-resistant work gloves", 9.99m, 180, 2, new[] { "safety" }),
                ("Tool Box", "26-inch steel tool box", 55.00m, 30, 2, Array.Empty<string>()),
                ("Extension Cord 50ft", "Heavy-duty outdoor extension cord", 32.40m, 0, 2, Array.Empty<string>()),
                ("Organic Coffee Beans 1kg", "Single-origin arabica", 18.50m, 90, 3, new[] { "organic" }),
                ("Green Tea (Box of 20)", "Loose-leaf sachets", 6.20m, 140, 3, new[] { "organic" }),
                ("Sparkling Water (12-pack)", "500ml cans", 11.99m, 75, 3, Array.Empty<string>()),
                ("Granola Bars (Box of 24)", "Oats and honey", 13.30m, 60, 3, new[] { "snack" }),
                ("Olive Oil 1L", "Extra virgin, cold-pressed", 12.80m, 55, 3, new[] { "organic" })
            };

            foreach (var item in sampleProducts)
            {
                var id = GetNextProductId();

                Products[id] = new Product
                {
                    Id = id,
                    Name = item.Name,
                    Description = item.Desc,
                    Sku = $"SKU-{id}",
                    Price = item.Price,
                    StockQuantity = item.Stock,
                    CategoryId = categories[item.CatIdx].Id,
                    IsActive = true,
                    Tags = item.Tags.ToList(),
                    Created = DateTime.Now
                };
            }

            _seeded = true;
        }
    }
}
