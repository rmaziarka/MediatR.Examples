(function () {
    beforeEach(angular.mock.module('app'));

    beforeEach(function () {
        angular.mock.module(function ($provide) {
            $provide.constant('appConfig', { rootUrl: '' });
            $provide.service('enumService', Antares.Mock.EnumServiceMock);
        });
    });
}())