using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Moq;
using System;
using System.Collections.Generic;

namespace Skanetrafiken.Crm.TestHelpers
{
    /// <summary>
    /// A simple fluent api for building mocks of the IOrganization service.
    /// </summary>
    public class OrganizationServiceMockFactory
    {
        public Mock<IOrganizationService> Mock { get; private set; }

        private OrganizationServiceMockFactory()
        {
            Mock = new Mock<IOrganizationService>(MockBehavior.Loose);
        }

        public static OrganizationServiceMockFactory Create()
        {
            return new OrganizationServiceMockFactory();
        }

        public OrganizationServiceMockFactory WithRetrieve(string logicalName, Guid id, Entity @return)
        {
            Mock.Setup(ios => ios.Retrieve(logicalName, id, It.IsAny<ColumnSet>())).Returns(@return);
            return this;
        }

        public OrganizationServiceMockFactory WithRetrieveMultiple<T>(EntityCollection @return) where T : QueryBase
        {
            Mock.Setup(s => s.RetrieveMultiple(It.IsAny<T>())).Returns(@return);
            return this;
        }

        public OrganizationServiceMockFactory WithRetrieveMultiple(List<Entity> @return)
        {
            return WithRetrieveMultiple<QueryExpression>(
                new EntityCollection(@return)
                {
                    TotalRecordCount = @return.Count
                });
        }

        public OrganizationServiceMockFactory WithExecute<T>() where T : OrganizationRequest
        {
            Mock.Setup(s => s.Execute(It.IsAny<T>()));
            return this;
        }

        public OrganizationServiceMockFactory WithExecute<T>(T request) where T : OrganizationRequest
        {
            Mock.Setup(s => s.Execute(request));
            return this;
        }
    }
}
