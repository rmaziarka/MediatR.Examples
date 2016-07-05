/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import IFilterNumberWithTrimZeroes = Common.Decorators.IFilterNumberWithTrimZeroes;
    var
        filter: IFilterNumberWithTrimZeroes,
        fractionSize = 2,
        trimZeroes = true;

    describe('trimZeroesNumberFilterDecorator', () => {
        beforeEach(angular.mock.module(($provide: angular.auto.IProvideService) => {
            $provide.decorator('numberFilter', Common.Decorators.TrimZeroesNumberFilterDecorator.decoratorFunction);
        }));    

        beforeEach(inject((
            $filter: ng.IFilterService) =>{

            filter = <IFilterNumberWithTrimZeroes>$filter('number');
        }));

        it('when number has no decimal digits then no trailing zeroes are present', () =>{
            var result = filter(5.00, fractionSize, trimZeroes);

            expect(result).toEqual('5');
        });

        it('when number has decimal digits then those digits are present', () => {
            var result = filter(5.25, fractionSize, trimZeroes);

            expect(result).toEqual('5.25');
        });
    });
}