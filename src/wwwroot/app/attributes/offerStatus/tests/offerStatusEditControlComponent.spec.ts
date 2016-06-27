/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import Dto = Common.Models.Dto;
    import OfferStatusEditControlController = Attributes.OfferStatusEditControlController;

    interface IScope extends ng.IScope {
    }

    describe('Given offer status edit component', () => {
        var scope: IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            controller: OfferStatusEditControlController,
            assertValidator: TestHelpers.AssertValidators;

        var statusMock: number = 1;
        var statusChangedMock = () => { };
        var pageObjectSelector = {
            status: '#status'
        };

        var mockedComponentHtml = '<offer-status-control ng-model="vm.ngModel" config="vm.config" on-offer-status-changed="vm.statusChanged()"></offer-status-control>';

        var offerStatusCodes = [
            { id: "status1", code: "status1" },
            { id: "status2", code: "status2" }
        ];

        var configMock: Attributes.IOfferStatusEditControlConfig = TestHelpers.ConfigGenerator.generateOfferStatusEditConfig();

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            enumService: Mock.EnumServiceMock) => {
            
            // init
            scope = <IScope>$rootScope.$new();
            scope['vm'] = { ngModel: statusMock, config: configMock, statusChanged: statusChangedMock };
            spyOn(scope['vm'], 'statusChanged');
            enumService.setEnum(Dto.EnumTypeCode.OfferStatus.toString(), offerStatusCodes);

            compile = $compile;
            element = compile(mockedComponentHtml)(scope);
            (<ng.IScope>scope).$apply();
            controller = element.controller('offerStatusControl');

            assertValidator = new TestHelpers.AssertValidators(element, scope);
        }));

        describe('when is rendered', () => {
            it('with config it should display status', () => {
                var offerStatus = element.find(pageObjectSelector.status);
                expect(offerStatus.length).toEqual(1);
            });

            it('without config it should not display status', () => {
                scope['vm']['config'] = undefined;
                (<ng.IScope>scope).$apply();
                var offerStatus = element.find(pageObjectSelector.status);
                expect(offerStatus.length).toEqual(0);
            });

            it('verify select html attributes', () => {
                scope['vm']['config'] = TestHelpers.ConfigGenerator.generateOfferStatusEditConfig();
                scope['vm']['config'].statusId = { required: true, active: true };
                (<ng.IScope>scope).$apply();
                var offerStatus = element.find(pageObjectSelector.status);
                expect((<HTMLSelectElement>offerStatus[0]).disabled).toEqual(false);
                expect((<HTMLSelectElement>offerStatus[0]).required).toEqual(true);
            });

            it('when select value is changed then on change method is called', () => {
                scope['vm']['config'] = TestHelpers.ConfigGenerator.generateOfferStatusEditConfig();

                controller.ngModel = 'status2';
                (<ng.IScope>scope).$apply();
                element.find(pageObjectSelector.status).triggerHandler('change');
                expect(scope['vm']['statusChanged']).toHaveBeenCalled();
            });
        });
    });
}