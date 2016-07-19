/// <reference path="../../../typings/_all.d.ts" />
module Antares {
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;
    import Component = Offer.Component;

    describe('Given OfferEditPreviewPanelController', () => {
        var
            offer: Business.Offer,
            requirement: Business.Requirement,
            createController: (mode: Enums.PanelMode) => Component.OfferEditPreviewPanelController;

        var timeout: any;

        beforeEach(inject(($q: ng.IQService, $timeout: ng.ITimeoutService, $controller: any) =>{

            timeout = $timeout;

            var getOfferMock = (pageType: Enums.PageTypeEnum, requirementTypeId: string, offerTypeId: string, entity: any): ng.IPromise<any> => {
                var deferred = $q.defer();
                timeout(() =>{
                    deferred.resolve();
                });
                return deferred.promise;
            };

            var updateOfferMock = (updateOfferCommand: Business.UpdateOfferCommand): ng.IPromise<any> => {
                var deferred = $q.defer();
                timeout(() =>{
                    deferred.resolve();
                });
                return deferred.promise;
            };

            offer = TestHelpers.OfferGenerator.generate();
            requirement = TestHelpers.RequirementGenerator.generate();

            createController = (mode: Enums.PanelMode): Component.OfferEditPreviewPanelController => {

                var parameters = {
                    'configService': { 'getOffer': getOfferMock },
                    'eventAggregator': { 'publish': {} },
                    'offerService': { 'updateOffer': updateOfferMock }
                };

                var bindings = {
                    'config': {},
                    'offer': offer,
                    'mode': mode,
                    'requirement': requirement
                };

                return <Component.OfferEditPreviewPanelController>$controller('offerEditPreviewPanelController', parameters, bindings);
            }

        }));

        describe('when panelShow in Preview Mode', () =>{

            var controller: Component.OfferEditPreviewPanelController,
                resetState: jasmine.Spy;

            beforeEach(inject(() =>{
                controller = createController(Enums.PanelMode.Preview);
                spyOn(controller['configService'], 'getOffer').and.callThrough();
                resetState = spyOn(controller, 'resetState').and.callThrough();
                controller.panelShown();
                timeout.flush();
            }));

            it('then should prepare preview view', () => {
                expect(controller.cardPristine).toBeDefined();
                expect(controller.isPreviewCardVisible).toBeTruthy();
                expect(controller.isEditCardVisible).toBeFalsy();
                expect(controller['configService'].getOffer).toHaveBeenCalledWith(Enums.PageTypeEnum.Preview, requirement.requirementTypeId, offer.offerTypeId, offer);
                expect(resetState).toHaveBeenCalled();
            });

        });

        describe('when panelShow in Edit Mode', () => {

            var controller: Component.OfferEditPreviewPanelController,
                resetState: jasmine.Spy;

            beforeEach(inject(() =>{
                controller = createController(Enums.PanelMode.Edit);
                spyOn(controller['configService'], 'getOffer').and.callThrough();
                resetState = spyOn(controller, 'resetState').and.callThrough();
                controller.panelShown();
                timeout.flush();
            }));

            it('then should prepare preview view', () => {
                expect(controller.backToPreview).toBeFalsy();
                expect(controller.cardPristine).toBeDefined();
                expect(controller.isPreviewCardVisible).toBeFalsy();
                expect(controller.isEditCardVisible).toBeTruthy();
                expect(controller['configService'].getOffer).toHaveBeenCalledWith(Enums.PageTypeEnum.Update, requirement.requirementTypeId, offer.offerTypeId, offer);
                expect(resetState).toHaveBeenCalled();
            });

        });

        describe('when save in Edit Mode go from Preview Mode', () => {

            var controller: Component.OfferEditPreviewPanelController;

            beforeEach(inject(() =>{
                controller = createController(Enums.PanelMode.Preview);
                spyOn(controller['configService'], 'getOffer').and.callThrough();
                spyOn(controller['offerService'], 'updateOffer').and.callThrough();
                spyOn(controller['eventAggregator'], 'publish');

                controller.panelShown();
                timeout.flush();
                controller.edit();
                timeout.flush();
                controller.save(offer);
                timeout.flush();
            }));

            it('then should updated ', () => {
                expect(controller['offerService'].updateOffer).toHaveBeenCalledWith(jasmine.any(Business.UpdateOfferCommand));
                expect(controller['eventAggregator'].publish).toHaveBeenCalledTimes(1);
                expect(controller.mode).toEqual(Enums.PanelMode.Preview);
            });

        });

        describe('when save in Edit Mode', () => {

            var controller: Component.OfferEditPreviewPanelController;

            beforeEach(inject(() =>{
                controller = createController(Enums.PanelMode.Edit);
                spyOn(controller['offerService'], 'updateOffer').and.callThrough();
                spyOn(controller['eventAggregator'], 'publish');

                controller.panelShown();
                timeout.flush();
                controller.save(offer);
                timeout.flush();
            }));

            it('then should updated ', () => {
                expect(controller['offerService'].updateOffer).toHaveBeenCalledWith(jasmine.any(Business.UpdateOfferCommand));
                expect(controller['eventAggregator'].publish).toHaveBeenCalledTimes(2);
                expect(controller['eventAggregator'].publish).toHaveBeenCalledWith(new Common.Component.CloseSidePanelEvent());
            });
        });

    });
}