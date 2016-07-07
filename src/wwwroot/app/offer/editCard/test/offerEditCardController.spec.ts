/// <reference path="../../../typings/_all.d.ts" />
module Antares {
    import Business = Common.Models.Business;
    describe('offerEditCardController', () => {
        var
            controller: Offer.OfferEditCardController,
            offer: Business.Offer = new Business.Offer();

        offer.id = 'first_offer_id';

        beforeEach(() => {
            var controllerFunction = Offer.OfferEditCardController;
            controller = Object.create(controllerFunction.prototype);
            controllerFunction.apply(controller);
            controller.offer = new Business.Offer(offer);
            controller.originalOffer = new Business.Offer(offer);
        });

        describe('when is initialized', () => {
            it('offer should be set', () => {
                expect(controller.offer).toBeDefined();
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
            describe('when offer has changed', () => {
                var newOffer = new Business.Offer();
                newOffer.id = 'test_id';

                var changeObj = {
                    offer: {
                        currentValue: newOffer
                    }
                };

                beforeEach(() => {
                    controller.$onChanges(changeObj);
                });

                it('original offer should be set to new offer', () => {
                    expect(controller.originalOffer).toBeDefined();
                    expect(controller.originalOffer).not.toBe(newOffer);
                    expect(controller.originalOffer.id).toEqual(newOffer.id);
                });

                it('offer on controller should be replaced', () => {
                    expect(controller.offer).not.toBe(offer);
                    expect(controller.offer).not.toBe(newOffer);
                    expect(controller.offer.id).toEqual(newOffer.id);
                });
            });

            describe('when pristineFlag has changed', () => {
                it('when pristine flag is set then offer should be reset to original offer', () => {
                    var changeObj = {
                        pristineFlag: {
                            currentValue: new Object()
                        }
                    };

                    controller.$onChanges(changeObj);
                    expect(controller.offer).toBe(controller.originalOffer);
                });

                it('when pristine flag is changed then offer should be reset to original offer', () => {
                    var changeObj = {
                        pristineFlag: {
                            currentValue: new Object(),
                            previousValue: new Object()
                        }
                    };

                    controller.$onChanges(changeObj);
                    expect(controller.offer).toBe(controller.originalOffer);
                });
            });
        });

        describe('when save is called', () => {
            var expectedSavedOffer: Business.CreateOfferCommand;

            beforeEach(() => {
                controller.onSave = (obj: { offer: Business.Offer }) => { expectedSavedOffer = obj.offer; };
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