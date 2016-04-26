/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
	import Business = Common.Models.Business;

    export class CharacteristicSelectController {
        // set via binding on characteristic definition within repeater on all characteristocs definitions (so this object always exists)
        public characteristic: Business.Characteristic;
        // set via binding on characteristicSelect within property characteristicsMap (not always exists - exists only if property has this characterisrtic selected)
        public characteristicSelect: Business.CharacteristicSelect;
		public isCommentVisible: boolean = false;

        constructor() {
            // if characteristic select is not set via binding (does not exist within property characteristicsMap), new object will be created (and set to bounded characteristic item within property)
            this.characteristicSelect = this.characteristicSelect || new Business.CharacteristicSelect();
            this.characteristicSelect.characteristicId = this.characteristic.id;
        }

		toggleComment = () => {
			this.isCommentVisible = !this.isCommentVisible;
		};

        onSelectCharacteristicChange = () => {
            if (!this.characteristicSelect.isSelected) {
				this.characteristicSelect.text = '';
				this.isCommentVisible = false;
			}
		};

        isTextEntered = () => {            
            return this.characteristicSelect.text == null ? false : this.characteristicSelect.text.length > 0;
        };
	}

	angular.module('app').controller('CharacteristicSelectController', CharacteristicSelectController);
}