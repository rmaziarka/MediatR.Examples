/// <reference path="../../../../../typings/_all.d.ts" />
module Antares {
    import Business = Common.Models.Business;
    import RangeAttributeController = Common.Component.RangeAttributeController

    describe('Given range attribute component is loaded', () =>{
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            compile: ng.ICompileService,
            state: ng.ui.IStateService,
            assertValidator: TestHelpers.AssertValidators,
            pageObjectSelectors = {
                minField : 'input#min',
                maxField : 'input#max'
            };

        var testValidationMessageDisplay = (shouldBeDisplayed: Boolean, fieldSelector: string) =>{
            var isActive = element.find(fieldSelector).closest('.form-group').find('[ng-messages]').hasClass('ng-active');
            var isHidden = element.find(fieldSelector).closest('.form-group').find('[ng-messages]').hasClass('ng-hide');

            expect(isActive).toBe(shouldBeDisplayed);
            if (isActive) {
                expect(isHidden).toBe(false);
            }
        };

        describe('when attribute is initialized', () =>{
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $state: ng.ui.IStateService) =>{

                var attributeMock = new Business.Attribute();
                attributeMock.nameKey = "TestAttribute";
                attributeMock.min = "min";
                attributeMock.max = "max";

                scope = $rootScope.$new();
                scope['attribute'] = attributeMock;
                scope['mode'] = 'edit';
                compile = $compile;
                state = $state;

                element = $compile('<form name="form"><range-attribute mode="{{mode}}" attribute="attribute" min-value="1" max-value="4"></range-attribute></form>')(scope);
                scope.$apply();

                assertValidator = new TestHelpers.AssertValidators(element, scope);
            }));

            it('when min field value is empty then min validation message should not be displayed', () =>{
                assertValidator.assertMinValueValidator(null, true, pageObjectSelectors.minField);
                testValidationMessageDisplay(false, pageObjectSelectors.minField);
            });

            it('when min field value is empty then max validation message should not be displayed', () =>{
                assertValidator.assertMaxValueValidator(null, true, pageObjectSelectors.minField);
                testValidationMessageDisplay(false, pageObjectSelectors.minField);
            });

            it('when min field value is not lower than zero validation message should not be displayed', () =>{
                assertValidator.assertMinValueValidator(0, true, pageObjectSelectors.minField);
                testValidationMessageDisplay(false, pageObjectSelectors.minField);
            });

            it('when min field value is lower than zero then validation message should be displayed', () =>{
                assertValidator.assertMinValueValidator(-1, false, pageObjectSelectors.minField);
                testValidationMessageDisplay(true, pageObjectSelectors.minField);
            });

            it('when max field value is not lower than 0 then validation message should not be displayed', () =>{
                assertValidator.assertMinValueValidator(0, true, pageObjectSelectors.maxField);
                testValidationMessageDisplay(false, pageObjectSelectors.maxField);
            });

            it('when max field value is lower then 0 then validation message should be displayed', () =>{
                assertValidator.assertMinValueValidator(-1, false, pageObjectSelectors.maxField);
                testValidationMessageDisplay(true, pageObjectSelectors.maxField);
            });

            it('when min field value is higher than max field value then validation message should be displayed', () =>{
                var value = 1000;
                var max = element.find(pageObjectSelectors.maxField);
                max.val(value).trigger('input').trigger('change').trigger('blur');
                assertValidator.assertMaxValueValidator(value + 1, false, pageObjectSelectors.minField);
                testValidationMessageDisplay(true, pageObjectSelectors.minField);
            });

            it('when min field value is not higher than max field value then validation message should not be displayed', () =>{
                var value = 1000;
                var max = element.find(pageObjectSelectors.maxField);
                max.val(value).trigger('input').trigger('change').trigger('blur');
                assertValidator.assertMaxValueValidator(value, true, pageObjectSelectors.minField);
                testValidationMessageDisplay(false, pageObjectSelectors.minField);
            });

            it('when max field value is changed to lower than min field value then validation message should be displayed', () =>{
                var value = 0;
                var max = element.find(pageObjectSelectors.maxField);
                max.val(value).trigger('input').trigger('change').trigger('blur');
                testValidationMessageDisplay(true, pageObjectSelectors.minField);
        });

            describe('given attribute range is rendered', () =>{
            var scope: ng.IScope,
                element: ng.IAugmentedJQuery;

            var pageObjectSelectors = {
                    attributeValue : '.attribute-value div:not(.ng-hide) span:not(.ng-hide)'
            };

            var controller: RangeAttributeController;

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                    $compile: ng.ICompileService) =>{

                scope = $rootScope.$new();
                element = $compile('<range-attribute mode="view" min-value="attribute.minValue" max-value="attribute.maxValue" attribute="attribute"></range-attribute>')(scope);

                scope.$apply();

                controller = element.controller('rangeAttributeController');
            }));

                it('and attribute values are specified then valid text is visible', () =>{
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

                    testData.forEach((data) =>{
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
    });
};