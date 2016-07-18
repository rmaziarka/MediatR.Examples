/// <reference path="../typings/_all.d.ts" />

module Antares.Company {

    export class CompanyControls {

        public static config = {
            relationshipManager: <Attributes.ISelectUserEditControlConfig>{
                user: {
                    required: false,
                    active: true
                }
            }
        };

        public static controlSchemas = {
            relationshipManager: <Attributes.ISelectUserEditControlSchema>{
                formName: "relationshipManager",
                fieldName: "relationshipManager",
                translationKey: "COMPANY.RELATIONSHIP_MANAGER",
                controlId: "relationshipManager",
                placeholderTranslationKey: "COMPANY.RELATIONSHIP_MANAGER_PLACEHOLDER"
            }
        };

        public static descriptionMaxLength: number = 4000;

        public static formatUrlWithProtocol = (url: string): string => {
            //regular expression for url with a protocol (case insensitive)
            if (url && url.length > 0) {
                let r = new RegExp('^(?:[a-z]+:)?//', 'i');

                if (!r.test(url)) {
                    return `http://${url}`;
                }
            }
            return url;
        }
    }
};