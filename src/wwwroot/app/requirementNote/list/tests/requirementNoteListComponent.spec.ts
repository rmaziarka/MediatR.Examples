///<reference path="../../../typings/_all.d.ts"/>

module Antares {
    import RequirementNoteListController = RequirementNote.RequirementNoteListController;
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    describe('Given requirement note list component is displayed', () =>{

        var scope: ng.IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            filter: ng.IFilterService,
            $http: ng.IHttpBackendService;

        var pageObjectSelectors = {
            noteList: '#requirement-notes',
            noteItems: '#requirement-notes [data-type="note-item"]',
            noteItem: '#requirement-notes #note-'
        }

        var controller: RequirementNoteListController;

        describe('and requirement is loaded', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $filter: ng.IFilterService,
                $httpBackend: ng.IHttpBackendService) =>{

                $http = $httpBackend;
                filter = $filter;

                Antares.Mock.AddressForm.mockHttpResponce($http, 'a1', [200, Antares.Mock.AddressForm.AddressFormWithOneLine]);
                $http.whenGET(/\/api\/enums\/.*\/items/).respond(() =>{
                    return [];
                });

                scope = $rootScope.$new();
                compile = $compile;
            }));

            it('when no requirement notes then no notes should be visible', () => {
                // arrange
                var requirementMock: Business.Requirement = TestHelpers.RequirementGenerator.generate({ requirementNotes: [] });
                scope['requirement'] = requirementMock;

                // act
                element = compile('<requirement-note-list requirement="requirement"></requirement-note-list>')(scope);
                scope.$apply();
                controller = element.controller('requirementNoteList');

                // assert
                var noteListElement = element.find(pageObjectSelectors.noteList);
                var noteItems = element.find(pageObjectSelectors.noteItems);
                expect(noteListElement.length).toBe(1);
                expect(noteItems.length).toBe(0);
            });

            it('when existing requirement notes then notes should be visible', () => {
                // arrange
                var requirementMock: Business.Requirement = TestHelpers.RequirementGenerator.generate({
                    requirementNotes: [
                        new Business.RequirementNote({ id: 'note1', requirementId: '111', description: 'descr 1', createdDate: new Date(), user: null }),
                        new Business.RequirementNote({ id: 'note2', requirementId: '111', description: 'descr 2', createdDate: new Date(), user: null })
                    ]
                });
                scope['requirement'] = requirementMock;

                // act
                element = compile('<requirement-note-list requirement="requirement"></requirement-note-list>')(scope);
                scope.$apply();
                controller = element.controller('requirementNoteList');

                // assert
                var noteItems = element.find(pageObjectSelectors.noteItems);
                expect(noteItems.length).toBe(2);
            });

            it('when existing requirement notes then notes should have proper data', () => {
                // arrange
                var date1Mock = new Date('2011-01-01');
                var requirementMock: Business.Requirement = TestHelpers.RequirementGenerator.generate({
                    requirementNotes: [
                        { id: 'note1', requirementId: '111', description: 'descr 1', createdDate: date1Mock, user: null },
                        { id: 'note2', requirementId: '111', description: 'descr 2', createdDate: new Date(), user: null }
                    ]
                });
                scope['requirement'] = requirementMock;

                // act
                element = compile('<requirement-note-list requirement="requirement"></requirement-note-list>')(scope);
                scope.$apply();
                controller = element.controller('requirementNoteList');

                // assert
                var noteElement = element.find(pageObjectSelectors.noteItem + "note1");

                var timeElement = noteElement.find('time');
                var descriptionElement = noteElement.find('[data-type="note-description"]');

                var formattedDate = filter('date')(Core.DateTimeUtils.convertDateToUtc(date1Mock), 'dd-MM-yyyy, HH:mm');
                expect(timeElement.text()).toBe(formattedDate);
                expect(descriptionElement.text()).toBe('descr 1');
            });
        });
    });
}