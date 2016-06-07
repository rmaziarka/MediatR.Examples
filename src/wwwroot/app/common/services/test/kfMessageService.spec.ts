/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import KfMessageService = Services.KfMessageService;
    import runDescribe = TestHelpers.runDescribe;

    describe('KfMessageService: Given application started', () => {
        var
            messageService: KfMessageService,
            growlService: angular.growl.IGrowlService,
            growlMessageMock = <angular.growl.IGrowlMessage>{},
            message = 'message',
            messageCode = 'MESSAGE.CODE',
            titleCode = 'TITLE.CODE';

        beforeEach(inject((
            growl: angular.growl.IGrowlService,
            kfMessageService: KfMessageService) => {

            growlService = growl;
            messageService = kfMessageService;
        }));

        it('service should be defined', () => {
            expect(messageService).toBeDefined();
        });

        it('when showSuccess() is called, growl.success is called', () => {
            spyOn(growlService, 'success').and.returnValue(growlMessageMock);

            var result = messageService.showSuccess(message);

            expect(result).toBe(growlMessageMock);
            expect(growlService.success).toHaveBeenCalledWith(message);
        });

        it('when showSuccessByCode() is called, growl.success is called', () => {
            spyOn(growlService, 'success').and.returnValue(growlMessageMock);
            var result = messageService.showSuccessByCode(messageCode);

            expect(result).toBe(growlMessageMock);
            expect(growlService.success).toHaveBeenCalledWith(messageCode);
        });

        it('when showError() is called, growl.error is called', () => {
            spyOn(growlService, 'error').and.returnValue(growlMessageMock);

            var result = messageService.showError(message);

            expect(result).toBe(growlMessageMock);
            expect(growlService.error).toHaveBeenCalledWith(message);
        });

        it('when showErrorByCode() is called with message, growl.error is called with message', () => {
            spyOn(growlService, 'error').and.returnValue(growlMessageMock);

            var result = messageService.showErrorByCode(messageCode);

            expect(result).toBe(growlMessageMock);
            expect(growlService.error).toHaveBeenCalledWith(messageCode);
        });


        it('when showErrorByCode() is called with message and title, growl.error is called with message and title', () => {
            spyOn(growlService, 'error').and.returnValue(growlMessageMock);

            var result = messageService.showErrorByCode(messageCode, titleCode);

            expect(result).toBe(growlMessageMock);
            expect(growlService.error).toHaveBeenCalledWith(messageCode, { title: titleCode });
        });

        runDescribe('when showErrors() is called with no errors ')
            .data<any>([null, {}, { errors: null }, { errors: [] }])
            .dataIt(() =>
                `then growl.error should not be called`)
            .run((response: any) => {
                spyOn(growlService, 'error').and.returnValue(growlMessageMock);

                var result = messageService.showErrors(response);

                expect(growlService.error).not.toHaveBeenCalled();
                expect(result).toBeDefined();
                expect(angular.isArray(result)).toBeTruthy();
                expect(result.length).toBe(0);
            });

        it('when showErrors() is called with errors then growl.error should be called with every error', () =>{
            var response = {
                data : [
                    { message : 'error1' },
                    { message : 'error2' }
                ]
            };

            spyOn(growlService, 'error').and.returnValue(growlMessageMock);

            var result = messageService.showErrors(response);

            expect(growlService.error).toHaveBeenCalledTimes(response.data.length);
            response.data.forEach((error) => expect(growlService.error).toHaveBeenCalledWith(error.message));
            expect(result.length).toEqual(response.data.length);
        });
    });
}