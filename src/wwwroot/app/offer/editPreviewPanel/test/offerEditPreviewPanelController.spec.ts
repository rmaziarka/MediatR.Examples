/// <reference path="../../../typings/_all.d.ts" />
module Antares {
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;
    import Component = Offer.Component;
    import TestHelpers = Antares.TestHelpers;

    describe('Given OfferEditPreviewPanelController', () => {
        var
            offer: Business.Offer,
            requirement: Business.Requirement,
            createController: (mode: Component.OfferPanelMode) => Component.OfferEditPreviewPanelController;

        var timeout: any;

        beforeEach(inject(function (
            $q: ng.IQService,
            $timeout: ng.ITimeoutService,
            $controller: any) {

            timeout = $timeout;

            var getOfferMock = (pageType: Enums.PageTypeEnum, requirementTypeId: string, offerTypeId: string, entity: any): ng.IPromise<any> => {
                var deferred = $q.defer();
                timeout(function () {
                    deferred.resolve();
                });
                return deferred.promise;
            };

            var updateOfferMock = (offer: Business.Offer): ng.IPromise<any> => {
                var deferred = $q.defer();
                timeout(function () {
                    deferred.resolve();
                });
                return deferred.promise;
            };

            offer = TestHelpers.OfferGenerator.generate();
            requirement = TestHelpers.RequirementGenerator.generate();

            createController = (mode: Component.OfferPanelMode): Component.OfferEditPreviewPanelController => {

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

        describe('when panelShow in Preview Mode', () => {

            var controller: Component.OfferEditPreviewPanelController;

            beforeEach(inject(function () {
                controller = createController(Component.OfferPanelMode.Preview);
                spyOn(controller['configService'], 'getOffer').and.callThrough();
                controller.panelShown();
                timeout.flush();
            }));

            it('then should prepare preview view', () => {
                expect(controller.backToPreview).toBeTruthy();
                expect(controller.cardPristine).toBeDefined();
                expect(controller.isPreviewCardVisible).toBeTruthy();
                expect(controller.isEditCardVisible).toBeFalsy();
                expect(controller['configService'].getOffer).toHaveBeenCalledWith(Enums.PageTypeEnum.Preview, requirement.requirementTypeId, offer.offerTypeId, offer);
            });

        });

        describe('when panelShow in Edit Mode', () => {

            var controller: Component.OfferEditPreviewPanelController;

            beforeEach(inject(function () {
                controller = createController(Component.OfferPanelMode.Edit);
                spyOn(controller['configService'], 'getOffer').and.callThrough();
                controller.panelShown();
                timeout.flush();
            }));

            it('then should prepare preview view', () => {
                expect(controller.backToPreview).toBeFalsy();
                expect(controller.cardPristine).toBeDefined();
                expect(controller.isPreviewCardVisible).toBeFalsy();
                expect(controller.isEditCardVisible).toBeTruthy();
                expect(controller['configService'].getOffer).toHaveBeenCalledWith(Enums.PageTypeEnum.Update, requirement.requirementTypeId, offer.offerTypeId, offer);
            });

        });

        describe('when save in Edit Mode go from Preview Mode', () => {

            var controller: Component.OfferEditPreviewPanelController;

            beforeEach(inject(function () {
                controller = createController(Component.OfferPanelMode.Preview);
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
                expect(controller['offerService'].updateOffer).toHaveBeenCalledWith(offer);
                expect(controller['eventAggregator'].publish).toHaveBeenCalledTimes(1);
                expect(controller.mode).toEqual(Component.OfferPanelMode.Preview);
            });

        });

        describe('when save in Edit Mode', () => {

            var controller: Component.OfferEditPreviewPanelController;

            beforeEach(inject(function () {
                controller = createController(Component.OfferPanelMode.Edit);
                spyOn(controller['offerService'], 'updateOffer').and.callThrough();
                spyOn(controller['eventAggregator'], 'publish');

                controller.panelShown();
                timeout.flush();
                controller.save(offer);
                timeout.flush();
            }));

            it('then should updated ', () => {
                expect(controller['offerService'].updateOffer).toHaveBeenCalledWith(offer);
                expect(controller['eventAggregator'].publish).toHaveBeenCalledTimes(2);
                expect(controller['eventAggregator'].publish).toHaveBeenCalledWith(new Antares.Common.Component.CloseSidePanelEvent());
            });
        });

    });
}