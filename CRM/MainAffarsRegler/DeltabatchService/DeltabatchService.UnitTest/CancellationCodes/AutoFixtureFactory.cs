using AutoFixture.Kernel;
using AutoFixture;
using Microsoft.Xrm.Sdk;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Endeavor.Crm.UnitTest.CancellationCodes
{
    internal class SkipEntityProperties : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var pi = request as PropertyInfo;
            if (pi == null)
            {
                return new NoSpecimen();
            }

            if (typeof(ExtensionDataObject).IsAssignableFrom(pi.PropertyType))
            {
                return new OmitSpecimen();
            }

            if (pi.DeclaringType == typeof(Entity))
            {
                return new OmitSpecimen();
            }

            // Property is for an Entity Class, and the Property has a generic type parameter that is an entity, or is an entity
            if (typeof(Entity).IsAssignableFrom(pi.DeclaringType)
                &&
                (pi.PropertyType.IsGenericType && pi.PropertyType.GenericTypeArguments.Any(t => typeof(Entity).IsAssignableFrom(t))
                 || typeof(Entity).IsAssignableFrom(pi.PropertyType)
                 || typeof(AttributeCollection).IsAssignableFrom(pi.PropertyType)
                 )
               )
            {
                return new OmitSpecimen();
            }

            return new NoSpecimen();
        }
    }

    /// <summary>
    /// Configures AutoFixture to correctly handle early bound entities.
    /// Source: https://d365hub.com/Posts/Details/a68a59e0-62b1-48d2-9722-f86ccd29af76/using-autofixture-to-create-early-bound-entities
    /// </summary>
    public class AutoFixtureFactory
    {
        public static Fixture CreateFixture()
        {
            var fixture = new Fixture();
            fixture.Register<ExtensionDataObject>(() => null);
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            fixture.Customizations.Add(new SkipEntityProperties());

            return fixture;
        }
    }
}
