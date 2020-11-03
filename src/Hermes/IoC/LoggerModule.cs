using Autofac;
using Hermes.Data.Gateways;

namespace Hermes.IoC
{
    public class LoggerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ContextGateway>().As<IContextGateway>();
        }
    }
}