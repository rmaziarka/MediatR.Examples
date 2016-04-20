/// <reference path="../../../../typings/_all.d.ts" />

module Antares {
    import PropertyDetailsController = Property.View.Details.PropertyDetailsController;
    import Dto = Common.Models.Dto;

    describe('Given property details component is rendered', () => {
        var scope: ng.IScope,
            $http: ng.IHttpBackendService,
            element: ng.IAugmentedJQuery;

        var pageObjectSelectors = {
            propertyType: '.page-header:contains("PROPERTY.VIEW.DETAILS")~.row :contains("PROPERTY.VIEW.TYPE") + div'
        };

        var userMock = { name: "user", email: "user@gmail.com", country: "GB", division: { id: "0acc9d28-51fa-e511-828b-8cdcd42baca7", value: "Commercial", code: "Commercial" }, divisionCode: <any>null, roles: ["admin", "superuser"] };

        var controller: PropertyDetailsController;

        describe('and property is specified', () => {
            var propertyMock: Dto.IProperty = {
                id: '1',
                propertyTypeId: 'propType1',
                divisionId: '',
                division: null,
                address: Mock.AddressForm.FullAddress,
                ownerships: [],
                activities: [],
                attributeValues: []
            };

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) =>{

                $http = $httpBackend;

                $http.whenGET(/\/api\/properties\/attributes/).respond(() => {
                    return [200, {}];
                });

                scope = $rootScope.$new();
                scope['property'] = propertyMock;
                scope['userData'] = userMock;
                element = $compile('<property-details property="property" user-data="userData"></property-details>')(scope);

                scope.$apply();

                controller = element.controller('propertyDetails');
            }));

            it('property type is visible', () => {
                // assert
                var propertyTypeElement = element.find(pageObjectSelectors.propertyType);
                expect(propertyTypeElement.text()).toBe('DYNAMICTRANSLATIONS.propType1');
            });
        });
    });
}