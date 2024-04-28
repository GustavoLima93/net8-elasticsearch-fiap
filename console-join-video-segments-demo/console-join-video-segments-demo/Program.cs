// See https://aka.ms/new-console-template for more information

using console_join_video_segments_demo.Infra;
using console_join_video_segments_demo.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        
        services.Configure<RabbitSettings>(context.Configuration.GetSection(nameof(RabbitSettings)));
        #region [RabbitMQ]
        services.Configure<RabbitSettings>(context.Configuration.GetSection(nameof(RabbitSettings)));
        services.AddSingleton<IRabbitSettings>(sp => sp.GetRequiredService<IOptions<RabbitSettings>>().Value);
        #endregion

        #region [DI]
        services.AddSingleton(typeof(IRabbitClient), typeof(RabbitClient));
        services.AddHostedService<JoinService>();
        #endregion
    })
    .Build();

host.Run();



