using Avalonia;
using Avalonia.ReactiveUI;
using Serilog;
//using Avalonia.Logging;
using System;

namespace SutUpdateAv
{
    internal sealed class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        // Avalonia configuration, don't remove; also used by visual designer.
        //public static AppBuilder BuildAvaloniaApp()
        //    => AppBuilder.Configure<App>()
        //        .UsePlatformDetect()
        //        .WithInterFont()
        //        .LogToTrace()
        //        .UseReactiveUI();

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
        {

            var builder = AppBuilder.Configure<App>();


            //            builder.UseSerilog() =>
            //            {
            //                loggerConfiguration
            //                    .ReadFrom.Configuration(ctx.Configuration)
            //                    .Enrich.FromLogContext()
            //                    .Enrich.WithProperty("Application", typeof(Program).Assembly.GetName().Name)
            //                    .Enrich.WithProperty("Environment", ctx.HostingEnvironment)
            //                    //.Enrich.WithProcessId()
            //                    .Enrich.WithThreadId();

            //#if DEBUG



            //                // Used to filter out potentially bad data due debugging.
            //                // Very useful when doing Seq dashboards and want to remove logs under debugging session.
            //                loggerConfiguration.Enrich.WithProperty("DebuggerAttached", Debugger.IsAttached);
            //#endif
            //            });


            // .WriteTo.Async(a => a.RollingFile("logs\\myapp-{Date}.txt",
            //    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level} {Type}] {Message}{NewLine}{Exception} {SourceContext}"))
            //.CreateLogger();


            //Log.Logger = new LoggerConfiguration()
            //     //.Filter.ByIncludingOnly(Matching.WithProperty("Area", LogArea.Control))
            //     .MinimumLevel.Verbose()
            //     .WriteTo.File("log.txt",
            //        rollingInterval: RollingInterval.Day,
            //        rollOnFileSizeLimit: true)
            //     .CreateLogger();

            Log.Logger = new LoggerConfiguration()
                 //.Filter.ByIncludingOnly(Matching.WithProperty("Area", LogArea.Control))
                 .MinimumLevel.Verbose()
                  .WriteTo.File("logs\\sutupdate-.txt", 
                  rollingInterval: RollingInterval.Day, 
                  outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level} {Type}] {Message}{NewLine}{Exception} {SourceContext}" )
                 .CreateLogger();





            //Log.Logger = new LoggerConfiguration()
            //     //.Filter.ByIncludingOnly(Matching.WithProperty("Area", LogArea.Control))
            //     .MinimumLevel.Verbose()
            //     .WriteTo.File("log.txt",
            //        rollingInterval: RollingInterval.Day,
            //        rollOnFileSizeLimit: true)
            //     .CreateLogger();

            // SerilogLogger.Initialize(Log.Logger);



            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                //.LogToDebug()
                .LogToTrace()
                .UseReactiveUI();

    
        }
    }
}
