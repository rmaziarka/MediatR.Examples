/// <reference path="../../../typings/_all.d.ts" />
module Antares {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import Enums = Common.Models.Enums;

    describe('offerAddPanelController', () => {
        var
            controller: Offer.OfferAddPanelController,
            configSrv: Services.ConfigService,
            eventAggr: Core.EventAggregator,
            offerSrv: Services.OfferService,
            requirement: Business.Requirement = TestHelpers.RequirementGenerator.generate({
                requirementType: { enumCode: 'ResidentialLetting' }
            }),
            activity: Business.Activity = TestHelpers.ActivityGenerator.generate(),
            offerTypesMock: Dto.IOfferType[] = [
                { id: '1', enumCode: 'ResidentialLetting' },
                { id: '2', enumCode: 'ResidentialSale' }
            ],
            controlsConfig: Offer.IOfferAddPanelConfig = TestHelpers.ConfigGenerator.generateOfferAddPanelConfig(),
            scope: ng.IScope,
            promiseWith: (obj: any) => ng.IPromise<any>;


        beforeEach(inject((
            configService: Services.ConfigService,
            eventAggregator: Core.EventAggregator,
            offerService: Services.OfferService,
            $q: ng.IQService,
            $rootScope: ng.IRootScopeService,
            $controller: ng.IControllerService) => {

            configSrv = configService;
            eventAggr = eventAggregator;
            offerSrv = offerService;
            scope = $rootScope.$new();

            promiseWith = (obj: any) => {
                var deferred = $q.defer();
                deferred.resolve(obj);
                return deferred.promise;
            };

            controller = $controller(Offer.OfferAddPanelController);
            controller.requirement = requirement;
            controller.activity = activity;
        }));

        describe('when panelShown is called', () => {
            beforeEach(() => {
                spyOn(offerSrv, 'getOfferTypes').and.returnValue(promiseWith(offerTypesMock));
                spyOn(configSrv, 'getOffer').and.returnValue(promiseWith(controlsConfig));

                controller.panelShown();
                scope.$apply(); // needed to resolve promises
            });

            it('offer types are loaded', () => {
                var resiLettingOfferType = _.find(offerTypesMock, { enumCode: 'ResidentialLetting' });

                expect(offerSrv.getOfferTypes).toHaveBeenCalled();
                expect(controller.offerTypes).toBe(offerTypesMock);
                expect(controller.offerTypeId).toEqual(resiLettingOfferType.id);
            });

            it('control configuration is loaded', () => {
                expect(configSrv.getOffer).toHaveBeenCalledWith(Enums.PageTypeEnum.Create, requirement.requirementTypeId, controller.offerTypeId, {});
            });

            it('isBusy flag is false', () => {
                expect(controller.isBusy).toBeFalsy();
            });
        });

        describe('when cancel is called', () => {
            beforeEach(() => {
                spyOn(eventAggr, 'publish');
                controller.cancel();
            });

            it('isBusy flag is false', () => {
                expect(controller.isBusy).toBeFalsy();
            });

            it('CloseSidePanelEvent should be published', () => {
                expect(eventAggr.publish).toHaveBeenCalledWith(new Common.Component.CloseSidePanelEvent());
            });
        });

        describe('when save is called', () => {
            var
                savedOfferMock: Dto.IOffer = TestHelpers.OfferGenerator.generate(),
                createOfferCommand: Business.CreateOfferCommand;

            beforeEach(() => {
                createOfferCommand = new Business.CreateOfferCommand();
                spyOn(offerSrv, 'createOffer').and.returnValue(promiseWith(savedOfferMock));
                spyOn(eventAggr, 'publish');

                controller.save(createOfferCommand);
                scope.$apply();
            });

            it('offer is saved', () => {
                expect(offerSrv.createOffer).toHaveBeenCalledWith(createOfferCommand);
            });

            it('CloseSidePanelEvent should be published', () => {
                expect(eventAggr.publish).toHaveBeenCalledWith(new Common.Component.CloseSidePanelEvent());
            });

            it('OfferAddedSidePanelEvent should be published', () => {
                expect(eventAggr.publish).toHaveBeenCalledWith(new Offer.OfferAddedSidePanelEvent(savedOfferMock));
            });

            it('isBusy flag is false', () => {
                expect(controller.isBusy).toBeFalsy();
            });
        });
    });
}