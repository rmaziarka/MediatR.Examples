/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    import Enums = Antares.Common.Models.Enums;
    export abstract class BaseSidePanelController {
                
        isBusy: boolean;
        isVisible: Enums.SidePanelState = Enums.SidePanelState.Untouched;

        protected panelShown = () => {
        };

        protected panelHidden = () => {
        };        

        public $onChanges(changesObj: IBaseSidePanelChange) {
            if (changesObj.isVisible && changesObj.isVisible.currentValue !== changesObj.isVisible.previousValue) {
                if (changesObj.isVisible.currentValue === Enums.SidePanelState.Opened) {
                    this.panelShown();
                }
                else if (changesObj.isVisible.currentValue === Enums.SidePanelState.Closed) {
                    this.panelShown();
                }
            }
        }
    }

    interface IBaseSidePanelChange {
        isVisible: { currentValue: any, previousValue: any, isFirstChange: () => boolean }
    }
}
