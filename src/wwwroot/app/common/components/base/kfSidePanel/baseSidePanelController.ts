/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    import Enums = Antares.Common.Models.Enums;
    export abstract class BaseSidePanelController {
                
        isBusy: boolean;
        isVisible: Enums.SidePanelState = Enums.SidePanelState.Untouched;
        busyLabelKey: string;

        protected panelShown = () => {
        };

        protected panelHidden = () => {
        };        

        public $onChanges(changesObj: IBaseSidePanelChange) {
            if (changesObj.isVisible && changesObj.isVisible.currentValue !== changesObj.isVisible.previousValue) {
                if (changesObj.isVisible.currentValue === Enums.SidePanelState.Opened) {
                    this.panelShown();
                }
                else if (changesObj.isVisible.currentValue === Enums.SidePanelState.Closed
                    && changesObj.isVisible.previousValue === Enums.SidePanelState.Untouched) {
                    this.isVisible = Enums.SidePanelState.Untouched;
                }
                else if (changesObj.isVisible.currentValue === Enums.SidePanelState.Closed) {
                    this.panelHidden();
                }
            }

            this.onChanges(changesObj);
        }

        protected onChanges = (obj: any) =>{
        };
    }

    interface IBaseSidePanelChange {
        isVisible: { currentValue: Enums.SidePanelState, previousValue: Enums.SidePanelState, isFirstChange: () => boolean }
    }
}
