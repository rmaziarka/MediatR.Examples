(function () {
    beforeEach(function () {
        angular.mock.module(function ($provide) {
            $provide.constant('appConfig', { rootUrl: '' });
        });
    });

    beforeEach(angular.mock.module('app'));
}())