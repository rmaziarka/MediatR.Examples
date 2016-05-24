/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import KfErrorInterceptor = Services.KfErrorInterceptor;
    import KfMessageService = Services.KfMessageService;

    describe('Given application started, KfMessageInterceptor', () => {
        var
            errorInterceptor: KfErrorInterceptor,
            messageService: KfMessageService;

        beforeEach(inject((
            kfErrorInterceptor: KfErrorInterceptor,
            kfMessageService: KfMessageService) => {

            errorInterceptor = kfErrorInterceptor;
            messageService = kfMessageService;
        }));

        it('should be defined', () => {
            expect(errorInterceptor).toBeDefined();
        });

        it('should have a handler for response error', () => {
            expect(angular.isFunction(errorInterceptor.responseError)).toBeTruthy();
        });

        it('should display error message for unhandled server error', () => {
            var rejection = { status: 500 };
            spyOn(messageService, 'showErrorByCode');

            errorInterceptor.responseError(rejection);

            expect(messageService.showErrorByCode).toHaveBeenCalledWith('COMMON.UNEXPECTED_SERVER_ERROR');
        });
    });
}