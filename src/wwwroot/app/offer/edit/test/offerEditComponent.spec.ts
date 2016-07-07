/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import OfferEditController = Offer.OfferEditController;
    import KfMessageService = Services.KfMessageService;    

    describe('Given offer component', () =>{
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            state: ng.ui.IStateService,
            assertValidator: TestHelpers.AssertValidators,
            $compileService: ng.ICompileService,
            messageService: KfMessageService,
            controller: OfferEditController,
            offerSrv: Services.OfferService,
            promiseWith: (obj: any) => ng.IPromise<any>;

        var pageObjectSelectors: any = {
            offerDate: 'date-edit-control[ng-model$=offerDate]',
            offer: 'price-edit-control[ng-model$=price]',
            exchangeDate: 'date-edit-control[ng-model$=exchangeDate]',
            completionDate: 'date-edit-control[ng-model$=completionDate]',
            specialConditions: 'textarea-edit-control[ng-model$=specialConditions]',
            status: 'enum-item-edit-control[ng-model$=statusId]',
            activity: '#offer-edit-activity-details',
            negotiator: '#offer-edit-negotiator-details',
            negotiatorSection: '#offer-edit-negotiator-section'
        };
        
        var offerMock = TestHelpers.OfferGenerator.generate();

        beforeEach(angular.mock.module(($provide: angular.auto.IProvideService) => {
            $provide.decorator('formDirective', Common.Decorators.FormDecorator.decoratorFunction);
            $provide.factory('addressFormViewDirective', () => { return {}; });
        }));    

        beforeEach(() => {
            angular.mock.module(($provide: any) => {
                $provide.factory('enumItemEditControlDirective', () => { return {}; });
                $provide.factory('priceEditControlDirective', () => { return {}; });
                $provide.factory('dateEditControlDirective', () => { return {}; });
                $provide.factory('textareaEditControlDirective', () => { return {}; });
                $provide.factory('ngMessageDirective', () => { return {}; });
            });
        });

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            $state: ng.ui.IStateService,
            $q: ng.IQService,
            kfMessageService: KfMessageService,
            offerService: Services.OfferService) => {

            state = $state;
            messageService = kfMessageService;
            $compileService = $compile;
            offerSrv = offerService;
            promiseWith = (obj: any) => {
                var deferred = $q.defer();
                deferred.resolve(obj);
                return deferred.promise;
            };
            
            scope = $rootScope.$new();
            scope["offer"] = offerMock;
            scope["config"] = TestHelpers.ConfigGenerator.generateOfferEditConfig();

            element = $compileService('<offer-edit offer="offer" config="config"></offer-edit>')(scope);
            scope.$apply();

            controller = element.controller('offerEdit');
            assertValidator = new TestHelpers.AssertValidators(element, scope);
            scope.$apply();
            
        }));

        it('all form elements are rendered', () => {
            for (var formElementSelector in pageObjectSelectors) {
                if (pageObjectSelectors.hasOwnProperty(formElementSelector)) {
                    var formElement = element.find(pageObjectSelectors[formElementSelector]);
                    expect(formElement.length).toBe(1, `Element ${formElementSelector} not found`);
                }
            }
        });

        describe('when valid data and save button is clicked', () => {
            it('then save method is called', () => {
                spyOn(controller, 'save');
                scope.$apply();

                var button = element.find('button#offer-edit-save');
                button.click();

                expect(controller.save).toHaveBeenCalled();
            });

            it('then put request is called and redirect to view page with success message', () => {
                spyOn(offerSrv, 'updateOffer').and.returnValue(promiseWith(offerMock));
                spyOn(state, 'go').and.returnValue(promiseWith({}));
                spyOn(messageService, 'showSuccessByCode');

                scope.$apply();

                var button = element.find('button#offer-edit-save');
                button.click();

                expect(state.go).toHaveBeenCalled();
                expect(messageService.showSuccessByCode).toHaveBeenCalledWith('OFFER.EDIT.OFFER_EDIT_SUCCESS');
            });
        });

    });
}