/// <reference path="../../../../../typings/_all.d.ts" />
module Antares {
    import Business = Common.Models.Business;
    import RangeAttributeController = Antares.Common.Component.RangeAttributeController
    import Dto = Common.Models.Dto;

    describe('Given range attribute component is loaded', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            compile: ng.ICompileService,
            state: ng.ui.IStateService,
            assertValidator: TestHelpers.AssertValidators;

        describe('when attribute is initialized', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $state: ng.ui.IStateService) => {

                var attributeValuesMock = [{ min: 0 }, { max: 0 }];
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

            it('when min field value is empty then min validation message should not be displayed', () => {
                assertValidator.assertMinValueValidator(null, true, 'input#min');
            });

            it('when min field value is empty then max validation message should not be displayed', () => {
                assertValidator.assertMaxValueValidator(null, true, 'input#min');
            });

            it('when min field value is not lower than minimum validation message should not be displayed', () => {
                var minValue = 0;
                assertValidator.assertMinValueValidator(minValue, true, 'input#min');
            });

            it('when min field value is lower then minimum then validation message should be displayed', () => {
                var minValue = 0;
                assertValidator.assertMinValueValidator(minValue - 1, false, 'input#min');
            });

            it('when max field value is not lower then minimum then validation message should be displayed', () => {
                var minValue = 0;
                assertValidator.assertMinValueValidator(minValue, true, 'input#max');
            });

            it('when max field value is lower then minimum then validation message should be displayed', () => {
                var minValue = 0;
                assertValidator.assertMinValueValidator(minValue - 1, false, 'input#max');
            });

            it('when min field value is higher than max field value then validation message should be displayed', () => {
                var value = 1000;
                var max = element.find('input#max');
                max.val(value).trigger('input').trigger('change').trigger('blur');
                assertValidator.assertMinValueValidator(value + 1, false, 'input#min');
            });

            it('when min field value is not higher than max field value then validation message should not be displayed', () => {
                var value = 1000;
                var max = element.find('input#max');
                max.val(value).trigger('input').trigger('change').trigger('blur');
                assertValidator.assertMinValueValidator(value, true, 'input#min');
            });
        });

        describe('given attribute range is rendered', () => {
            var scope: ng.IScope,
                element: ng.IAugmentedJQuery;

            var pageObjectSelectors = {
                attributeValue: '.attribute-value div:not(.ng-hide) span:not(.ng-hide)'
            };

            var controller: RangeAttributeController;

            var attribute = {};

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) => {

                scope = $rootScope.$new();
                element = $compile('<range-attribute mode="view" min-value="attribute.minValue" max-value="attribute.maxValue" attribute="attribute"></range-attribute>')(scope);

                scope.$apply();

                controller = element.controller('rangeAttributeController');
            }));

            it('and attribute values are specified then valid text is visible', () => {
                var testData = [
                    { attribute: { minValue: 1, maxValue: 2 }, expectedText: '1-2' },
                    { attribute: { minValue: 1, maxValue: 1 }, expectedText: '1' },
                    { attribute: { minValue: null, maxValue: 2 }, expectedText: 'common.max 2' },
                    { attribute: { minValue: 2, maxValue: null }, expectedText: 'common.min 2' },
                    { attribute: { minValue: 0, maxValue: 3 }, expectedText: '0-3' },
                    { attribute: { minValue: 0, maxValue: 0 }, expectedText: '0' },
                    { attribute: { minValue: null, maxValue: null }, expectedText: '-' },
                    { attribute: { minValue: 0, maxValue: null }, expectedText: 'common.min 0' },
                    { attribute: { minValue: null, maxValue: 0 }, expectedText: 'common.max 0' },
                    { attribute: { minValue: 1, maxValue: 1, unit: 'UNITS.SQUARE_FEET' }, expectedText: '1 UNITS.SQUARE_FEET' },
                    { attribute: { minValue: 1, maxValue: 3, unit: 'UNITS.SQUARE_FEET' }, expectedText: '1-3 UNITS.SQUARE_FEET' },
                    { attribute: { minValue: null, maxValue: null, unit: 'UNITS.SQUARE_FEET' }, expectedText: '-' }
                ];

                testData.forEach((data) => {
                    var attribute = data.attribute;

                    scope['attribute'] = attribute;
                    scope.$apply();
                     
                    var attributeText : string = element.find(pageObjectSelectors.attributeValue).text().trim()
                        .replace(/\s/g, " "); //replaces all type of spaces i.e. nonbreaking spaces into regular spaces
                    expect(attributeText).toBe(data.expectedText);
                });

            });
        });
    });
}