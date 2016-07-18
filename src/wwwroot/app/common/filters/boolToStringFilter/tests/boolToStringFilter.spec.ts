/// <reference path="../../../../typings/_all.d.ts" />
module Antares {
    import runDescribe = TestHelpers.runDescribe;
    import BoolToStringFilter = Common.Filters.BoolToStringFilter;

    describe('Given boolToString filter', () =>{

        type TestCaseForDefaultValueFilter = [any, any];

        runDescribe('when using for transformation')
            .data<TestCaseForDefaultValueFilter>([
                [null, ''],
                [undefined, ''],
                [true, 'Yes'],
                [false, 'No'],
                [123, ''],
                ['xyz', '']])
            .dataIt((data: TestCaseForDefaultValueFilter) =>
                `value "${data[0]}" then should get "${data[1]}"`)
            .run((data: TestCaseForDefaultValueFilter) => {
                // arrange
                var filter = new BoolToStringFilter(TranslationMock.getAsService());

                // act
                var transformedValue = filter.transform(data[0]);

                // assert
                expect(transformedValue).toBe(data[1]);
            });
    });

    class TranslationMock {
        public instant(key: string){
            switch (key) {
                case "COMMON.YES":
                    return "Yes";
                case "COMMON.NO":
                    return "No";
                default:
                    return "-bad-mock-value-";
            }
        }

        public static getAsService(): ng.translate.ITranslateService{
            return <ng.translate.ITranslateService><any>new TranslationMock();
        }
    }
}