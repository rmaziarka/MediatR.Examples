/// <reference path="../../../../typings/_all.d.ts" />
module Antares {
    import runDescribe = TestHelpers.runDescribe;
    import DefaultValueFilter = Common.Filters.DefaultValueFilter;

    describe('Given defaultValue filter', () =>{
        var date = new Date();

        type TestCaseForDefaultValueFilter = [any, any];
        runDescribe('when using for transformation')
            .data<TestCaseForDefaultValueFilter>([
                [null, '-'],
                [undefined, '-'],
                [NaN, '-'],
                [0, '-'],
                [false, '-'],
                ['', '-'],
                [1, 1],
                ['foo', 'foo'],
                [date, date]])
            .dataIt((data: TestCaseForDefaultValueFilter) =>
                `value "${data[0]}" then should get "${data[1]}"`)
            .run((data: TestCaseForDefaultValueFilter) => {
                // arrange
                var filter = new DefaultValueFilter();

                // act
                var transformedValue = filter.transform(data[0]);

                // assert
                expect(transformedValue).toBe(data[1]);
            });
    });
}