using crop_api_elastic_demo.Infra;
using crop_api_elastic_demo.Mappers;
using crop_api_elastic_demo.Services;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);


#region [Database]
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection(nameof(DatabaseSettings)));
builder.Services.AddSingleton<IDatabaseSettings>(sp => sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);
#endregion

#region [RabbitMQ]
builder.Services.Configure<RabbitSettings>(builder.Configuration.GetSection(nameof(RabbitSettings)));
builder.Services.AddSingleton<IRabbitSettings>(sp => sp.GetRequiredService<IOptions<RabbitSettings>>().Value);
#endregion

#region [DI]
builder.Services.AddSingleton(typeof(IMongoRepository<>), typeof(MongoRepository<>));
builder.Services.AddSingleton(typeof(IRabbitClient), typeof(RabbitClient));
builder.Services.AddSingleton<CropService>();
#endregion

#region [AutoMapper]
builder.Services.AddAutoMapper(typeof(EntityToViewModelMapping), typeof(ViewModelToEntityMapping));
#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

