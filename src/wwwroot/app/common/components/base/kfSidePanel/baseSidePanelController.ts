/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    export abstract class BaseSidePanelController {

        isVisible: boolean;
        stateChanged: boolean = false;
        isBusy: boolean;

        protected panelShown = () => {

        };

        show = () => {
            this.stateChanged = true;
            this.panelShown();
        }

        hide = () => {
            this.stateChanged = true;
        }

        $onChanges = (changesObj: IBaseSidePanelChange) => {
            if (changesObj.isVisible && changesObj.isVisible.currentValue !== changesObj.isVisible.previousValue) {
                if (changesObj.isVisible.currentValue === true) {
                    this.show();
                }
                else if (changesObj.isVisible.currentValue === false) {
                    this.hide();
                }
            }
        }
    }

    interface IBaseSidePanelChange {
        isVisible: { currentValue: any, previousValue: any }
    }
}
