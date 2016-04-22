/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
	import Business = Common.Models.Business;

	export class CharacteristicSelectController {
        public characteristic: Business.Characteristic = new Business.Characteristic();
        public characteristicSelect: Business.CharacteristicSelect = new Business.CharacteristicSelect();
		public isCommentVisible: boolean = false;

		toggleComment = () => {
			this.isCommentVisible = !this.isCommentVisible;
		};

		onSelectCharacteristicChange = () => {
			if (!this.characteristicSelect.isSelected) {
				this.characteristicSelect.text = null;
				this.isCommentVisible = false;
			}
		};

        isTextEntered = () => {
	        return this.characteristicSelect.text.length > 0;
        };
	}

	angular.module('app').controller('CharacteristicSelectController', CharacteristicSelectController);
}