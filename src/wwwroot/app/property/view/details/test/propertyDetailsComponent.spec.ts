/// <reference path="../../../../typings/_all.d.ts" />

module Antares {
    import PropertyDetailsController = Property.View.Details.PropertyDetailsController;
    import Dto = Common.Models.Dto;
    import Business = Antares.Common.Models.Business;

    describe('Given property details component is rendered', () => {
        var scope: ng.IScope,
            $http: ng.IHttpBackendService,
            element: ng.IAugmentedJQuery;

        var pageObjectSelectors = {
            propertyType: '.page-header:contains("PROPERTY.VIEW.DETAILS")~.row :contains("PROPERTY.VIEW.TYPE") + div',
            attributeListElement: 'attribute-list',
            attributeElements: 'attribute-list .attribute',
            attributeLabel: '.attribute-label',
            attributeMinValue: '.attribute-value .min-value',
            attributeMaxValue: '.attribute-value .max-value'
        };

        var userMock = { name: "user", email: "user@gmail.com", country: "GB", division: { id: "0acc9d28-51fa-e511-828b-8cdcd42baca7", value: "Commercial", code: "Commercial" }, divisionCode: <any>null, roles: ["admin", "superuser"] };

        var attributesMock: any = {
            attributes: [
                new Business.Attribute( { order: 1, nameKey: "Receptions", labelKey: "PROPERTY.RECEPTIONS" }),
                new Business.Attribute({ order: 0, nameKey: "Bedrooms", labelKey: "PROPERTY.BEDROOMS" }),
                new Business.Attribute({ order: 2, nameKey: "Bathrooms", labelKey: "PROPERTY.BATHROOMS" }),
                new Business.Attribute({ order: 3, nameKey: "Area", labelKey: "PROPERTY.AREA" }),
                new Business.Attribute({ order: 4, nameKey: "LandArea", labelKey: "PROPERTY.LANDAREA" }),
                new Business.Attribute( { order: 5, nameKey: "CarParkingSpaces",    labelKey: "PROPERTY.CARPARKINGSPACES" })]
        };

        var controller: PropertyDetailsController;

        describe('and property is specified', () =>{
            var propertyMock = TestHelpers.PropertyGenerator.generate(
            {
                id : '1',
                propertyTypeId : 'propType1',
                attributeValues : {
                    minBedrooms : 1,
                    maxBedrooms : 2,
                    minReceptions : 3,
                    maxReceptions : 4,
                    minBathrooms : 5,
                    maxBathrooms : 6,
                    minArea : 7,
                    maxArea : null,
                    minLandArea : 8,
                    maxLandArea : 9,
                    minCarParkingSpaces : 10,
                    maxCarParkingSpaces : 11,
                }
            });

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) => {

                $http = $httpBackend;

                $http.whenGET(/\/api\/properties\/attributes/).respond(() => {
                    return [200, attributesMock];
                });

                scope = $rootScope.$new();
                scope['property'] = propertyMock;
                scope['userData'] = userMock;
                element = $compile('<property-details property="property" user-data="userData"></property-details>')(scope);

                scope.$apply();
                $http.flush();
                controller = element.controller('propertyDetails');
            }));

            it('property type is visible', () => {
                // assert
                var propertyTypeElement = element.find(pageObjectSelectors.propertyType);
                expect(propertyTypeElement.text()).toBe('DYNAMICTRANSLATIONS.propType1');
            });

            it('attributes component is in view mode', () => {
                // assert
                var attributeListElement: ng.IAugmentedJQuery = element.find(pageObjectSelectors.attributeListElement);
                expect(attributeListElement.attr('mode')).toBe('view');
            });

            it('attributes are in proper order and have proper values', () => {
                var attributeElements: ng.IAugmentedJQuery = element.find(pageObjectSelectors.attributeElements);
                attributesMock.attributes.forEach((am: Business.Attribute) => {

                    var attributeLabel : string = $(attributeElements[am.order]).find(pageObjectSelectors.attributeLabel).first().text();
                    var attributeMinValue : string = $(attributeElements[am.order]).find(pageObjectSelectors.attributeMinValue).first().text();
                    var attributeMaxValue : string = $(attributeElements[am.order]).find(pageObjectSelectors.attributeMaxValue).first().text();

                    expect(attributeLabel).toBe(am.labelKey);
                    expect(attributeMinValue).toEqual((propertyMock.attributeValues['min' + am.nameKey] || '').toString());
                    expect(attributeMaxValue).toEqual((propertyMock.attributeValues['max' + am.nameKey] || '').toString());
                });
            });
        });
    });
}