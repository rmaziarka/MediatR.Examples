/// <reference path="typings/_all.d.ts" />
module Antares {
    describe('Test dummy', () => {
        it('sum should return numbers sum', () => {
            var dummy: Dummy = new Dummy();

            var result = dummy.sum(1, 2);

            expect(result).toBe(3);
        });
    });
}