namespace KnightFrank.Antares.Domain.UnitTests.AttributeConfiguration.EntityConfigurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;

    using Xunit;

    public class BaseControlsConfigurationTests<TFixture> : IClassFixture<TFixture>
        where TFixture : class
    {
        protected void ControlsConfigurationPerOneTypeItemTest<TEntity, TEnum1>(
            ControlsConfigurationPerOneTypeItem<TEntity, TEnum1> controlConfigurationItem,
            ControlsConfigurationPerOneType<TEnum1> configuration) where TEnum1 : struct
        {
            // Arrange
            Tuple<Tuple<TEnum1>, PageType> configurationKey = this.Create(controlConfigurationItem);

            // Assert
            configuration.ControlPageConfiguration.ContainsKey(configurationKey).Should().BeTrue();

            IList<Tuple<Control, IList<IField>>> controlsForGivenConditions =
                configuration.ControlPageConfiguration[configurationKey];
            controlsForGivenConditions.Should().NotBeNullOrEmpty();

            Tuple<Control, IList<IField>> controlConfig =
                controlsForGivenConditions.First(x => x.Item1.ControlCode == controlConfigurationItem.ControlCode);
            Control control = controlConfig.Item1;
            IList<IField> fields = controlConfig.Item2;

            fields.Should().NotBeNullOrEmpty();
            if (controlConfigurationItem.IsRequired.HasValue)
            {
                fields.Select(x => x.InnerField.Required).All(x => x).Should().Be(controlConfigurationItem.IsRequired.Value);
            }

            this.AssertExpression<TEntity>(
                controlConfigurationItem.ControlHiddenExpression,
                control.IsHiddenExpression);

            this.AssertExpression<TEntity>(
                controlConfigurationItem.ControlReadonlyExpression,
                control.IsReadonlyExpression);
        }

        protected void ControlsConfigurationPerTwoTypesItemTest<TEntity, TEnum1, TEnum2>(
            ControlsConfigurationPerTwoTypesItem<TEntity, TEnum1, TEnum2> controlConfigurationItem,
            ControlsConfigurationPerTwoTypes<TEnum1, TEnum2> configuration) where TEnum1 : struct where TEnum2 : struct
        {
            // Arrange
            Tuple<Tuple<TEnum1, TEnum2>, PageType> configurationKey = this.Create(controlConfigurationItem);

            // Assert
            configuration.ControlPageConfiguration.ContainsKey(configurationKey).Should().BeTrue();

            IList<Tuple<Control, IList<IField>>> controlsForGivenConditions =
                configuration.ControlPageConfiguration[configurationKey];
            controlsForGivenConditions.Should().NotBeNullOrEmpty();

            Tuple<Control, IList<IField>> controlConfig =
                controlsForGivenConditions.First(x => x.Item1.ControlCode == controlConfigurationItem.ControlCode);
            Control control = controlConfig.Item1;
            IList<IField> fields = controlConfig.Item2;

            fields.Should().NotBeNullOrEmpty();
            if (controlConfigurationItem.IsRequired.HasValue)
            {
                fields.Select(x => x.InnerField.Required).All(x => x).Should().Be(controlConfigurationItem.IsRequired.Value);
            }

            this.AssertExpression<TEntity>(
                controlConfigurationItem.ControlHiddenExpression,
                control.IsHiddenExpression);

            this.AssertExpression<TEntity>(
                controlConfigurationItem.ControlReadonlyExpression,
                control.IsReadonlyExpression);
        }

        protected void ControlsConfigurationPerThreeTypesItemTest<TEntity, TEnum1, TEnum2, TEnum3>(
            ControlsConfigurationPerThreeTypesItem<TEntity, TEnum1, TEnum2, TEnum3> controlConfigurationItem,
            ControlsConfigurationPerThreeTypes<TEnum1, TEnum2, TEnum3> configuration) where TEnum1 : struct where TEnum2 : struct
            where TEnum3 : struct
        {
            // Arrange
            Tuple<Tuple<TEnum1, TEnum2, TEnum3>, PageType> configurationKey = this.Create(controlConfigurationItem);

            // Assert
            configuration.ControlPageConfiguration.ContainsKey(configurationKey).Should().BeTrue();

            IList<Tuple<Control, IList<IField>>> controlsForGivenConditions =
                configuration.ControlPageConfiguration[configurationKey];
            controlsForGivenConditions.Should().NotBeNullOrEmpty();

            Tuple<Control, IList<IField>> controlConfig =
                controlsForGivenConditions.First(x => x.Item1.ControlCode == controlConfigurationItem.ControlCode);
            Control control = controlConfig.Item1;
            IList<IField> fields = controlConfig.Item2;

            fields.Should().NotBeNullOrEmpty();
            if (controlConfigurationItem.IsRequired.HasValue)
            {
                fields.Select(x => x.InnerField.Required).All(x => x).Should().Be(controlConfigurationItem.IsRequired.Value);
            }

            this.AssertExpression<TEntity>(
                controlConfigurationItem.ControlHiddenExpression,
                control.IsHiddenExpression);
            this.AssertExpression<TEntity>(
                controlConfigurationItem.ControlReadonlyExpression,
                control.IsReadonlyExpression);
        }

        private Tuple<Tuple<TEnum1>, PageType> Create<TEntity, TEnum1>(
            ControlsConfigurationPerOneTypeItem<TEntity, TEnum1> controlsConfigurationItem) where TEnum1 : struct
        {
            return new Tuple<Tuple<TEnum1>, PageType>(
                new Tuple<TEnum1>(controlsConfigurationItem.Enum1),
                controlsConfigurationItem.PageType);
        }

        private Tuple<Tuple<TEnum1, TEnum2>, PageType> Create<TEntity, TEnum1, TEnum2>(
            ControlsConfigurationPerTwoTypesItem<TEntity, TEnum1, TEnum2> controlsConfigurationItem) where TEnum1 : struct
            where TEnum2 : struct
        {
            return
                new Tuple<Tuple<TEnum1, TEnum2>, PageType>(
                    new Tuple<TEnum1, TEnum2>(controlsConfigurationItem.Enum1, controlsConfigurationItem.Enum2),
                    controlsConfigurationItem.PageType);
        }

        private Tuple<Tuple<TEnum1, TEnum2, TEnum3>, PageType> Create<TEntity, TEnum1, TEnum2, TEnum3>(
            ControlsConfigurationPerThreeTypesItem<TEntity, TEnum1, TEnum2, TEnum3> controlsConfigurationItem)
            where TEnum1 : struct where TEnum2 : struct where TEnum3 : struct
        {
            return
                new Tuple<Tuple<TEnum1, TEnum2, TEnum3>, PageType>(
                    new Tuple<TEnum1, TEnum2, TEnum3>(
                        controlsConfigurationItem.Enum1,
                        controlsConfigurationItem.Enum2,
                        controlsConfigurationItem.Enum3),
                    controlsConfigurationItem.PageType);
        }

        private void AssertExpression<T>(Expression expectedExpression, LambdaExpression actualExpression)
        {
            if (expectedExpression == null)
            {
                actualExpression.Should().BeNull();
            }
            else
            {
                actualExpression.Should().NotBeNull();
                actualExpression.ToString().ShouldBeEquivalentTo(expectedExpression.ToString());
            }
        }
    }
}
