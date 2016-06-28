/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import Business = Common.Models.Business;

    describe('Given activity dto with 7 viewings',
    () =>{
        var activityDto = TestHelpers.ActivityGenerator.generateDto();
        var viewingsDto =
        [
            Antares.TestHelpers.ViewingGenerator.generateDto({ startDate : new Date(2001, 10, 9, 10, 10) }),
            Antares.TestHelpers.ViewingGenerator.generateDto({ startDate : new Date(2001, 10, 10, 10, 20) }),
            Antares.TestHelpers.ViewingGenerator.generateDto({ startDate : new Date(2001, 10, 10, 10, 10) }),
            Antares.TestHelpers.ViewingGenerator.generateDto({ startDate : new Date(2001, 10, 12) }),
            Antares.TestHelpers.ViewingGenerator.generateDto({ startDate : new Date(2001, 10, 10, 11, 10) }),
            Antares.TestHelpers.ViewingGenerator.generateDto({ startDate : new Date(2001, 10, 11) }),
            Antares.TestHelpers.ViewingGenerator.generateDto({ startDate : new Date(2001, 10, 10, 8, 20) })
        ];

        beforeEach(() =>{
            activityDto.viewings = viewingsDto;
        });

        it('when creating activity must be 5 viewings grupped by day',
        () =>{
            var activity = new Business.Activity(activityDto);
            // assert
            expect(activity.viewingsByDay.length).toBe(3);
            expect(activity.viewingsByDay[0].day).toBe("2001-11-12");
            expect(activity.viewingsByDay[0].viewings).toEqual([new Business.Viewing(viewingsDto[3])]);
            expect(activity.viewingsByDay[1].day).toBe("2001-11-11");
            expect(activity.viewingsByDay[1].viewings).toEqual([new Business.Viewing(viewingsDto[5])]);
            expect(activity.viewingsByDay[2].day).toBe("2001-11-10");
            expect(activity.viewingsByDay[2].viewings)
                .toEqual([
                    new Business.Viewing(viewingsDto[4]),
                    new Business.Viewing(viewingsDto[1]),
                    new Business.Viewing(viewingsDto[2])
                ]);
        });
    });
}