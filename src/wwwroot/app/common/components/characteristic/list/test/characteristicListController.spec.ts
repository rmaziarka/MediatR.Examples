/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import CharacteristicListController = Common.Component.CharacteristicListController;
    import Business = Common.Models.Business;

    describe('Given characteristic list controller', () => {
        var $scope: ng.IScope,
            $http: ng.IHttpBackendService,
            controller: CharacteristicListController;

        describe('when loadCharacteristics is called', () =>{
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                // 'any' must be used instead of 'ng.IControllerService' because there is invalid typing for this service function,
                // that sais that 3rd argument is bool, but in fact it is an object containing bindings for controller (for comonents and directives)
                $controller: any,
                $httpBackend: ng.IHttpBackendService) =>{

                // init
                $scope = $rootScope.$new();
                $http = $httpBackend;

                var bindings = { property : new Business.Property() };
                controller = <CharacteristicListController>$controller('CharacteristicListController', {}, bindings);
            }));

            //TODO - uncomment this test when loading attributes and characteristics by coubntryId/Isocode is properly implemented
            xit('and country is empty then characteristicGroups is not updated', () => {
                // arrange
                var characteristicGroupsMock = TestHelpers.CharacteristicGroupUsageGenerator.generateMany(5);
                var propertyMock: Business.Property = TestHelpers.PropertyGenerator.generate({
                    propertyTypeId: 'testPropertyTypeId'
                });
                propertyMock.address.countryId = '';

                controller.property = propertyMock;
                controller.characteristicGroups = characteristicGroupsMock;

                //act
                controller.loadCharacteristics();

                // assert
                expect(controller.characteristicGroups).toEqual(characteristicGroupsMock);
            });

            it('and property type is empty then characteristicGroups is not updated', () => {
                // arrange
                var characteristicGroupsMock = TestHelpers.CharacteristicGroupUsageGenerator.generateMany(5);
                var propertyMock: Business.Property = TestHelpers.PropertyGenerator.generate({
                    propertyTypeId: ''
                });

                controller.property = propertyMock;
                controller.characteristicGroups = characteristicGroupsMock;

                //act
                controller.loadCharacteristics();

                // assert
                expect(controller.characteristicGroups).toEqual(characteristicGroupsMock);
            });

            it('and property type and country are set then request GET for characteristicGroups is called and data returned from request is set to characteristicGroups', () => {
                // arrange
                var characteristicGroupsMock = TestHelpers.CharacteristicGroupUsageGenerator.generateManyDtos(3);
                var propertyMock: Business.Property = TestHelpers.PropertyGenerator.generate({
                    propertyTypeId: 'testPropertyTypeId'
                });

                $http.expectGET(/\/api\/characteristicGroups\?countryCode=GB&propertyTypeId=testPropertyTypeId/).respond(() => {
                    return [200, characteristicGroupsMock];
                });

                controller.property = propertyMock;

                //act
                controller.loadCharacteristics();
                $http.flush();

                // assert
                expect(controller.characteristicGroups.length).toEqual(characteristicGroupsMock.length);
                expect(controller.characteristicGroups[0].id).toEqual(characteristicGroupsMock[0].characteristicGroupId);
                expect(controller.characteristicGroups[1].id).toEqual(characteristicGroupsMock[1].characteristicGroupId);
                expect(controller.characteristicGroups[2].id).toEqual(characteristicGroupsMock[2].characteristicGroupId);
            });
        });
    });
}