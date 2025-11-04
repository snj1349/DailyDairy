using System;
using DailyDairyDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DailyDairyDAL.Repositories;

public class DailyDairyRepository
{
	private readonly DailyDairyDbContext _context;

	public IUserRepository Users { get; }
	public IProductRepository Products { get; }
	public ICategoryRepository Categories { get; }
	public IOrderRepository Orders { get; }
	public IOrderItemRepository OrderItems { get; }
	public IPaymentRepository Payments { get; }
	public IReviewRepository Reviews { get; }
	public ICartRepository Cart { get; }

	public DailyDairyRepository()
	{
		var options = BuildDbContextOptions();
		_context = new DailyDairyDbContext(options);

		Users = new UserRepository(_context);
		Products = new ProductRepository(_context);
		Categories = new CategoryRepository(_context);
		Orders = new OrderRepository(_context);
		OrderItems = new OrderItemRepository(_context);
		Payments = new PaymentRepository(_context);
		Reviews = new ReviewRepository(_context);
		Cart = new CartRepository(_context);
	}

	private static DbContextOptions<DailyDairyDbContext> BuildDbContextOptions()
	{
		var builder = new ConfigurationBuilder()
			.SetBasePath(AppContext.BaseDirectory)
			.AddJsonFile("appsettings.json", optional: true)
			.AddJsonFile("..\\..\\..\\DailyDairyDAL\\appsettings.json", optional: true)
			.AddEnvironmentVariables();
		var config = builder.Build();

		var connectionString = config.GetConnectionString("DailyDairyDBConnection");
		if (string.IsNullOrWhiteSpace(connectionString))
		{
			connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DailyDairyDB;Integrated Security=true";
		}

		var optionsBuilder = new DbContextOptionsBuilder<DailyDairyDbContext>();
		optionsBuilder.UseSqlServer(connectionString);
		return optionsBuilder.Options;
	}
}


