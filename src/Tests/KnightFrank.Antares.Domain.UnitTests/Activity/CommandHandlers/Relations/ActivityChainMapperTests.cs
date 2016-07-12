namespace KnightFrank.Antares.Domain.UnitTests.Activity.CommandHandlers.Relations
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.CommandHandlers.Relations;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    public class ActivityChainMapperTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_Handling_Then_CommandChainTransactionShouldMapToActivityChainTransactions(
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<IGenericRepository<ChainTransaction>> chainTransactionRepository,
            ActivityChainMapper mapper)
        {
            // Arrange
            Guid guid = Guid.Parse("60FAB3B3-2584-41ED-ADA5-05111009C749");
            var command = new UpdateActivityCommand
            {
                ChainTransactions = new List<ChainTransaction>
                {
                    new ChainTransaction { Id = guid },
                    new ChainTransaction { Id = Guid.NewGuid() },
                    new ChainTransaction()
                }
            };
            var activity = new Activity
            {
                ChainTransactions = new List<ChainTransaction>
                {
                    new ChainTransaction { Id = guid },
                    new ChainTransaction { Id = Guid.NewGuid() },
                    new ChainTransaction { Id = Guid.NewGuid() },
                    new ChainTransaction { Id = Guid.NewGuid() }
                }
            };

            chainTransactionRepository.Setup(x => x.Delete(It.IsAny<ChainTransaction>()))
                                      .Callback<ChainTransaction>(x => activity.ChainTransactions.Remove(x));

            // Act
            mapper.ValidateAndAssign(command, activity);

            // Assert
            chainTransactionRepository.Verify(r => r.Delete(It.IsAny<ChainTransaction>()), Times.Exactly(3));
            activity.ChainTransactions.ShouldBeEquivalentTo(command.ChainTransactions);
        }

        [Theory]
        [MemberData("InvalidChainTransactions")]
        public void Given_InvalidCommand_When_Handling_Then_ShouldThrowException(
            List<ChainTransaction> chainTransactions)
        {
            // Arrange
            var activity = new Activity();
            var command = new UpdateActivityCommand
            {
                ChainTransactions = chainTransactions
            };

            var fixture = new Fixture();
            var entityValidator = fixture.Freeze<Mock<IEntityValidator>>();
            var chainTransactionRepository = fixture.Freeze<Mock<IGenericRepository<ChainTransaction>>>();
            var mapper = new ActivityChainMapper(entityValidator.Object, chainTransactionRepository.Object);

            // Act
            // Assert
            Assert.Throws<BusinessValidationException>(() => mapper.ValidateAndAssign(command, activity));
        }

        public static IEnumerable<object[]> InvalidChainTransactions => new[]
        {
            new object[]
            {
                new List<ChainTransaction>
                {
                    new ChainTransaction
                    {
                        IsEnd = true,
                    },
                    new ChainTransaction
                    {
                        IsEnd = true,
                    }
                }
            },
            new object[]
            {
                new List<ChainTransaction>
                {
                    new ChainTransaction
                    {
                        AgentContactId = Guid.Parse("60FAB3B3-2584-41ED-ADA5-05111009C749"),
                        AgentUserId = Guid.Parse("50FAB333-2584-41ED-ADA5-051110091749")
                    }
                }
            },
            new object[]
            {
                new List<ChainTransaction>
                {
                    new ChainTransaction
                    {
                        Vendor =
                            "50FAB333-2584-41ED-ADA5-05111009174950FAB333-2584-41ED-ADA5-05111009174950FAB333-2584-41ED-ADA5-05111009174950FAB333-2584-41ED-ADA5-0511100917491"
                    }
                }
            },
            new object[]
            {
                new List<ChainTransaction>
                {
                    new ChainTransaction
                    {
                        Id = Guid.Parse("60FAB3B3-2584-41ED-ADA5-05111009C749"),
                        IsEnd = true
                    },
                    new ChainTransaction
                    {
                        ParentId = Guid.Parse("60FAB3B3-2584-41ED-ADA5-05111009C749")
                    }
                }
            }
        };
    }
}
