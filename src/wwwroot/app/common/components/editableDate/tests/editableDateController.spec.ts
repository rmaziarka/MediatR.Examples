/// <reference path="../../../../typings/_all.d.ts" />

module Antares {
    import EditableDateController = Antares.Common.Component.EditableDateController;

    import runDescribe = TestHelpers.runDescribe;
    type TestCase = [Date, boolean, string]; // selected date, result, test case name

    describe('Given editable date controller',
    () =>{
        var $scope: ng.IScope,
            $q: ng.IQService,
            controller: EditableDateController;

        var datesToTest: any = {
            today : moment(),
            inThePast : moment().day(-7),
            longAgo : moment().year(1700),
            inTheFuture : moment().day(7)
        };

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            _$q_: ng.IQService,
            $controller: ng.IControllerService) =>{

            // init
            $scope = $rootScope.$new();
            $q = _$q_;
            controller = <EditableDateController>$controller('EditableDateController', { $scope : $scope });
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
                `${data[2]} then "isBeforeToday" must be ${data[1] ? 'true' : 'false'}`)
            .run((data: TestCase) =>{
                // arrange
                controller.selectedDate = data[0];

                //  act / assert
                var isBeforeToday = controller.isBeforeToday();

                // assert
                expect(isBeforeToday).toBe(data[1]);
            });
        describe('and canBeEdited is true',
        () =>{
            beforeEach(() =>{
                controller.canBeEdited = true;
            });

            it('when `openEditMode` then edit mode must be turned on and date must be set to selectedDate',
            () =>{
                // arrange
                var expectedSelectedDate = new Date(2012, 2, 2);

                controller.date = new Date(2010, 1, 1);
                controller.selectedDate = expectedSelectedDate;

                // act
                controller.openEditMode();

                // assert
                expect(controller.isInEditMode()).toBe(true);
                expect(controller.date).toEqual(expectedSelectedDate);
            });

            it('when `cancel` then edit mode must be turned off ',
            () =>{
                // arrange
                controller.openEditMode();

                // act
                controller.cancel();

                // assert
                expect(controller.isInEditMode()).toBe(false);
            });

            describe('and is in EditMode',
            () =>{
                beforeEach(() =>{
                    controller.openEditMode();
                });

                it('when `save` then edit mode must be turned off',
                () =>{
                    // arrange
                    var defered = $q.defer();
                    controller.onSave = () =>{
                        return (date: Date) =>{
                            return defered.promise;
                        }
                    };

                    // act
                    controller.save();
                    defered.resolve({});
                    $scope.$apply();

                    // assert
                    expect(controller.isInEditMode()).toBe(false);
                    });

                it('when change `canBeEdited` and `$onChanges` execute then `isInEditMode` must be false', () => {
                    // arrange
                    var onChangeArgument = {
                        canBeEdited:
                        {
                            currentValue: false,
                            previousValue: true
                        }
                    }

                    // act
                    controller.canBeEdited = false;
                    controller.$onChanges(onChangeArgument);

                    // assert
                    expect(controller.isInEditMode()).toBe(false);
                });

                it('when not change `canBeEdited` and `$onChanges` execute then `isInEditMode` must be not changed', () => {
                    // arrange
                    var onChangeArgument = {
                        canBeEdited:
                        {
                            currentValue: true,
                            previousValue: true
                        }
                    }

                    // act
                    controller.$onChanges(onChangeArgument);

                    // assert
                    expect(controller.isInEditMode()).toBe(true);
                });

            });
        });

        describe('and canBeEdited is false',
        () =>{
            beforeEach(() =>{
                controller.canBeEdited = false;
            });
            it('when `openEditMode` then cannot switch to edit mode and isInEditMode must be false',
            () =>{
                // act
                controller.openEditMode();

                // assert
                expect(controller.isInEditMode()).toBe(false);
            });
        });
    });
}