namespace Fields
{
    using System;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Common;
    using KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;
    using KnightFrank.Antares.Domain.AttributeConfiguration.ToRemove;

    class Program
    {
        static void Main(string[] args)
        {
            // pobierz czysta konfiguracje
            // pobierz konfiguracje dla obietku
            // Waliduj

            //date range validator

            var createCommand = new CreateCommand
            {
                BuyPrice = 200m,
                StatusId = 2
            };

            var createCommand2 = new CreateCommand
            {
                BuyPrice = 10m,
                StatusId = 1
            };


            var xyz = new ActivityControlsConfiguration();

            foreach (Control item in xyz.ControlsDictionary[PageType.Create])
            {
                Console.WriteLine("IsReadonly: " + item.IsReadonly(createCommand));
                Console.WriteLine("IsHidden: " + item.IsHidden(createCommand));
                foreach (InnerField innerField in item.Fields)
                {
                    innerField.Validate(createCommand);
                }
            }

            //var states = xyz.GetInnerFieldsState(PageType.Create, PropertyType.Flat, ActivityType.Lettings, createCommand);
            //foreach (var state in states)
            //{
            //    Console.WriteLine("[{0}] -- hidden: {1}, readonly: {2}, no. of validators: {3}", state.Name, state.Hidden, state.Readonly, state.Validators.Count);
            //}

            //states = xyz.GetInnerFieldsState(PageType.Create, PropertyType.Flat, ActivityType.Lettings, createCommand2);
            //foreach (var state in states)
            //{
            //    Console.WriteLine("[{0}] -- hidden: {1}, readonly: {2}, no. of validators: {3}", state.Name, state.Hidden, state.Readonly, state.Validators.Count);
            //}


            var attributeValidator = new AttributeValidator();
            attributeValidator.Validate(createCommand);

            Console.ReadLine();
        }
    }
}
