using DailyDairyDAL.Models;
using DailyDairyDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace DailyDairyConsoleApp
{
	internal class Program
	{
		public static DailyDairyRepository Repository { get; set; }

		static Program()
		{
			Repository = new DailyDairyRepository();
		}

		static async Task Main(string[] args)
		{
			await TestGetAllUsers();
			await TestGetAllProducts();
			await TestAddProduct();
			await TestPlaceOrder();
			await TestGetOrdersByUser();
			await TestUpdateOrderStatus();
			await TestDeleteProduct();
		}

		public static async Task TestGetAllUsers()
		{
			var users = await Repository.Users.GetAllAsync();
			Console.WriteLine("\n=== User List ===");
			if (users != null)
			{
				foreach (var user in users)
					Console.WriteLine($"ID: {user.UserId}, Name: {user.FullName}, Email: {user.Email}");
			}
		}

		public static async Task TestGetAllProducts()
		{
			var products = await Repository.Products.GetAllAsync();
			Console.WriteLine("\n=== Product List ===");
			if (products != null)
			{
				foreach (var product in products)
					Console.WriteLine($"ID: {product.ProductId}, Name: {product.Name}, Price: ₹{product.Price}");
			}
		}

		public static async Task TestAddProduct()
		{
			var product = new Product
			{
				Name = "Paneer 200g",
				Description = "Fresh homemade paneer",
				Price = 120.00M,
				StockQuantity = 25,
				CategoryId = 3
			};
			await Repository.Products.AddAsync(product);
			Console.WriteLine("✅ Product added successfully!");
		}

		public static async Task TestPlaceOrder()
		{
			var order = await Repository.Orders.AddAsync(new Order
			{
				UserId = 1,
				TotalAmount = 180.00M,
				Status = "Pending",
				DeliveryAddress = "Customer Address"
			});
			Console.WriteLine($"✅ Order placed with ID: {order.OrderId}");
		}

		public static async Task TestGetOrdersByUser()
		{
			var orders = await Repository.Orders.GetOrdersByUserAsync(1);
			Console.WriteLine("\n=== Orders of User ID 1 ===");
			foreach (var order in orders)
				Console.WriteLine($"Order ID: {order.OrderId}, Status: {order.Status}, Amount: ₹{order.TotalAmount}");
		}

		public static async Task TestUpdateOrderStatus()
		{
			var result = await Repository.Orders.UpdateOrderStatusAsync(1, "Delivered");
			Console.WriteLine(result ? "✅ Order status updated to Delivered." : "⚠️ Order not found or update failed.");
		}

		public static async Task TestDeleteProduct()
		{
			var result = await Repository.Products.DeleteAsync(3);
			Console.WriteLine(result ? "✅ Product deleted successfully." : "⚠️ Product not found.");
		}
	}
}
