namespace KnightFrank.Antares.Api.UnitTests.Services.AzureStorage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using KnightFrank.Antares.Api.Models;
    using KnightFrank.Antares.Api.Services.AzureStorage;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob;

    using Moq;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    using EnumType = KnightFrank.Antares.Domain.Common.Enums.EnumType;

    public class DocumentStorageProviderTests
    {
        private readonly Dictionary<CloudStorageContainerType, Func<AttachmentUrlParameters, Expression<Func<IEntityDocumentStorageProvider, AzureUploadUrlContainer>>>> uploadUrlFuncDict =
                new Dictionary<CloudStorageContainerType, Func<AttachmentUrlParameters, Expression<Func<IEntityDocumentStorageProvider, AzureUploadUrlContainer>>>>();
        private readonly Dictionary<CloudStorageContainerType, Func<AttachmentDownloadUrlParameters, Expression<Func<IEntityDocumentStorageProvider, AzureDownloadUrlContainer>>>> downloadUrlFuncDict =
                new Dictionary<CloudStorageContainerType, Func<AttachmentDownloadUrlParameters, Expression<Func<IEntityDocumentStorageProvider, AzureDownloadUrlContainer>>>>();

        public DocumentStorageProviderTests()
        {
            this.uploadUrlFuncDict.Add(CloudStorageContainerType.Activity, parameters => (x => x.GetUploadSasUri<Activity>(parameters, EnumType.ActivityDocumentType)));
            this.uploadUrlFuncDict.Add(CloudStorageContainerType.Property, parameters => (x => x.GetUploadSasUri<Property>(parameters, EnumType.PropertyDocumentType)));
            this.uploadUrlFuncDict.Add(CloudStorageContainerType.Requirement, parameters => (x => x.GetUploadSasUri<Requirement>(parameters, EnumType.RequirementDocumentType)));

            this.downloadUrlFuncDict.Add(CloudStorageContainerType.Activity, parameters => (x => x.GetDownloadSasUri<Activity>(parameters, EnumType.ActivityDocumentType)));
            this.downloadUrlFuncDict.Add(CloudStorageContainerType.Property, parameters => (x => x.GetDownloadSasUri<Property>(parameters, EnumType.PropertyDocumentType)));
            this.downloadUrlFuncDict.Add(CloudStorageContainerType.Requirement, parameters => (x => x.GetDownloadSasUri<Requirement>(parameters, EnumType.RequirementDocumentType)));
        }

        [Theory]
        [InlineAutoMoqData(CloudStorageContainerType.Activity)]
        [InlineAutoMoqData(CloudStorageContainerType.Property)]
        [InlineAutoMoqData(CloudStorageContainerType.Requirement)]
        public void Given_ConfigureUploadUrl_When_Called_Then_ProperUploadUrlMethodIsSetForEntity(
            CloudStorageContainerType cloudStorageContainerType,
            [Frozen] Mock<IEntityDocumentStorageProvider> entityDocumentStorageProvider,
            DocumentStorageProvider documentStorageProvider,
            AttachmentUrlParameters parameters)
        {
            // Arrange
            // Act
            documentStorageProvider.ConfigureUploadUrl();

            // Assert
            DocumentStorageProvider.GetUploadSasUri method = documentStorageProvider.GetUploadUrlMethod(cloudStorageContainerType);
            method(parameters);

            entityDocumentStorageProvider.Verify(this.uploadUrlFuncDict[cloudStorageContainerType](parameters), Times.Once);
        }

        [Theory]
        [InlineAutoMoqData(CloudStorageContainerType.Activity)]
        [InlineAutoMoqData(CloudStorageContainerType.Property)]
        [InlineAutoMoqData(CloudStorageContainerType.Requirement)]
        public void Given_ConfigureDownloadUrl_When_Called_Then_ProperUploadUrlMethodsAreSetForEntity(
            CloudStorageContainerType cloudStorageContainerType,
            [Frozen] Mock<IEntityDocumentStorageProvider> entityDocumentStorageProvider,
            DocumentStorageProvider documentStorageProvider,
            AttachmentDownloadUrlParameters parameters)
        {
            // Arrange
            // Act
            documentStorageProvider.ConfigureDownloadUrl();

            // Assert
            DocumentStorageProvider.GetDownloadSasUri method = documentStorageProvider.GetDownloadUrlMethod(cloudStorageContainerType);
            method(parameters);

            entityDocumentStorageProvider.Verify(this.downloadUrlFuncDict[cloudStorageContainerType](parameters), Times.Once);
        }

        [Theory]
        [InlineAutoMoqData(CloudStorageContainerType.Activity)]
        [InlineAutoMoqData(CloudStorageContainerType.Property)]
        [InlineAutoMoqData(CloudStorageContainerType.Requirement)]
        public void Given_GetUploadUrlMethod_When_CalledWithConfiguredType_Then_ProperUploadUrlMethodIsSetForEntity(
            CloudStorageContainerType cloudStorageContainerType,
            DocumentStorageProvider documentStorageProvider,
            AttachmentUrlParameters parameters)
        {
            // Arrange
            documentStorageProvider.ConfigureUploadUrl();

            // Act
            DocumentStorageProvider.GetUploadSasUri method = documentStorageProvider.GetUploadUrlMethod(cloudStorageContainerType);

            // Assert
            method.Should().NotBeNull();
        }

        [Theory]
        [InlineAutoMoqData(CloudStorageContainerType.Activity)]
        [InlineAutoMoqData(CloudStorageContainerType.Property)]
        [InlineAutoMoqData(CloudStorageContainerType.Requirement)]
        public void Given_GetUploadUrlMethod_When_CalledWithNotConfiguredType_Then_ShoulThrowException(
            CloudStorageContainerType cloudStorageContainerType,
            DocumentStorageProvider documentStorageProvider,
            AttachmentUrlParameters parameters)
        {
            // Arrange
            // Act
            Action act = () => documentStorageProvider.GetUploadUrlMethod(cloudStorageContainerType);

            // Asset
            act.ShouldThrow<DomainValidationException>().Which.Errors.Single(x => x.ErrorMessage == "Entity is not supported");
        }

        [Theory]
        [InlineAutoMoqData(CloudStorageContainerType.Activity)]
        [InlineAutoMoqData(CloudStorageContainerType.Property)]
        [InlineAutoMoqData(CloudStorageContainerType.Requirement)]
        public void Given_GetDownloadUrlMethod_When_CalledWithConfiguredType_Then_ProperDownloadUrlMethodIsSetForEntity(
           CloudStorageContainerType cloudStorageContainerType,
           DocumentStorageProvider documentStorageProvider,
           AttachmentDownloadUrlParameters parameters)
        {
            // Arrange
            documentStorageProvider.ConfigureDownloadUrl();

            // Act
            DocumentStorageProvider.GetDownloadSasUri method = documentStorageProvider.GetDownloadUrlMethod(cloudStorageContainerType);

            // Assert
            method.Should().NotBeNull();
        }

        [Theory]
        [InlineAutoMoqData(CloudStorageContainerType.Activity)]
        [InlineAutoMoqData(CloudStorageContainerType.Property)]
        [InlineAutoMoqData(CloudStorageContainerType.Requirement)]
        public void Given_GettDownloadUrlMethod_When_CalledWithNotConfiguredType_Then_ShoulThrowException(
            CloudStorageContainerType cloudStorageContainerType,
            DocumentStorageProvider documentStorageProvider,
            AttachmentDownloadUrlParameters parameters)
        {
            // Arrange
            // Act
            Action act = () => documentStorageProvider.GetDownloadUrlMethod(cloudStorageContainerType);

            // Asset
            act.ShouldThrow<DomainValidationException>().Which.Errors.Single(x => x.ErrorMessage == "Entity is not supported");
        }
    }
}