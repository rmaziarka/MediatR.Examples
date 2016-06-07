/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import DepartmentsController = Common.Component.DepartmentsController;
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import Enums = Common.Models.Enums;
    import runDescribe = TestHelpers.runDescribe;

    describe('Given departments controller', () => {
        var $scope: ng.IScope,
            $http: ng.IHttpBackendService,
            controller: DepartmentsController;

        var datesToTest: any = {
            today: moment(),
            inThePast: moment().day(-7),
            inTheFuture: moment().day(7),
            inTheFutureOther: moment().day(10)
        };

        var departmentTypes = {
            managing: <Dto.IEnumTypeItem>{ id: "managingId", code: "Managing", enumTypeId: '' },
            standard: <Dto.IEnumTypeItem>{ id: "standardId", code: "Standard", enumTypeId: '' }
        };

        var leadNegotiatorMock = TestHelpers.ActivityUserGenerator.generate(Enums.NegotiatorTypeEnum.LeadNegotiator);
        var secondaryNegotiatorsMock = TestHelpers.ActivityUserGenerator.generateMany(3, Enums.NegotiatorTypeEnum.SecondaryNegotiator);

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $controller: any,
            $httpBackend: ng.IHttpBackendService) => {

            // init
            $scope = $rootScope.$new();
            $http = $httpBackend;

            var bindings = { activityId: 'testId', departments: <Business.ActivityDepartment[]>[], leadNegotiator: leadNegotiatorMock, secondaryNegotiators: secondaryNegotiatorsMock };
            controller = <DepartmentsController>$controller('DepartmentsController', {}, bindings);

            controller.managingDepartmentType = <Dto.IEnumTypeItem>departmentTypes.managing;
            controller.standardDepartmentType = <Dto.IEnumTypeItem>departmentTypes.standard;
        }));

        describe('when deleteDepartment is called', () => {
            it('and its not related with negotiator then depeartment is removed', () => {
                // arrange
                spyOn(controller, 'departmentIsRelatedWithNegotiator').and.returnValue(false);

                controller.departments = TestHelpers.ActivityDepartmentGenerator.generateMany(3);

                //act
                var departmentToRemove: Business.ActivityDepartment = controller.departments[2];
                controller.deleteDepartment(departmentToRemove);

                // assert
                expect(controller.departments.length).toBe(2);
                expect(_.some(controller.departments, (item: Business.ActivityDepartment) => item.departmentId === departmentToRemove.departmentId)).toBe(false);
            });

            it('and its related with negotiator then depeartment is not removed', () => {
                // arrange
                spyOn(controller, 'departmentIsRelatedWithNegotiator').and.returnValue(true);

                controller.departments = TestHelpers.ActivityDepartmentGenerator.generateMany(3);

                //act
                var departmentToRemove: Business.ActivityDepartment = controller.departments[2];
                controller.deleteDepartment(departmentToRemove);

                // assert
                expect(controller.departments.length).toBe(3);
                expect(_.some(controller.departments, (item: Business.ActivityDepartment) => item.departmentId === departmentToRemove.departmentId)).toBe(true);
            });
        });

        describe('when setAsManagingDepartment is called', () => {
            type TestCaseForDepartmentsChange = [number, number];
            runDescribe('for department ')
                .data<TestCaseForDepartmentsChange>([
                    [0, 2],
                    [1, 0],
                    [1, 1],
                    [2, 1]])
                .dataIt((data: TestCaseForDepartmentsChange) =>
                    `${data[1]} and current managing department is ${data[0]} then new managing department is ${data[1]}`)
                .run((data: TestCaseForDepartmentsChange) => {
                    // arrange
                    controller.departments = TestHelpers.ActivityDepartmentGenerator.generateMany(3);
                    controller.departments[data[1]].departmentType = departmentTypes.managing;

                    //act
                    var newManagingDepartment = controller.departments[data[0]];
                    controller.setAsManagingDepartment(newManagingDepartment);

                    // assert
                    var managingDepartment = _.filter(controller.departments, (department) =>
                        angular.equals(department.departmentType, departmentTypes.managing) &&
                        department.departmentTypeId === departmentTypes.managing.id);

                    expect(managingDepartment.length).toBe(1);
                    expect(managingDepartment[0].departmentId).toBe(newManagingDepartment.departmentId);
                });
        });

        describe('when departmentIsRelatedWithNegotiator is called', () => {
            type TestCaseForDepartmentsCheck = [Business.ActivityDepartment, number, Business.ActivityDepartment, string, boolean]; //[leadNegotiatorDepartment, secondaryNegotiatorIndex, secondaryNegotiatorDepartment, message]
            var departmentToCheck = TestHelpers.ActivityDepartmentGenerator.generate();

            runDescribe('and when depertment is related to ')
                .data<TestCaseForDepartmentsCheck>([
                    [TestHelpers.ActivityDepartmentGenerator.generate(), 0, TestHelpers.ActivityDepartmentGenerator.generate(), 'no negotiator', false],
                    [departmentToCheck, 0, TestHelpers.ActivityDepartmentGenerator.generate(), 'lead negotiator', true],
                    [TestHelpers.ActivityDepartmentGenerator.generate(), 0, departmentToCheck, 'secondary negotiator 0', true],
                    [TestHelpers.ActivityDepartmentGenerator.generate(), 1, departmentToCheck, 'secondary negotiator 1', true]])
                .dataIt((data: TestCaseForDepartmentsCheck) =>
                    `${data[3]} then it should return ${data[4]}`)
                .run((data: TestCaseForDepartmentsCheck) => {
                    // arrange
                    controller.departments.push(departmentToCheck);

                    controller.leadNegotiator.user.departmentId = data[0].departmentId;
                    controller.leadNegotiator.user.department = data[0].department;

                    controller.secondaryNegotiators[data[1]].user.departmentId = data[2].departmentId;
                    controller.secondaryNegotiators[data[1]].user.department = data[2].department;

                    // act / assert
                    expect(controller.departmentIsRelatedWithNegotiator(departmentToCheck.department)).toBe(data[4]);
                });
        });
    });
}