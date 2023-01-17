using MassTransit;
using MassTransit.Futures.Contracts;
using MassTransit.Internals;
using System.Text.RegularExpressions;

namespace AppointmentsManagerMs.BusinessLayer.Utils.ServiceBus
{
    public class CustomEntityNameFormatter : IEntityNameFormatter
    {
        readonly IEntityNameFormatter _entityNameFormatter;

        public CustomEntityNameFormatter(IEntityNameFormatter entityNameFormatter)
        {
            _entityNameFormatter = entityNameFormatter;
        }

        public string FormatEntityName<T>()
        {
            if (typeof(T).ClosesType(typeof(CorrelatedBy<>)))
            {
                return Regex.Replace(typeof(T).Name, "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z0-9])", "-$1", RegexOptions.Compiled)
                            .Trim()
                            .ToLower();
            }

            return _entityNameFormatter.FormatEntityName<T>();
        }
    }

    public static class CustomConfigurationExtensions
    {
        /// <summary>
        /// Should be using on every UsingRabbitMq configuration
        /// </summary>
        /// <param name="configurator"></param>
        public static void SetCustomEntityNameFormatter(this IBusFactoryConfigurator configurator)
        {
            var entityNameFormatter = configurator.MessageTopology.EntityNameFormatter;

            configurator.MessageTopology.SetEntityNameFormatter(new CustomEntityNameFormatter(entityNameFormatter));
        }
    }
}
