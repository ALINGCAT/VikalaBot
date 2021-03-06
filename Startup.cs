using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using OneBot.CommandRoute.Mixin;
using OneBot.CommandRoute.Models.VO;
using OneBot.CommandRoute.Services;
using VikalaBot.Middleware;
using VikalaBot.Modules;
using VikalaBot.Database;
using Microsoft.EntityFrameworkCore;
using System;

namespace VikalaBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var ConnectionString = Configuration.GetSection("ConnectionString").Value;
            services.AddDbContext<VikalaDbContext>(
                options => options
                .UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString))
            );
            

            // 配置机器人核心
            // 设置 OneBot 配置
            services.Configure<CQHttpServerConfigModel>(Configuration.GetSection("CQHttpConfig"));
            services.ConfigureOneBot();



            // 添加中间件
            // 单例模式或原型模式都可以，问题不大。
            // services.AddSingleton<IOneBotMiddleware, TestMiddleware>();
            // services.AddScoped<IOneBotMiddleware, TestMiddleware>();

            // 添加指令 / 事件
            // 推荐使用单例模式（而实际上框架代码也是当单例模式使用的）
            // services.AddSingleton<IOneBotController, TestModule>();
            // 一行一行地将指令模块加进去
            services.AddSingleton<IOneBotController, GBFModule>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure()
        {
            /*
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
            */

            // 再也不需要手动启动 Sora 服务了
            // 具体改动见 https://github.com/ParaParty/OneBot-CommandRoute-Template/issues/2
        }
    }
}
