using AutoFixture;
using Endeavor.Crm;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using Moq;
using NUnit.Framework;
using Skanetrafiken.Crm;
using Skanetrafiken.Crm.Entities;
using Skanetrafiken.Crm.Schema.Generated;
using Skanetrafiken.Crm.TestHelpers;
using System;
using System.Reflection;
using static Endeavor.Crm.Plugin;

namespace Skanetrafiken.Crm.Tests
{
    [TestFixture]
    public class ContactEntity_ClearSSN_Tests
    {
        private TestFactory testFactory;
        private Fixture fixture;

        private Mock<IServiceProvider> serviceProviderMock;
        private Mock<IOrganizationService> organizationServiceMock;
        private Mock<ITracingService> tracerMock;

        private XrmFakedContext context;

        [SetUp]
        public void Setup()
        {
            testFactory = new TestFactory();
            fixture = testFactory.Fixture;
            tracerMock = testFactory.TracingServiceMock;

#pragma warning disable CS0618 // Deprecated library
            context = new XrmFakedContext
            {
                ProxyTypesAssembly = Assembly.GetAssembly(typeof(Contact))
            };
#pragma warning restore

            serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Loose);
            organizationServiceMock = OrganizationServiceMockFactory.Create().Mock;
        }

        [Test(Description = "Test if method is safe from deep recursion")]
        public void Execute_ShouldAbort_WhenRecursionDepthIsTooHigh()
        {
            // Arrange
            ContactEntity contact = new ContactEntity();

            XrmFakedPluginExecutionContext xrmFakeContext = context.GetFakePluginExecutionContext(target: contact, "Update");
            xrmFakeContext.Depth = 2;

            LocalPluginContext pluginContext = new LocalPluginContext(serviceProviderMock.Object, organizationServiceMock.Object, xrmFakeContext, tracerMock.Object);

            ContactEntity postImage = fixture.Create<ContactEntity>();
            ContactEntity preImage = fixture.Create<ContactEntity>();

            // Act
            contact.ClearContactFieldsRelatedToSSN(pluginContext, postImage, preImage);

            // Assert
            organizationServiceMock.Verify(service =>
                service.Update(It.IsAny<Contact>()),
                Times.Never()
            );
        }

        [Test(Description = "Test if plugin clears fields in the happy case")]
        [TestCase(Contact.Fields.abssr_DOB)]
        [TestCase(Contact.Fields.BirthDate)]
        [TestCase(Contact.Fields.ed_CreditsafeRejectionCode)]
        [TestCase(Contact.Fields.ed_CreditsafeRejectionText)]
        public void Execute_ShouldClearFields_WhenSSNHasBeenCleared(string assertField)
        {
            // Arrange
            ContactEntity contact = fixture.Build<ContactEntity>()
                                           .Without(c => c.ed_SocialSecurityNumberFormat)
                                           .Create();

            XrmFakedPluginExecutionContext xrmFakeContext = context.GetFakePluginExecutionContext(target: contact, "Update");
            LocalPluginContext pluginContext = new LocalPluginContext(serviceProviderMock.Object, organizationServiceMock.Object, xrmFakeContext, tracerMock.Object);

            ContactEntity postImage = fixture.Build<ContactEntity>()
                                             .Without(c => c.ed_SocialSecurityNumberFormat)
                                             .Create();
            ContactEntity preImage = fixture.Build<ContactEntity>()
                                 .Without(c => c.ed_SocialSecurityNumberFormat)
                                 .Create();

            // Act
            contact.ClearContactFieldsRelatedToSSN(pluginContext, postImage, preImage);

            // Assert
            organizationServiceMock.Verify(service =>
                service.Update(It.Is<Contact>(e => e[assertField] == null)),
                Times.Once()
            );
        }

        [TestCase(Contact.Fields.abssr_DOB)]
        [TestCase(Contact.Fields.BirthDate)]
        [TestCase(Contact.Fields.ed_CreditsafeRejectionCode)]
        [TestCase(Contact.Fields.ed_CreditsafeRejectionText)]
        [Test(Description = "Test if plugin aborts when ssn is updated but not empty")]
        public void Execute_ShouldNotClearFields_WhenSSNIsNotEmpty(string assertField)
        {
            // Arrange
            ContactEntity contact = fixture.Create<ContactEntity>();

            XrmFakedPluginExecutionContext xrmFakeContext = context.GetFakePluginExecutionContext(target: contact, "Update");
            LocalPluginContext pluginContext = new LocalPluginContext(serviceProviderMock.Object, organizationServiceMock.Object, xrmFakeContext, tracerMock.Object);

            ContactEntity postImage = fixture.Create<ContactEntity>();
            ContactEntity preImage = fixture.Create<ContactEntity>();

            // Act
            contact.ClearContactFieldsRelatedToSSN(pluginContext, postImage, preImage);

            // Assert
            organizationServiceMock.Verify(service =>
                service.Update(It.IsAny<Contact>()),
                Times.Never()
            );
        }

        [Test(Description = "Test if plugin runs for private contacts as well")]
        public void Execute_ShouldNotAbort_WhenContactIsPrivateContact()
        {
            // Arrange
            DateTime now = DateTime.Now;
            ContactEntity contact = fixture.Build<ContactEntity>()
                                     .Without(c => c.ed_SocialSecurityNumberFormat)
                                     .With(c => c.BirthDate, now)
                                     .With(c => c.ed_Serviceresor, false)
                                     .With(c => c.ed_PrivateCustomerContact, true)
                                     .Create();

            XrmFakedPluginExecutionContext xrmFakeContext = context.GetFakePluginExecutionContext(target: contact, "Update");
            LocalPluginContext pluginContext = new LocalPluginContext(serviceProviderMock.Object, organizationServiceMock.Object, xrmFakeContext, tracerMock.Object);

            ContactEntity postImage = fixture.Build<ContactEntity>()
                                             .Without(c => c.ed_SocialSecurityNumberFormat)
                                             .Create();
            ContactEntity preImage = fixture.Build<ContactEntity>()
                                             .Without(c => c.ed_SocialSecurityNumberFormat)
                                             .Create();

            // Act
            contact.ClearContactFieldsRelatedToSSN(pluginContext, postImage, preImage);

            // Assert
            organizationServiceMock.Verify(service =>
                service.Update(It.Is<Contact>(e => e.BirthDate == null)),
                Times.Once()
            );
        }
    }
}
