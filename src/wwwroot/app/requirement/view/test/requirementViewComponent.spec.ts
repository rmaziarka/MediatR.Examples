/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import RequirementViewController = Requirement.View.RequirementViewController;
    import Business = Common.Models.Business;

    describe('Given view requirement page is loaded', () =>{
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            compile: ng.ICompileService,
            $http: ng.IHttpBackendService,
            filter: ng.IFilterService;

        var controller: RequirementViewController;

        var pageObjectSelectors = {
            header : {
                notesButton : '#notes-button'
            },
            notes : {
                noteAddDescription: '#requirement-note-add textarea#note-description',
                noteAddButton: '#requirement-note-add button#note-add-button',
                noteList: '#requirement-notes',
                noteItems: '#requirement-notes [data-type="note-item"]'
            },
            viewings:
            {
                viewingGroups: '#viewings-list .viewing-group',
                viewingGroupTitles:'#viewings-list card-list-group-header h5 time',
                viewings: '#viewings-list .viewing-item'
            }
        }

        describe('and proper requirement id is provided', () => {
            var requirementMock: Business.Requirement = TestHelpers.RequirementGenerator.generate({
                createDate: '2016-03-17T12:35:29.82',
                maxPrice: 444,
                minPrice: 0,
                maxBedrooms: 3,
                maxReceptionRooms: 2,
                maxBathrooms: 2,
                maxParkingSpaces: 12,
                maxArea: 1234,
                maxLandArea: 5678,
                description: 'sample',
                contacts: [
                    { id: '1', firstName: 'John', surname: 'Doe', title: 'Mr.' },
                    { id: '2', firstName: 'Jane', surname: 'Doe', title: 'Mrs.' }
                ],
                requirementNotes: [
                    new Business.RequirementNote({ id: 'note1', requirementId: '111', description: 'descr 1', createdDate: new Date(), user: null }),
                    new Business.RequirementNote({ id: 'note2', requirementId: '111', description: 'descr 2', createdDate: new Date(), user: null })
                ],
                address: Mock.AddressForm.FullAddress,
                viewings: [                    
                    {
                        id: '1',
                        startDate: "2016-01-01T10:00:00Z",
                        endDate: "2016-01-01T11:00:00Z"
                    },
                    {
                        id: '2',
                        startDate: "2016-01-01T13:00:00Z",
                        endDate: "2016-01-01T14:00:00Z"
                    },
                    {
                        id: '3',
                        startDate: "2016-01-02T10:00:00Z",
                        endDate: "2016-01-02T11:00:00Z"
                    },
                    {
                        id: '4',
                        startDate: "2016-01-03T00:00:00Z",
                        endDate: "2016-01-03T01:00:00Z"
                    }
                ]
            });

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService,
                $filter: ng.IFilterService) => {

                filter = $filter;
                $http = $httpBackend;

                Mock.AddressForm.mockHttpResponce($http, 'a1', [200, Mock.AddressForm.AddressFormWithOneLine]);

                scope = $rootScope.$new();
                scope['requirement'] = requirementMock;
                element = $compile('<requirement-view requirement="requirement"></requirement-view>')(scope);

                scope.$apply();
                $http.flush();

                controller = element.controller('requirementView');
            }));

            it('requirement details are loaded', () => {
                for (var property in requirementMock) {
                    if (requirementMock.hasOwnProperty(property)) {
                        var value = requirementMock[property];
                        if (angular.isArray(value)) {
                            expect(controller.requirement[property].length).toBe(value.length);
                        }
                        else {
                            expect(controller.requirement[property]).toBe(value);
                        }
                    }
                }
            });

            it('header information is displayed', () => {
                var header = element.find('.requirement-details-header');
                var createDate = header.find('[translate="REQUIREMENT.VIEW.CREATEDDATE"]').next();
                var formattedDate = filter('date')(requirementMock.createDate, 'longDate');

                expect(createDate.length).toBe(1);
                expect(createDate.text()).toBe(formattedDate);
            });

            it('notes count is displayed within notes button', () => {
                var buttonText = element.find(pageObjectSelectors.header.notesButton + ' span');

                expect(buttonText.length).toBe(1);
                expect(buttonText.text()).toBe('(' + requirementMock.requirementNotes.length + ')');
            });

            it('description is displayed', () => {
                var descrptionParagraph = element.find('[translate="REQUIREMENT.VIEW.DESCRIPTION"]+p');

                expect(descrptionParagraph.text()).toBe(requirementMock.description);
            });

            xit('location requirements are displayed', () => {
                // TODO: after address view component is implemented
            });

            it('basic property requirements are displayed in proper order and with correct values', () => {
                var expectedBasicDetailsResults = [
                    { label: 'REQUIREMENT.VIEW.PRICE', min: 'vm.requirement.minPrice', max: 'vm.requirement.maxPrice', suffix: "'GBP'" },
                    { label: 'REQUIREMENT.VIEW.BEDROOMS', min: 'vm.requirement.minBedrooms', max: 'vm.requirement.maxBedrooms', suffix: null },
                    { label: 'REQUIREMENT.VIEW.RECEPTIONROOMS', min: 'vm.requirement.minReceptionRooms', max: 'vm.requirement.maxReceptionRooms', suffix: null },
                    { label: 'REQUIREMENT.VIEW.BATHROOMS', min: 'vm.requirement.minBathrooms', max: 'vm.requirement.maxBathrooms', suffix: null },
                    { label: 'REQUIREMENT.VIEW.PARKINGSPACES', min: 'vm.requirement.minParkingSpaces', max: 'vm.requirement.maxParkingSpaces', suffix: null },
                    { label: 'REQUIREMENT.VIEW.AREA', min: 'vm.requirement.minArea', max: 'vm.requirement.maxArea', suffix: "'sq ft'" },
                    { label: 'REQUIREMENT.VIEW.LANDAREA', min: 'vm.requirement.minLandArea', max: 'vm.requirement.maxLandArea', suffix: "'sq ft'" }
                ];

                var rangeViewElements = element.find('range-view');

                expect(rangeViewElements.length).toBe(expectedBasicDetailsResults.length);
                for (var i = 0; i < rangeViewElements.length; i++) {
                    var rangeView = rangeViewElements[i];
                    var expected = expectedBasicDetailsResults[i];
                    expect(rangeView.getAttribute('label')).toBe(expected.label);
                    expect(rangeView.getAttribute('min')).toBe(expected.min);
                    expect(rangeView.getAttribute('max')).toBe(expected.max);
                    expect(rangeView.getAttribute('suffix')).toBe(expected.suffix);
                };

            });

            it('applicant list is displayed', () => {
                var applicantList = element.find('#applicant-list');
                var applicants = applicantList.find('.applicant');

                expect(applicantList.length).toBe(1);
                expect(applicants.length).toBe(requirementMock.contacts.length);
            });

            it('viewing list is displayed and grouped correctly', () => {
                var viewingGroups = element.find(pageObjectSelectors.viewings.viewingGroups);
                var viewingGroupTitles = element.find(pageObjectSelectors.viewings.viewingGroupTitles);
                var viewings = element.find(pageObjectSelectors.viewings.viewings);

                expect(viewingGroups.length).toBe(requirementMock.viewingsByDay.length);
                expect(viewings.length).toBe(requirementMock.viewings.length);

                requirementMock.viewingsByDay.sort((a, b) => new Date(b.day).getTime() - new Date(a.day).getTime()).forEach((g: Business.ViewingGroup, i: number) => {
                    var formatedDate: string = formatDate(g.day);
                    expect(formatedDate).toBe(viewingGroupTitles[i].textContent);
                });                
            });

            function formatDate(d: string) {
                var date = new Date(d);
                var yyyy = date.getFullYear().toString();
                var mm = (date.getMonth() + 1).toString(); // getMonth() is zero-based
                var dd = date.getDate().toString();
                return (dd[1] ? dd : "0" + dd[0]) + '-' + (mm[1] ? mm : "0" + mm[0]) + '-'+ yyyy; // padding
            };
        });

        describe('and notes button is clicked', () => {
            var requirementMock: Business.Requirement = TestHelpers.RequirementGenerator.generate({
                address: Mock.AddressForm.FullAddress
            });
            var addNoteUrlRegex = new RegExp('api/requirements/' + requirementMock.id);

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService,
                $filter: ng.IFilterService) => {

                filter = $filter;
                $http = $httpBackend;
                compile = $compile;

                Mock.AddressForm.mockHttpResponce($http, 'a1', [200, Mock.AddressForm.AddressFormWithOneLine]);

                scope = $rootScope.$new();
            }));

            it('then note list is displayed', () => {
                // arrange
                requirementMock.requirementNotes = [
                    new Business.RequirementNote({ id : 'note1', requirementId : '111', description : 'descr 1', createdDate : new Date(), user : null }),
                    new Business.RequirementNote({ id : 'note2', requirementId : '111', description : 'descr 2', createdDate : new Date(), user : null })
                ];

                scope['requirement'] = requirementMock;
                element = compile('<requirement-view requirement="requirement"></requirement-view>')(scope);
                scope.$apply();

                // assert
                var noteListElement = element.find(pageObjectSelectors.notes.noteList);
                var noteItems = element.find(pageObjectSelectors.notes.noteItems);

                expect(noteListElement.length).toBe(1);
                expect(noteItems.length).toBe(2);
            });

            it('and new note is added then note is added to list', () => {
                // arrange
                requirementMock.requirementNotes = [
                    new Business.RequirementNote({ id: 'note1', requirementId: '111', description: 'descr 1', createdDate: new Date(), user: null }),
                    new Business.RequirementNote({ id: 'note2', requirementId: '111', description: 'descr 2', createdDate: new Date(), user: null })
                ];

                scope['requirement'] = requirementMock;
                element = compile('<requirement-view requirement="requirement"></requirement-view>')(scope);
                scope.$apply();

                var expectedResponse = new Business.RequirementNote();
                expectedResponse.id = 'newNoteId';
                $http.expectPOST(addNoteUrlRegex).respond(201, expectedResponse);

                // act
                element.find(pageObjectSelectors.notes.noteAddDescription)
                    .val('abc').trigger('input').trigger('change').trigger('blur');
                element.find(pageObjectSelectors.notes.noteAddButton).click();
                scope.$apply();
                $http.flush();

                // assert
                var noteItems = element.find(pageObjectSelectors.notes.noteItems);
                var addedItem = _.find(requirementMock.requirementNotes, (item) => { return item.id === expectedResponse.id });

                expect(noteItems.length).toBe(3);
                expect(requirementMock.requirementNotes.length).toBe(3);
                expect(addedItem).toBeDefined();
                expect(requirementMock.requirementNotes.indexOf(addedItem)).toBe(0);
            });

            it('and new note is added then notes count within notes button is updated', () => {
                // arrange
                requirementMock.requirementNotes = [
                    new Business.RequirementNote({ id: 'note1', requirementId: '111', description: 'descr 1', createdDate: new Date(), user: null }),
                    new Business.RequirementNote({ id: 'note2', requirementId: '111', description: 'descr 2', createdDate: new Date(), user: null })
                ];

                scope['requirement'] = requirementMock;
                element = compile('<requirement-view requirement="requirement"></requirement-view>')(scope);
                scope.$apply();

                var expectedResponse = new Business.RequirementNote();
                expectedResponse.id = 'newNoteId';
                $http.expectPOST(addNoteUrlRegex).respond(201, expectedResponse);

                // act
                element.find(pageObjectSelectors.notes.noteAddDescription)
                    .val('abc').trigger('input').trigger('change').trigger('blur');
                element.find(pageObjectSelectors.notes.noteAddButton).click();
                scope.$apply();
                $http.flush();

                // assert
                var buttonText = element.find(pageObjectSelectors.header.notesButton + ' span');

                expect(buttonText.length).toBe(1);
                expect(buttonText.text()).toBe('(3)');
            });

            it('and notes button is clicked then description field is empty', () => {
                // arrange
                requirementMock.requirementNotes = [
                    new Business.RequirementNote({ id: 'note1', requirementId: '111', description: 'descr 1', createdDate: new Date(), user: null }),
                    new Business.RequirementNote({ id: 'note2', requirementId: '111', description: 'descr 2', createdDate: new Date(), user: null })
                ];

                scope['requirement'] = requirementMock;
                element = compile('<requirement-view requirement="requirement"></requirement-view>')(scope);
                scope.$apply();

                // act
                var descriptionElement = element.find(pageObjectSelectors.notes.noteAddDescription);
                descriptionElement.val('abc').trigger('input').trigger('change').trigger('blur');
                scope.$apply();

                element.find(pageObjectSelectors.header.notesButton).click();
                scope.$apply();

                // assert
                expect(descriptionElement.text()).toBe('');
            });
        });
    });
}