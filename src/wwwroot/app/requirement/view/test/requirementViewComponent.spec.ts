/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import RequirementViewController = Requirement.View.RequirementViewController;
    import Business = Common.Models.Business;
    import OfferAddEditController = Component.OfferAddEditController;
    import SidePanelController = Common.Component.SidePanelController;
    declare var moment: any;

    describe('Given view requirement page is loaded', () => {
        beforeEach(() => {
            angular.mock.module(($provide: any) => {
                $provide.service('addressFormsProvider', Mock.AddressFormsProviderMock);
            });
        });

        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            compile: ng.ICompileService,
            $http: ng.IHttpBackendService,
            filter: ng.IFilterService;

        var controller: RequirementViewController;
        var pageObjectSelectors = {
            header: {
                notesButton: '#notes-button'
            },
            notes: {
                noteAddDescription: '#requirement-note-add textarea#note-description',
                noteAddButton: '#requirement-note-add button#note-add-button',
                noteList: '#requirement-notes',
                noteItems: '#requirement-notes [data-type="note-item"]'
            },
            viewings:
            {
                viewingGroups: '#viewings-list .viewing-group',
                viewingGroupTitles: '#viewings-list card-list-group-header h5 time',
                viewings: '#viewings-list .viewing-item'
            },
            offers:
            {
                saveButton: '#offer-save-btn',
                cancelButton: '#offer-cancel-btn',
                sidePanel: '#add-offer-panel',
                makeOfferAction: 'context-menu-item[type=makeoffer]',
                list: '.requirement-view-offers',
                offerStatus:'.offer-status',
                statusText:
                {
                    New: 'DYNAMICTRANSLATIONS.1',
                    Accepted: 'DYNAMICTRANSLATIONS.4',
                    Rejected: 'DYNAMICTRANSLATIONS.3',
                    Withdrawn: 'DYNAMICTRANSLATIONS.2'
                }
            }
        }

        describe('and proper requirement id is provided', () => {
            var activityMock: Business.Activity = TestHelpers.ActivityGenerator.generate();
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
                    { id: '1', firstName: 'John', lastName: 'Doe', title: 'Mr.' },
                    { id: '2', firstName: 'Jane', lastName: 'Doe', title: 'Mrs.' }
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
                        endDate: "2016-01-01T11:00:00Z",
                        activity: activityMock
                    },
                    {
                        id: '2',
                        startDate: "2016-01-01T13:00:00Z",
                        endDate: "2016-01-01T14:00:00Z",
                        activity: activityMock
                    },
                    {
                        id: '3',
                        startDate: "2016-01-02T10:00:00Z",
                        endDate: "2016-01-02T11:00:00Z",
                        activity: activityMock
                    },
                    {
                        id: '4',
                        startDate: "2016-01-03T00:00:00Z",
                        endDate: "2016-01-03T01:00:00Z",
                        activity: activityMock
                    }
                ],
                offers: [
                    {
                        id: "1",
                        statusId: "1",
                        createdDate: new Date(2016, 1, 1)
                    },
                    {
                        id: "2",
                        statusId: "2",
                        createdDate: new Date(2014, 1, 1)
                    },
                    {
                        id: "3",
                        statusId: "3",
                        createdDate: new Date(2015, 1, 1)
                    },
                    {
                        id: "4",
                        statusId: "3",
                        createdDate: new Date(2011, 1, 1)
                    },
                    {
                        id: "5",
                        statusId: "4",
                        createdDate: new Date(2012, 1, 1)
                    }
                ]
            });

            var appConfigMock: Common.Models.IAppConfig =
                {
                    rootUrl: "",
                    appRootUrl: "",
                    fileChunkSizeInBytes: 12,
                    enumOrder: {
                        "OfferStatus": {
                            "Accepted": 1,
                            "New": 2,
                            "Rejected": 3,
                            "Withdrawn": 4
                        }
                    }
                }

            
            beforeEach(angular.mock.module(($provide: angular.auto.IProvideService) => {
                $provide.service('enumService', Antares.Mock.EnumServiceMock);
                $provide.constant('appConfig', appConfigMock);
            }));

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService,
                $filter: ng.IFilterService,
                enumProvider: Providers.EnumProvider) => {

                filter = $filter;
                $http = $httpBackend;

                type Dictionary = { [id: string]: string };
                var offerStatusToCodeDict: Dictionary = {
                    '1': 'New',
                    '2': 'Withdrawn',
                    '3': 'Rejected',
                    '4': 'Accepted'
                };

                enumProvider.getEnumCodeById = (statusId: string) =>{
                    return offerStatusToCodeDict[statusId];
                };

                scope = $rootScope.$new();
                scope['requirement'] = requirementMock;
                element = $compile('<requirement-view requirement="requirement"></requirement-view>')(scope);

                scope.$apply();

                controller = element.controller('requirementView');
            }));

            it('offers are displayed and sorted proper way (first by status then by date descending)', () => {
                var offersList = element.find(pageObjectSelectors.offers.list);
                expect(offersList.length).toEqual(5);
                expect($(<Element>offersList[0]).find(pageObjectSelectors.offers.offerStatus).text()).toEqual(pageObjectSelectors.offers.statusText.Accepted);
                expect($(<Element>offersList[1]).find(pageObjectSelectors.offers.offerStatus).text()).toEqual(pageObjectSelectors.offers.statusText.New);
                expect($(<Element>offersList[2]).find(pageObjectSelectors.offers.offerStatus).text()).toEqual(pageObjectSelectors.offers.statusText.Rejected);
                expect($(<Element>offersList[3]).find(pageObjectSelectors.offers.offerStatus).text()).toEqual(pageObjectSelectors.offers.statusText.Rejected);
                expect($(<Element>offersList[4]).find(pageObjectSelectors.offers.offerStatus).text()).toEqual(pageObjectSelectors.offers.statusText.Withdrawn);
            });

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
                var viewingItems = element.find(pageObjectSelectors.viewings.viewings);

                expect(viewingGroups.length).toBe(requirementMock.viewingsByDay.length);
                expect(viewingItems.length).toBe(requirementMock.viewings.length);

                requirementMock.viewingsByDay.sort((a, b) => new Date(b.day).getTime() - new Date(a.day).getTime()).forEach((g: Business.ViewingGroup, i: number) => {
                    expect(viewingGroupTitles[i].textContent).toBe(moment(g.day, 'YYYY-MM-DD').format('DD-MM-YYYY'));
                });
            });

            describe('when make new offer action is called', () => {
                var
                    newOfferController: OfferAddEditController,
                    newOfferPanelController: SidePanelController,
                    chosenViewing: Business.Viewing;

                beforeEach(() => {
                    newOfferController = controller.components.offerAdd();
                    newOfferPanelController = controller.components.panels.offerAdd();

                    spyOn(newOfferController, 'reset');

                    chosenViewing = requirementMock.viewings[0];
                    controller.showAddOfferPanel(chosenViewing);
                });

                it('new offer side panel is shown', () => {
                    var sidePanel = element.find(pageObjectSelectors.offers.sidePanel);
                    expect(sidePanel.length).toBe(1, 'Side panel not found');
                    expect(newOfferPanelController.visible).toBeTruthy('Side panel not visible');
                });

                it('new offer form is reset', () => {
                    expect(newOfferController.reset).toHaveBeenCalledTimes(1);
                });

                it('activity on offer is set', () => {
                    expect(newOfferController.activity).toBeDefined();
                    expect(newOfferController.activity.id).toEqual(chosenViewing.activity.id);
                });

                it('when cancel action is called side panel is hidden', () => {
                    controller.cancelSaveOffer();

                    expect(newOfferPanelController.visible).toBeFalsy('Side panel is not hidden');
                });

                xdescribe('when save action is called', () => {
                    var
                        offerMock: Business.Offer;

                    beforeEach(inject((
                        $q: ng.IQService) => {

                        offerMock = TestHelpers.OfferGenerator.generate();
                        var deferred = $q.defer();
                        spyOn(newOfferController, 'saveOffer').and.returnValue(deferred.promise);

                        controller.saveOffer();
                        deferred.resolve(offerMock);

                        scope.$apply();
                    }));

                    it('side panel is hidden', () => {
                        expect(newOfferPanelController.visible).toBeFalsy('Side panel is not hidden');
                    });

                    it('offer has been added to offers list', () => {
                        expect(controller.requirement.offers).toContain(offerMock);
                    });
                });
            });
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

                scope = $rootScope.$new();
            }));

            it('then note list is displayed', () => {
                // arrange
                requirementMock.requirementNotes = [
                    new Business.RequirementNote({ id: 'note1', requirementId: '111', description: 'descr 1', createdDate: new Date(), user: null }),
                    new Business.RequirementNote({ id: 'note2', requirementId: '111', description: 'descr 2', createdDate: new Date(), user: null })
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