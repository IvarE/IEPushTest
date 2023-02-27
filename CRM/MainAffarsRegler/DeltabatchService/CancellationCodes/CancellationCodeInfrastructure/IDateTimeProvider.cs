using System;

namespace Endeavor.Crm.DeltabatchService.CancellationCodes.CancellationCodeLogic
{
    /// <summary>
    /// Interface for a class that provides DateTime.Now to other classes. Can be used to make certain types of logic testable.
    /// </summary>
    public interface IDateTimeProvider
    {
        DateTime Now();
    }
}
