module Antares {
    import OfferChainsListControlController = Attributes.Offer.OfferChainsListControlController;
    import ChainTransaction = Common.Models.Business.ChainTransaction;

    describe('Given offer chains list control controller', () =>{
        var controller: OfferChainsListControlController;

        beforeEach(inject((
            $controller: ng.IControllerService) =>{
            controller = <OfferChainsListControlController>$controller('OfferChainsListControlController');
        }));

        it('when markLastChainElement function is called and 1 element is specified then this element is marked in transaction chain', () => {
            // arrange
            var ct1 = <ChainTransaction>{
                id: '1',
                parentId: null
            };

            controller.chains = <ChainTransaction[]>[];
            controller.chains.push(ct1);

            // act
            controller.markLastChainElement();

            // assert
            expect(ct1.lastElementInChainLink).toBe(true);
        });

        it('when markLastChainElement function is called and 3 elements are specified then last element is marked in transaction chain', () =>{
            // arrange
            var ct1 = <ChainTransaction> {
                id : '1',
                parentId : null
            };
            var ct2 = <ChainTransaction>{
                id : '2',
                parentId :'1'
            };
            var ct3 = <ChainTransaction>{
                id: '3',
                parentId: '2'
            };

            controller.chains = <ChainTransaction[]>[];
            controller.chains.push(ct1);
            controller.chains.push(ct2);
            controller.chains.push(ct3);

            // act
            controller.markLastChainElement();

            // assert
            expect(ct1.lastElementInChainLink).toBe(false);
            expect(ct2.lastElementInChainLink).toBe(false);
            expect(ct3.lastElementInChainLink).toBe(true);
        });
    });
}