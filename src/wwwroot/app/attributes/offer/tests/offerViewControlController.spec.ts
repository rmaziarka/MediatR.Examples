/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import OfferViewControlController = Attributes.OfferViewControlController;
    import Business = Common.Models.Business;
    import LatestViewsProvider = Providers.LatestViewsProvider;

    describe('Given offer panel control', () => {
        var controller: OfferViewControlController,
            eventAggregator: Core.EventAggregator;

        describe('when show offer panel event has been invoked', () => {

            beforeEach(inject((
                _eventAggregator_: Antares.Core.EventAggregator,
                $controller: ng.IControllerService) => {
                eventAggregator = _eventAggregator_;
                
                controller = <OfferViewControlController>$controller('OfferViewControlController');
            }));

            it('then event to open offer side panel must be published', () => {
		        // arrange
		        var isOfferSidePanelOpened: boolean = false;

		        eventAggregator.with({})
			        .subscribe(Antares.Attributes.OpenOfferPreviewPanelEvent, () => {
				        isOfferSidePanelOpened = true;
			        });		        

		        // act
		        controller.showOfferPreview(TestHelpers.OfferGenerator.generateDto());

		        // assert
		        expect(isOfferSidePanelOpened).toBe(true);
	        });
        });
    });
}