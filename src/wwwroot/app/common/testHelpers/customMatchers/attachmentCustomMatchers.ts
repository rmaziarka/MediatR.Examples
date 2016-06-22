/// <reference path="../../../typings/_all.d.ts" />

// ReSharper disable InconsistentNaming
declare module jasmine {
    interface Matchers {
        // ReSharper restore InconsistentNaming
        toBeSameAsAttachmentModel(expected: any): void;
        toBeSameAsAttachment(expected: any): void;
    }
}

module Antares.TestHelpers.CustomMatchers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class AttachmentCustomMatchersGenerator {
        public static generate = () => {
            var attachmentCustomMatchers: jasmine.CustomMatcherFactories = {
                toBeSameAsAttachmentModel: (util: jasmine.MatchersUtil): jasmine.CustomMatcher => ({
                    compare: (actual: Common.Component.Attachment.AttachmentSaveCommand, expected: Common.Component.Attachment.AttachmentUploadCardModel): jasmine.CustomMatcherResult => {
                        var result: jasmine.CustomMatcherResult = {
                            pass: false,
                            message: ""
                        };

                        var attachmentModel = AttachmentUploadCardModelGenerator.generate();
                        var requestData: Property.Command.PropertyAttachmentSaveCommand;

                        result.pass = util.equals(actual.fileName, expected.fileName)
                            && util.equals(actual.size, expected.size)
                            && util.equals(actual.documentTypeId, expected.documentTypeId)
                            && util.equals(actual.externalDocumentId, expected.externalDocumentId);

                        if (!result.pass) {
                            result.message = "Expected actual attachment: " + JSON.stringify(actual, null, 4) + " to be same as expected attachment: " + JSON.stringify(expected, null, 4);
                        }
                        else {
                            // if used with .not
                            result.message = "Expected actual attachment to be different than expected attachment but they are the same.";
                        }

                        return result;
                    }
                }),
                toBeSameAsAttachment: (util: jasmine.MatchersUtil): jasmine.CustomMatcher => ({
                    compare: (actual: Business.Attachment, expected: Dto.IAttachment): jasmine.CustomMatcherResult => {
                        var result: jasmine.CustomMatcherResult = {
                            pass: false,
                            message: ""
                        };

                        var attachmentModel = TestHelpers.AttachmentUploadCardModelGenerator.generate();
                        var requestData: Property.Command.PropertyAttachmentSaveCommand;

                        result.pass = util.equals(actual.id, expected.id)
                            && util.equals(actual.user, expected.user)
                            && util.equals(actual.createdDate, expected.createdDate)
                            && util.equals(actual.fileName, expected.fileName)
                            && util.equals(actual.size, expected.size)
                            && util.equals(actual.documentTypeId, expected.documentTypeId)
                            && util.equals(actual.externalDocumentId, expected.externalDocumentId);

                        if (!result.pass) {
                            result.message = "Expected actual attachment: " + JSON.stringify(actual, null, 4) + " to be same as expected attachment: " + JSON.stringify(expected, null, 4);
                        }
                        else {
                            // if used with .not
                            result.message = "Expected actual attachment to be different than expected attachment but they are the same.";
                        }

                        return result;
                    }
                })
            }

            return attachmentCustomMatchers;
        }

    }
}