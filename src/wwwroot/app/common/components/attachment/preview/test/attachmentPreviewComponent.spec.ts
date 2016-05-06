/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import AttachmentPreviewController = Common.Component.AttachmentPreviewController;
    import Business = Common.Models.Business;

    describe('Given attachment preview component is loaded', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            $http: ng.IHttpBackendService,
            filter: ng.IFilterService,
            controller: AttachmentPreviewController;

        var pageObjectSelectors = {
            fileNameLink: '#attachment-preview-fileName a',
            fileNameSpan: '#attachment-preview-fileName span',
            fileTypeId: '#attachment-preview-type',
            size: '#attachment-preview-size',
            createdDate: '#attachment-preview-created-date',
            user: '#attachment-preview-user'
        }

        describe('and proper attachment is set', () =>{
            var activityIdMock = 'testActivityId';
            var attachmentMock = TestHelpers.AttachmentGenerator.generate({ user: new Business.User({ id: 'us1', firstName: 'firstName1', lastName: 'lastName1' })});

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService,
                $filter: ng.IFilterService) => {

                $http = $httpBackend;
                filter = $filter;
                scope = $rootScope.$new();

                $http.whenGET(/\/api\/services\/attachment\/download/).respond(() => {
                    return {};
                });

                element = $compile('<attachment-preview></attachment-preview>')(scope);
                scope.$apply();

                // arrange + act
                controller = element.controller('attachmentPreview');
                controller.setAttachment(attachmentMock, activityIdMock);
                scope.$apply();
            }));

            it('then attachment file name value should be displayed as link if file url exists', () => {
                // arrange + act
                controller.attachmentUrl = 'some_url';
                scope.$apply();

                // assert
                var fileNameElement = element.find(pageObjectSelectors.fileNameLink);
                expect(fileNameElement.text()).toBe(attachmentMock.fileName);
            });

            it('then attachment file name value should be displayed as span if file url does not exists', () => {
                // arrange + act
                controller.attachmentUrl = '';
                scope.$apply();

                // assert
                var fileNameElement = element.find(pageObjectSelectors.fileNameSpan);
                expect(fileNameElement.text()).toBe(attachmentMock.fileName);
            });

            it('then attachment file type value should be displayed', () => {
                // assert
                var fileTypeIdElement = element.find(pageObjectSelectors.fileTypeId);
                expect(fileTypeIdElement.text()).toBe('DYNAMICTRANSLATIONS.' + attachmentMock.documentTypeId);
            });

            it('then attachment file size value should be displayed', () => {
                // assert
                var sizeElement = element.find(pageObjectSelectors.size);
                expect(sizeElement.text()).toBe(attachmentMock.size + 'MB');
            });

            it('then attachment creation date value should be displayed in proper format', () => {
                // assert
                var formattedDate = filter('date')(attachmentMock.createdDate, 'dd-MM-yyyy');
                var dateElement = element.find(pageObjectSelectors.createdDate);
                expect(dateElement.text()).toBe(formattedDate);
            });

            it('then attachment user value should be displayed', () => {
                // assert
                var userElement = element.find(pageObjectSelectors.user);
                expect(userElement.text()).toBe('firstName1 lastName1');
            });
        });
    });
}