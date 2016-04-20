/// <reference path="../../../typings/_all.d.ts" />
module Antares {
    import Dto = Common.Models.Dto;

    describe('propertyAddController', () => {
        var controller: Property.PropertyAddController;
        
        beforeEach(inject(($rootScope: ng.IRootScopeService, dataAccessService: Services.DataAccessService, componentRegistry: Core.Service.ComponentRegistry) => {
            var controllerFunction = Property.PropertyAddController;
            controller = Object.create(controllerFunction.prototype);
            controller.userData = <Dto.IUserData>{
                division: <Dto.IEnumTypeItem>{id: 'enumId', code: 'code'}
            };
            var scope: ng.IScope = $rootScope.$new();

            controllerFunction.apply(controller, [componentRegistry, dataAccessService, scope]);
        }));

        describe('when is constructed', () =>{
            it('property should contain division from userData', () =>{
                expect(controller.property.division.id).toBe('enumId');
                expect(controller.property.division.code).toBe('code');
            });

            it('property should contain attribute values initiated', () => {
                expect(controller.property.attributeValues).not.toBeUndefined();
            });
        });

        describe('when changeDivision', () =>{
            beforeEach(() =>{
                var division: Dto.IEnumTypeItem = <Dto.IEnumTypeItem>{ id : 'newEnumId', code : 'newCode' }
                controller.changeDivision(division);
            });

            it('should change property division', () =>{
                expect(controller.property.division.id).toBe('newEnumId');
                expect(controller.property.division.code).toBe('newCode');
            });
        });
    });
}