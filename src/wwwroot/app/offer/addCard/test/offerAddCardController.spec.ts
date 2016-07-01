/// <reference path="../../../typings/_all.d.ts" />
module Antares {
    import Business = Common.Models.Business;
    declare var moment: any;

    describe('offerAddCardController', () => {
        var controller: Offer.OfferAddCardController;

        beforeEach(inject((
            enumProvider: Providers.EnumProvider) => {

            enumProvider.enums = TestHelpers.EnumDictionaryGenerator.generateDictionary();

            var controllerFunction = Offer.OfferAddCardController;
            controller = Object.create(controllerFunction.prototype);
            controllerFunction.apply(controller, [enumProvider]);
            controller.$onInit();
        }));

        describe('when is initialized', () => {
            it('default status code is New', () => {
                expect(controller.defaultOfferStatusCode).toBe(Common.Models.Enums.OfferStatus[Common.Models.Enums.OfferStatus.New]);
            });

            it('offer should be set', () => {
                expect(controller.offer).toBeDefined();
            });

            it('offer status id is set', () => {
                expect(controller.offer.statusId).toBeDefined();
            });

            it('offer date should be set to today', () => {
                expect(controller.offer.offerDate).toEqual(moment().startOf('day').toDate());
            });

            it('has control schema defined for all controls', () => {
                expect(controller.controlSchemas).toBeDefined();
                expect(controller.controlSchemas.price).toBeDefined('No control schema for price');
                expect(controller.controlSchemas.pricePerWeek).toBeDefined('No control schema for pricePerWeek');
                expect(controller.controlSchemas.offerDate).toBeDefined('No control schema for offerDate');
                expect(controller.controlSchemas.exchangeDate).toBeDefined('No control schema for exchangeDate');
                expect(controller.controlSchemas.completionDate).toBeDefined('No control schema for completionDate');
                expect(controller.controlSchemas.specialConditions).toBeDefined('No control schema for specialConditions');
            });
        });

        describe('$onChanges called (component bindings change)', () => {
            describe('when pristineFlag has changed', () => {
                var resetCardData: jasmine.Spy;
                beforeEach(() => {
                    resetCardData = spyOn(controller, 'resetCardData');
                });

                it('when pristine flag is set then resetCardData should be called', () => {
                    var changeObj = {
                        pristineFlag: {
                            currentValue: new Object()
                        }
                    };

                    controller.$onChanges(changeObj);
                    expect(resetCardData).toHaveBeenCalledTimes(1);
                });

                it('when pristine flag is changed then resetCardData should be called', () => {
                    var changeObj = {
                        pristineFlag: {
                            currentValue: new Object(),
                            previousValue: new Object()
                        }
                    };

                    controller.$onChanges(changeObj);
                    expect(resetCardData).toHaveBeenCalledTimes(1);
                });
            });
        });

        describe('when save is called', () => {
            var expectedSavedOffer: Business.CreateOfferCommand;

            beforeEach(() => {
                controller.onSave = (obj: { offer: Business.CreateOfferCommand }) => { expectedSavedOffer = obj.offer; };
                controller.save();
            });

            it('save callback is called', () => {
                expect(expectedSavedOffer).toBe(controller.offer);
            });
        });

        describe('when cancel is called', () => {
            var cancelCalled = false;

            beforeEach(() => {
                controller.onCancel = () => { cancelCalled = true; };
                controller.cancel();
            });

            it('cancel callback is called', () => {
                expect(cancelCalled).toBeTruthy();
            });
        });
    });
}