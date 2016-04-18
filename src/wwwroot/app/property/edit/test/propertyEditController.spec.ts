/// <reference path="../../../typings/_all.d.ts" />
module Antares {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import PropertyEditController = Property.PropertyEditController;

    describe('propertyEditController', () => {
        var controller: Property.PropertyEditController;
        
        beforeEach(inject((dataAccessService: Services.DataAccessService) => {
            var controllerFunction = Property.PropertyEditController;
            controller = Object.create(controllerFunction.prototype);
            controller.userData = <Dto.IUserData>{
                division: <Dto.IEnumTypeItem>{id: 'enumId', code: 'code'}
            }; 
            var property = <Dto.IProperty>{};
            controller.property = new Business.Property();
            
            controllerFunction.apply(controller, [dataAccessService]);
        }));

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