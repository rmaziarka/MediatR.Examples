/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import RequirementAddController = Requirement.Add.RequirementAddController;
    import Business = Common.Models.Business;

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

        var requirementMock: Business.Requirement = TestHelpers.RequirementGenerator.generate({
            maxPrice: 444,
            minPrice: 0,
            maxBedrooms: 3,
            maxReceptionRooms: 2,
            maxBathrooms: 2,
            maxParkingSpaces: 12,
            maxArea: 1234,
            maxLandArea: 5678
        });

        var configMock: Requirement.IRequirementAddConfig = TestHelpers.ConfigGenerator.generateRequirementAddConfig();
        var controller: RequirementAddController;

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            $httpBackend: ng.IHttpBackendService) =>{

            $http = $httpBackend;
            $http.whenGET(/\/api\/resources\/countries\/addressform/).respond(() => {
                return [200, []];
            });

            $http.whenGET(/\/api\/requirements\/types/).respond(() => {
                return [200, []];
            });

            scope = $rootScope.$new();
            scope['config'] = configMock;
            element = $compile('<requirement-add config="config"></requirement-add>')(scope);
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
    });
}