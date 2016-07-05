/// <reference path="../../../typings/_all.d.ts" />
module Antares {
    import Business = Common.Models.Business;

    describe('Given offerPreviewCardController', () => {
        var controller: Offer.OfferPreviewCardController;

        beforeEach(inject(function ($controller: any) {
            var bindings = {
                'config': {},
                'offer': {},
                'onEdit': {}
            };

            controller = <Offer.OfferPreviewCardController>$controller('OfferPreviewCardController', {}, bindings);
        }));

        describe('when is initialized', () => {

            it('then schemas should be defined for all controls', () => {
                expect(controller.controlSchemas).toBeDefined();
                expect(controller.controlSchemas.status).toBeDefined('No control schema for status');
                expect(controller.controlSchemas.price).toBeDefined('No control schema for price');
                expect(controller.controlSchemas.price).toBeDefined('No control schema for pricePerWeek');
                expect(controller.controlSchemas.offerDate).toBeDefined('No control schema for offerDate');
                expect(controller.controlSchemas.exchangeDate).toBeDefined('No control schema for exchangeDate');
                expect(controller.controlSchemas.completionDate).toBeDefined('No control schema for completionDate');
                expect(controller.controlSchemas.specialConditions).toBeDefined('No control schema for specialConditions');
            });

            describe('and edit is calling', () => {
                it('then onEdit should be called', () => {
                    // arrange
                    spyOn(controller, 'onEdit');

                    // act
                    controller.edit();

                    // assert
                    expect(controller['onEdit']).toHaveBeenCalled();
                });
            });

        });

    });
}