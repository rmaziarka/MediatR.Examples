/// <reference path="../../../typings/_all.d.ts" />

module Antares.TestHelpers {
    class DataDescribeRunner {
        testCases: any[] = [];
        getTestCaseName: Function = undefined;

        constructor(private describeName: string){
        }

        data<T>(data: T[]): DataDescribeRunner{
            this.testCases = data;

            return this;
        }

        dataIt(getTestNameForData: Function): DataDescribeRunner{
            this.getTestCaseName = getTestNameForData;

            return this;
        }

        run(testAction: Function){
            describe(this.describeName, () =>{

                this.testCases.forEach((testCase) =>{
                    let testCaseName = this.getTestCaseName(testCase);

                    it(testCaseName, () =>{
                        testAction(testCase);
                    });
                });
            });
        }
    }

    export function runDescribe(name: string){
        return new DataDescribeRunner(name);
    }
}