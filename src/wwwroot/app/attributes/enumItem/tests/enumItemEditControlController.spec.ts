/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import EnumItemEditControlController = Attributes.EnumItemEditControlController;

    describe('Given enum item edit control controller', () => {
        var $scope: ng.IScope,
            controller: EnumItemEditControlController,
            enumProvider: Antares.Providers.EnumProvider;

        var schemaMock: Attributes.IEnumItemEditControlSchema = {
            fieldName: 'activityStatusId',
            formName: 'formName',
            controlId: 'controlId',
            enumTypeCode: Dto.EnumTypeCode.ActivityStatus,
            translationKey: 'translationKey'
        };

        var activityStatusCodes = [
            { id: "status1", code: "code1" },
            { id: "status2", code: "code2" }
        ];

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $controller: any,
            $httpBackend: ng.IHttpBackendService,
            _enumProvider_: Antares.Providers.EnumProvider) => {

            // init
            $scope = $rootScope.$new();
            enumProvider = _enumProvider_;
            controller = <EnumItemEditControlController>$controller('EnumItemEditControlController');
            enumProvider.enums = <Dto.IEnumDictionary>{
                activityStatus: activityStatusCodes
            };


            controller.config = TestHelpers.ConfigGenerator.generateActivityStatusEditConfig();;
            controller.schema = schemaMock;
            controller.$onInit();
        }));

        it('when no filter for allowed code then all enum items should be available', () => {
            var configMock: Attributes.IActivityStatusEditControlConfig = TestHelpers.ConfigGenerator.generateActivityStatusEditConfig();
            configMock.activityStatusId.allowedCodes = null;
            controller.config = configMock;
            
            // act
            var enumItems = controller.getEnumItems();

            // assert
            expect(enumItems.length).toBe(2);
            expect(enumItems[0]).toBe(enumProvider.enums.activityStatus[0]);
            expect(enumItems[1]).toBe(enumProvider.enums.activityStatus[1]);
        });

        it('when filter for allowed code then those enum items should be available', () => {
            var configMock: Attributes.IActivityStatusEditControlConfig = TestHelpers.ConfigGenerator.generateActivityStatusEditConfig();
            configMock.activityStatusId.allowedCodes = ['code2'];
            controller.config = configMock;

            // act
            var enumItems = controller.getEnumItems();

            // assert
            expect(enumItems.length).toBe(1);
            expect(enumItems[0]).toBe(enumProvider.enums.activityStatus[1]);
        });

    });
}