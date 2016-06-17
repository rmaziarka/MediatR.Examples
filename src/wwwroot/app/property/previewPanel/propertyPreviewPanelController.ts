/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import PageTypeEnum = Antares.Common.Models.Enums.PageTypeEnum;
    import PreviewProperty = Antares.Common.Models.Business.PreviewProperty;

    export class PropertyPreviewPanelController extends Antares.Common.Component.BaseSidePanelController {
        // binding
        property: PreviewProperty;
    }

    angular.module('app').controller('PropertyPreviewPanelController', PropertyPreviewPanelController);
}