module Antares.Attributes {
    import Event = Core.Event;

    export class OpenPropertyPrewiewPanelEvent extends Event {
        private openPropertyPrewiewPanelEvent: boolean;
        getKey(): string { return "propertyPrewiew.openPanel"; }
    }
}