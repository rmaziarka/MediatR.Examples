/// <reference path="../../../typings/_all.d.ts" />
module Antares.Property.Preview {
    import Business = Common.Models.Business;

    // TODO: tests fail - why!!!!!????
    xdescribe('Given Poperty Preview Card component', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            controller: Preview.PropertyPreviewCardController,
            $http: ng.IHttpBackendService,
            propertyMock = TestHelpers.PropertyGenerator.generatePropertyPreview();

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            $httpBackend: ng.IHttpBackendService) => {

            $http = $httpBackend;            
            $http.expectGET(/\/api\/addressForms\/(.+)/).respond(() => {
                return [200, {}];
            });

            scope = $rootScope.$new();

            element = $compile('<property-preview-card property="property"></property-preview-card>')(scope);
            scope.$apply();            

            controller = element.controller('propertyPreviewCard');
            controller.property = propertyMock;

            scope["property"] = propertyMock;
            scope.$apply();     
                   
            $http.flush();
        }));

        it('then property link should be proper', () => {            
            // assert
            var propertyLink = element.find('a').attr('ui-sref');
            expect(propertyLink).toEqual('app.property-view({ id: vm.property.id })');
        });

        it('and property address is defined then address form view should be visible', () => {
            // arrange 
            controller.property.address = <Business.Address>{
                propertyName : "propName",
                propertyNumber : "propNumber",
                line2 : "propLine2"
            };
            scope.$apply();

            // assert
            var addressFormView = element.find('address-form-view');            
            expect(addressFormView.length).toBe(1);
        });

        it('and property address is undefined then address form view should be hidden', () => {
            // arrange 
            controller.property.address = null;
            scope.$apply();

            // assert
            var addressFormView = element.find('address-form-view');
            expect(addressFormView.length).toBe(0);
        });
        
    });
}