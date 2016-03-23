/// <reference path="../../../typings/_all.d.ts" />
/// <reference path="../../../common/testhelpers/assertvalidators.ts" />

module Antares {
    import RequirementAddController = Antares.Requirement.Add.RequirementAddController;
    describe('Given requirement is being added', () =>{
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            assertValidator: TestHelpers.AssertValidators,
            $http: ng.IHttpBackendService;

        var pageObjectSelectors = {
            descriptionSelector: 'textarea#description',
            priceMinSelector: 'input#price-min',
            priceMaxSelector: 'input#price-max',
            bedroomsMinSelector: 'input#bedrooms-min',
            bedroomsMaxSelector: 'input#bedrooms-max',
            receptionMinSelector: 'input#reception-min',
            receptionMaxSelector: 'input#reception-max',
            bathroomsMinSelector: 'input#bathrooms-min',
            bathroomsMaxSelector: 'input#bathrooms-max',
            parkingMinSelector: 'input#parking-min',
            parkingMaxSelector: 'input#parking-max',
            areaMinSelector: 'input#area-min',
            areaMaxSelector: 'input#area-max',
            landMinSelector: 'input#land-min',
            landMaxSelector: 'input#land-max'
        };

        var requirementMock = {
            maxPrice: 444,
            minPrice: 0,
            maxBedrooms: 3,
            maxReceptionRooms: 2,
            maxBathrooms: 2,
            maxParkingSpaces: 12,
            maxArea: 1234,
            maxLandArea: 5678
        }

        var controller: RequirementAddController;

        beforeEach(angular.mock.module('app'));

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            $httpBackend: ng.IHttpBackendService) =>{

            $http = $httpBackend;
            $http.whenGET(/\/api\/addressform\/countries/).respond(() => {
                return [200, []];
            });

            scope = $rootScope.$new();
            element = $compile('<requirement-add></requirement-add>')(scope);
            scope.$apply();

            controller = element.controller('requirementAdd');
            controller.requirement = requirementMock;
            scope.$apply();

            assertValidator = new TestHelpers.AssertValidators(element, scope);
        }));

        it('includes address form component', () => {
            var addressFormComponent = element.find('address-form-edit');
            expect(addressFormComponent.length).toBe(1);
        });

        // Description Validations
        it('when description value is too long then validation message should be displayed', () => {
            var maxLength = 4000;
            assertValidator.assertMaxLengthValidator(maxLength + 1, false, pageObjectSelectors.descriptionSelector);
        });

        it('when description value is not too long then validation message should not be displayed', () => {
            var maxLength = 4000;
            assertValidator.assertMaxLengthValidator(maxLength, true, pageObjectSelectors.descriptionSelector);
        });
        
        // Price Validations
        it('when price-min value is lower than minimum value then validation message should be displayed', () => {
            var minValue = 0;
            assertValidator.assertMinValueValidator(minValue - 1, false, pageObjectSelectors.priceMinSelector);
        });

        it('when price-min value is not lower than minimum value then validation message should not be displayed', () => {
            var minValue = 0;
            assertValidator.assertMinValueValidator(minValue, true, pageObjectSelectors.priceMinSelector);
        });

        it('when price-min value is higher than maximum value then validation message should be displayed', () => {
            var maxValue = 444;
            assertValidator.assertMaxValueValidator(maxValue + 1, false, pageObjectSelectors.priceMinSelector);
        });

        it('when price-min value is not higher than maximum value then validation message should not be displayed', () => {
            var maxValue = 444;
            assertValidator.assertMaxValueValidator(maxValue, true, pageObjectSelectors.priceMinSelector);
        });

        it('when price-max value is lower than minimum value then validation message should be displayed', () => {
            var minValue = 0;
            assertValidator.assertMinValueValidator(minValue - 1, false, pageObjectSelectors.priceMaxSelector);
        });

        it('when price-max value is higher than minimum value then validation message should not be displayed', () => {
            var minValue = 0;
            assertValidator.assertMinValueValidator(minValue, true, pageObjectSelectors.priceMaxSelector);
        });

        // Bedrooms
        it('when bedrooms-min value is lower than minimum value then validation message should be displayed', () => {
            var minValue = 0;
            assertValidator.assertMinValueValidator(minValue - 1, false, pageObjectSelectors.bedroomsMinSelector);
        });

        it('when bedrooms-min value is not lower than minimum value then validation message should not be displayed', () => {
            var minValue = 0;
            assertValidator.assertMinValueValidator(minValue, true, pageObjectSelectors.bedroomsMinSelector);
        });

        it('when bedrooms-min value is higher than maximum value then validation message should be displayed', () => {
            var maxValue = 3;
            assertValidator.assertMaxValueValidator(maxValue + 1, false, pageObjectSelectors.bedroomsMinSelector);
        });

        it('when bedrooms-max value is not higher than maximum value then validation message should not be displayed', () => {
            var maxValue = 3;
            assertValidator.assertMaxValueValidator(maxValue, true, pageObjectSelectors.bedroomsMinSelector);
        });

        it('when bedrooms-max value is lower than minimum value then validation message should be displayed', () => {
            var minValue = 0;
            assertValidator.assertMinValueValidator(minValue - 1, false, pageObjectSelectors.bedroomsMaxSelector);
        });

        it('when bedrooms-max value is higher than minimum value then validation message should not be displayed', () => {
            var minValue = 0;
            assertValidator.assertMinValueValidator(minValue, true, pageObjectSelectors.bedroomsMaxSelector);
        });

        // Reception rooms
        it('when reception-min value is lower than minimum value then validation message should be displayed', () => {
            var minValue = 0;
            assertValidator.assertMinValueValidator(minValue - 1, false, pageObjectSelectors.receptionMinSelector);
        });

        it('when reception-min value is not lower than minimum value then validation message should not be displayed', () => {
            var minValue = 0;
            assertValidator.assertMinValueValidator(minValue, true, pageObjectSelectors.receptionMinSelector);
        });

        it('when reception-min value is higher than maximum value then validation message should be displayed', () => {
            var maxValue = 2;
            assertValidator.assertMaxValueValidator(maxValue + 1, false, pageObjectSelectors.receptionMinSelector);
        });

        it('when reception-max value is not higher than maximum value then validation message should not be displayed', () => {
            var maxValue = 2;
            assertValidator.assertMaxValueValidator(maxValue, true, pageObjectSelectors.receptionMinSelector);
        });

        it('when reception-max value is lower than minimum value then validation message should be displayed', () => {
            var minValue = 0;
            assertValidator.assertMinValueValidator(minValue - 1, false, pageObjectSelectors.receptionMaxSelector);
        });

        it('when reception-max value is higher than minimum value then validation message should not be displayed', () => {
            var minValue = 0;
            assertValidator.assertMinValueValidator(minValue, true, pageObjectSelectors.receptionMaxSelector);
        });

        // Bathrooms
        it('when bathrooms-min value is lower than minimum value then validation message should be displayed', () => {
            var minValue = 0;
            assertValidator.assertMinValueValidator(minValue - 1, false, pageObjectSelectors.bathroomsMinSelector);
        });

        it('when bathrooms-min value is not lower than minimum value then validation message should not be displayed', () => {
            var minValue = 0;
            assertValidator.assertMinValueValidator(minValue, true, pageObjectSelectors.bathroomsMinSelector);
        });

        it('when bathrooms-min value is higher than maximum value then validation message should be displayed', () => {
            var maxValue = 2;
            assertValidator.assertMaxValueValidator(maxValue + 1, false, pageObjectSelectors.bathroomsMinSelector);
        });

        it('when bathrooms-min value is not higher than maximum value then validation message should not be displayed', () => {
            var maxValue = 2;
            assertValidator.assertMaxValueValidator(maxValue, true, pageObjectSelectors.bathroomsMinSelector);
        });

        it('when bathrooms-max value is lower than minimum value then validation message should be displayed', () => {
            var minValue = 0;
            assertValidator.assertMinValueValidator(minValue - 1, false, pageObjectSelectors.bathroomsMaxSelector);
        });

        it('when bathrooms-max value is higher than minimum value then validation message should not be displayed', () => {
            var minValue = 0;
            assertValidator.assertMinValueValidator(minValue, true, pageObjectSelectors.bathroomsMaxSelector);
        });

        // Parking spaces
        it('when parking-min value is lower than minimum value then validation message should be displayed', () => {
            var minValue = 0;
            assertValidator.assertMinValueValidator(minValue - 1, false, pageObjectSelectors.parkingMinSelector);
        });

        it('when parking-min value is not lower than minimum value then validation message should not be displayed', () => {
            var minValue = 0;
            assertValidator.assertMinValueValidator(minValue, true, pageObjectSelectors.parkingMinSelector);
        });

        it('when parking-min value is higher than maximum value then validation message should be displayed', () => {
            var maxValue = 12;
            assertValidator.assertMaxValueValidator(maxValue + 1, false, pageObjectSelectors.parkingMinSelector);
        });

        it('when parking-min value is not higher than maximum value then validation message should not be displayed', () => {
            var maxValue = 12;
            assertValidator.assertMaxValueValidator(maxValue, true, pageObjectSelectors.parkingMinSelector);
        });

        it('when parking-max value is lower than minimum value then validation message should be displayed', () => {
            var minValue = 0;
            assertValidator.assertMinValueValidator(minValue - 1, false, pageObjectSelectors.parkingMaxSelector);
        });

        it('when parking-max value is higher than minimum value then validation message should not be displayed', () => {
            var minValue = 0;
            assertValidator.assertMinValueValidator(minValue, true, pageObjectSelectors.parkingMaxSelector);
        });

        // Area
        it('when area-min value is lower than minimum value then validation message should be displayed', () => {
            var minValue = 0;
            assertValidator.assertMinValueValidator(minValue - 1, false, pageObjectSelectors.areaMinSelector);
        });

        it('when area-min value is not lower than minimum value then validation message should not be displayed', () => {
            var minValue = 0;
            assertValidator.assertMinValueValidator(minValue, true, pageObjectSelectors.areaMinSelector);
        });

        it('when area-min value is higher than maximum value then validation message should be displayed', () => {
            var maxValue = 1234;
            assertValidator.assertMaxValueValidator(maxValue + 1, false, pageObjectSelectors.areaMinSelector);
        });

        it('when area-min value is not higher than maximum value then validation message should not be displayed', () => {
            var maxValue = 1234;
            assertValidator.assertMaxValueValidator(maxValue, true, pageObjectSelectors.areaMinSelector);
        });

        it('when area-max value is lower than minimum value then validation message should be displayed', () => {
            var minValue = 0;
            assertValidator.assertMinValueValidator(minValue - 1, false, pageObjectSelectors.areaMaxSelector);
        });

        it('when area-max value is higher than minimum value then validation message should not be displayed', () => {
            var minValue = 0;
            assertValidator.assertMinValueValidator(minValue, true, pageObjectSelectors.areaMaxSelector);
        });

        // Land area
        it('when land-min value is lower than minimum value then validation message should be displayed', () => {
            var minValue = 0;
            assertValidator.assertMinValueValidator(minValue - 1, false, pageObjectSelectors.landMinSelector);
        });

        it('when land-min value is not lower than minimum value then validation message should not be displayed', () => {
            var minValue = 0;
            assertValidator.assertMinValueValidator(minValue, true, pageObjectSelectors.landMinSelector);
        });

        it('when land-min value is higher than maximum value then validation message should be displayed', () => {
            var maxValue = 5678;
            assertValidator.assertMaxValueValidator(maxValue + 1, false, pageObjectSelectors.landMinSelector);
        });

        it('when land-min value is not higher than maximum value then validation message should not be displayed', () => {
            var maxValue = 5678;
            assertValidator.assertMaxValueValidator(maxValue, true, pageObjectSelectors.landMinSelector);
        });

        it('when land-max value is lower than minimum value then validation message should be displayed', () => {
            var minValue = 0;
            assertValidator.assertMinValueValidator(minValue - 1, false, pageObjectSelectors.landMaxSelector);
        });

        it('when land-max value is higher than minimum value then validation message should not be displayed', () => {
            var minValue = 0;
            assertValidator.assertMinValueValidator(minValue, true, pageObjectSelectors.landMaxSelector);
        });
    });
}