using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Builder;
using Microsoft.Bot.Builder.Dialogs.Internals;

namespace InsuranceBotMaster.Dialogs.Technical
{
    public class PostUnhandledExceptionToUserModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PostUnhandledExceptionToUser>().Keyed<IPostToBot>(typeof(PostUnhandledExceptionToUser))
                .InstancePerLifetimeScope();

            RegisterAdapterChain<IPostToBot>(builder,
                    typeof(EventLoopDialogTask),
                    typeof(SetAmbientThreadCulture),
                    typeof(PersistentDialogTask),
                    typeof(ExceptionTranslationDialogTask),
                    typeof(SerializeByConversation),
                    typeof(PostUnhandledExceptionToUser),
                    typeof(LogPostToBot)
                )
                .InstancePerLifetimeScope();
        }

        public static IRegistrationBuilder<TLimit, SimpleActivatorData, SingleRegistrationStyle>
            RegisterAdapterChain<TLimit>(ContainerBuilder builder, params Type[] types)
        {
            return
                builder
            .Register(c =>
                                 {
                                     // http://stackoverflow.com/questions/23406641/how-to-mix-decorators-in-autofac
                                     TLimit service = default(TLimit);
                                     for (int index = 0; index < types.Length; ++index)
                                     {
                                         // resolve the keyed adapter, passing the previous service as the inner parameter
                                         service = c.ResolveKeyed<TLimit>(types[index], TypedParameter.From(service));
                                     }

                                     return service;
                                 })
            .As<TLimit>();
        }
    }
}