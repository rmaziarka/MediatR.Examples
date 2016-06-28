/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import AttachmentsManagerController = Common.Component.Attachment.AttachmentsManagerController;
    import Business = Common.Models.Business;

    interface IAttachmentsManagerScope extends ng.IScope {
        entityId: string;
        entityType: string;
        attachments: Business.Attachment[];
    }

    describe('Given attachment manager component', () => {
        var scope: IAttachmentsManagerScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            filter: ng.IFilterService,
            controller: AttachmentsManagerController;

        var pageObjectSelectors = {
            common: {
                detailsElement: '.card-body'
            },
            attachments: {
                list: 'card-list#card-list-attachments'
            }
        };

        describe('and attachments are loaded', () => {
            var entityId = 'testEntityId',
                entityType = 'TestEntity'
            var attachments: Business.Attachment[] = [];

            var fileSizeFilter = Mock.FileSize.generate();

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $filter: ng.IFilterService,
                $httpBackend: ng.IHttpBackendService) => {

                filter = $filter;
                scope = <IAttachmentsManagerScope>$rootScope.$new();
                compile = $compile;

                var expectedUrl = new RegExp(`\/api\/services\/attachment\/download\/${entityType}`);
                $httpBackend.whenGET(expectedUrl).respond(() => {
                    return [200, <Antares.Common.Models.Dto.IAzureDownloadUrlContainer>{ url: '' }];
                });

                scope.entityId = entityId;
                scope.entityType = entityType;
                scope.attachments = attachments;
            }));

            describe('and not limited',
            () =>{
                beforeEach(() =>{
                    element =
                        compile('<attachments-manager entity-id="entityId" entity-type="{{entityType}}" attachments="attachments"></attachments-manager>')(scope);
                        scope.$apply();

                        controller = element.controller('attachmentsManager');
                });

                it('when no attachments then "no items" element must be visible', () => {
                    // arrange / act
                    controller.attachments = [];
                    scope.$apply();

                    // assert
                    var cardListElement = element.find(pageObjectSelectors.attachments.list),
                        cardListNoItemsElement = cardListElement.find('card-list-no-items'),
                        cardListNoItemsElementContent = cardListNoItemsElement.find('[translate="ATTACHMENT.LIST.NO_ATTACHMENTS"]'),
                        cardListItemElement = cardListElement.find('card-list-item');

                    expect(cardListNoItemsElement.hasClass('ng-hide')).toBeFalsy();
                    expect(cardListNoItemsElementContent.length).toBe(1);
                    expect(cardListItemElement.length).toBe(0);
                });

                it('when existing attachments then card components must be visible', () => {
                    // arrange / act
                    controller.attachments = TestHelpers.AttachmentGenerator.generateMany(2);
                    scope.$apply();

                    // assert
                    var cardListElement = element.find(pageObjectSelectors.attachments.list),
                        cardListNoItemsElement = cardListElement.find('card-list-no-items'),
                        cardListItemElement = cardListElement.find('card-list-item'),
                        cardListItemCardElement = cardListItemElement.find('card');

                    expect(cardListNoItemsElement.hasClass('ng-hide')).toBeTruthy();
                    expect(cardListItemElement.length).toBe(2);
                    expect(cardListItemCardElement.length).toBe(2);

                });

                it('when existing attachments then card components must have proper order', () => {
                    // arrange / act
                    var date1Mock = new Date('2016-01-12');
                    var date2Mock = new Date('2016-01-18');
                    controller.attachments = TestHelpers.AttachmentGenerator.generateMany(2);
                    controller.attachments[0].createdDate = date1Mock;
                    controller.attachments[1].createdDate = date2Mock;
                    scope.$apply();

                    // assert
                    var cardListElement = element.find(pageObjectSelectors.attachments.list),
                        cardListItemCardElements = cardListElement.find('card-list-item card'),
                        cardListItem1CardElement = cardListElement.find('card[id="attachment-card-' + controller.attachments[0].id + '"]'),
                        cardListItem2CardElement = cardListElement.find('card[id="attachment-card-' + controller.attachments[1].id + '"]');

                    expect(cardListItemCardElements[0]).toBe(cardListItem2CardElement[0]);
                    expect(cardListItemCardElements[1]).toBe(cardListItem1CardElement[0]);
                });

                it('when existing attachments then card components must have proper data', () => {
                    // arrange / act
                    var dateMock = new Date('2011-01-01');
                    var attachmentMock = TestHelpers.AttachmentGenerator.generate({ user: new Business.User({ id: 'us1', firstName: 'firstName1', lastName: 'lastName1', departmentId: 'depId', department: null }) });
                    attachmentMock.createdDate = dateMock;

                    controller.attachments = [attachmentMock];
                    scope.$apply();

                    // assert
                    var cardListElement = element.find(pageObjectSelectors.attachments.list),
                        cardListItemElement = cardListElement.find('card-list-item'),
                        cardListItemCardElement = cardListItemElement.find('card[id="attachment-card-' + attachmentMock.id + '"]');

                    var attachmentDataElement = cardListItemCardElement.find('[id="attachment-data-' + attachmentMock.id + '"]');
                    var attachmentDateElement = cardListItemCardElement.find('[id="attachment-created-date-' + attachmentMock.id + '"]');
                    var attachmentTypeElement = cardListItemCardElement.find('[id="attachment-type-' + attachmentMock.id + '"]');
                    var attachmentFileSizeElement = cardListItemCardElement.find('[id="attachment-file-size-' + attachmentMock.id + '"]');

                    expect(attachmentDataElement.text()).toBe(attachmentMock.fileName);
                    var formattedDate = filter('date')(dateMock, 'dd-MM-yyyy');
                    expect(attachmentDateElement.text()).toBe(formattedDate);
                    expect(attachmentTypeElement.text()).toBe('DYNAMICTRANSLATIONS.' + attachmentMock.documentTypeId);
                    expect(fileSizeFilter).toHaveBeenCalledWith(attachmentMock.size);
                });

                describe('and attachment details is clicked', () => {
                    it('then selectedAttachment object must be updated', () => {
                        // arrange
                        controller.attachments = TestHelpers.AttachmentGenerator.generateMany(4);
                        scope.$apply();

                        var attachmentForDetailsClick = controller.attachments[2];

                        // act
                        var cardElement = element.find('card[id="attachment-card-' + attachmentForDetailsClick.id + '"]');
                        cardElement.find(pageObjectSelectors.common.detailsElement).click();

                        // assert
                        expect(controller.selectedAttachment).toBe(attachmentForDetailsClick);
                    });
                });
            });

            describe('and limited to 1',
                () => {
                    beforeEach(() => {
                        element =
                            compile('<attachments-manager files-number-limit="1" entity-id="entityId" entity-type="{{entityType}}" attachments="attachments"></attachments-manager>')(scope);
                        scope.$apply();

                        controller = element.controller('attachmentsManager');
                    });

                    it('when existing 2 attachments then card component must show 1 item', () => {
                        // arrange / act
                        controller.attachments = TestHelpers.AttachmentGenerator.generateMany(2);
                        scope.$apply();

                        // assert
                        var cardListElement = element.find(pageObjectSelectors.attachments.list),
                            cardListNoItemsElement = cardListElement.find('card-list-no-items'),
                            cardListItemElement = cardListElement.find('card-list-item'),
                            cardListItemCardElement = cardListItemElement.find('card');

                        expect(cardListNoItemsElement.hasClass('ng-hide')).toBeTruthy();
                        expect(cardListItemElement.length).toBe(1);
                        expect(cardListItemCardElement.length).toBe(1);

                    });
                });
        });
    });
}