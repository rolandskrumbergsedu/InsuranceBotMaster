﻿using System;
using System.Runtime.Caching;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using InsuranceBotMaster.AIML;
using InsuranceBotMaster.AIML.Utils;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;

namespace InsuranceBotMaster
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Conversation.UpdateContainer(builder =>
            {
                var store = new InMemoryDataStore();
                builder.Register(c => store)
                    .Keyed<IBotDataStore<BotData>>(AzureModule.Key_DataStore)
                    .AsSelf()
                    .SingleInstance();
            });

            var settingsPath = HttpContext.Current.Server.MapPath("~/bin/ConfigurationFiles/Settings.xml");
            var aimlPath = HttpContext.Current.Server.MapPath("~/bin/AIMLFiles");
            var basePath = HttpContext.Current.Server.MapPath("~/bin");

            var bot = new Bot(basePath);
            bot.LoadSettings(settingsPath);
            var loader = new AIMLLoader(bot);
            loader.LoadAIML(aimlPath);

            var cache = MemoryCache.Default;
            cache.Set("bot", bot, new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddDays(1.0)
            });
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();

            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Error($"Global exception: {ex.Message}");
            logger.Error($"Global stacktrace: {ex.StackTrace}");
        }
    }
}
