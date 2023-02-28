using System.Configuration;
using DistributedInventory.Application.Commands;
using DistributedInventory.Application.Commands.Handlers;
using DistributedInventory.Domain.Entities;
using DistributedInventory.Domain.Models;
using DistributedInventory.Infrastructure;
using DistributedInventory.Infrastructure.Context;
using DistributedInventory.Infrastructure.Handlers;
using DistributedInventory.Infrastructure.Repositories;
using Microsoft.Azure.Cosmos;

namespace DistributedInventory.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            // Add services to the container.
            InitializeCosmos(builder, configuration);

            InventoryCountPartitionKeyProvider pkProvider = new();

            builder.Services
                .AddSingleton<IInventoryCountPartitionKeyProvider>(pkProvider)
                .AddScoped<IContainerContext, CosmosContainerContext>()
                .AddScoped<IDomainEventRepository, DomainEventRepository>()
                .AddScoped<IInventoryCountRepository, InventoryCountRepository>()
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddMediatR(cfg =>
                {
                    cfg.RegisterServicesFromAssemblies(
                        typeof(InventoryCount).Assembly,
                        typeof(InventoryCountCreatedHandler).Assembly,
                        typeof(CreateInventoryCountCommand).Assembly);
                });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        public static void InitializeCosmos(WebApplicationBuilder builder,
            Microsoft.Extensions.Configuration.ConfigurationManager configuration)
        {
            var cosmosOptions = new CosmosClientOptions()
            {
                SerializerOptions = new CosmosSerializationOptions()
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase,
                    IgnoreNullValues = true
                }
            };
            var cosmosSection = configuration.GetSection("Cosmos");
            var url = cosmosSection["Url"];
            var key = cosmosSection["Key"];
            var dbName = cosmosSection["Db"];
            var containerName = cosmosSection["Container"];
            var containers = new List<(string db, string container)>
            {
                (dbName, containerName)
            };

            var cosmosClient = CosmosClient.CreateAndInitializeAsync(
                url, key, containers, cosmosOptions).GetAwaiter().GetResult();
            var container = cosmosClient.GetContainer(dbName, containerName);

            builder.Services
                .AddSingleton(container);
        }
    }
}
