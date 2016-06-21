namespace KnightFrank.Antares.Domain.UnitTests.Common.BusinessValidators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Xunit;

    [Collection("CollectionValidator")]
    [Trait("FeatureTitle", "Common validators")]
    public class CollectionValidatorTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_TwoDifferentCollections_WhenCallingCollectionContainsAll_Then_ExceptionThrown(
            CollectionValidator collectionValidator,
            List<Guid> collection,
            List<Guid> otherCollection)
        {
            Assert.Throws<BusinessValidationException>(() => collectionValidator.CollectionContainsAll(collection, otherCollection, ErrorMessage.Missing_Requirement_Attendees_Id));
        }

        [Theory]
        [AutoMoqData]
        public void Given_TwoIdenticalCollections_WhenCallingCollectionContainsAll_Then_NoExceptionThrown(
            CollectionValidator collectionValidator,
            List<Guid> collection)
        {
            collectionValidator.CollectionContainsAll(collection, collection, ErrorMessage.Missing_Requirement_Attendees_Id);
        }

        [Theory]
        [AutoMoqData]
        public void Given_SubsetOfTheCollection_WhenCallingCollectionContainsAll_Then_NoExceptionThrown(
            CollectionValidator collectionValidator,
            List<Guid> collection)
        {
            List<Guid> subset = collection.Take(1).ToList();

            collectionValidator.CollectionContainsAll(collection, subset, ErrorMessage.Missing_Requirement_Attendees_Id);
        }

        [Theory]
        [AutoMoqData]
        public void Given_UniqueCollection_WhenCallingCollectionIsUnique_Then_NoExceptionThrown(
            CollectionValidator collectionValidator,
            List<Guid> collection)
        {
            collectionValidator.CollectionIsUnique(collection, ErrorMessage.Entity_List_Item_Not_Exists);
        }

        [Theory]
        [AutoMoqData]
        public void Given_EmptyCollection_WhenCallingCollectionIsUnique_Then_NoExceptionThrown(
            CollectionValidator collectionValidator)
        {
            collectionValidator.CollectionIsUnique(new List<Guid>(), ErrorMessage.Entity_List_Item_Not_Exists);
        }

        [Theory]
        [AutoMoqData]
        public void Given_NotUniqueCollection_WhenCallingCollectionIsUnique_Then_ExceptionThrown(
            CollectionValidator collectionValidator,
            List<Guid> collection)
        {
            collection.Add(collection[0]);
            Assert.Throws<BusinessValidationException>(() => collectionValidator.CollectionIsUnique(collection, ErrorMessage.Entity_List_Item_Not_Exists));
        }
    }
}
