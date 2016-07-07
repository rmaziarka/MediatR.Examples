/// <reference path="../../../../typings/_all.d.ts" />

module Antares {
    import KfMessageService = Services.KfMessageService;
    xdescribe('Given company contact add panel component', () =>{
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            state: ng.ui.IStateService,
            q: ng.IQService,
            $http: ng.IHttpBackendService,
            $compileService: ng.ICompileService,
            messageService: KfMessageService;

        var methodMockups: any = {
            saveMethodMockup: () =>{ }
        }

        var pageObjectSelectors: any = {
            cardItem: '.card-item',
            card: '.card',
            selectedCards: '.card.card-selected .card-item',
            btnSave: '#ownership-save-button'
        };

        var companyContactsMock = TestHelpers.CompanyContactGenerator.generateMany(5);

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            $httpBackend: ng.IHttpBackendService,
            $q: ng.IQService,
            $state: ng.ui.IStateService,
            kfMessageService: KfMessageService) => {

            state = $state;
            q = $q;
            messageService = kfMessageService;
            $http = $httpBackend;
            $compileService = $compile;
            scope = $rootScope.$new();
            scope["allowMultipleSelection"] = false;
            scope["saveMethodMockup"] = methodMockups.saveMethodMockup;
            scope["initialContacts"] = [];


            spyOn(scope, 'saveMethodMockup');
            $http.whenGET(/\/api\/companycontacts/).respond(() => {
                return [200, companyContactsMock];
            });
            
            element = $compileService('<company-contacts-add-panel is-visible="true" on-save="saveMethodMockup" allow-multiple-select="allowMultipleSelection" initialy-selected-company-contacts="initialContacts"></company-contacts-add-panel>')(scope);
            scope.$apply();
            $http.flush();
        }));

        it('all company contacts are loaded', () => {
            var cardItems = element.find(pageObjectSelectors.cardItem);
            expect(cardItems.length).toBe(5);
            for (var i = 0; i < cardItems.length; i++) {
                expect(cardItems[i].innerText).toBe(companyContactsMock[i].contact.getName());
            }
        });

        it('and allow only one selection then clicking multiple elements select only one', () => {
            var cardItems = element.find(pageObjectSelectors.card);
            cardItems[0].click();
            cardItems[1].click();
            cardItems[3].click();
            var selectedElements = element.find(pageObjectSelectors.selectedCards);
            expect(selectedElements.length).toBe(1);
            expect(selectedElements[0].innerText).toBe(companyContactsMock[3].contact.getName());
        });

        it('and allow multiple selection then clicking multiple elements select all of them', () => {
            scope["allowMultipleSelection"] = true;
            scope.$apply();
            var cardItems = element.find(pageObjectSelectors.card);
            cardItems[0].click();
            cardItems[1].click();
            cardItems[3].click();
            var selectedElements = element.find(pageObjectSelectors.selectedCards);
            expect(selectedElements.length).toBe(3);
        });

        xit('clicking card two times selects and deselects it', () => {
            var cardItems = element.find(pageObjectSelectors.cardItem);
            cardItems[1].click();
            var selectedElements = element.find(pageObjectSelectors.selectedCards);
            expect(selectedElements.length).toBe(1);
            expect(cardItems[1].innerText).toBe(companyContactsMock[1].contact.getName());
            cardItems[1].click();
            selectedElements = element.find(pageObjectSelectors.selectedCards);
            expect(selectedElements.length).toBe(0);
        });

        it('when save is clicked on save method is called', () => {
            var saveBtn = element.find(pageObjectSelectors.btnSave);
            saveBtn.click();
            expect(scope["saveMethodMockup"]).toHaveBeenCalled();
        });

        it('initialy given company contacts are loaded', () => {
            scope["initialContacts"] = [companyContactsMock[0]];
            element = $compileService('<company-contacts-add-panel is-visible="true" on-save="saveMethodMockup" allow-multiple-select="allowMultipleSelection" initialy-selected-company-contacts="initialContacts"></company-contacts-add-panel>')(scope);
            scope.$apply();
            $http.flush();
            var selectedElements = element.find(pageObjectSelectors.selectedCards);
            expect(selectedElements.length).toBe(1);
            expect(selectedElements[0].innerText).toBe(companyContactsMock[0].contact.getName());
        });
    });
}