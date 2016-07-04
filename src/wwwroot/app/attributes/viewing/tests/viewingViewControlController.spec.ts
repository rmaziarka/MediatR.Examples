/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import ViewingViewControlController = Attributes.ViewingViewControlController;
    import Business = Common.Models.Business;
    import LatestViewsProvider = Providers.LatestViewsProvider;

    describe('Given viewing panel control', () => {
        var controller: ViewingViewControlController,
            eventAggregator: Core.EventAggregator;

        describe('when show viewing panel event has been invoked', () => {

            beforeEach(inject((
                _eventAggregator_: Antares.Core.EventAggregator,
                $controller: ng.IControllerService) => {
                eventAggregator = _eventAggregator_;

                controller = <ViewingViewControlController>$controller('ViewingViewControlController');
            }));

            it('then event to open viewing side panel must be published', () => {
                // arrange		        
                var isViewingSidePanelOpened: boolean = false;

                eventAggregator.with({})
                    .subscribe(Antares.Attributes.OpenViewingPreviewPanelEvent, () => {
                        isViewingSidePanelOpened = true;
                    });

                // act
                controller.showViewingPreview(TestHelpers.ViewingGenerator.generateDto());

                // assert
                expect(isViewingSidePanelOpened).toBe(true);
            });
        });
    });
}