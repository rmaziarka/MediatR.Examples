/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import ActivityAddPanelController = Activity.ActivityAddPanelController;
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    import runDescribe = TestHelpers.runDescribe;

    describe('Given add activity panel', () => {
        var $scope: ng.IScope,
            $http: ng.IHttpBackendService,
            controller: ActivityAddPanelController,
            eventAggregator: Antares.Core.EventAggregator;

        describe('when save', () => {
            var newActivity: Antares.Activity.AddCard.ActivityAddCardModel;

            beforeEach(inject((
                _eventAggregator_: Antares.Core.EventAggregator,
                $controller: ng.IControllerService,
                $httpBackend: ng.IHttpBackendService) => {

                $http = $httpBackend;
                eventAggregator = _eventAggregator_;

                newActivity = generateActivityAddCardModel();
                controller = <ActivityAddPanelController>$controller('ActivityAddPanelController');

                controller.propertyTypeId = TestHelpers.StringGenerator.generate();
                controller.ownerships = [];
            }));

            it('then correct activity data should be saved', () => {
                // arrange
                var requestData: Antares.Activity.AddPanel.IActivityAddPanelCommand;

                var contactIds = newActivity.contacts.map((contact: Business.Contact) => contact.id);

                $http.expectPOST(/\/api\/activities/, (data: string) => {
                    requestData = JSON.parse(data);

                    return true;
                }).respond(200, new Business.Activity());

                // act
                controller.save(newActivity);
                $http.flush();

                // assert
                expect(requestData.activityStatusId).toEqual(newActivity.activityStatusId);
                expect(requestData.propertyId).toEqual(controller.propertyId);
                expect(requestData.contactIds.length).toEqual(contactIds.length);
                expect(requestData.contactIds).toEqual(contactIds);
            });

            it('then event for closing panel should be published', () => {
                // arrange
                var isCloseSidePanelEventPublished: boolean = false;

                eventAggregator.with({}).subscribe(Antares.Common.Component.CloseSidePanelEvent, () => {
                    isCloseSidePanelEventPublished = true;
                });

                $http.expectPOST(/\/api\/activities/, (data: string) => {
                    return true;
                }).respond(200, new Business.Activity());

                // act
                controller.save(newActivity);
                $http.flush();

                // assert
                expect(isCloseSidePanelEventPublished).toBeTruthy();
            })

            it('then event for adding new activity should be published', () => {
                // arrange
                var isActivityAddedSidePanelEventPublished: boolean = false;
                var activityFromEvent: Antares.Common.Models.Dto.IActivity = null;
                var activitySaved: Antares.Common.Models.Dto.IActivity = TestHelpers.ActivityGenerator.generateDto();

                eventAggregator.with({}).subscribe(Activity.ActivityAddedSidePanelEvent, (msg: Activity.ActivityAddedSidePanelEvent) => {
                    isActivityAddedSidePanelEventPublished = true;
                    activityFromEvent = msg.activityAdded;
                });

                $http.expectPOST(/\/api\/activities/, (data: string) => {
                    return true;
                }).respond(200, activitySaved);

                // act
                controller.save(newActivity);
                $http.flush();

                // assert
                expect(isActivityAddedSidePanelEventPublished).toBeTruthy();
                expect(activityFromEvent).toEqual(activitySaved);
            })
        });

        describe('when load config', () => {
            var newActivityCommand: Antares.Activity.AddPanel.ActivityAddPanelCommand;

            beforeEach(inject((
                $controller: ng.IControllerService,
                $httpBackend: ng.IHttpBackendService) => {

                $http = $httpBackend;

                controller = <ActivityAddPanelController>$controller('ActivityAddPanelController');

                controller.propertyTypeId = TestHelpers.StringGenerator.generate();
                controller.ownerships = [];

                newActivityCommand = new Antares.Activity.AddPanel.ActivityAddPanelCommand(generateActivityAddCardModel(), controller.propertyId);
            }));

            it('then correct data should be sent for configuration', () => {
                // arrange
                var requestData: Antares.Activity.AddPanel.IActivityAddPanelCommand;
                var url: string;

                $http.expectPOST((urlPost: string) => {
                    var regularExpression = /\/api\/metadata\/attributes\/activity/;
                    url = urlPost;
                    return regularExpression.test(url);
                }, (data: string) => {
                    requestData = JSON.parse(data);

                    return true;
                    }).respond(200, {});


                // act
                controller.loadConfig(newActivityCommand);
                $http.flush();

                // assert
                expect(Antares.TestHelpers.UrlParser.getParameter(url, 'pageType')).toBe(Antares.Common.Models.Enums.PageTypeEnum.Create);
                expect(Antares.TestHelpers.UrlParser.getParameter(url, 'activityTypeId')).toBe(newActivityCommand.activityTypeId);
                expect(Antares.TestHelpers.UrlParser.getParameter(url, 'propertyTypeId')).toBe(controller.propertyTypeId);

                expect(requestData).not.toBeNull();
                expect(requestData.activityStatusId).toBe(newActivityCommand.activityStatusId);
                expect(requestData.contactIds).toEqual(newActivityCommand.contactIds);
            });
        });

        var generateActivityAddCardModel = (): Antares.Activity.AddCard.ActivityAddCardModel => {
            var activity = new Antares.Activity.AddCard.ActivityAddCardModel();
            activity.activityStatusId = TestHelpers.StringGenerator.generate();
            activity.activityTypeId = TestHelpers.StringGenerator.generate();
            activity.id = TestHelpers.StringGenerator.generate();
            activity.contacts = TestHelpers.ContactGenerator.generateMany(3);

            return activity;
        }

    });
}