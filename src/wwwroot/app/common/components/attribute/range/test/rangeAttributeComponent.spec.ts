/// <reference path="../../../../../typings/_all.d.ts" />
module Antares {
    import Business = Common.Models.Business;
    describe('Given range attribute component is loaded', () =>{
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            compile: ng.ICompileService,
            state: ng.ui.IStateService,
            assertValidator: TestHelpers.AssertValidators;

        describe('when attribute is initialized', () =>{
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $state: ng.ui.IStateService) =>{

                var attributeValuesMock = [{ min : 0 }, { max : 0 }];
                var attributeMock = new Business.Attribute();
                attributeMock.nameKey = "TestAttribute";
                attributeMock.min = "min";
                attributeMock.max = "max";

                scope = $rootScope.$new();
                scope['attributeValues'] = attributeValuesMock;
                scope['attribute'] = attributeMock;
                scope['mode'] = 'edit';
                compile = $compile;
                state = $state;

                element = $compile('<range-attribute mode="{{mode}}" attribute="attribute" min-value="attributeValues[attribute.min]" max-value="attributeValues[attribute.max]"></range-attribute>')(scope);
                scope.$apply();
                
                assertValidator = new TestHelpers.AssertValidators(element, scope);                
            }));

            it('when min field value is empty then min validation message should not be displayed', () =>{
                assertValidator.assertMinValueValidator(null, true, 'input#min');
            });

            it('when min field value is empty then max validation message should not be displayed', () =>{
                assertValidator.assertMaxValueValidator(null, true, 'input#min');
            });

            it('when min field value is not lower than minimum validation message should not be displayed', () =>{
                var minValue = 0;
                assertValidator.assertMinValueValidator(minValue, true, 'input#min');
            });

            it('when min field value is lower then minimum then validation message should be displayed', () =>{
                var minValue = 0;
                assertValidator.assertMinValueValidator(minValue - 1, false, 'input#min');
            });

            it('when max field value is not lower then minimum then validation message should be displayed', () =>{
                var minValue = 0;
                assertValidator.assertMinValueValidator(minValue, true, 'input#max');
            });

            it('when max field value is lower then minimum then validation message should be displayed', () =>{
                var minValue = 0;
                assertValidator.assertMinValueValidator(minValue - 1, false, 'input#max');
            });

            it('when min field value is higher than max field value then validation message should be displayed', () =>{
                var value = 1000;
                var max = element.find('input#max');
                max.val(value).trigger('input').trigger('change').trigger('blur');
                assertValidator.assertMinValueValidator(value + 1, false, 'input#min');
            });

            it('when min field value is not higher than max field value then validation message should not be displayed', () =>{
                var value = 1000;
                var max = element.find('input#max');
                max.val(value).trigger('input').trigger('change').trigger('blur');
                assertValidator.assertMinValueValidator(value, true, 'input#min');
            });
        });
    });
}