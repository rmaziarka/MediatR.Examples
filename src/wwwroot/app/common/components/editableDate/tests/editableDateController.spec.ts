/// <reference path="../../../../typings/_all.d.ts" />

module Antares {
    import EditableDateController = Antares.Common.Component.EditableDateController;
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    import runDescribe = TestHelpers.runDescribe;
    type TestCase = [Date, boolean, string]; // selected date, result, test case name

    describe('Given editable date controller', () => {
        var $scope: ng.IScope,
            $http: ng.IHttpBackendService,
            controller: EditableDateController;

        var datesToTest: any = {
            today: moment(),
            inThePast: moment().day(-7),
            longAgo: moment().year(1700),
            inTheFuture: moment().day(7)
        };

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $controller: ng.IControllerService) => {

            // init
            $scope = $rootScope.$new();

            controller = <EditableDateController>$controller('EditableDateController', { $scope: $scope });
        }));

        runDescribe('when date ')
            .data<TestCase>([
                [datesToTest.inThePast, true, 'is set to recent date'],
                [datesToTest.longAgo, true, 'is set to past date'],
                [datesToTest.today, false, 'is set to today'],
                [datesToTest.inTheFuture, false, 'is set to future date'],
                [null, false, 'is not set'],
            ])
            .dataIt((data: TestCase) =>
                `${data[2]} then "isBeforeToday" should be ${data[1] ? 'true' : 'false'}`)
            .run((data: TestCase) => {
                // arrange
                controller.date = data[0];

                //  act / assert
                var isBeforeToday = controller.isBeforeToday();

                // assert
                expect(isBeforeToday).toBe(data[1]);
            });

    });
}