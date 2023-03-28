using Endeavor.Crm.CleanRecordsService.PandoraExtensions;
using Endeavor.Crm.CleanRecordsService.PandoraExtensions.Helpers;
using AutoFixture;
using Moq;
using NUnit.Framework;
using System;
using LocalPluginContext = Endeavor.Crm.Plugin.LocalPluginContext;
using SR.Generated;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using System.Linq;
using System.Linq.Expressions;

namespace SkaneCleanRecorsSerive.UnitTest
{
    [TestFixture]
    public class InactivatePermitsTests
    {
        Mock<IHelperWrapper> helperMock;
        Mock<IRetrieveHelperWrapper> retrieveHelperMock;
        LocalPluginContext pluginContextMock;

        TestFactory testFactory;
        Fixture fixture;

        [SetUp]
        public void Init()
        {
            helperMock = new Mock<IHelperWrapper>();
            retrieveHelperMock = new Mock<IRetrieveHelperWrapper>();
            pluginContextMock = new LocalPluginContext(null, null, null, null);

            testFactory = new TestFactory();
            fixture = testFactory.Fixture;
        }

        [Test]
        public void ShouldNotSendRequestsToDynamics_If_NoPermits()
        {
            // Arrange
            var emptyList = new List<abssr_permit>();
            retrieveHelperMock.Setup(r => 
                r.RetrieveMultiple<abssr_permit>(
                    It.IsAny<LocalPluginContext>(), 
                    It.IsAny<QueryExpression>(), 
                    It.IsAny<PagingInfo>()))
                .Returns(emptyList);

            InactivatePermits service = new InactivatePermits(helperMock.Object, retrieveHelperMock.Object);

            DateTime executionDate = DateTime.Now;

            // Act
            service.RunInactivatePermits(pluginContextMock, executionDate, false);

            // Assert
            VerifyOrganizationRequest(l => !l.Any(), 1);
        }

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        public void ShouldSendRequestsToDynamics_If_OnePermitsFound(int numPermits)
        {
            // Arrange
            var permits = fixture.Build<abssr_permit>()
                   .With(p => p.StateCode, abssr_permitState.Active)
                   .With(p => p.StatusCode, abssr_permit_StatusCode.Active)
                   .CreateMany(numPermits)
                   .ToList();

            retrieveHelperMock.Setup(r =>
                r.RetrieveMultiple<abssr_permit>(
                    It.IsAny<LocalPluginContext>(),
                    It.IsAny<QueryExpression>(),
                    It.IsAny<PagingInfo>()))
                .Returns(permits);

            InactivatePermits service = new InactivatePermits(helperMock.Object, retrieveHelperMock.Object);

            DateTime executionDate = DateTime.Now;

            // Act
            var response = service.RunInactivatePermits(pluginContextMock, executionDate, false);
            
            // Assert
            VerifyOrganizationRequest(l => l.Count == numPermits, 1);
            VerifyOrganizationRequest(l => l.All(req => (req["Target"] as abssr_permit).StateCode == abssr_permitState.Inactive), 1);
        }

        [Test]
        public void ShouldNotSendRequest_If_ToDateIsNull()
        {
            // Arrange
            var permits = fixture.Build<abssr_permit>()
                   .With(p => p.StateCode, abssr_permitState.Active)
                   .With(p => p.StatusCode, abssr_permit_StatusCode.Active)
                   .CreateMany(2)
                   .ToList();

            permits.First().abssr_Todate = null;

            retrieveHelperMock.Setup(r =>
                r.RetrieveMultiple<abssr_permit>(
                    It.IsAny<LocalPluginContext>(),
                    It.IsAny<QueryExpression>(),
                    It.IsAny<PagingInfo>()))
                .Returns(permits);

            InactivatePermits service = new InactivatePermits(helperMock.Object, retrieveHelperMock.Object);

            DateTime executionDate = DateTime.Now;

            // Act
            var response = service.RunInactivatePermits(pluginContextMock, executionDate, false);

            // Assert
            VerifyOrganizationRequest(l => l.Count == 1, 1);
        }

        private void VerifyOrganizationRequest(Expression<Func<List<OrganizationRequest>, bool>> requestValidator, int callCount)
        {
            helperMock.Verify(x =>
                x.ExecuteMultipleRequests(
                    It.IsAny<LocalPluginContext>(),
                    It.Is<List<OrganizationRequest>>(requestValidator),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<int>()),
                Times.Exactly(callCount));
        }
    }
}
