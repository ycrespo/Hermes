using Autofac;
using Hermes.Core.Gateways;
using Hermes.Data.Gateways;

namespace Hermes.IoC
{
    public class HermesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ReadInboxGateway>().As<IReadInboxGateway>();
            builder.RegisterType<ContextGateway>().As<IContextGateway>();
        }
    }
}