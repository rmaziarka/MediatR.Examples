/// <reference path="../../../../typings/_all.d.ts" />
module Antares {

    declare var filesize: Filesize.IFilesize;


    import FileSizeFilter = Antares.Common.Filters.FileSizeFilter;
    import runDescribe = TestHelpers.runDescribe;

    describe('Given fileSize filter', () => {

        var
            createFilter: any;// = (fs: Filesize.IFilesize): FileSizeFilter => { return null; };

        beforeEach(inject(() => {
            createFilter = (filesize: Filesize.IFilesize) => {
                return new FileSizeFilter(filesize);
            };
        }));

        type TestCaseForFileSizeFilter = [number, string];
        runDescribe('when using for transformation of')
            .data<TestCaseForFileSizeFilter>([
                [0, '0 B'],
                [1, '1 B'],
                [1024, '1 KB'],
                [2024000, '1.9 MB'],
                [4024041250, '3.7 GB'],
                [8024009926431, '7.3 TB']])
            .dataIt((data: TestCaseForFileSizeFilter) =>
                `value "${data[0]}" then should get "${data[1]}"`)
            .run((data: TestCaseForFileSizeFilter) => {
                // arrange
                var filter = createFilter(filesize);
                // act
                var transformedValue = filter.transform(data[0]);
                // assert
                expect(transformedValue).toBe(data[1]);
            });

        it('when using then should call filesize global function', () => {
            // arrange
            var fileSizeMock = jasmine.createSpy('filesize');
            var filter = createFilter(fileSizeMock);

            // act
            filter.transform(0);

            // assert
            expect(fileSizeMock).toHaveBeenCalledTimes(1);
        });
    });
}