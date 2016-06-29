/// <reference path="../../../../typings/_all.d.ts" />

module Antares {
    import SearchController = Common.Component.SearchController;

    interface ISearchComponentScope extends ng.IScope {
        searchForm: any;
    }

    describe('Given search component', () => {
        var scope: ISearchComponentScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            q: ng.IQService,
            controller: SearchController;

        var pageObjectSelectors = {
            searchInput: 'input[name="searchText"]',
            cancel: 'button[data-type="cancel"]'
        }

        var loadItemsDefferedMock: ng.IDeferred<string[]>;

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $q: ng.IQService,
            $compile: ng.ICompileService) => {

            // init
            scope = <ISearchComponentScope>$rootScope.$new();
            q = $q;
            compile = $compile;

            loadItemsDefferedMock = q.defer();

            scope['searchOptions'] = new Common.Component.SearchOptions({
                minLength: 3,
                modelOptions : {
                    debounce : {
                        default : 0,
                        blur : 0
                    }
                }
            });
            scope['onSelectItem'] = () => {};
            scope['onCancel'] = () => { };
            scope['loadItems'] = () => { };

            var template = '<script type="text/ng-template" id="searchTestTemplate.html"><a class="test-template-element"><span>{{match.model.itemName}}</span></a></script>';
            element = $compile(template + '<form name="searchForm"><search options="searchOptions" item-template-url="searchTestTemplate.html" on-select-item="onSelectItem" on-cancel="onCancel" load-items="loadItems"></search></form>')(scope);
            scope.$apply();

            controller = element.find('search').controller('search');
        }));

        describe('when filling search field', () =>{
            it('and provided less signs than minLength setting than loadItems is not called', () => {
                // arrange
                spyOn(controller, 'loadItems');

                // act
                scope.searchForm.searchText.$setViewValue('12');

                // assert
                expect(controller.loadItems).not.toHaveBeenCalled();
            });

            it('and provided signs equal to minLength setting than loadItems is called', () => {
                // arrange
                spyOn(controller, 'loadItems');

                // act
                scope.searchForm.searchText.$setViewValue('123');

                // assert
                expect(controller.loadItems).toHaveBeenCalled();
            });
        });

        describe('when selecting item', () => {
            it('than select action is called', () => {
                // arrange
                var testItem = 'testItem';

                spyOn(controller, 'loadItems').and.returnValue(loadItemsDefferedMock.promise);
                spyOn(controller, 'select');

                // act
                scope.searchForm.searchText.$setViewValue('123');
                loadItemsDefferedMock.resolve([testItem]);
                scope.$apply();

                element.find('li').first().click();

                // assert
                expect(controller.select).toHaveBeenCalledWith(testItem);
            });
        });

        describe('when clicking cancel', () => {

            it('than cancel action is called', () => {
                // arrange
                spyOn(controller, 'cancel');

                // act
                var button = element.find(pageObjectSelectors.cancel);
                button.click();

                // assert
                expect(controller.cancel).toHaveBeenCalled();
            });
        });
    });
}