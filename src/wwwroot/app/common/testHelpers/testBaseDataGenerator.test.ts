/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    export class TestDataGenerator<T, TD> {
        protected makeRandom(text: string): string {
            return text + _.random(1, 1000000);
        }

        public generateDto(specificData?: any): T {
            return null;
        };

        public generateManyDtos(n: number): T[] {
            return _.times(n, this.generateDto);
        }

        public generateMany(n: number, businessConstructor: { new (T: any): TD; }): TD[] {
            return _.map<T, TD>(this.generateManyDtos(n), (dto: T) => { return new businessConstructor(dto); });
        }

        public generate(businessConstructor: { new (T: any): TD; }, specificData?: any): TD {
            return new businessConstructor(this.generateDto(specificData));
        }
    }
}