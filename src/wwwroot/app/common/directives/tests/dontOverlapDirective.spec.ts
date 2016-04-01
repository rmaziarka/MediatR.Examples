/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    interface IDontOverlapScope extends ng.IScope {
        rangeList: Array<any>;
        model: any;
        form: any;
        minValue: any;
        maxValue: any;
    }

    describe('Given input rendered with dont-overlap attribute', () => {
        var scope: IDontOverlapScope,
            element: ng.IAugmentedJQuery,
            compile: ng.ICompileService;

        beforeEach(angular.mock.module('app'));

        describe('and no dont-overlap-max or dont-overlap-min attributes are specified', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) => {

                compile = $compile;
                scope = <IDontOverlapScope>$rootScope.$new();
                scope.model = null;
                element = compile('<form name="form"><input type="text" dont-overlap="{{rangeList}}" ng-model="model" name="model" /></form>')(scope);
            }));

            it('when range list is empty there are no validation errors', () => {
                scope.rangeList = [];
                scope.$apply();

                expect(scope.form.model.$valid).toBeTruthy();
            });

            describe('when range list has well formed ranges', () => {
                beforeEach(() => {
                    scope.rangeList = [
                        { from: new Date(2000, 1, 1), to: new Date(2001, 1, 1) },
                        { from: new Date(2002, 1, 1), to: new Date(2003, 1, 1) }
                    ];
                    scope.$apply();
                });

                describe('there are no validation errors', () => {
                    it('when model value is below defined ranges', () => {
                        scope.form.model.$setViewValue(new Date(1990, 1, 1));
                        scope.$apply();

                        expect(scope.form.model.$valid).toBeTruthy();
                    });

                    it('when model value is above defined ranges', () => {
                        scope.form.model.$setViewValue(new Date(2010, 1, 1));
                        scope.$apply();

                        expect(scope.form.model.$valid).toBeTruthy();
                    });
                    it('when model value is between defined ranges', () => {
                        scope.form.model.$setViewValue(new Date(2001, 10, 1));
                        scope.$apply();

                        expect(scope.form.model.$valid).toBeTruthy();
                    });
                });

                describe('there are validation errors', () => {
                    it('when model value is inside defined ranges', () => {
                        scope.form.model.$setViewValue(new Date(2000, 10, 1));
                        scope.$apply();

                        expect(scope.form.model.$valid).toBeFalsy();
                    });
                });
            });

            describe('when range list has ranges with open ends', () => {
                beforeEach(() => {
                    scope.rangeList = [
                        { from: null, to: new Date(2001, 1, 1) },
                        { from: new Date(2002, 1, 1), to: new Date(2003, 1, 1) },
                        { from: new Date(2004, 1, 1), to: null }
                    ];
                    scope.$apply();
                });

                it('there are validation errors when model value is null', () => {
                    scope.form.model.$setViewValue(null);
                    scope.$apply();

                    expect(scope.form.model.$valid).toBeFalsy();
                });

                it('there are no validation errors when model value is not null and is between defined ranges', () => {
                    scope.form.model.$setViewValue(new Date(2001, 10, 1));
                    scope.$apply();

                    expect(scope.form.model.$valid).toBeTruthy();
                });
            });
        });

        //--- dont-overlap-max START

        describe('and dont-overlap-max attribute is specified', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) => {

                compile = $compile;
                scope = <IDontOverlapScope>$rootScope.$new();
                scope.model = null;
                element = compile('<form name="form"><input type="text" dont-overlap="{{rangeList}}" dont-overlap-max="{{maxValue}}" ng-model="minValue" name="minValue" /></form>')(scope);
            }));

            describe('when range list has well formed ranges', () => {
                beforeEach(() => {
                    scope.rangeList = [
                        { from: new Date(2000, 1, 1), to: new Date(2001, 1, 1) },
                        { from: new Date(2002, 1, 1), to: new Date(2003, 1, 1) }
                    ];
                    scope.$apply();
                });

                describe('there are no validation errors', () => {
                    it('when both values form a range above defined ranges', () =>{
                        scope.maxValue = new Date(2005, 1, 1);
                        scope.form.minValue.$setViewValue(new Date(2004, 1, 1));
                        scope.$apply();

                        expect(scope.form.minValue.$valid).toBeTruthy();
                    });

                    it('when both values form a range below defined ranges', () => {
                        scope.maxValue = new Date(1991, 1, 1);
                        scope.form.minValue.$setViewValue(new Date(1990, 1, 1));
                        scope.$apply();

                        expect(scope.form.minValue.$valid).toBeTruthy();
                    });

                    it('when both values form a range between defined ranges and not overlapping them', () => {
                        scope.maxValue = new Date(2001, 10, 1);
                        scope.form.minValue.$setViewValue(new Date(2001, 2, 1));
                        scope.$apply();

                        expect(scope.form.minValue.$valid).toBeTruthy();
                    });
                });

                describe('there are validation errors', () => {
                    it('when min value is below and max value is above defined ranges', () => {
                        scope.maxValue = new Date(2008, 1, 1);
                        scope.form.minValue.$setViewValue(new Date(1990, 1, 1));
                        scope.$apply();

                        expect(scope.form.minValue.$valid).toBeFalsy();
                    });

                    it('when min value is below and max value is between defined ranges', () => {
                        scope.maxValue = new Date(2002, 10, 1);
                        scope.form.minValue.$setViewValue(new Date(1990, 1, 1));
                        scope.$apply();

                        expect(scope.form.minValue.$valid).toBeFalsy();
                    });
                });
            });

            describe('when range list has ranges with open ends', () => {
                beforeEach(() => {
                    scope.rangeList = [
                        { from: null, to: new Date(2001, 1, 1) },
                        { from: new Date(2002, 1, 1), to: new Date(2003, 1, 1) },
                        { from: new Date(2004, 1, 1), to: null }
                    ];
                    scope.$apply();
                });

                describe('there are validation errors', () => {
                    it('when min and max values are below lowest range value', () => {
                        scope.maxValue = new Date(2000, 1, 1);
                        scope.form.minValue.$setViewValue(new Date(1990, 1, 1));
                        scope.$apply();

                        expect(scope.form.minValue.$valid).toBeFalsy();
                    });

                    it('when min and max values are above highest range value', () => {
                        scope.maxValue = new Date(2006, 1, 1);
                        scope.form.minValue.$setViewValue(new Date(2005, 1, 1));
                        scope.$apply();

                        expect(scope.form.minValue.$valid).toBeFalsy();
                    });

                    it('when min and max are null', () => {
                        scope.maxValue = null;
                        scope.form.minValue.$setViewValue(null);
                        scope.$apply();

                        expect(scope.form.minValue.$valid).toBeFalsy();
                    });

                    it('when min is null and max is below lowest range value', () => {
                        scope.maxValue = new Date(1990, 1, 1);
                        scope.form.minValue.$setViewValue(null);
                        scope.$apply();

                        expect(scope.form.minValue.$valid).toBeFalsy();
                    });

                    it('when max is null and min is above highest range value', () => {
                        scope.maxValue = null;
                        scope.form.minValue.$setViewValue(new Date(2005, 1, 1));
                        scope.$apply();

                        expect(scope.form.minValue.$valid).toBeFalsy();
                    });
                });

                describe('there are no validation errors', () => {
                    it('when min nad max form a range between defined ranges and not overlapping them', () => {
                        scope.maxValue = new Date(2003, 11, 1);
                        scope.form.minValue.$setViewValue(new Date(2003, 10, 1));
                        scope.$apply();

                        expect(scope.form.minValue.$valid).toBeTruthy();
                    });
                });
            });
        });

        //--- dont-overlap-max END        
        //--- dont-overlap-min START

        describe('and dont-overlap-min attribute is specified', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) => {

                compile = $compile;
                scope = <IDontOverlapScope>$rootScope.$new();
                scope.model = null;
                element = compile('<form name="form"><input type="text" dont-overlap="{{rangeList}}" dont-overlap-min="{{minValue}}" ng-model="maxValue" name="maxValue" /></form>')(scope);
            }));

            describe('when range list has well formed ranges', () => {
                beforeEach(() => {
                    scope.rangeList = [
                        { from: new Date(2000, 1, 1), to: new Date(2001, 1, 1) },
                        { from: new Date(2002, 1, 1), to: new Date(2003, 1, 1) }
                    ];
                    scope.$apply();
                });

                describe('there are no validation errors', () => {
                    it('when both values form a range above defined ranges', () => {
                        scope.minValue = new Date(2004, 1, 1);
                        scope.form.maxValue.$setViewValue(new Date(2005, 1, 1));
                        scope.$apply();

                        expect(scope.form.maxValue.$valid).toBeTruthy();
                    });

                    it('when both values form a range below defined ranges', () => {
                        scope.minValue = new Date(1990, 1, 1);
                        scope.form.maxValue.$setViewValue(new Date(1991, 1, 1));
                        scope.$apply();

                        expect(scope.form.maxValue.$valid).toBeTruthy();
                    });

                    it('when both values form a range between defined ranges and not overlapping them', () => {
                        scope.minValue = new Date(2001, 2, 1);
                        scope.form.maxValue.$setViewValue(new Date(2001, 10, 1));
                        scope.$apply();

                        expect(scope.form.maxValue.$valid).toBeTruthy();
                    });
                });

                describe('there are validation errors', () => {
                    it('when min value is below and max value is above defined ranges', () => {
                        scope.minValue = new Date(1990, 1, 1);
                        scope.form.maxValue.$setViewValue(new Date(2008, 1, 1));
                        scope.$apply();

                        expect(scope.form.maxValue.$valid).toBeFalsy();
                    });

                    it('when min value is below and max value is between defined ranges', () => {
                        scope.minValue = new Date(1990, 1, 1);
                        scope.form.maxValue.$setViewValue(new Date(2002, 10, 1));
                        scope.$apply();

                        expect(scope.form.maxValue.$valid).toBeFalsy();
                    });
                });
            });

            describe('when range list has ranges with open ends', () => {
                beforeEach(() => {
                    scope.rangeList = [
                        { from: null, to: new Date(2001, 1, 1) },
                        { from: new Date(2002, 1, 1), to: new Date(2003, 1, 1) },
                        { from: new Date(2004, 1, 1), to: null }
                    ];
                    scope.$apply();
                });

                describe('there are validation errors', () => {
                    it('when min and max values are below lowest range value', () => {
                        scope.minValue = new Date(1990, 1, 1);
                        scope.form.maxValue.$setViewValue(new Date(2000, 1, 1));
                        scope.$apply();

                        expect(scope.form.maxValue.$valid).toBeFalsy();
                    });

                    it('when min and max values are above highest range value', () => {
                        scope.minValue = new Date(2005, 1, 1);
                        scope.form.maxValue.$setViewValue(new Date(2006, 1, 1));
                        scope.$apply();

                        expect(scope.form.maxValue.$valid).toBeFalsy();
                    });

                    it('when min and max are null', () => {
                        scope.minValue = null;
                        scope.form.maxValue.$setViewValue(null);
                        scope.$apply();

                        expect(scope.form.maxValue.$valid).toBeFalsy();
                    });

                    it('when max is null and min is above highest range value', () =>{
                        scope.minValue = new Date(2005, 1, 1);
                        scope.form.maxValue.$setViewValue(null);
                        scope.$apply();

                        expect(scope.form.maxValue.$valid).toBeFalsy();
                    });

                    it('when min is null and max is below lowest range value', () =>{
                        scope.minValue = null;
                        scope.form.maxValue.$setViewValue(new Date(1990, 1, 1));
                        scope.$apply();

                        expect(scope.form.maxValue.$valid).toBeFalsy();
                    });
                });

                describe('there are no validation errors', () => {
                    it('when min nad max form a range between defined ranges and not overlapping them', () => {
                        scope.minValue = new Date(2003, 10, 1);
                        scope.form.maxValue.$setViewValue(new Date(2003, 11, 1));
                        scope.$apply();

                        expect(scope.form.maxValue.$valid).toBeTruthy();
                    });
                });
            });
        });

        //--- dont-overlap-min END
    });
}
