/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import NegotiatorsController = Common.Component.NegotiatorsController;
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import Enums = Common.Models.Enums;

    describe('Given negotiators controller', () => {
        var $scope: ng.IScope,
            $http: ng.IHttpBackendService,
            controller: NegotiatorsController;

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $controller: any,
            $httpBackend: ng.IHttpBackendService) => {

            // init
            $scope = $rootScope.$new();
            $http = $httpBackend;

            var bindings = { activityId: 'testId' };
            controller = <NegotiatorsController>$controller('NegotiatorsController', {}, bindings);
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
                controller.changeLeadNegotiator(new Business.DepartmentUser(TestHelpers.UserGenerator.generateDto()));

                // assert
                expect(controller.isLeadNegotiatorInEditMode).toBe(false);
            });

            it('then leadNegotiator is set properly', () => {
                // arrange
                controller.leadNegotiator = null;

                //act
                var user = new Business.DepartmentUser(TestHelpers.UserGenerator.generateDto());
                controller.changeLeadNegotiator(user);

                // assert
                expect(controller.leadNegotiator.user).toBe(user);
                expect(controller.leadNegotiator.userId).toBe(user.id);
                expect(controller.leadNegotiator.activityId).toBe(controller.activityId);
                expect(controller.leadNegotiator.userType).toBe(Enums.NegotiatorTypeEnum.LeadNegotiator);
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
                controller.addSecondaryNegotiator(new Business.DepartmentUser(TestHelpers.UserGenerator.generateDto()));

                // assert
                expect(controller.isLeadNegotiatorInEditMode).toBe(true);
            });

            it('then negotiator is added to secondaryNegotiators', () => {
                // arrange
                controller.secondaryNegotiators = [];

                //act
                var user = new Business.DepartmentUser(TestHelpers.UserGenerator.generateDto());
                controller.addSecondaryNegotiator(user);

                // assert
                expect(controller.secondaryNegotiators.length).toBe(1);
                expect(controller.secondaryNegotiators[0].user).toBe(user);
                expect(controller.secondaryNegotiators[0].userId).toBe(user.id);
                expect(controller.secondaryNegotiators[0].activityId).toBe(controller.activityId);
                expect(controller.secondaryNegotiators[0].userType).toBe(Enums.NegotiatorTypeEnum.SecondaryNegotiator);
            });
        });

        describe('when deleteSecondaryNegotiator is called', () => {
            it('then negotiator is removed from secondaryNegotiators', () => {
                // arrange
                controller.secondaryNegotiators = [];
                controller.addSecondaryNegotiator(new Business.DepartmentUser(TestHelpers.UserGenerator.generateDto()));
                controller.addSecondaryNegotiator(new Business.DepartmentUser(TestHelpers.UserGenerator.generateDto()));
                controller.addSecondaryNegotiator(new Business.DepartmentUser(TestHelpers.UserGenerator.generateDto()));

                //act
                var negotiatroToRemove = controller.secondaryNegotiators[2];
                controller.deleteSecondaryNegotiator(negotiatroToRemove);

                // assert
                expect(controller.secondaryNegotiators.length).toBe(2);
                expect(_.find(controller.secondaryNegotiators, (item) => { return item.user.id === negotiatroToRemove.user.id; })).toBeUndefined();
            });
        });

        describe('when getUsersQuery is called', () => {
            it('then proper query is returned', () => {
                // arrange
                controller.changeLeadNegotiator(new Business.DepartmentUser(TestHelpers.UserGenerator.generateDto()));

                controller.secondaryNegotiators = [];
                controller.addSecondaryNegotiator(new Business.DepartmentUser(TestHelpers.UserGenerator.generateDto()));
                controller.addSecondaryNegotiator(new Business.DepartmentUser(TestHelpers.UserGenerator.generateDto()));
                controller.addSecondaryNegotiator(new Business.DepartmentUser(TestHelpers.UserGenerator.generateDto()));

                //act
                var result = controller.getUsersQuery('searchText');

                // assert
                var expectedExcludedIds = _.map(controller.secondaryNegotiators, (item) =>{ return item.user.id });
                expectedExcludedIds.push(controller.leadNegotiator.user.id);

                expect(result.partialName).toBe('searchText');
                expect(result.take).toBe(100);
                expect(result['excludedIds[]']).toEqual(expectedExcludedIds);

            });
        });

        describe('when getUsers is called', () => {
            it('then proper Api GET request is called', () => {
                // arrange
                controller.changeLeadNegotiator(new Business.DepartmentUser(TestHelpers.UserGenerator.generateDto()));

                controller.secondaryNegotiators = [];
                controller.addSecondaryNegotiator(new Business.DepartmentUser(TestHelpers.UserGenerator.generateDto()));
                controller.addSecondaryNegotiator(new Business.DepartmentUser(TestHelpers.UserGenerator.generateDto()));
                controller.addSecondaryNegotiator(new Business.DepartmentUser(TestHelpers.UserGenerator.generateDto()));

                // act
                var result: Dto.IUser[] = [];
                var requestUser = TestHelpers.UserGenerator.generateDto();

                $http.expectGET(/\/api\/users/).respond(200, [requestUser]);

                controller.getUsers('searchText').then((requestResult) =>{
                    result = requestResult;
                });
                $http.flush();

                // assert
                expect(result[0].id).toEqual(requestUser.id);
            });
        });
	});
}