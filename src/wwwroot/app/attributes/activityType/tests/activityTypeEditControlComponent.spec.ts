/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import ActivityTypeEditControlController = Attributes.ActivityTypeEditControlController;

    interface IScope extends ng.IScope {
        leadNegotiator: Business.ActivityUser;
        secondaryNegotiators: Business.ActivityUser[];
        propertyDivisionId: string;
    }

    describe('Given activity type edit component', () => {
        var scope: IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            $http: ng.IHttpBackendService,
            controller: ActivityTypeEditControlController,
            assertValidator: TestHelpers.AssertValidators;

        var typeMock: number = 1;
        var typeChangedMock = () => { };
        var pageObjectSelector = {
            type: '#type'
        };

        var mockedComponentHtml = '<activity-type-edit-control ng-model="vm.ngModel" config="vm.config" on-activity-type-changed="vm.typeChanged()"></activity-type-edit-control>';

        var activityTypeCodes = [
            { id: "type1", code: "type1" },
            { id: "type2", code: "type2" }
        ];

        var configMock: Attributes.IActivityTypeEditControlConfig = TestHelpers.ConfigGenerator.generateActivityTypeEditConfig();

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            $httpBackend: ng.IHttpBackendService,
            enumService: Mock.EnumServiceMock) => {
            
            // init
            $http = $httpBackend;
            scope = <IScope>$rootScope.$new();
            scope['vm'] = { ngModel: typeMock, config: configMock, typeChanged: typeChangedMock };
            spyOn(scope['vm'], 'typeChanged');
            enumService.setEnum(Dto.EnumTypeCode.ActivityStatus.toString(), activityTypeCodes);

            $http.whenGET(/\/api\/activities\/types/).respond(() => {
                return [200, activityTypeCodes];
            });

            compile = $compile;
            element = compile(mockedComponentHtml)(scope);
            scope.$apply();
            controller = element.controller('activityTypeEditControl');

            assertValidator = new TestHelpers.AssertValidators(element, scope);
        }));

        describe('when is rendered', () => {
            it('with config it should display type', () => {
                var activityType = element.find(pageObjectSelector.type);
                expect(activityType.length).toEqual(1);
            });

            it('when select value is change then on change method is called', () => {
                scope['vm']['config'] = TestHelpers.ConfigGenerator.generateActivityTypeEditConfig();

                controller.ngModel = 'type2';
                scope.$apply();
                element.find(pageObjectSelector.type).triggerHandler('change');
                expect(scope['vm']['typeChanged']).toHaveBeenCalled();
            });
        });
    });
}