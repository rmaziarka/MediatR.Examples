/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import ActivityNegotiatorsEditControlController = Antares.Attributes.ActivityNegotiatorsEditControlController;
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import Enums = Common.Models.Enums;
    import runDescribe = TestHelpers.runDescribe;

    describe('Given edit negotiators controller', () => {
        var $scope: ng.IScope,
            $http: ng.IHttpBackendService,
            controller: ActivityNegotiatorsEditControlController;

        var convertToCallDate = (moment: moment.Moment): Date => moment.startOf('day').toDate();

        var datesToTest: any = {
            today: convertToCallDate(moment()),
            inThePast: convertToCallDate(moment().day(-7)),
            inTheFuture: convertToCallDate(moment().day(7)),
            inTheFutureOther: convertToCallDate(moment().day(10))
        };

        var departmentTypes = [
            { id: "managingId", code: "Managing" },
            { id: "standardId", code: "Standard" }
        ];

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $controller: any,
            $httpBackend: ng.IHttpBackendService) => {

            // init
            $scope = $rootScope.$new();
            $http = $httpBackend;

            var scopeMock = {
                negotiatorForm: {
                    callDate: {}
                }
            }

            var bindings = { activityId: 'testId', departments: <Business.ActivityDepartment[]>[] };
            controller = <ActivityNegotiatorsEditControlController>$controller('ActivityNegotiatorsEditControlController', { $scope: scopeMock }, bindings);

            controller.negotiatorsForm = <Antares.Attributes.IActivityNegotiatorsEditControlForm>{
                callDate: <ng.INgModelController>{
                    $invalid: false,
                    $dirty: false
                }
            }

            controller.onNegotiatorAdded = () => { };
            controller.onNegotiatorRemoved = () => { };
        }));

        describe('when editLeadNegotiator is called', () => {
            it('then isLeadNegotiatorInEditMode is set to true', () => {
                // arrange
                controller.isLeadNegotiatorInEditMode = false;

                //act
                controller.editLeadNegotiator();

                // assert
                expect(controller.isLeadNegotiatorInEditMode).toBe(true);
            });
        });

        describe('when cancelChangeLeadNegotiator is called', () => {
            it('then isLeadNegotiatorInEditMode is set to false', () => {
                // arrange
                controller.isLeadNegotiatorInEditMode = true;

                //act
                controller.cancelChangeLeadNegotiator();

                // assert
                expect(controller.isLeadNegotiatorInEditMode).toBe(false);
            });
        });

        describe('when changeLeadNegotiator is called', () => {
            it('then isLeadNegotiatorInEditMode is set to false', () => {
                // arrange
                controller.isLeadNegotiatorInEditMode = true;

                //act
                controller.changeLeadNegotiator(new Business.User(TestHelpers.UserGenerator.generateDto()));

                // assert
                expect(controller.isLeadNegotiatorInEditMode).toBe(false);
            });

            it('then leadNegotiator is set properly', () => {
                // arrange
                controller.leadNegotiator = null;

                //act
                var user = new Business.User(TestHelpers.UserGenerator.generateDto());
                controller.changeLeadNegotiator(user);

                // assert
                expect(controller.leadNegotiator.user).toEqual(user);
                expect(controller.leadNegotiator.userId).toBe(user.id);
                expect(controller.leadNegotiator.activityId).toBe(controller.activityId);
            });

            type TestCaseForCallDate = [Date, Date, string, string]; //[lead CallDate, expected lead CallDate, test description part 1, , test description part 2]
            runDescribe('and when call date for leadNegotiator is ')
                .data<TestCaseForCallDate>([
                    [null, datesToTest.today, 'not set', 'not be changed'],
                    [undefined, datesToTest.today, 'undefined', 'not be changed'],
                    [datesToTest.inTheFuture, datesToTest.inTheFuture, 'in the future', 'not be changed'],
                    [datesToTest.today, datesToTest.today, 'today', 'not be changed'],
                    [datesToTest.inThePast, datesToTest.today, 'in the past', 'be changed to today']])
                .dataIt((data: TestCaseForCallDate) =>
                    `${data[2]} then call date should ${data[3]}`)
                .run((data: TestCaseForCallDate) => {
                    // arrange
                    controller.today = datesToTest.today;
                    controller.changeLeadNegotiator(new Business.User(TestHelpers.UserGenerator.generateDto()));

                    controller.leadNegotiator.callDate = data[0];

                    //act
                    controller.changeLeadNegotiator(new Business.User(TestHelpers.UserGenerator.generateDto()));

                    // assert
                    expect(controller.leadNegotiator.callDate).toEqual(data[1]);
                });
        });

        describe('when editSecondaryNegotiators is called', () => {
            it('then isSecondaryNegotiatorsInEditMode is set to true', () => {
                // arrange
                controller.isSecondaryNegotiatorsInEditMode = false;

                //act
                controller.editSecondaryNegotiators();

                // assert
                expect(controller.isSecondaryNegotiatorsInEditMode).toBe(true);
            });
        });

        describe('when cancelAddSecondaryNegotiator is called', () => {
            it('then isSecondaryNegotiatorsInEditMode is set to false', () => {
                // arrange
                controller.isSecondaryNegotiatorsInEditMode = true;

                //act
                controller.cancelAddSecondaryNegotiator();

                // assert
                expect(controller.isSecondaryNegotiatorsInEditMode).toBe(false);
            });
        });

        describe('when addSecondaryNegotiator is called', () => {
            it('then isSecondaryNegotiatorsInEditMode is not set to false', () => {
                // arrange
                controller.secondaryNegotiators = [];
                controller.isLeadNegotiatorInEditMode = true;

                //act
                controller.addSecondaryNegotiator(new Business.User(TestHelpers.UserGenerator.generateDto()));

                // assert
                expect(controller.isLeadNegotiatorInEditMode).toBe(true);
            });

            it('then negotiator is added to secondaryNegotiators', () => {
                // arrange
                controller.secondaryNegotiators = [];

                //act
                var user = new Business.User(TestHelpers.UserGenerator.generateDto());
                controller.addSecondaryNegotiator(user);

                // assert
                expect(controller.secondaryNegotiators.length).toBe(1);
                expect(controller.secondaryNegotiators[0].user).toEqual(user);
                expect(controller.secondaryNegotiators[0].userId).toBe(user.id);
                expect(controller.secondaryNegotiators[0].activityId).toBe(controller.activityId);
                expect(controller.secondaryNegotiators[0].callDate).toBeNull();
            });
        });

        describe('when deleteSecondaryNegotiator is called', () => {
            it('then negotiator is removed from secondaryNegotiators', () => {
                // arrange
                controller.secondaryNegotiators = [];
                controller.addSecondaryNegotiator(new Business.User(TestHelpers.UserGenerator.generateDto()));
                controller.addSecondaryNegotiator(new Business.User(TestHelpers.UserGenerator.generateDto()));
                controller.addSecondaryNegotiator(new Business.User(TestHelpers.UserGenerator.generateDto()));

                //act
                var negotiatroToRemove = controller.secondaryNegotiators[2];
                controller.deleteSecondaryNegotiator(negotiatroToRemove);

                // assert
                expect(controller.secondaryNegotiators.length).toBe(2);
                expect(_.find(controller.secondaryNegotiators, (item) => item.user.id === negotiatroToRemove.user.id)).toBeUndefined();
            });
        });

        describe('when switchToLeadNegotiator is called', () => {
            it('then negotiators are changed properly', () => {
                // arrange
                controller.changeLeadNegotiator(new Business.User(TestHelpers.UserGenerator.generateDto()));

                controller.secondaryNegotiators = [];
                controller.addSecondaryNegotiator(new Business.User(TestHelpers.UserGenerator.generateDto()));
                controller.addSecondaryNegotiator(new Business.User(TestHelpers.UserGenerator.generateDto()));
                controller.addSecondaryNegotiator(new Business.User(TestHelpers.UserGenerator.generateDto()));
                controller.addSecondaryNegotiator(new Business.User(TestHelpers.UserGenerator.generateDto()));

                //act
                var oldLeadNegotiator = controller.leadNegotiator;
                var negotiatorToSwitch = controller.secondaryNegotiators[1];
                controller.switchToLeadNegotiator(negotiatorToSwitch);

                // assert
                expect(controller.leadNegotiator).toBe(negotiatorToSwitch);
                expect(controller.secondaryNegotiators.indexOf(oldLeadNegotiator)).not.toBe(-1);
            });

            type TestCaseForCallDate = [Date, Date, Date, Date, string, string, string, string]; //[lead CallDate, secondary CallDate, expected lead CallDate, expected secondary CallDate, test description part 1 - 4]
            runDescribe('and when call date ')
                .data<TestCaseForCallDate>([
                    [datesToTest.inTheFuture, undefined, datesToTest.inTheFuture, datesToTest.inTheFuture, 'in the future', 'undefined', 'not be changed', 'be changed to the future'],
                    [datesToTest.inTheFuture, null, datesToTest.inTheFuture, datesToTest.inTheFuture, 'in the future', 'not set', 'not be changed', 'be changed to the future'],
                    [datesToTest.inTheFuture, datesToTest.inTheFuture, datesToTest.inTheFuture, datesToTest.inTheFuture, 'in the future', 'in the future', 'not be changed', 'not be changed'],
                    [datesToTest.inTheFuture, datesToTest.inTheFutureOther, datesToTest.inTheFutureOther, datesToTest.inTheFuture, 'in the future', 'in other future', 'be changed to other future', 'be changed to the future'],
                    [datesToTest.inTheFuture, datesToTest.today, datesToTest.today, datesToTest.inTheFuture, 'in the future', 'today', 'be changed to today', 'be changed to the future'],
                    [datesToTest.inTheFuture, datesToTest.inThePast, datesToTest.today, datesToTest.inTheFuture, 'in the future', 'in the past', 'be changed to today', 'be changed to the future'],

                    [datesToTest.today, undefined, datesToTest.today, datesToTest.today, 'today', 'undefined', 'not be changed', 'be changed to today'],
                    [datesToTest.today, null, datesToTest.today, datesToTest.today, 'today', 'not set', 'not be changed', 'be changed to today'],
                    [datesToTest.today, datesToTest.today, datesToTest.today, datesToTest.today, 'today', 'today', 'not be changed', 'not be changed'],
                    [datesToTest.today, datesToTest.inTheFuture, datesToTest.inTheFuture, datesToTest.today, 'today', 'in the future', 'be changed to the future', 'be changed to today'],
                    [datesToTest.today, datesToTest.inThePast, datesToTest.today, datesToTest.today, 'today', 'in the past', 'be changed to today', 'be changed to today'],

                    [datesToTest.inThePast, undefined, datesToTest.today, datesToTest.inThePast, 'in the past', 'undefined', 'be changed to today', 'be changed to the past'],
                    [datesToTest.inThePast, null, datesToTest.today, datesToTest.inThePast, 'in the past', 'not set', 'be changed to today', 'be changed to the past'],
                    [datesToTest.inThePast, datesToTest.inThePast, datesToTest.today, datesToTest.inThePast, 'in the past', 'in the past', 'be changed to today', 'be changed to the past'],
                    [datesToTest.inThePast, datesToTest.inTheFuture, datesToTest.inTheFuture, datesToTest.inThePast, 'in the past', 'in the future', 'be changed to the future', 'be changed to the past'],
                    [datesToTest.inThePast, datesToTest.today, datesToTest.today, datesToTest.inThePast, 'in the past', 'in the past', 'be changed to today', 'be changed to the past']])
                .dataIt((data: TestCaseForCallDate) =>
                    `for lead is ${data[4]} and for secondary is ${data[5]} then call date for new lead should ${data[6]} and for new secondary should ${data[7]}`)
                .run((data: TestCaseForCallDate) => {
                    // arrange
                    controller.today = datesToTest.today;
                    controller.secondaryNegotiators = [];

                    controller.changeLeadNegotiator(new Business.User(TestHelpers.UserGenerator.generateDto()));
                    controller.addSecondaryNegotiator(new Business.User(TestHelpers.UserGenerator.generateDto()));

                    controller.leadNegotiator.callDate = data[0];
                    controller.secondaryNegotiators[0].callDate = data[1];

                    //act
                    controller.switchToLeadNegotiator(controller.secondaryNegotiators[0]);

                    // assert
                    expect(controller.leadNegotiator.callDate).toEqual(data[2]);
                    expect(controller.secondaryNegotiators[0].callDate).toEqual(data[3]);
                });
        });

        describe('when openNegotiatorCallDate is called', () => {
            it('then proper "open flag" for negotiator is set', () => {
                // arrange
                controller.changeLeadNegotiator(new Business.User(TestHelpers.UserGenerator.generateDto()));

                controller.secondaryNegotiators = [];
                controller.addSecondaryNegotiator(new Business.User(TestHelpers.UserGenerator.generateDto()));
                controller.addSecondaryNegotiator(new Business.User(TestHelpers.UserGenerator.generateDto()));

                //act
                controller.openNegotiatorCallDate(controller.leadNegotiator.userId);
                controller.openNegotiatorCallDate(controller.secondaryNegotiators[0].userId);

                // assert
                expect(controller.nagotiatorCallDateOpened[controller.leadNegotiator.userId]).toBeTruthy();
                expect(controller.nagotiatorCallDateOpened[controller.secondaryNegotiators[0].userId]).toBeTruthy();
                expect(controller.nagotiatorCallDateOpened[controller.secondaryNegotiators[1].userId]).toBeFalsy();
            });
        });

        describe('when getUsersQuery is called', () => {
            it('then proper query is returned', () => {
                // arrange
                controller.changeLeadNegotiator(new Business.User(TestHelpers.UserGenerator.generateDto()));

                controller.secondaryNegotiators = [];
                controller.addSecondaryNegotiator(new Business.User(TestHelpers.UserGenerator.generateDto()));
                controller.addSecondaryNegotiator(new Business.User(TestHelpers.UserGenerator.generateDto()));
                controller.addSecondaryNegotiator(new Business.User(TestHelpers.UserGenerator.generateDto()));

                //act
                var result = controller.getUsersQuery('searchText');

                // assert
                var expectedExcludedIds = _.map(controller.secondaryNegotiators, (item) => { return item.user.id });
                expectedExcludedIds.push(controller.leadNegotiator.user.id);

                expect(result.partialName).toBe('searchText');
                expect(result.take).toBe(100);
                expect(result['excludedIds[]']).toEqual(expectedExcludedIds);

            });
        });

        describe('when getUsers is called', () => {
            it('then data is fetched from API', () => {
                // arrange
                controller.changeLeadNegotiator(new Business.User(TestHelpers.UserGenerator.generateDto()));

                controller.secondaryNegotiators = [];
                controller.addSecondaryNegotiator(new Business.User(TestHelpers.UserGenerator.generateDto()));
                controller.addSecondaryNegotiator(new Business.User(TestHelpers.UserGenerator.generateDto()));
                controller.addSecondaryNegotiator(new Business.User(TestHelpers.UserGenerator.generateDto()));

                // act
                var result: Dto.IUser[] = [];
                var requestUser = TestHelpers.UserGenerator.generateDto();

                $http.expectGET(/\/api\/users/).respond(200, [requestUser]);

                controller.getUsers('searchText').then((requestResult) => {
                    result = requestResult;
                });
                $http.flush();

                // assert
                expect(result[0].id).toEqual(requestUser.id);
            });
        });
    });
}