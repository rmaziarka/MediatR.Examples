/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import CharacteristicListController = Common.Component.CharacteristicListController;
    import Business = Common.Models.Business;

    describe('Given characteristic list controller', () => {
        var $scope: ng.IScope,
            $http: ng.IHttpBackendService,
            controller: CharacteristicListController;

        var propertyTypes: any = {
            House: { id: "8b152e4f-f505-e611-828c-8cdcd42baca7", parentId: null, name: "House", children: [], order: 22 }
        }
        var countries: any = {
            GB: { id: 'countryGB', isoCode: 'GB' }
        };

        describe('when clearCharacteristics is called', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                // 'any' must be used instead of 'ng.IControllerService' because there is invalid typing for this service function,
                // that sais that 3rd argument is bool, but in fact it is an object containing bindings for controller (for comonents and directives)
                $controller: any,
                $httpBackend: ng.IHttpBackendService) => {

                // init
                $scope = $rootScope.$new();
                $http = $httpBackend;

                var bindings = { property: new Business.Property() };
                controller = <CharacteristicListController>$controller('CharacteristicListController', {}, bindings);
            }));

            it('then characteristicGroups is set to empty', () => {
                // arrange
                var characteristicGroupsMock = TestHelpers.CharacteristicGroupUsageGenerator.generateMany(5);
                controller.characteristicGroups = characteristicGroupsMock;

                //act
                controller.clearCharacteristics();

                // assert
                expect(controller.characteristicGroups).toEqual([]);
            });
        });

        describe('when loadCharacteristics is called', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                // 'any' must be used instead of 'ng.IControllerService' because there is invalid typing for this service function,
                // that sais that 3rd argument is bool, but in fact it is an object containing bindings for controller (for comonents and directives)
                $controller: any,
                $httpBackend: ng.IHttpBackendService) => {

                // init
                $scope = $rootScope.$new();
                $http = $httpBackend;

                var bindings = { property: new Business.Property() };
                controller = <CharacteristicListController>$controller('CharacteristicListController', {}, bindings);
            }));

            it('and country is empty then characteristicGroups is set to empty', () => {
                // arrange
                var characteristicGroupsMock = TestHelpers.CharacteristicGroupUsageGenerator.generateMany(5);
                var propertyMock: Business.Property = TestHelpers.PropertyGenerator.generate({
                    propertyTypeId: propertyTypes.House.id
                });
                propertyMock.address.countryId = '';

                controller.property = propertyMock;
                controller.characteristicGroups = characteristicGroupsMock;

                //act
                controller.loadCharacteristics();

                // assert
                expect(controller.characteristicGroups).toEqual([]);
            });

            it('and property type is empty then characteristicGroups is set to empty', () => {
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
                expect(controller.characteristicGroups).toEqual([]);
            });

            it('and property type and country are set then request GET for characteristicGroups is called and data returned from request is set to characteristicGroups', () => {
                // arrange
                var characteristicGroupsMock = TestHelpers.CharacteristicGroupUsageGenerator.generateManyDtos(3);
                var propertyMock: Business.Property = TestHelpers.PropertyGenerator.generate({
                    propertyTypeId: propertyTypes.House.id
                });
                propertyMock.address.countryId = countries.GB.id;

                var requestMock = new RegExp('/api/characteristicGroups\\?countryId=' + countries.GB.id + '&propertyTypeId=' + propertyTypes.House.id);

                $http.expectGET(requestMock).respond(() => {
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