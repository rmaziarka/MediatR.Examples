(function () {
    beforeEach(angular.mock.module('app'));

    beforeEach(function () {
        angular.mock.module(function ($provide) {
            $provide.constant('appConfig', {
                rootUrl: '', enumOrder: {
                    "OfferStatus": {
                        "New": 2,
                        "Withdrawn": 4,
                        "Rejected": 3,
                        "Accepted": 1
                    },
                    "ActivityDepartmentType": {
                        "Managing": 1,
                        "Standard": 2
                    }
                }
            });
            $provide.service('enumService', Antares.Mock.EnumServiceMock);
        });
    });
} ())