namespace KnightFrank.Antares.Api.Services.AzureStorage.Factories
{
    using System;
    using System.Web.Configuration;

    using Microsoft.WindowsAzure.Storage.Blob;

    public class SharedAccessBlobPolicyFactory : ISharedAccessBlobPolicyFactory
    {
        public SharedAccessBlobPolicy Create()
        {
            int expiryTimeFromNowInMinutes = int.Parse(WebConfigurationManager.AppSettings["CloudStorageExpiryTimeFromNowInMinutes"]);

            return new SharedAccessBlobPolicy
            {
                // Microsoft recommendation to set date from past
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-30),
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(expiryTimeFromNowInMinutes),
                // TODO check permissions
                Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write |
                              SharedAccessBlobPermissions.List
            };
        }
    }
}